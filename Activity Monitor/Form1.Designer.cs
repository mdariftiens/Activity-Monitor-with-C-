namespace Activity_Monitor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCommunicate = new System.Windows.Forms.Button();
            this.txtwebaddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDeviceKey = new System.Windows.Forms.TextBox();
            this.tmrCaptureImage = new System.Windows.Forms.Timer(this.components);
            this.tmrSendFiles = new System.Windows.Forms.Timer(this.components);
            this.tmrCommand = new System.Windows.Forms.Timer(this.components);
            this.tmrMessage = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCommunicate
            // 
            this.btnCommunicate.Location = new System.Drawing.Point(131, 76);
            this.btnCommunicate.Name = "btnCommunicate";
            this.btnCommunicate.Size = new System.Drawing.Size(168, 30);
            this.btnCommunicate.TabIndex = 0;
            this.btnCommunicate.Text = "&Verify Device";
            this.btnCommunicate.UseVisualStyleBackColor = true;
            this.btnCommunicate.Click += new System.EventHandler(this.btnCommunicate_Click);
            // 
            // txtwebaddress
            // 
            this.txtwebaddress.Location = new System.Drawing.Point(131, 24);
            this.txtwebaddress.Name = "txtwebaddress";
            this.txtwebaddress.Size = new System.Drawing.Size(168, 20);
            this.txtwebaddress.TabIndex = 1;
            this.txtwebaddress.Text = "http://tyb-pc/cimonitor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Website Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Device Key";
            // 
            // txtDeviceKey
            // 
            this.txtDeviceKey.Location = new System.Drawing.Point(131, 50);
            this.txtDeviceKey.Name = "txtDeviceKey";
            this.txtDeviceKey.Size = new System.Drawing.Size(168, 20);
            this.txtDeviceKey.TabIndex = 3;
            this.txtDeviceKey.Text = "KEY-1-TEST";
            // 
            // tmrCaptureImage
            // 
            this.tmrCaptureImage.Tick += new System.EventHandler(this.tmrCaptureImage_Tick);
            // 
            // tmrSendFiles
            // 
            this.tmrSendFiles.Tick += new System.EventHandler(this.tmrSendFiles_Tick);
            // 
            // tmrCommand
            // 
            this.tmrCommand.Tick += new System.EventHandler(this.tmrCommand_Tick);
            // 
            // tmrMessage
            // 
            this.tmrMessage.Tick += new System.EventHandler(this.tmrMessage_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 31);
            this.button1.TabIndex = 5;
            this.button1.Text = "&Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 306);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDeviceKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtwebaddress);
            this.Controls.Add(this.btnCommunicate);
            this.Name = "Form1";
            this.Opacity = 0D;
            this.Text = "User Activity Monitor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCommunicate;
        private System.Windows.Forms.TextBox txtwebaddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDeviceKey;
        private System.Windows.Forms.Timer tmrCaptureImage;
        private System.Windows.Forms.Timer tmrSendFiles;
        private System.Windows.Forms.Timer tmrCommand;
        private System.Windows.Forms.Timer tmrMessage;
        private System.Windows.Forms.Button button1;
    }
}

