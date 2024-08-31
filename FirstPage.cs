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
    public partial class FirstPage : Form
    {
        public FirstPage()
        {
            InitializeComponent();
           
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            AllUsersLoginPage1 obj = new AllUsersLoginPage1();
            obj.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            StudentRegisterPage studreg = new StudentRegisterPage();
            studreg.Show();
            this.Hide();
        }

       
    }
}
