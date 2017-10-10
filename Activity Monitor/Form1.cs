using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Collections.Specialized;
using System.Collections;
using System.Management;

using System.Net;
using System.Windows.Forms.Design;
using System.Drawing.Imaging;

namespace Activity_Monitor
{
    public partial class Form1 : Form
    {
        public static string webaddress;
        public static string device_key;
        public static string device_location;
        public static string device_name;
        public static string institute_id;
        public static string device_id;
        public static string capture_frequency;
        private bool allowshowdisplay = false;

        mc mc = new mc();
        
        public static string file_path = Application.StartupPath + "\\sys\\";


        public Form1()
        {
            InitializeComponent();
            
           
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        /*********
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }
        /**/
        
        private void btnCommunicate_Click(object sender, EventArgs e)
        {
            string responsedata = "";

            //**         device verify 
            try
            {
                webaddress = txtwebaddress.Text.Trim('/');
                device_key = txtDeviceKey.Text.Trim();
                responsedata = MySpace.api.device_verify(webaddress, device_key);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server Or Key not match");
                return;
            }
           


            if (responsedata == "")
            {
                MessageBox.Show("Device verify fail");
                return;
            }

            string[] rcv = responsedata.Split(new[] { "^#" }, StringSplitOptions.RemoveEmptyEntries);

            device_id           = rcv[0].Split(new[] { '=' })[1];
            device_name         = rcv[1].Split(new[] { '=' })[1];
            device_location     = rcv[2].Split(new[] { '=' })[1];
            institute_id        = rcv[3].Split(new[] { '=' })[1];
            capture_frequency   = rcv[4].Split(new[] { '=' })[1];

            string msg = "Device varified. \n";
            msg += "\nDevice ID. :" + device_id;
            msg += "\nDevice Name: " + device_name;
            msg += "\nDevice Location: " + device_location;
            msg += "\nDevice institute ID:. " + institute_id;
            msg += "\nDevice Captute Frequency: " + capture_frequency;
            msg += "\n\nWant to save config?" ;

            if (MessageBox.Show(msg, "Save Settings confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                Properties.Settings.Default.webaddress = webaddress;
                Properties.Settings.Default.device_key = device_key;
                Properties.Settings.Default.device_location = device_location;
                Properties.Settings.Default.device_name = device_name;
                Properties.Settings.Default.institute_id = institute_id;
                Properties.Settings.Default.device_id = device_id;
                Properties.Settings.Default.capture_frequency = ( Convert.ToInt32( capture_frequency) * 1000*60).ToString();
                Properties.Settings.Default.Save();
                //Properties.Settings.Default.Reset();


                //**************************             
                //create registry to auto startup software
                string dir = Application.ExecutablePath;
                appAutoStart.appAutoStart.SetAutoStart("system", dir);
                //******************************/
                this.Form1_Load(sender, e);

            }

         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            
            if (
                Properties.Settings.Default.webaddress.Trim().Length == 0 ||
                Properties.Settings.Default.institute_id.Trim().Length == 0 ||
                Properties.Settings.Default.device_name.Trim().Length == 0 ||
                Properties.Settings.Default.device_location.Trim().Length == 0 ||
                Properties.Settings.Default.device_key.Trim().Length == 0 ||
                Properties.Settings.Default.capture_frequency.Trim().Length == 0
                )
            {
                this.Show();

                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.ShowInTaskbar = true;
                this.Visible = true;
                this.Opacity = 100;

                tmrCaptureImage.Enabled = false;
                tmrCommand.Enabled = false;
                tmrSendFiles.Enabled = false;
                tmrMessage.Enabled = false;

            }
                
            else
            {
                this.Hide();
                this.FormBorderStyle = FormBorderStyle.None;
                this.ShowInTaskbar = false;
                this.Visible = false;
                this.Opacity = 0;

                    webaddress = Properties.Settings.Default.webaddress;
                    device_key = Properties.Settings.Default.device_key;
                    device_location = Properties.Settings.Default.device_location;
                    device_name = Properties.Settings.Default.device_name;
                    institute_id = Properties.Settings.Default.institute_id;
                    device_id = Properties.Settings.Default.device_id;
                    capture_frequency = Properties.Settings.Default.capture_frequency;
                
                this.Text = file_path;
                tmrCaptureImage.Enabled = true;
                tmrCaptureImage.Interval = Convert.ToInt32(capture_frequency);

                tmrSendFiles.Enabled = true;
                tmrSendFiles.Interval = Convert.ToInt32(capture_frequency);

               tmrCommand.Enabled = true;
               tmrCommand.Interval = 10000;

               tmrMessage.Enabled = true;
               tmrMessage.Interval = 10000;
            }                       

        }


        private  void tmrCaptureImage_Tick(object sender, EventArgs e)
        {
            
            tmrCaptureImage.Enabled = false;

            if (!System.IO.Directory.Exists(file_path))

                System.IO.Directory.CreateDirectory(file_path);

            string path = file_path + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day +" "+ DateTime.Now.Hour + "+" +
            DateTime.Now.Minute + "+" + DateTime.Now.Second + "@__"+device_id+"__.invact";

            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics gr = Graphics.FromImage(bmp);
            gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);


            tmrCaptureImage.Enabled = true;
            
        }


        private  void tmrSendFiles_Tick(object sender, EventArgs e)
        {
            
            MySpace.api.sendFiles(webaddress, file_path, device_id, institute_id);

            
        }

        private  void tmrCommand_Tick(object sender, EventArgs e)
        {
            

                string com_str = MySpace.api.getCommand(webaddress, device_id).Trim();
                if (com_str == "" || com_str == null) return;
                string [] parts=com_str.Split(new[] { "||" },StringSplitOptions.RemoveEmptyEntries);

                tmrCommand.Stop();
            
                MessageBox.Show(parts[1] + "(" + parts[2] + ") command to your PC","Command",MessageBoxButtons.OK);
            
                tmrCommand.Start();

                int command= Convert.ToInt16(  parts[3]);
                //*
                if (command == -1) return;
                if (command == 0) MySpace.commands.runCommand(MySpace.commands.DOS_command.shutdownImmediateFource);
                if (command == 1) MySpace.commands.runCommand(MySpace.commands.DOS_command.shutdownImmediateFourceAfter5min);
                if (command == 2) MySpace.commands.runCommand(MySpace.commands.DOS_command.shutdownImmediateFourceAfter10min);

                if (command == 3) MySpace.commands.runCommand(MySpace.commands.DOS_command.restartImmediateFource);
                if (command == 4) MySpace.commands.runCommand(MySpace.commands.DOS_command.restartImmediateFourceAfter5min);
                if (command == 5) MySpace.commands.runCommand(MySpace.commands.DOS_command.restartImmediateFourceafter10min);
                if (command == 6) MySpace.commands.lock_n_displayOff();
                if (command == 7) MySpace.commands.lockDesktop();
                if (command == 8) MySpace.commands.displayOff();
                  // */

            

            
        }

        private void tmrMessage_Tick(object sender, EventArgs e)
        {
            

             string message = MySpace.api.getMessage(webaddress, device_id).Trim();
            
            if (message == "") return;
            tmrMessage.Stop();

            //if (MessageBox.Show(message, "Message", MessageBoxButtons.OK) == DialogResult.OK)
                MessageBox.Show(message, "Message", MessageBoxButtons.OK);
                tmrMessage.Start();
           
          /*
            mc.Hide();
            
             mc.Show();
            mc.Tag = message;
           * */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


    namespace MySpace
    {
       class commands
        {
            public static class DOS_command
            {
               public static string shutdownImmediateFource = "shutdown -s -f -t 00";
               public static string shutdownImmediateFourceAfter5min = "shutdown -s -f -t 300";
               public static string shutdownImmediateFourceAfter10min = "shutdown -s -f -t 600";
               public static string restartImmediateFource = "shutdown -r -f -t 00";
               public static string restartImmediateFourceAfter5min = "shutdown -r -f -t 300";
               public static string restartImmediateFourceafter10min = "shutdown -r -f -t 600";
            }

            //------------------------------------------------------------------

            private const int WmSyscommand = 0x0112;
            private const int ScMonitorpower = 0xF170;
            private const int HwndBroadcast = 0xFFFF;
            private const int ShutOffDisplay = 2;

            [DllImport("user32.dll")]
            private static extern void LockWorkStation();

            [DllImport("user32.dll", SetLastError = true)]
            private static extern bool PostMessage(IntPtr hWnd, uint msg,
                          IntPtr wParam, IntPtr lParam);

            private static void TurnOffDisplay()
            {
                PostMessage((IntPtr)HwndBroadcast, (uint)WmSyscommand,
                        (IntPtr)ScMonitorpower, (IntPtr)ShutOffDisplay);
            }

            public static void lock_n_displayOff()
            {
                LockWorkStation();
                TurnOffDisplay();
            }



            public static void lockDesktop()
            {
                LockWorkStation();
            }


            public static void displayOff()
            {
                TurnOffDisplay();
            }
            //-----------------------------------------------------------------


            //==================================================================================
           public static void runCommand(string command)
            {

                //* Create your Process
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c "+command;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                //* Set your output and error (asynchronous) handlers
                process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
                //* Start process and handlers
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
           static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
           {
               //* Do your stuff with the output (write to console/log/StringBuilder)
               Console.WriteLine(outLine.Data);
           }
            
        }

        
       class api
       {


           public static string getCommand(string websiteaddress, string device_id)
           {
               string responsedata = UrlSendRcvData(websiteaddress + "/command/get_command/" + device_id);

               return responsedata;
           }


           public static string getMessage(string websiteaddress, string device_id)
           {
               string responsedata = UrlSendRcvData(websiteaddress + "/message/get_message/" + device_id);

               return responsedata;
           }


           public static void sendFiles(string webaddress,string file_path,string device_id,string institute_id)
           {
               if (!System.IO.Directory.Exists(file_path))
                   System.IO.Directory.CreateDirectory(file_path);

               string[] a = System.IO.Directory.GetFiles(file_path, "*.invact", SearchOption.AllDirectories);

               
               for (int i = 0; i < a.Length;i++)
               {
                   NameValueCollection nvc = new NameValueCollection();
                   nvc.Add("id", "TTR");
                   nvc.Add("btn-submit-photo", "Upload");
                   string sfilename = a[i];
                   string dfilename = sfilename.Replace(".invact", ".jpg");

                   System.IO.File.Move(sfilename, dfilename);
               
                   string responsedata = HttpUploadFile(webaddress+"/windows/up/" + device_id + "/" + institute_id, dfilename, "file", "image/jpeg", nvc);

                   if (responsedata.Trim() == "done")
                       System.IO.File.Delete(dfilename);
                   else
                       System.IO.File.Move(dfilename, sfilename);
               
                   //System.Threading.Thread.Sleep(60);
              }
           }

           
           public static string HttpUploadFile(string url, string file, string paramName, string contentType, System.Collections.Specialized.NameValueCollection nvc)
           {
                try
                   {
               //log.Debug(string.Format("Uploading {0} to {1}", file, url));
               string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
               byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

               HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
               wr.ContentType = "multipart/form-data; boundary=" + boundary;
               wr.Method = "POST";
               wr.KeepAlive = true;
               wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
               Stream rs;
           

                    rs = wr.GetRequestStream();
              
                   string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                   foreach (string key in nvc.Keys)
                   {
                       rs.Write(boundarybytes, 0, boundarybytes.Length);
                       string formitem = string.Format(formdataTemplate, key, nvc[key]);
                       byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                       rs.Write(formitembytes, 0, formitembytes.Length);
                   }
                   rs.Write(boundarybytes, 0, boundarybytes.Length);

                   string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                   string header = string.Format(headerTemplate, paramName, file, contentType);
                   byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                   rs.Write(headerbytes, 0, headerbytes.Length);

                   FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                   byte[] buffer = new byte[4096];
                   int bytesRead = 0;
                   while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                   {
                       rs.Write(buffer, 0, bytesRead);
                   }
                   fileStream.Close();

                   byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                   rs.Write(trailer, 0, trailer.Length);
                   rs.Close();

                   WebResponse wresp = null;
                   try
                   {
                       wresp = wr.GetResponse();
                       Stream stream2 = wresp.GetResponseStream();
                       StreamReader reader2 = new StreamReader(stream2);
                       return reader2.ReadToEnd();
                   }
                   catch (Exception ex)
                   {

                       if (wresp != null)
                       {
                           wresp.Close();
                           wresp = null;
                       }
                       return "error" + ex.Message;
                   }
               finally
               {
                   wr = null;
               }


               }
               catch (Exception ex)
               {
                   return "";
               }
               
           }


           public static string device_verify(string websiteaddress,string device_key)
           {
               return UrlSendRcvData(websiteaddress+"/windows/device_verify/"+device_key);
           }

           public static string UrlSendRcvData(string url)
           {


               HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);

               WebResponse wresp = null;

               try
               {
                   wresp = wr.GetResponse();
                   Stream stream2 = wresp.GetResponseStream();
                   StreamReader reader2 = new StreamReader(stream2);
                   return reader2.ReadToEnd();
               }
               catch (Exception ex)
               {

                   if (wresp != null)
                   {
                       wresp.Close();
                       wresp = null;
                   }
                   
                   //MessageBox.Show(ex.Message);

                   return "";
               }
               finally
               {
                   wr = null;
               }
           }


		   static bool isSiteActive()
		   {
			   var ping = new System.Net.NetworkInformation.Ping();

               var result = ping.Send(Form1.webaddress); 

				if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
					return true;
				
				return false;
		   }

       }


       class myDeviceInfo
       {
           public static string get_pc_name()
           {
               return System.Net.Dns.GetHostName();
           }

           public static string get_pc_IPs()
           {
               IPHostEntry hostname = Dns.GetHostEntry(System.Net.Dns.GetHostName());
               
               IPAddress[] Aips = hostname.AddressList;
               string IPS = "";
               foreach (IPAddress ip in Aips)
                   IPS += ip.ToString() + ";";
               return IPS;
           }

           
           public static bool isNetAvailable()
           {
               if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
                   return false;

               try
               {
                   using (var client = new WebClient())
                   using (var stream = client.OpenRead("http://www.google.com"))
                   {
                       return true;
                   }
               }
               catch
               {
                   return false;
               }
           }

           /// <summary>
           /// Return MAC addresses of the host
           /// </summary>
           /// <returns>MAC addresses.</returns>
           public static string GetNicID()
           {
               string NicIds = "";
               System.Net.NetworkInformation.NetworkInterface[] all_nic = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
               foreach (System.Net.NetworkInformation.NetworkInterface nic in all_nic)
               {
                   NicIds += nic.Id + ";";
               }

               return NicIds;
           }


           /// <summary>
           /// Finds the MAC addresses of the NIC with maximum speed.
           /// </summary>
           /// <returns>The MAC address.</returns>
           public static Dictionary<string, string> GetMacAddress()
           {
               const int MIN_MAC_ADDR_LENGTH = 12;
               string macAddress = string.Empty;
               long maxSpeed = -1;
               Dictionary<string, string> mac_s = new Dictionary<string, string>();

               foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
               {
                   //"Found MAC Address: " + nic.GetPhysicalAddress() +
                   //" Type: " + nic.NetworkInterfaceType;

                   string tempMac = nic.GetPhysicalAddress().ToString();
                   if (nic.Speed > maxSpeed &&
                       !string.IsNullOrEmpty(tempMac) &&
                       tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                   {
                       //log.Debug("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                       maxSpeed = nic.Speed;
                       macAddress = tempMac;

                       mac_s.Add(tempMac, maxSpeed.ToString() + ";" + nic.NetworkInterfaceType);
                   }
               }

               return mac_s;
           }

           public static bool IsValidEmail(string email)
           {
               try
               {
                   var addr = new System.Net.Mail.MailAddress(email);
                   return addr.Address == email;
               }
               catch
               {
                   return false;
               }
           }
           public static string get_processor_id()
           {

               String cpuid = "";
               try
               {
                   
                   ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select ProcessorID From Win32_processor");
                   ManagementObjectCollection mbsList = mbs.Get();

                   foreach (ManagementObject mo in mbsList)
                   {
                       cpuid = mo["ProcessorID"].ToString();
                   }
                   return cpuid;
               }
               catch (Exception) { return cpuid; }


           }



       }

        


    }

    namespace appAutoStart
    {
        using Microsoft.Win32;

        /// <summary>
        /// This is for Start an application Automatically and also can off this feature.
        /// 
        /// </summary>
        public class appAutoStart
        {
            //for current user
            //private const string RUN_LOCATION = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";


            //for all users
            private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";

            //dummy
            //private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";

            /// <summary>
            /// Sets the autostart value for the assembly.
            /// </summary>
            /// <param name="keyName">Registry Key Name</param>
            /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
            public static void SetAutoStart(string keyName, string assemblyLocation)
            {
                // RegistryKey key = Registry.LocalMachine.CreateSubKey(RUN_LOCATION);
                //for All users
                //RegistryKey key=Registry.Users.CreateSubKey(RUN_LOCATION);

                //for current user
                RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
                key.SetValue(keyName, assemblyLocation);
            }

            /// <summary>
            /// Returns whether auto start is enabled.
            /// </summary>
            /// <param name="keyName">Registry Key Name</param>
            /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
            public static bool IsAutoStartEnabled(string keyName, string assemblyLocation)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
                if (key == null)
                    return false;

                string value = (string)key.GetValue(keyName);
                if (value == null)
                    return false;

                return (value == assemblyLocation);
            }

            /// <summary>
            /// Unsets the autostart value for the assembly.
            /// </summary>
            /// <param name="keyName">Registry Key Name</param>
            public static void UnSetAutoStart(string keyName)
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
                key.DeleteValue(keyName);
            }
        }
    }

}
