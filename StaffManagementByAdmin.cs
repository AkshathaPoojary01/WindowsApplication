using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AssessmentPortalApp
{
    public partial class StaffManagementByAdmin : Form
    {
        private string connectionString = "data source=akshu\\sqlexpress; initial catalog=AssessmentPortalDB; user id=sa;password=akshu";
        SqlConnection con;
        public int adminUserId; 
        public string userName;
        public int roleId; 

        public StaffManagementByAdmin()
        {
            InitializeComponent();
            con = new SqlConnection(connectionString);
        }

        private void StaffManagementByAdmin_Load(object sender, EventArgs e)
        {
            lblMsg.Text = userName;
            lblId.Text = "0"; 
            PopulateRoleComboBox();
            LoadStaff();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }

        private void LoadStaff()
        {
            string query = "SELECT userid, name, mblno, email, gender, roleid, statusid FROM AllUsers WHERE roleid = 2";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void PopulateRoleComboBox()
        {
            string query = "SELECT roleId, roleName FROM RoleMaster";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                DataTable dt = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }

                DataRow defaultRow = dt.NewRow();
                defaultRow["roleName"] = "Select Role";
                dt.Rows.InsertAt(defaultRow, 0);

                cmbRole.DataSource = dt;
                cmbRole.DisplayMember = "roleName";
                cmbRole.ValueMember = "roleId";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            string name = txtName.Text.Trim();
            string mobileNo = txtMblNum.Text.Trim();
            string email = txtEmail.Text.Trim();
            string gender = radioM.Checked ? "M" : "F";
            int selectedRoleId = (int)cmbRole.SelectedValue;
            bool isActive = checkActv.Checked;

            
            if (selectedRoleId != 2)
            {
                MessageBox.Show("You can only add or update staff members.");
                return;
            }

            string query;
            int staffUserId = Convert.ToInt32(lblId.Text); 

            if (staffUserId == 0)  
            {
                query = "INSERT INTO AllUsers (name, mblno, email, gender, roleid, statusid) " +
                        "VALUES (@name, @mblno, @email, @gender, @roleid, @statusid); " +
                        "SELECT SCOPE_IDENTITY();";
            }
            else  
            {
                query = "UPDATE AllUsers SET name = @name, mblno = @mblno, email = @email, " +
                        "gender = @gender, roleid = @roleid, statusid = @statusid " +
                        "WHERE userid = @userid";
            }

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@mblno", mobileNo);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@roleid", selectedRoleId);
                cmd.Parameters.AddWithValue("@statusid", isActive);

                if (staffUserId != 0)
                {
                    cmd.Parameters.AddWithValue("@userid", staffUserId);
                }

                try
                {
                    con.Open();
                    if (staffUserId == 0)
                    {
                        int newUserId = Convert.ToInt32(cmd.ExecuteScalar());
                        lblId.Text = $"{newUserId}";
                        MessageBox.Show("New staff created successfully.");
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Staff details updated successfully.");
                    }

                    LoadStaff();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchName))
            {
                LoadStaff();
                return;
            }

            string query = "SELECT userid, name, mblno, email, gender, roleid, statusid FROM AllUsers WHERE name LIKE @searchName AND roleid = 2";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@searchName", searchName + "%");

                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                int staffUserId = Convert.ToInt32(row.Cells["userid"].Value);
                txtName.Text = row.Cells["name"].Value.ToString();
                txtMblNum.Text = row.Cells["mblno"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                if (row.Cells["gender"].Value.ToString() == "M")
                {
                    radioM.Checked = true;
                }
                else
                {
                    radioF.Checked = true;
                }

                cmbRole.SelectedValue = Convert.ToInt32(row.Cells["roleid"].Value);
                checkActv.Checked = Convert.ToBoolean(row.Cells["statusid"].Value);
                lblId.Text = $"{staffUserId}";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

       public void ClearFields()
        {
            txtName.Clear();
            txtMblNum.Clear();
            txtEmail.Clear();
            radioM.Checked = false;
            radioF.Checked = false;
            cmbRole.SelectedIndex = 0;
            checkActv.Checked = false;
            lblId.Text = "0"; 
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter the name.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtMblNum.Text) || !Regex.IsMatch(txtMblNum.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Please enter a valid 10-digit mobile number.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.");
                return false;
            }
            if (cmbRole.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a role.");
                return false;
            }
            if (!radioM.Checked && !radioF.Checked)
            {
                MessageBox.Show("Please select a gender.");
                return false;
            }
            return true;
        }

       

        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FirstPage page = new FirstPage();
            page.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AdminHomePage obj = new AdminHomePage
            {
                userId = adminUserId,
                userName = userName,
                roleId = roleId
            };
            obj.Show();
            this.Hide();
        }

        private void lnkLogout_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FirstPage page = new FirstPage();
            page.Show();
            this.Hide();
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ClearFields();
            txtSearch.Clear();
        }
    }
}
