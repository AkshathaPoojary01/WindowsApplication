using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AssessmentPortalApp
{
    public partial class ViewResults : Form
    {
        public int userId;
        public string userName;
        public int roleId;
        private string connectionString = "data source=akshu\\sqlexpress; initial catalog=AssessmentPortalDB; user id=sa; password=akshu";

        public ViewResults()
        {
            InitializeComponent();
        }

        private void ViewResults_Load(object sender, EventArgs e)
        {
            DisplayResults();
            lblUsername.Text = userName;
        }

        private void DisplayResults()
        {
            string query = @"
        SELECT 
            r.totalmarks,
            r.obtainedmarks,
            r.percentage,
            r.examdate
        FROM ExamResults r
        INNER JOIN ExamMaster m ON r.examid = m.examid
        WHERE m.studentid = @userId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                int totalMarks = 0;
                                int obtainedMarks = 0;
                                float percentage = 0.0f;

                              
                                string totalMarksStr = reader["totalmarks"].ToString();
                                string obtainedMarksStr = reader["obtainedmarks"].ToString();
                                string percentageStr = reader["percentage"].ToString();
                                DateTime examDate = (DateTime)reader["examdate"];


                                if (int.TryParse(totalMarksStr, out totalMarks) &&
                                    int.TryParse(obtainedMarksStr, out obtainedMarks) &&
                                    float.TryParse(percentageStr, out percentage))
                                {
                                   
                                    lblMarks.Text = $"Total Marks: {totalMarks}\nObtained Marks: {obtainedMarks}";
                                    lblPercentage.Text = $"Percentage: {percentage:F2}%";//percentage:F2 after the decimal it will show 2 dight 97.38
                                    lblScore.Text = $"Final Score: {obtainedMarks} out of {totalMarks} on {examDate:MMMM dd, yyyy}";

                                  
                                    if (obtainedMarks < 8)
                                    {
                                        lblClearedAssessment.Text = "You have not cleared the assessment.";
                                        lblClearedAssessment.ForeColor = System.Drawing.Color.Maroon;
                                    }
                                    else
                                    {
                                        lblClearedAssessment.Text = "You have cleared the assessment.";
                                        lblClearedAssessment.ForeColor = System.Drawing.Color.DarkGreen;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Error parsing result data.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("No results found for this student.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading results: " + ex.Message);
                }
            }
        }


        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FirstPage page = new FirstPage();
            page.Show();
            this.Hide();
        }
    }
}
