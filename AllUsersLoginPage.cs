using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


namespace AssessmentPortalApp
{
    public partial class AllUsersLoginPage : Form
    {
        private string connectionString = "data source=akshu\\sqlexpress; initial catalog=AssessmentPortalDB; user id=sa;password=akshu";
        SqlConnection con;
        
        public AllUsersLoginPage()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
        }

        Regex emailPattern = new Regex(@"^[a-zA-Z0-9._%+-]+@gmail\.com$");
        Regex mblnoPattern = new Regex("^[6-9][0-9]{9}$");


       

        public bool ValidateFields()
        {
            bool isValid = true;
            StringBuilder errorMessage = new StringBuilder();

         
            if (txtEmail.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtEmail, "Please enter your email address.");
                errorMessage.AppendLine("Please enter your email address.");
                isValid = false;
            }
            else if (!emailPattern.IsMatch(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Please enter a valid email address.");
                errorMessage.AppendLine("Please enter a valid email address.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtEmail, string.Empty); 
            }

       
            if (txtMblNo.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtMblNo, "Please enter your mobile number.");
                errorMessage.AppendLine("Please enter your mobile number.");
                isValid = false;
            }
            else if (!mblnoPattern.IsMatch(txtMblNo.Text))
            {
                errorProvider1.SetError(txtMblNo, "Please enter a valid mobile number.");
                errorMessage.AppendLine("Please enter a valid mobile number.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtMblNo, string.Empty); 
            }

     
            lblMsg.Text = errorMessage.ToString().Trim();

            return isValid;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            FirstPage obj = new FirstPage();
            obj.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(!ValidateFields())
            {
                return;
            }
            if(ValidateFields())
            {
                
                SqlCommand query = new SqlCommand("select * from AllUsers where email=@email",con);
              
                query.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                if(con.State==ConnectionState.Closed)
                {
                    con.Open();
                }
                 SqlDataReader result =query.ExecuteReader();
                if(result.HasRows)
                {
                    result.Read();
                    int userid = Convert.ToInt32(result["userid"]);
                    string name = result["name"].ToString();
                    string mblno= result["mblno"].ToString();
                    int roleid = Convert.ToInt32(result["roleid"]);
            

                    if (mblno==txtMblNo.Text.Trim())
                    {
                        if(roleid==1)
                        {
                            AdminHomePage admin = new AdminHomePage();
                            admin.userId = userid;
                            admin.userName = name;
                            admin.roleId = roleid;
                           
                            admin.Show();
                            this.Hide();
                        }
                        else if(roleid==2)
                        {
                           AStaffHomePage staff = new AStaffHomePage();
                           staff.userId = userid;
                           staff.userName = name;
                            staff.roleId = roleid;
                           staff.Show();
                           this.Hide();
                        }
                        else if(roleid==3)
                        {
                            AStudentHomePage student = new AStudentHomePage();
                            student.userId = userid;
                            student.userName = name;
                            student.roleId = roleid;
                            student.Show();
                            this.Hide();
                        }
                        ChangePasswordPage changePasswordPage = new ChangePasswordPage();
                        changePasswordPage.userId = userid;
                        changePasswordPage.userName = name;
                        changePasswordPage.roleId = roleid;

                        StaffManagementByAdmin obj = new StaffManagementByAdmin();
                        obj.adminUserId = userid;
                        obj.userName = name;
                        obj.roleId = roleid;


                       



                    }
                    else
                    {
                        lblMsg.Text = "email or mobile number does not exist in the server";
                    }
                }
         

            }
             if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

        }

        private void btnGoToAdminLogin_Click(object sender, EventArgs e)
        {
            AdminHomePage admin = new AdminHomePage();
            admin.Show();
            this.Hide();
        }
    }
}
