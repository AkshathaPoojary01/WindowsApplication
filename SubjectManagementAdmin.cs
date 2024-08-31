using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using AssessmentPortalApp;

namespace StudentPortal
{
    public partial class SubjectManagementAdmin : Form
    {
        private readonly SqlConnection con;
        public int userId;
        public string userName;
        public int roleId;
        private int action = 1;
        private int idToBeEdited = 0;

        public SubjectManagementAdmin()
        {
            InitializeComponent();

            con = new SqlConnection("data source=akshu\\sqlexpress; initial catalog=AssessmentPortalDB; user id=sa;password=akshu");
            con.Open();
        }

        private void SubjectManagementAdmin_Load(object sender, EventArgs e)
        {
            lblMsg.Text = userName;
            BindGrid();
            LoadStaff();
            LoadSubject(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlCommand cmdSave = new SqlCommand())
            {
                cmdSave.Connection = con;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                if (action == 1)
                {
                    cmdSave.CommandText = "INSERT INTO SubjectMaster(subjectname) VALUES(@subjectname)";
                }
                else if (action == 2)
                {
                    cmdSave.CommandText = "UPDATE SubjectMaster SET subjectname=@subjectname WHERE SubjectId=@subjectid";
                }

                cmdSave.Parameters.AddWithValue("@subjectid", idToBeEdited);
                cmdSave.Parameters.AddWithValue("@subjectname", txtSubject.Text);

                try
                {
                    if (cmdSave.ExecuteNonQuery() > 0)
                    {
                        lblMsg.Text = action == 1 ? $"Subject added: {txtSubject.Text}" : "Subject details updated";
                        lblMsg.ForeColor = Color.Green;
                        btnSave.Enabled = false;
                        BindGrid();
                    }
                    else
                    {
                        lblMsg.Text = $"Subject could not be updated: {txtSubject.Text}";
                        lblMsg.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = $"Error: {ex.Message}";
                    lblMsg.ForeColor = Color.Red;
                }
            }
        }

        private void LoadStaff()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT userid, Name FROM AllUsers WHERE roleid=2", con);
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
                "SELECT s.SubjectId, s.SubjectName, a.AllocatedBy, u.Name AS StaffName FROM SubjectMaster s " +
                "JOIN StaffSubjectAllocation a ON s.SubjectId = a.SubjectId " +
                "JOIN AllUsers u ON u.UserId = a.StaffId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView2.DataSource = dt;

            con.Close();
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowClicked = e.RowIndex;

            if (rowClicked >= 0 && rowClicked < dataGridView1.Rows.Count - 1) 
            {
                idToBeEdited = Convert.ToInt32(dataGridView1.Rows[rowClicked].Cells[0].Value);
                txtSubject.Text = dataGridView1.Rows[rowClicked].Cells[1].Value.ToString();
                action = 2;
                btnSave.Text = "Update";
            }
        }

        private void cmbSelectStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand cmd = new SqlCommand("SELECT subjectid, subjectname FROM SubjectMaster WHERE SubjectId NOT IN (SELECT SubjectId FROM StaffSubjectAllocation WHERE StaffId = @staffId)", con);

            try
            {
                cmd.Parameters.AddWithValue("@staffId", Convert.ToInt32(cmbSelectStaff.SelectedValue));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow r = dt.NewRow();
                r[0] = 0;
                r[1] = "Select a subject";
                dt.Rows.InsertAt(r, 0);

                cmbSelectSubject.DataSource = dt;
                cmbSelectSubject.DisplayMember = "subjectname";
                cmbSelectSubject.ValueMember = "subjectid";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private void btnAllot_Click(object sender, EventArgs e)
        {
            
            if (cmbSelectStaff.SelectedValue == null || cmbSelectSubject.SelectedValue == null || 
                (int)cmbSelectSubject.SelectedValue == 0) 
            {
                MessageBox.Show("Please select both Staff and Subject.");
                return;
            }

            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand cmd = new SqlCommand(
                "INSERT INTO StaffSubjectAllocation (StaffId, SubjectId, AllocatedBy) VALUES (@staffId, @subjectId, @allocatedBy)", con);

            cmd.Parameters.AddWithValue("@staffId", Convert.ToInt32(cmbSelectStaff.SelectedValue));
            cmd.Parameters.AddWithValue("@subjectId", Convert.ToInt32(cmbSelectSubject.SelectedValue));
            cmd.Parameters.AddWithValue("@allocatedBy", userId);

            try
            {
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Subject allocated successfully!");
                    cmbSelectSubject.DataSource = null;
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
                con.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AdminHomePage adminHome = new AdminHomePage();
            adminHome.Show();
            this.Hide();
        }
    }
}
