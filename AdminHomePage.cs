using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;


namespace AssessmentPortalApp
{
    public partial class AdminHomePage : Form
    {
        public int userId;
        public string userName;
        public int roleId;

        public AdminHomePage()
        {
            InitializeComponent();
        }

        private void AdminHomePage_Load(object sender, EventArgs e)
        {
            lblMsg.Text = userName;
            timer1.Enabled = true;  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
            panel1.Visible = !panel1.Visible;
        }

        private bool isColored = false;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (isColored)
            {
                panel1.BackColor = SystemColors.Control; 
            }
            else
            {
                panel1.BackColor = Color.LightPink; 
            }
            isColored = !isColored;
        }




        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FirstPage obj = new FirstPage();
            obj.Show();
            this.Hide();
        }

       

        private void lnkStaffManagement_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StaffManagementByAdmin obj = new StaffManagementByAdmin
            {
                userName = userName,
                adminUserId = userId
            };
            obj.Show();
            this.Hide();
        }

        private void lnkSubjectManagement_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SubjectManagementAdmin1 sub = new SubjectManagementAdmin1();
         


            sub.userName = userName;
            sub.userId = userId;
            
            sub.Show();
            this.Hide();

        }

        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePasswordPage1 obj = new ChangePasswordPage1
            {
                userId = userId,
                userName = userName,
                roleId = roleId
            };
            obj.Show();
            this.Hide();

        }

       
    }
    }
