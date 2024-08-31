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
    public partial class AStaffHomePage : Form
    {
        public int userId;
        public string userName;
        public int roleId;

        public AStaffHomePage()
        {
            InitializeComponent();
        }

        private void AStaffHomePage_Load(object sender, EventArgs e)
        {
            lblMsg.Text = userName;
        }

        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePasswordPage1 staffChangePassword_obj = new ChangePasswordPage1();
           

            staffChangePassword_obj.Show();
            this.Hide();
        }

        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
                FirstPage obj = new FirstPage();
                obj.Show();
                this.Hide();
            
        }

        private void lnkQueManagement_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            QuestionManagement que = new QuestionManagement();
            que.userId = userId;
            que.userName = userName;
            que.roleId = roleId;
            que.Show();
            this.Hide();

        }
    }
}
