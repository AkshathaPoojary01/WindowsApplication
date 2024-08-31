using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AssessmentPortalApp
{
    public partial class TakeExam : Form
    {
        public int userId;
        public string userName;
        public int roleId;
        private string connectionString = "data source=akshu\\sqlexpress; initial catalog=AssessmentPortalDB; user id=sa; password=akshu";

        private List<Question> questions = new List<Question>();
        private int currentQuestionIndex = 0;
        private Timer examTimer = new Timer();
        private int timeLeft = 900; //sec sec/60=>900/60=15min
        private Dictionary<int, string> questionStates = new Dictionary<int, string>(); //int- it will store qid (index)and string will store state of answered (answer(any of the option ,skipped,na)
        //for ex { 0, "use of Pointers" }, { 1, "32bit" }, {7,NA} // Answer for the first question (index 0),Answer for the secound question (index 1)
       //If we want to tract the state of question whether is answerd skipped or not answerwed use index of question and state of the question   
      private Dictionary<int, string> answers = new Dictionary<int, string>(); 
        //if u want to store the answer then use the quid and selected answer

        public TakeExam(int userId, string userName, int roleId) //roleid is optional it will check validation 2nd time
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;
            this.roleId = roleId;
            SetupQuestionButtons();
            SetupTimer();
            lblQuestion.Visible = false;
            lblOptionA.Visible = false;
            lblOptionB.Visible = false;
            lblOptionC.Visible = false;
            lblOptionD.Visible = false;
            radioA.Visible = false;
            radioB.Visible = false;
            radioC.Visible = false;
            radioD.Visible = false;
            btnSave.Visible = false;
            btnSkip.Visible = false;
            btnEndExam.Visible = false;
            DisableAllButtons();
        }

        private void DisableAllButtons()
        {
            foreach (Control control in groupBoxQuestions.Controls) 
            {
                if (control is Button button)
                {
                    button.Enabled = false;
                }
            }
        }

        private void TakeExam_Load(object sender, EventArgs e)
        {
            lblMsg.Text = userName;
            LoadSubjects();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string selectedSubject = cmbChooseSubject.SelectedItem?.ToString(); //?.null conditional opeator
            if (string.IsNullOrEmpty(selectedSubject))
            {
                MessageBox.Show("Please select a subject.");
                return;
            }

            if (questions.Count == 0) //questions=[] list is empty add the questions to the list
            {
                LoadQuestions(selectedSubject);
                RandomizeQuestions();
                currentQuestionIndex = 0;
                DisplayQuestion();
            }
            else
            {
                MessageBox.Show("Questions are already loaded.");
            }

            examTimer.Start();

            lblQuestion.Visible = true;
            lblOptionA.Visible = true;
            lblOptionB.Visible = true;
            lblOptionC.Visible = true;
            lblOptionD.Visible = true;
            radioA.Visible = true;
            radioB.Visible = true;
            radioC.Visible = true;
            radioD.Visible = true;
            btnSave.Visible = true;
            btnSkip.Visible = true;
            btnEndExam.Visible = true;

            EnableAllButtons();
        }

        private void EnableAllButtons()
        {
            foreach (Control control in groupBoxQuestions.Controls)
            {
                if (control is Button button && button.Name.StartsWith("btnQue"))
                {
                    button.Enabled = true;
                    button.BackColor = Color.DarkGreen;
                }
            }
        }

        private void LoadSubjects()
        {
            string query = "SELECT subjectid, subjectname FROM SubjectMaster";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            cmbChooseSubject.Items.Clear();
                            while (reader.Read())
                            {
                                string subjectName = reader["subjectname"].ToString(); //it only store row by row subject which is there in the database 1st will read cloud from the database and adds that subject into the combo box next it read java from the database add tho the combobox
                                cmbChooseSubject.Items.Add(subjectName);
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

        private void LoadQuestions(string subjectName)
        {
            questions.Clear();
            string query = "SELECT qid, subjectid, question, optionA, optionB, optionC, optionD, correctanswer FROM QuestionBank WHERE subjectid =(SELECT subjectid FROM SubjectMaster WHERE subjectname = @subjectName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@subjectName", subjectName); //from load suibject=>when user select subject from the combo box the selected subject will stored in subjectName
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                questions.Add(new Question
                                {
                                    Qid = Convert.ToInt32(reader["qid"]),
                                    SubjectId=Convert.ToInt32(reader["subjectid"]),
                                    
                                    Text = reader["question"].ToString(),
                                    OptionA = reader["optionA"].ToString(),
                                    OptionB = reader["optionB"].ToString(),
                                    OptionC = reader["optionC"].ToString(),
                                    OptionD = reader["optionD"].ToString(),
                                    CorrectAnswer = reader["correctanswer"].ToString()
                                }) ;
                            }
                        }
                    }
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading questions: " + ex.Message);
                }
            }
        }

        private void RandomizeQuestions()
        {
            Random rng = new Random();
            questions = questions.OrderBy(x => rng.Next()).ToList();
        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex < questions.Count)  //questions={"Q1","Q2","Q3"_____"Q20"} qindex is 0 if(0<20),1<20,2<20,3<20
            {
                var question = questions[currentQuestionIndex]; //current question
                lblQuestion.Text = $"{currentQuestionIndex + 1}. {question.Text}"; //{currentQuestionIndex + 1} question number 1 => 0+1 is 1) like
                //1) text=What is the size of int?

                lblOptionA.Text = $"A) {question.OptionA}"; //current que options
                lblOptionB.Text = $"B) {question.OptionB}";
                lblOptionC.Text = $"C) {question.OptionC}";
                lblOptionD.Text = $"D) {question.OptionD}";

              
                if (answers.ContainsKey(question.Qid)) //current question id //if i go mback to some question button which is not attempted or already saved questoin then also we need displkauy the question right
                {
                    //when o go back to perticular button show the question with answer which i already attempted
                    string savedAnswer = answers[question.Qid];
                    switch (savedAnswer)
                    {
                        case "A":
                            radioA.Checked = true;
                            break;
                        case "B":
                            radioB.Checked = true;
                            break;
                        case "C":
                            radioC.Checked = true;
                            break;
                        case "D":
                            radioD.Checked = true;
                            break;
                    }
                }
                else
                {
                    //if i didn't attempted question
                    radioA.Checked = false;
                    radioB.Checked = false;
                    radioC.Checked = false;
                    radioD.Checked = false;
                }
            }
            else
            {
                MessageBox.Show("No more questions.");
            }
        }

        private void SetupQuestionButtons()
        {
            foreach (Control control in groupBoxQuestions.Controls)
            {
                if (control is Button button && button.Name.StartsWith("btnQue"))
                {
                    int questionNumber; //declare varibale to store question No (1,2,3________20)
                    if (int.TryParse(button.Name.Substring(6), out questionNumber)) //extract 1 from btnQue1 and store that 1 to questionNumber (since 1 is of string type convert that into integer)
                    { 
                        button.Tag = questionNumber - 1; //1-1 =0 1st button has tag 0
                        button.Click += QuestionButton_Click;
                        button.BackColor = SystemColors.Control;
                        questionStates[questionNumber - 1] = "NA"; //indicating that the question has not yet been attempted. initially all question is not answered_____Ensures that all questions start with a default state
                        /*Suppose you have 10 questions, and questionStates is an array of size 10:

                     questionStates:["NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA", "NA"] -it wl hold qid & states
                   If questionNumber is 5, then questionNumber -1 equals 4.

                  Action: questionStates[4] = "NA"; will set the value at index 4 to "NA".

                 Result: The state of the fifth question in questionStates is initialized to "NA"*/
                    }
                    else
                    {
                        MessageBox.Show($"Invalid button name: {button.Name}");
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string selectedAnswer = GetSelectedAnswer() ?? "NA"; //if GetSelectedAnswer() is not null it will return GetSelectedAnswer()
                                                                 //if  GetSelectedAnswer() is null it will return NA==>?? CHECKS NULL VALUE 
                                                                 //?? ENSURES NULL AND NOT NULL IF  GETSELECTANSWER IS NULL RETURNS RIGHT SIDE VALUE
                                                                 //IF  GETSELECTANSWER IS NOT NULL RETURNS LEFT SIDE SIDE VALUE
            questionStates[currentQuestionIndex] = selectedAnswer; 

            UpdateButtonColors();

         
            currentQuestionIndex++;
            DisplayQuestion();
        }

        private void QuestionButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                int questionNumber = (int)button.Tag; //0,1,2
                currentQuestionIndex = questionNumber; //1st que,2nd que,3rd que
                DisplayQuestion();

                
                if (questionStates.ContainsKey(currentQuestionIndex)) //means user selected the answer if the questionstate dictonary conatins qindex that means that question is attempted
                {
                    string savedAnswer = questionStates[currentQuestionIndex];
                    SetSelectedAnswer(savedAnswer);
                }
            }
        }

        private void SetSelectedAnswer(string answer)
        {
            radioA.Checked = answer == "A";
            radioB.Checked = answer == "B";
            radioC.Checked = answer == "C";
            radioD.Checked = answer == "D";
        }

        private void StoreExamDetails()
        {
            int examId = 0;
            int totalMarks = questions.Count; //20
            int obtainedMarks = 0;

          
            foreach (var question in questions)
            {
                int questionIndex = questions.IndexOf(question);
                if (questionStates.ContainsKey(questionIndex))
                {
                    string selectedAnswer = questionStates[questionIndex]; //
                    if (selectedAnswer == question.CorrectAnswer)
                    {
                        obtainedMarks++;
                    }
                }
            }

            float percentage = (float)obtainedMarks / totalMarks * 100;

            string insertExamQuery = "INSERT INTO ExamMaster(studentid, doe, subjectid) OUTPUT INSERTED.examid VALUES(@studentId, GETDATE(), @subjectId)";
            string insertResultQuery = "INSERT INTO ExamResults(examid, studentid, subjectid, totalmarks, obtainedmarks, percentage, examdate) VALUES(@examId, @studentId, @subjectId, @totalMarks, @obtainedMarks, @percentage, GETDATE())";
            string insertAnswerQuery = "INSERT INTO Answers(examid, qid, selectedanswer) VALUES(@examId, @qid, @selectedAnswer)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                 
                    using (SqlCommand command = new SqlCommand(insertExamQuery, connection))
                    {
                        command.Parameters.AddWithValue("@studentId", userId);
                        command.Parameters.AddWithValue("@subjectId", questions[0].SubjectId);

                        examId = (int)command.ExecuteScalar();
                    }

                 
                    using (SqlCommand resultCommand = new SqlCommand(insertResultQuery, connection))
                    {
                        resultCommand.Parameters.AddWithValue("@examId", examId);
                        resultCommand.Parameters.AddWithValue("@studentId", userId);
                        resultCommand.Parameters.AddWithValue("@subjectId", questions[0].SubjectId);
                        resultCommand.Parameters.AddWithValue("@totalMarks", totalMarks);
                        resultCommand.Parameters.AddWithValue("@obtainedMarks", obtainedMarks);
                        resultCommand.Parameters.AddWithValue("@percentage", percentage);

                        resultCommand.ExecuteNonQuery();
                    }

                  
                    foreach (var entry in questionStates)
                    {
                        int questionIndex = entry.Key;
                        string selectedAnswer = entry.Value;
                        int questionId = questions[questionIndex].Qid;

                        using (SqlCommand answerCommand = new SqlCommand(insertAnswerQuery, connection))
                        {
                            answerCommand.Parameters.AddWithValue("@examId", examId);
                            answerCommand.Parameters.AddWithValue("@qid", questionId);
                            answerCommand.Parameters.AddWithValue("@selectedAnswer", selectedAnswer);

                            answerCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error storing exam details: " + ex.Message);
                }
            }
        }


        private string GetSelectedAnswer()
        {
            if (radioA.Checked) return "A";
            if (radioB.Checked) return "B";
            if (radioC.Checked) return "C";
            if (radioD.Checked) return "D";
            return null;
        }

        private void UpdateButtonColors()
        {
            foreach (Control control in groupBoxQuestions.Controls)
            {
                if (control is Button button && button.Name.StartsWith("btnQue"))
                {
                    int questionIndex = (int)button.Tag;
                    string state = questionStates.ContainsKey(questionIndex) ? questionStates[questionIndex] : "NA";

                   
                    if (state == "A" || state == "B" || state == "C" || state == "D")
                    {
                        button.BackColor = Color.DarkBlue; 
                    }
                    else if (state == "SKIPPED")
                    {
                        button.BackColor = Color.Maroon; 
                    }
                    else
                    {
                        button.BackColor = Color.DarkGreen; 
                    }
                }
            }
        }





        private void btnSkip_Click(object sender, EventArgs e)
        {
           
            questionStates[currentQuestionIndex] = "SKIPPED";

           
            UpdateButtonColors();

           
            currentQuestionIndex++;
            DisplayQuestion();
        }


        private void SetupTimer()
        {
            examTimer.Interval = 1000; //1000 MILI SEC  WHICH MEANS THE TIMER WILL TRIGGER EVERY SECOND
            examTimer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"Time Left: {timeLeft / 60:D2}:{timeLeft % 60:D2}";  //60 gives min and %60 gives seconds

            if (timeLeft <= 0)
            {
                examTimer.Stop();
                MessageBox.Show("Time's up!");
                StoreExamDetails();
                Close();
            }
        }

        private void btnEndExam_Click(object sender, EventArgs e)
        {
            examTimer.Stop();
            StoreExamDetails();
            MessageBox.Show("Exam ended. Your responses have been saved.");

            ViewResults result = new ViewResults();
            result.userId = userId;
            result.userName = userName;
            
            result.Show();
            this.Hide();
        }
    }

    public class Question
    {
        public int Qid { get; set; }  //for ex: 1
        public int SubjectId { get; set; } //java
        public string Text { get; set; } //contains all the questions from 1 -20 //wt is the size of int
        public string OptionA { get; set; } //2byte
        public string OptionB { get; set; } //4byte
        public string OptionC { get; set; } //8 byte
        public string OptionD { get; set; } //1 byte
        public string CorrectAnswer { get; set; } //4 byte 
    }
}
