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
    public partial class StudentMangementAdmin : Form
    {
        public int userId;
        public string userName;
       public int roleId;
        public StudentMangementAdmin()
        {
            InitializeComponent();
        }

        private void StudentMangementAdmin_Load(object sender, EventArgs e)
        {
            lblMsg.Text = userName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            AdminHomePage adminHome = new AdminHomePage();
            adminHome.userId = userId;
            adminHome.userName = userName;
            adminHome.roleId = roleId;
            adminHome.Show();
            this.Hide();
        }
    }
}
