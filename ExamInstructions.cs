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
    public partial class ExamInstructions : Form
    {
        public string userName;
        public int roleId;
        public int userId;
        public ExamInstructions()
        {
            InitializeComponent();
        }

        private void lnkClickHere_Click(object sender, EventArgs e)
        {
            TakeExam take = new TakeExam(userId,userName,roleId);
            take.userName = userName;
            take.Show();
            this.Hide();
        }
    }
}
