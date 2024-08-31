using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AssessmentPortalApp
{
    public partial class QuestionManagement : Form
    {
        public int userId;
        public string userName;
        public int roleId;
        private string connectionString = "data source=akshu\\sqlexpress; initial catalog=AssessmentPortalDB; user id=sa;password=akshu";
        private ErrorProvider errorProvider = new ErrorProvider();
        private int selectedQuestionId; 

        public QuestionManagement()
        {
            InitializeComponent();
        }

        private void QuestionManagement_Load(object sender, EventArgs e)
        {
            PopulateSubjects();
            lblMsg.Text = userName;
            LoadQuestions();
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

        }

        private void PopulateSubjects()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT s.subjectid, sm.subjectname
                FROM StaffSubjectAllocation s
                INNER JOIN AllUsers u ON s.staffid = u.userid
                INNER JOIN SubjectMaster sm ON s.subjectid = sm.subjectid
                WHERE u.userid = @UserId ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            cmbChooseSubject.Items.Clear();

                            while (reader.Read())
                            {
                                int subjectId = reader.GetInt32(0);
                                string subjectName = reader.GetString(1);
                                cmbChooseSubject.Items.Add(new ComboBoxItem(subjectId, subjectName));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading subjects: " + ex.Message);
                }
            }
        }

        private bool ValidateFields()
        {
            bool allValid = true;

            if (cmbChooseSubject.SelectedIndex == -1)
            {
                errorProvider.SetError(cmbChooseSubject, "Please select a subject.");
                allValid = false;
            }
            else
            {
                errorProvider.SetError(cmbChooseSubject, "");
            }

            if (string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                errorProvider.SetError(txtQuestion, "Question field cannot be empty.");
                allValid = false;
            }
            else
            {
                errorProvider.SetError(txtQuestion, "");
            }

            if (string.IsNullOrWhiteSpace(txtOptionA.Text))
            {
                errorProvider.SetError(txtOptionA, "Option A cannot be empty.");
                allValid = false;
            }
            else
            {
                errorProvider.SetError(txtOptionA, "");
            }

            if (string.IsNullOrWhiteSpace(txtOptionB.Text))
            {
                errorProvider.SetError(txtOptionB, "Option B cannot be empty.");
                allValid = false;
            }
            else
            {
                errorProvider.SetError(txtOptionB, "");
            }

            if (string.IsNullOrWhiteSpace(txtOptionC.Text))
            {
                errorProvider.SetError(txtOptionC, "Option C cannot be empty.");
                allValid = false;
            }
            else
            {
                errorProvider.SetError(txtOptionC, "");
            }

            if (string.IsNullOrWhiteSpace(txtOptionD.Text))
            {
                errorProvider.SetError(txtOptionD, "Option D cannot be empty.");
                allValid = false;
            }
            else
            {
                errorProvider.SetError(txtOptionD, "");
            }

            if (!radioA.Checked && !radioB.Checked && !radioC.Checked && !radioD.Checked)
            {
                errorProvider.SetError(radioA, "Please select the correct option.");
                allValid = false;
            }
            else
            {
                errorProvider.SetError(radioA, "");
            }

            return allValid;
        }

        private void SaveQuestion()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO QuestionBank (subjectid, createdby, question, optionA, optionB, optionC, optionD, correctanswer, statusid)
                        VALUES (@SubjectId, @CreatedBy, @Question, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectAnswer, @StatusId)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        ComboBoxItem selectedSubject = cmbChooseSubject.SelectedItem as ComboBoxItem;
                        if (selectedSubject == null) return;

                        command.Parameters.AddWithValue("@SubjectId", selectedSubject.Id);
                        command.Parameters.AddWithValue("@CreatedBy", userId);
                        command.Parameters.AddWithValue("@Question", txtQuestion.Text);
                        command.Parameters.AddWithValue("@OptionA", txtOptionA.Text);
                        command.Parameters.AddWithValue("@OptionB", txtOptionB.Text);
                        command.Parameters.AddWithValue("@OptionC", txtOptionC.Text);
                        command.Parameters.AddWithValue("@OptionD", txtOptionD.Text);

                        string correctAnswer = radioA.Checked ? "A" :
                                               radioB.Checked ? "B" :
                                               radioC.Checked ? "C" : "D";

                        command.Parameters.AddWithValue("@CorrectAnswer", correctAnswer);
                        command.Parameters.AddWithValue("@StatusId", 1); 

                        command.ExecuteNonQuery();
                        MessageBox.Show("Question saved successfully!");

               
                        ClearFields();
                        LoadQuestions(); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving question: " + ex.Message);
                }
            }
        }

        private void ClearFields()
        {
            cmbChooseSubject.SelectedIndex = -1;
            txtQuestion.Clear();
            txtOptionA.Clear();
            txtOptionB.Clear();
            txtOptionC.Clear();
            txtOptionD.Clear();
            radioA.Checked = false;
            radioB.Checked = false;
            radioC.Checked = false;
            radioD.Checked = false;
        }

        private void LoadQuestions(int? subjectId = null, string subjectName = null, string searchText = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                SELECT q.qid, sm.subjectname, u.name AS createdby, q.question, q.optionA, q.optionB, q.optionC, q.optionD, q.correctanswer
                FROM QuestionBank q
                INNER JOIN SubjectMaster sm ON q.subjectid = sm.subjectid
                INNER JOIN AllUsers u ON q.createdby = u.userid
                WHERE q.createdby = @UserId";

                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        query += " AND LOWER(q.question) LIKE '%' + LOWER(@SearchText) + '%'";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        if (!string.IsNullOrWhiteSpace(searchText))
                        {
                            command.Parameters.AddWithValue("@SearchText", searchText);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading questions: " + ex.Message);
                }
            }
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            SaveQuestion();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AStaffHomePage staff = new AStaffHomePage();
            staff.userName = userName;
            staff.Show();
            this.Hide();
        }

        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FirstPage page = new FirstPage();
            page.Show();
            this.Hide();
        }

        private void txtSearchClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            string subjectName = cmbChooseSubject.SelectedItem != null ? (cmbChooseSubject.SelectedItem as ComboBoxItem)?.Name : null;
            ComboBoxItem selectedSubject = cmbChooseSubject.SelectedItem as ComboBoxItem;
            LoadQuestions(subjectId: selectedSubject?.Id, subjectName: subjectName);
        }

        



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedQuestionId = Convert.ToInt32(row.Cells["qid"].Value); 
                string subjectName = row.Cells["subjectname"].Value.ToString();
                string createdBy = row.Cells["createdby"].Value.ToString();
                string questionText = row.Cells["question"].Value.ToString();
                string optionA = row.Cells["optionA"].Value.ToString();
                string optionB = row.Cells["optionB"].Value.ToString();
                string optionC = row.Cells["optionC"].Value.ToString();
                string optionD = row.Cells["optionD"].Value.ToString();
                string correctAnswer = row.Cells["correctanswer"].Value.ToString();

                txtQuestion.Text = questionText;
                txtOptionA.Text = optionA;
                txtOptionB.Text = optionB;
                txtOptionC.Text = optionC;
                txtOptionD.Text = optionD;

             
                radioA.Checked = (correctAnswer == "A");
                radioB.Checked = (correctAnswer == "B");
                radioC.Checked = (correctAnswer == "C");
                radioD.Checked = (correctAnswer == "D");

            
                foreach (ComboBoxItem item in cmbChooseSubject.Items)
                {
                    if (item.Name.Equals(subjectName, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbChooseSubject.SelectedItem = item;
                        break;
                    }
                }
            }
        }





        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                UPDATE QuestionBank
                SET subjectid = @SubjectId, question = @Question, optionA = @OptionA, optionB = @OptionB, optionC = @OptionC, optionD = @OptionD, correctanswer = @CorrectAnswer
                WHERE qid = @QuestionId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        ComboBoxItem selectedSubject = cmbChooseSubject.SelectedItem as ComboBoxItem;
                        if (selectedSubject == null) return;

                        command.Parameters.AddWithValue("@SubjectId", selectedSubject.Id);
                        command.Parameters.AddWithValue("@Question", txtQuestion.Text);
                        command.Parameters.AddWithValue("@OptionA", txtOptionA.Text);
                        command.Parameters.AddWithValue("@OptionB", txtOptionB.Text);
                        command.Parameters.AddWithValue("@OptionC", txtOptionC.Text);
                        command.Parameters.AddWithValue("@OptionD", txtOptionD.Text);

                        string correctAnswer = radioA.Checked ? "A" :
                                               radioB.Checked ? "B" :
                                               radioC.Checked ? "C" : "D";

                        command.Parameters.AddWithValue("@CorrectAnswer", correctAnswer);
                        command.Parameters.AddWithValue("@QuestionId", selectedQuestionId);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Question updated successfully!");

                        selectedQuestionId = 0; // Reset selection after update
                        ClearFields();
                        LoadQuestions();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating question: " + ex.Message);
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedQuestionId == 0)
            {
                MessageBox.Show("Please select a question to delete.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM QuestionBank WHERE qid = @QuestionId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@QuestionId", selectedQuestionId);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Question deleted successfully!");

                     
                        ClearFields();
                        LoadQuestions(); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting question: " + ex.Message);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim(); 
            LoadQuestions(searchText: searchText); 
        }
    }


    public class ComboBoxItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ComboBoxItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
