using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssessmentPortalApp
{
   
    public partial class AStudentHomePage : Form
    {
        public int userId;
        public string userName;
        public int roleId;

        public AStudentHomePage()
        {
            InitializeComponent();
        }

        private void AStudentHomePage_Load(object sender, EventArgs e)
        {
            lblMsg.Text = userName;
        }

        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePasswordPage1 studentChangePassword_obj = new ChangePasswordPage1();
           


            studentChangePassword_obj.Show();
            this.Hide();
        }

        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FirstPage obj = new FirstPage();
            obj.Show();
            this.Hide();

        }

        private void lnkTakeExam_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExamInstructions exam = new ExamInstructions();
         
            exam.userName = userName;
            exam.roleId = roleId;
            exam.userId = userId;
            exam.Show();
            this.Hide();



        }

        private void viewResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ViewResults view = new ViewResults();
             view.userId = userId;
            view.roleId = roleId;
            view.userName = userName;
            view.Show();
            this.Hide();


        }
    }
}
