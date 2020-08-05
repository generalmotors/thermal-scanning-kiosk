namespace ThermalScanningKiosk
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
            this.pbWebCam = new System.Windows.Forms.PictureBox();
            this.pnlBanner = new System.Windows.Forms.Panel();
            this.pnlDebug = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOCVCertainty = new System.Windows.Forms.TextBox();
            this.txtAD = new System.Windows.Forms.TextBox();
            this.txtH = new System.Windows.Forms.TextBox();
            this.txtW = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.pbLogo1 = new System.Windows.Forms.PictureBox();
            this.pbLogo2 = new System.Windows.Forms.PictureBox();
            this.pbLogo3 = new System.Windows.Forms.PictureBox();
            this.pbLogo4 = new System.Windows.Forms.PictureBox();
            this.lblFooter = new System.Windows.Forms.Label();
            this.pbInfoGraphics = new System.Windows.Forms.PictureBox();
            this.lblVer = new System.Windows.Forms.Label();
            this.lblHostName = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.pnlNotification = new System.Windows.Forms.Panel();
            this.lblNoticeTimer = new System.Windows.Forms.Label();
            this.lblCameraNotice = new System.Windows.Forms.Label();
            this.lblNoticeTxt2 = new System.Windows.Forms.Label();
            this.lblNoticeTxt = new System.Windows.Forms.Label();
            this.lblNoticeHeading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbWebCam)).BeginInit();
            this.pnlBanner.SuspendLayout();
            this.pnlDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfoGraphics)).BeginInit();
            this.pnlNotification.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbWebCam
            // 
            this.pbWebCam.Location = new System.Drawing.Point(12, 141);
            this.pbWebCam.Name = "pbWebCam";
            this.pbWebCam.Size = new System.Drawing.Size(720, 540);
            this.pbWebCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbWebCam.TabIndex = 0;
            this.pbWebCam.TabStop = false;
            // 
            // pnlBanner
            // 
            this.pnlBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(157)))));
            this.pnlBanner.Controls.Add(this.pnlDebug);
            this.pnlBanner.Controls.Add(this.lblInstructions);
            this.pnlBanner.Location = new System.Drawing.Point(12, 78);
            this.pnlBanner.Name = "pnlBanner";
            this.pnlBanner.Size = new System.Drawing.Size(1469, 55);
            this.pnlBanner.TabIndex = 1;
            // 
            // pnlDebug
            // 
            this.pnlDebug.Controls.Add(this.label6);
            this.pnlDebug.Controls.Add(this.txtOCVCertainty);
            this.pnlDebug.Controls.Add(this.txtAD);
            this.pnlDebug.Controls.Add(this.txtH);
            this.pnlDebug.Controls.Add(this.txtW);
            this.pnlDebug.Controls.Add(this.txtY);
            this.pnlDebug.Controls.Add(this.txtX);
            this.pnlDebug.Controls.Add(this.btnSave);
            this.pnlDebug.Controls.Add(this.label5);
            this.pnlDebug.Controls.Add(this.label4);
            this.pnlDebug.Controls.Add(this.label3);
            this.pnlDebug.Controls.Add(this.label2);
            this.pnlDebug.Controls.Add(this.label1);
            this.pnlDebug.Location = new System.Drawing.Point(858, 0);
            this.pnlDebug.Name = "pnlDebug";
            this.pnlDebug.Size = new System.Drawing.Size(611, 55);
            this.pnlDebug.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(3, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "OCV Confidence";
            // 
            // txtOCVCertainty
            // 
            this.txtOCVCertainty.Location = new System.Drawing.Point(3, 28);
            this.txtOCVCertainty.Name = "txtOCVCertainty";
            this.txtOCVCertainty.Size = new System.Drawing.Size(86, 20);
            this.txtOCVCertainty.TabIndex = 11;
            // 
            // txtAD
            // 
            this.txtAD.Location = new System.Drawing.Point(428, 27);
            this.txtAD.Name = "txtAD";
            this.txtAD.Size = new System.Drawing.Size(70, 20);
            this.txtAD.TabIndex = 10;
            // 
            // txtH
            // 
            this.txtH.Location = new System.Drawing.Point(352, 27);
            this.txtH.Name = "txtH";
            this.txtH.Size = new System.Drawing.Size(70, 20);
            this.txtH.TabIndex = 9;
            // 
            // txtW
            // 
            this.txtW.Location = new System.Drawing.Point(276, 27);
            this.txtW.Name = "txtW";
            this.txtW.Size = new System.Drawing.Size(70, 20);
            this.txtW.TabIndex = 8;
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(200, 27);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(70, 20);
            this.txtY.TabIndex = 7;
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(124, 27);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(70, 20);
            this.txtX.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(529, 25);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(425, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Angle Distance";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(349, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(273, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(197, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y Offset";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(121, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X Offset";
            // 
            // lblInstructions
            // 
            this.lblInstructions.CausesValidation = false;
            this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 33.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstructions.ForeColor = System.Drawing.Color.White;
            this.lblInstructions.Location = new System.Drawing.Point(0, 0);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(950, 52);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = "lblInstructions";
            // 
            // pbLogo1
            // 
            this.pbLogo1.Location = new System.Drawing.Point(12, 12);
            this.pbLogo1.Name = "pbLogo1";
            this.pbLogo1.Size = new System.Drawing.Size(690, 60);
            this.pbLogo1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo1.TabIndex = 2;
            this.pbLogo1.TabStop = false;
            // 
            // pbLogo2
            // 
            this.pbLogo2.Location = new System.Drawing.Point(1288, 12);
            this.pbLogo2.Name = "pbLogo2";
            this.pbLogo2.Size = new System.Drawing.Size(193, 60);
            this.pbLogo2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo2.TabIndex = 3;
            this.pbLogo2.TabStop = false;
            this.pbLogo2.Click += new System.EventHandler(this.pbLogo2_Click);
            // 
            // pbLogo3
            // 
            this.pbLogo3.Location = new System.Drawing.Point(1231, 742);
            this.pbLogo3.Name = "pbLogo3";
            this.pbLogo3.Size = new System.Drawing.Size(250, 75);
            this.pbLogo3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo3.TabIndex = 4;
            this.pbLogo3.TabStop = false;
            // 
            // pbLogo4
            // 
            this.pbLogo4.InitialImage = null;
            this.pbLogo4.Location = new System.Drawing.Point(402, 687);
            this.pbLogo4.Name = "pbLogo4";
            this.pbLogo4.Size = new System.Drawing.Size(675, 75);
            this.pbLogo4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo4.TabIndex = 5;
            this.pbLogo4.TabStop = false;
            // 
            // lblFooter
            // 
            this.lblFooter.AutoSize = true;
            this.lblFooter.Font = new System.Drawing.Font("Microsoft Sans Serif", 33.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(157)))));
            this.lblFooter.Location = new System.Drawing.Point(393, 765);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(207, 52);
            this.lblFooter.TabIndex = 6;
            this.lblFooter.Text = "lblFooter";
            // 
            // pbInfoGraphics
            // 
            this.pbInfoGraphics.Location = new System.Drawing.Point(761, 141);
            this.pbInfoGraphics.Name = "pbInfoGraphics";
            this.pbInfoGraphics.Size = new System.Drawing.Size(720, 540);
            this.pbInfoGraphics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbInfoGraphics.TabIndex = 7;
            this.pbInfoGraphics.TabStop = false;
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVer.ForeColor = System.Drawing.Color.Gray;
            this.lblVer.Location = new System.Drawing.Point(12, 865);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(63, 15);
            this.lblVer.TabIndex = 8;
            this.lblVer.Text = "Version: ";
            // 
            // lblHostName
            // 
            this.lblHostName.AutoSize = true;
            this.lblHostName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHostName.ForeColor = System.Drawing.Color.Gray;
            this.lblHostName.Location = new System.Drawing.Point(12, 887);
            this.lblHostName.Name = "lblHostName";
            this.lblHostName.Size = new System.Drawing.Size(76, 15);
            this.lblHostName.TabIndex = 9;
            this.lblHostName.Text = "Hostname:";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIP.ForeColor = System.Drawing.Color.Gray;
            this.lblIP.Location = new System.Drawing.Point(12, 910);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(28, 15);
            this.lblIP.TabIndex = 10;
            this.lblIP.Text = "IP: ";
            // 
            // pnlNotification
            // 
            this.pnlNotification.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlNotification.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNotification.CausesValidation = false;
            this.pnlNotification.Controls.Add(this.lblNoticeTimer);
            this.pnlNotification.Controls.Add(this.lblCameraNotice);
            this.pnlNotification.Controls.Add(this.lblNoticeTxt2);
            this.pnlNotification.Controls.Add(this.lblNoticeTxt);
            this.pnlNotification.Controls.Add(this.lblNoticeHeading);
            this.pnlNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlNotification.Location = new System.Drawing.Point(376, 141);
            this.pnlNotification.Name = "pnlNotification";
            this.pnlNotification.Size = new System.Drawing.Size(720, 540);
            this.pnlNotification.TabIndex = 11;
            // 
            // lblNoticeTimer
            // 
            this.lblNoticeTimer.AutoSize = true;
            this.lblNoticeTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoticeTimer.Location = new System.Drawing.Point(676, 503);
            this.lblNoticeTimer.Name = "lblNoticeTimer";
            this.lblNoticeTimer.Size = new System.Drawing.Size(27, 20);
            this.lblNoticeTimer.TabIndex = 4;
            this.lblNoticeTimer.Text = "15";
            // 
            // lblCameraNotice
            // 
            this.lblCameraNotice.AutoSize = true;
            this.lblCameraNotice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCameraNotice.Location = new System.Drawing.Point(24, 74);
            this.lblCameraNotice.Name = "lblCameraNotice";
            this.lblCameraNotice.Size = new System.Drawing.Size(128, 20);
            this.lblCameraNotice.TabIndex = 3;
            this.lblCameraNotice.Text = "CameraNotice";
            // 
            // lblNoticeTxt2
            // 
            this.lblNoticeTxt2.AutoSize = true;
            this.lblNoticeTxt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoticeTxt2.Location = new System.Drawing.Point(72, 252);
            this.lblNoticeTxt2.Name = "lblNoticeTxt2";
            this.lblNoticeTxt2.Size = new System.Drawing.Size(70, 20);
            this.lblNoticeTxt2.TabIndex = 2;
            this.lblNoticeTxt2.Text = "Notice2";
            // 
            // lblNoticeTxt
            // 
            this.lblNoticeTxt.AutoSize = true;
            this.lblNoticeTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoticeTxt.Location = new System.Drawing.Point(74, 141);
            this.lblNoticeTxt.Name = "lblNoticeTxt";
            this.lblNoticeTxt.Size = new System.Drawing.Size(70, 20);
            this.lblNoticeTxt.TabIndex = 1;
            this.lblNoticeTxt.Text = "Notice1";
            // 
            // lblNoticeHeading
            // 
            this.lblNoticeHeading.AutoSize = true;
            this.lblNoticeHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoticeHeading.ForeColor = System.Drawing.Color.Firebrick;
            this.lblNoticeHeading.Location = new System.Drawing.Point(24, 22);
            this.lblNoticeHeading.Name = "lblNoticeHeading";
            this.lblNoticeHeading.Size = new System.Drawing.Size(96, 29);
            this.lblNoticeHeading.TabIndex = 0;
            this.lblNoticeHeading.Text = "Notice:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1604, 1054);
            this.Controls.Add(this.pnlNotification);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblHostName);
            this.Controls.Add(this.lblVer);
            this.Controls.Add(this.pbInfoGraphics);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.pbLogo4);
            this.Controls.Add(this.pbLogo3);
            this.Controls.Add(this.pbLogo2);
            this.Controls.Add(this.pbLogo1);
            this.Controls.Add(this.pnlBanner);
            this.Controls.Add(this.pbWebCam);
            this.Name = "Form1";
            this.Text = "Thermal Scanning Kiosk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbWebCam)).EndInit();
            this.pnlBanner.ResumeLayout(false);
            this.pnlDebug.ResumeLayout(false);
            this.pnlDebug.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfoGraphics)).EndInit();
            this.pnlNotification.ResumeLayout(false);
            this.pnlNotification.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWebCam;
        private System.Windows.Forms.Panel pnlBanner;
        private System.Windows.Forms.PictureBox pbLogo1;
        private System.Windows.Forms.PictureBox pbLogo2;
        private System.Windows.Forms.PictureBox pbLogo3;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.PictureBox pbLogo4;
        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.PictureBox pbInfoGraphics;
        private System.Windows.Forms.Panel pnlDebug;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAD;
        private System.Windows.Forms.TextBox txtH;
        private System.Windows.Forms.TextBox txtW;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOCVCertainty;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.Label lblHostName;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Panel pnlNotification;
        private System.Windows.Forms.Label lblNoticeTxt;
        private System.Windows.Forms.Label lblNoticeHeading;
        private System.Windows.Forms.Label lblNoticeTxt2;
        private System.Windows.Forms.Label lblCameraNotice;
        private System.Windows.Forms.Label lblNoticeTimer;
    }
}

