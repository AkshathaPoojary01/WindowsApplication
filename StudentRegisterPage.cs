using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AssessmentPortalApp
{
    public partial class StudentRegisterPage : Form
    {
        private string connectionString = "data source=akshu\\sqlexpress; initial catalog=AssessmentPortalDB; user id=sa;password=akshu";
        SqlConnection con;
        public StudentRegisterPage()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
            LoadRoles();
        }

        Regex emailPattern = new Regex(@"^[a-zA-Z0-9._%+-]+@gmail\.com$");
        Regex mblnoPattern = new Regex("^[6-9][0-9]{9}$");

        public bool ValidateFields()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Please enter email address.");
                isValid = false;
            }
            else if (!emailPattern.IsMatch(txtEmail.Text.Trim()))
            {
                errorProvider1.SetError(txtEmail, "Please enter a valid email address.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtEmail, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(txtMblNum.Text))
            {
                errorProvider1.SetError(txtMblNum, "Please enter mobile number.");
                isValid = false;
            }
            else if (!mblnoPattern.IsMatch(txtMblNum.Text.Trim()))
            {
                errorProvider1.SetError(txtMblNum, "Please enter a valid mobile number.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtMblNum, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider1.SetError(txtName, "Please enter name.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtName, string.Empty);
            }

            if (!radioM.Checked && !radioF.Checked)
            {
                errorProvider1.SetError(radioM, "Please select a gender.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(radioM, string.Empty);
            }

            if (cmbRole.SelectedIndex == 0) 
            {
                errorProvider1.SetError(cmbRole, "Please select a role.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(cmbRole, string.Empty);
            }

            if (!checkActv.Checked)
            {
                errorProvider1.SetError(checkActv, "Please check the status box.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(checkActv, string.Empty);
            }

            return isValid;
        }

        private void LoadRoles()
        {
            string query = "SELECT RoleId, RoleName FROM RoleMaster";

            using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
            {
                DataTable rolesTable = new DataTable();
                adapter.Fill(rolesTable);

               
                DataRow defaultRow = rolesTable.NewRow();
                defaultRow["RoleId"] = 0; 
                defaultRow["RoleName"] = "Select Role";
                rolesTable.Rows.InsertAt(defaultRow, 0);

                cmbRole.DataSource = rolesTable;
                cmbRole.DisplayMember = "RoleName";
                cmbRole.ValueMember = "RoleId";

              
                cmbRole.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            string gender = radioM.Checked ? "M" : "F";
            bool isActive = checkActv.Checked;

            int roleId = (int)cmbRole.SelectedValue;

            string query = "INSERT INTO AllUsers (Name, Email, MblNo, Gender, RoleId, StatusId) " +
                           "VALUES (@Name, @Email, @MblNo, @Gender, @RoleId, @StatusId); " +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@MblNo", txtMblNum.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@RoleId", roleId); 
                cmd.Parameters.AddWithValue("@StatusId", isActive);

                try
                {
                    con.Open();

                    int userId = Convert.ToInt32(cmd.ExecuteScalar());
                    lblId.Text = $"{userId}";
                    MessageBox.Show("User data saved successfully.", "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                   
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void ClearFields()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMblNum.Text = string.Empty;

            cmbRole.SelectedIndex = 0; 

            radioM.Checked = false;
            radioF.Checked = false;

            checkActv.Checked = false;

            errorProvider1.Clear();

            lblId.Text = string.Empty;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FirstPage page = new FirstPage();
            page.Show();
            this.Hide();
        }
    }
}
