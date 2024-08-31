
namespace AssessmentPortalApp
{
    partial class AStudentHomePage
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
            this.lnkTakeExam = new System.Windows.Forms.LinkLabel();
            this.lnkChangePassword = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkLogout = new System.Windows.Forms.LinkLabel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkTakeExam
            // 
            this.lnkTakeExam.AutoSize = true;
            this.lnkTakeExam.BackColor = System.Drawing.Color.Transparent;
            this.lnkTakeExam.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkTakeExam.Location = new System.Drawing.Point(382, 220);
            this.lnkTakeExam.Name = "lnkTakeExam";
            this.lnkTakeExam.Size = new System.Drawing.Size(82, 20);
            this.lnkTakeExam.TabIndex = 6;
            this.lnkTakeExam.TabStop = true;
            this.lnkTakeExam.Text = "Take Exam";
            this.lnkTakeExam.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTakeExam_LinkClicked);
            // 
            // lnkChangePassword
            // 
            this.lnkChangePassword.AutoSize = true;
            this.lnkChangePassword.BackColor = System.Drawing.Color.Transparent;
            this.lnkChangePassword.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkChangePassword.Location = new System.Drawing.Point(359, 161);
            this.lnkChangePassword.Name = "lnkChangePassword";
            this.lnkChangePassword.Size = new System.Drawing.Size(132, 20);
            this.lnkChangePassword.TabIndex = 4;
            this.lnkChangePassword.TabStop = true;
            this.lnkChangePassword.Text = "Change Password";
            this.lnkChangePassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangePassword_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.BackgroundImage = global::AssessmentPortalApp.Properties.Resources._2377060;
            this.groupBox1.Controls.Add(this.lnkLogout);
            this.groupBox1.Controls.Add(this.lblMsg);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(17, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(921, 98);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // lnkLogout
            // 
            this.lnkLogout.AutoSize = true;
            this.lnkLogout.LinkColor = System.Drawing.Color.Maroon;
            this.lnkLogout.Location = new System.Drawing.Point(827, 30);
            this.lnkLogout.Name = "lnkLogout";
            this.lnkLogout.Size = new System.Drawing.Size(59, 20);
            this.lnkLogout.TabIndex = 3;
            this.lnkLogout.TabStop = true;
            this.lnkLogout.Text = "Logout";
            this.lnkLogout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLogout_LinkClicked);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(89, 30);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(51, 20);
            this.lblMsg.TabIndex = 2;
            this.lblMsg.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome:";
            // 
            // AStudentHomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AssessmentPortalApp.Properties.Resources._2377060;
            this.ClientSize = new System.Drawing.Size(956, 527);
            this.Controls.Add(this.lnkTakeExam);
            this.Controls.Add(this.lnkChangePassword);
            this.Controls.Add(this.groupBox1);
            this.Name = "AStudentHomePage";
            this.Text = "AStudentHomePage";
            this.Load += new System.EventHandler(this.AStudentHomePage_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel lnkTakeExam;
        private System.Windows.Forms.LinkLabel lnkChangePassword;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel lnkLogout;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label label1;
    }
}