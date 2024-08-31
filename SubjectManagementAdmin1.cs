using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AssessmentPortalApp
{
    public partial class SubjectManagementAdmin1 : Form
    {
        private readonly SqlConnection con;
        public int userId;
        public string userName;
        public int roleId;
        private int action = 1;
        private int idToBeEdited = 0;

        public SubjectManagementAdmin1()
        {
            InitializeComponent();
            con = new SqlConnection("data source=akshu\\sqlexpress; initial catalog=AssessmentPortalDB; user id=sa;password=akshu");
            con.Open();
        }

        private void SubjectManagementAdmin1_Load(object sender, EventArgs e)
        {
            lblMsg.Text = userName;
            BindGrid();             
            BindGridAllocation(); 
            LoadStaff();
            LoadSubject();
         
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            this.cmbSelectStaff.SelectedIndexChanged += new System.EventHandler(this.cmbSelectStaff_SelectedIndexChanged);



        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            idToBeEdited = 0;
            action = 1;
            txtSubject.Text = "";
            txtSubject.Enabled = true;
            btnSave.Enabled = true;
            lblMsg.Text = "";
            btnSave.Text = "Save";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                MessageBox.Show("Subject name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlCommand cmdSave = new SqlCommand())
            {
                cmdSave.Connection = con;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM SubjectMaster WHERE subjectname = @subjectname AND SubjectId != @subjectid", con);
                cmdCheck.Parameters.AddWithValue("@subjectname", txtSubject.Text);
                cmdCheck.Parameters.AddWithValue("@subjectid", idToBeEdited);

                int count = (int)cmdCheck.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show($"Subject '{txtSubject.Text}' already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (action == 1)
                {
                    cmdSave.CommandText = "INSERT INTO SubjectMaster(subjectname) VALUES(@subjectname)";
                }
                else if (action == 2)
                {
                    cmdSave.CommandText = "UPDATE SubjectMaster SET subjectname = @subjectname WHERE SubjectId = @subjectid";
                    cmdSave.Parameters.AddWithValue("@subjectid", idToBeEdited);
                }

                cmdSave.Parameters.AddWithValue("@subjectname", txtSubject.Text);

                try
                {
                    int rowsAffected = cmdSave.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show(action == 1
                            ? $"Subject '{txtSubject.Text}' added successfully!"
                            : $"Subject '{txtSubject.Text}' updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        idToBeEdited = 0;
                        txtSubject.Text = "";
                        btnSave.Text = "Save";

                        LoadSubject(); 
                        BindGrid();     
                    }
                    else
                    {
                        MessageBox.Show("No changes were made.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        private void LoadStaff()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT userid, Name FROM AllUsers WHERE roleid = 2", con);
            DataTable dtStaff = new DataTable();
            da.Fill(dtStaff);
            DataRow r = dtStaff.NewRow();
            r[0] = 0;
            r[1] = "Select a staff";
            dtStaff.Rows.InsertAt(r, 0);
            cmbSelectStaff.DataSource = dtStaff;
            cmbSelectStaff.DisplayMember = "Name";
            cmbSelectStaff.ValueMember = "userid";
        }

        private void LoadSubject()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT subjectid, subjectname FROM SubjectMaster", con);
                DataTable dtSubject = new DataTable();
                da.Fill(dtSubject);

                DataRow r = dtSubject.NewRow();
                r[0] = 0;
                r[1] = "Select a subject";
                dtSubject.Rows.InsertAt(r, 0);

                cmbSelectSubject.DataSource = dtSubject;
                cmbSelectSubject.DisplayMember = "subjectname";
                cmbSelectSubject.ValueMember = "subjectid";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            idToBeEdited = 0;
            btnSave.Text = "Save";
            txtSubject.Text = "";
            txtSubject.Enabled = true;
            lblMsg.Text = "";
            BindGrid();
        }

        private void BindGrid()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand cmd = new SqlCommand("SELECT SubjectId, SubjectName FROM SubjectMaster", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;

            con.Close();
        }

        private void BindGridAllocation()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT a.StaffId, u.Name AS StaffName, s.SubjectName, u2.Name AS AllocatedByName, a.AllocatedOn " +
                "FROM StaffSubjectAllocation a " +
                "JOIN SubjectMaster s ON a.SubjectId = s.SubjectId " +
                "JOIN AllUsers u ON a.StaffId = u.UserId " +
                "JOIN AllUsers u2 ON a.AllocatedBy = u2.UserId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView2.DataSource = dt;

          
            dataGridView2.Columns[0].HeaderText = "Staff ID";
            dataGridView2.Columns[1].HeaderText = "Staff Name";
            dataGridView2.Columns[2].HeaderText = "Subject Name";
            dataGridView2.Columns[3].HeaderText = "Allocated By";
            dataGridView2.Columns[4].HeaderText = "Allocated Date";

            con.Close();
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowClicked = e.RowIndex;

            if (rowClicked >= 0 && rowClicked < dataGridView1.Rows.Count)
            {
                idToBeEdited = Convert.ToInt32(dataGridView1.Rows[rowClicked].Cells[0].Value);
                txtSubject.Text = dataGridView1.Rows[rowClicked].Cells[1].Value.ToString();
                action = 2;
                btnSave.Text = "Update";
            }
        }

        private void cmbSelectStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelectStaff.SelectedValue == null || (int)cmbSelectStaff.SelectedValue == 0)
                return;

            int staffId = Convert.ToInt32(cmbSelectStaff.SelectedValue);

            if (con.State == ConnectionState.Closed)
                con.Open();

           
            SqlCommand cmdCheckAllocation = new SqlCommand(
                "SELECT COUNT(*) FROM StaffSubjectAllocation WHERE StaffId = @staffId", con);
            cmdCheckAllocation.Parameters.AddWithValue("@staffId", staffId);

            int allocatedSubjectCount = (int)cmdCheckAllocation.ExecuteScalar();

            if (allocatedSubjectCount > 0)
            {
                cmbSelectSubject.Visible = false;
                cmbSelectSubject.Refresh();
                MessageBox.Show("selected staff member is already alloted to a subject."); 
                return;
            }
            else
            {
                cmbSelectSubject.Visible = true;
                cmbSelectSubject.Refresh();
            }

           
            string query = "SELECT subjectid, subjectname FROM SubjectMaster";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataRow newRow = dt.NewRow();
            newRow["subjectid"] = 0;
            newRow["subjectname"] = "Select a subject";
            dt.Rows.InsertAt(newRow, 0);

            cmbSelectSubject.DataSource = dt;
            cmbSelectSubject.DisplayMember = "subjectname";
            cmbSelectSubject.ValueMember = "subjectid";

            if (con.State == ConnectionState.Open)
                con.Close();
        }


        private void btnAllot_Click(object sender, EventArgs e)
        {
            if (cmbSelectStaff.SelectedValue == null || cmbSelectSubject.SelectedValue == null ||
                (int)cmbSelectSubject.SelectedValue == 0)
            {
                MessageBox.Show("Please select both Staff and Subject.");
                return;
            }

            int staffId = Convert.ToInt32(cmbSelectStaff.SelectedValue);
            int subjectId = Convert.ToInt32(cmbSelectSubject.SelectedValue);

         
            MessageBox.Show($"Staff ID: {staffId}, Subject ID: {subjectId}, Allocated By: {userId}");

            if (con.State == ConnectionState.Closed)
                con.Open();

         
            SqlCommand cmdCheckAssignment = new SqlCommand(
                "SELECT COUNT(*) FROM StaffSubjectAllocation WHERE StaffId = @staffId", con);
            cmdCheckAssignment.Parameters.AddWithValue("@staffId", staffId);

            int assignedSubjectCount = (int)cmdCheckAssignment.ExecuteScalar();

            if (assignedSubjectCount > 0)
            {
                MessageBox.Show("The selected staff member is already alloted to a subject.");
                return;
            }

            
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO StaffSubjectAllocation (StaffId, SubjectId, AllocatedBy, AllocatedOn) VALUES (@staffId, @subjectId, @allocatedBy, @allocatedOn)", con);

            cmd.Parameters.AddWithValue("@staffId", staffId);
            cmd.Parameters.AddWithValue("@subjectId", subjectId);
            cmd.Parameters.AddWithValue("@allocatedBy", userId);
            cmd.Parameters.AddWithValue("@allocatedOn", DateTime.Now);

            try
            {
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Subject allocated successfully!");

                  
                    cmbSelectStaff.SelectedIndex = 0;

                    LoadSubject(); 
                    BindGridAllocation(); 
                }
                else
                {
                    MessageBox.Show("Failed to allocate the subject.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FirstPage page = new FirstPage();
            page.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AdminHomePage adminHome = new AdminHomePage
            {
                userId = userId,
                userName = userName,
                roleId = roleId
            };
            adminHome.Show();
        }

     

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox2.BringToFront();
            pictureBox2.Visible = true;

            idToBeEdited = 0;      
            action = 1;             
            txtSubject.Text = "";  
            txtSubject.Enabled = true;  
            btnSave.Text = "Save";  
                 

            LoadSubject();         
            BindGrid();
        }
    }
}
