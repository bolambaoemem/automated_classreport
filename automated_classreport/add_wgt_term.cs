using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using automated_classreport.Entities;
using automated_classreport.ViewModel;


namespace automated_classreport
{
    public partial class add_wgt_term
        : Form
    {
        public event EventHandler Form2Closed;
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        int _sem;
        string _subject;
        string _course;
        string _termname;
        string _mount;
        public add_wgt_term()
        {
            InitializeComponent();
        }
        public add_wgt_term(int id, int sem, string subject, string course,string termname, string mount) : this()
        {
            _id = id;
            _sem = sem;
            _subject = subject;
            _course = course;
            _termname = termname;
            _mount = mount;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            string termwgt = wgt_term.Text.Trim();
            Decimal termg = Convert.ToDecimal(wgt_term.Text.Trim());
            List<int> student_info = _context.Students.Where(q => q.teach_id == _id && q.course_year == _course && q.subject == _subject && q.sem_Id == _sem).Select(s=>s.t_Id).ToList();
            foreach (int studentId in student_info)
            {
                for (int term = 1; term <= 4; term++)
                {
                    Entities.class_Record record = new Entities.class_Record
                    {
                        stud_Id = studentId,
                        term_exam = _termname,
                        teach_Id = _id,
                        course = _course,
                        subject = _subject,
                        sem = _sem.ToString(),
                        wgt = Convert.ToInt32(termwgt),
                        typeof_column = GetColumnName(term),
                        mount = _mount

                    };

                    _context.class_Record.Add(record);
                    _context.SaveChanges();
                }

            }
            //Entities.high_Score hih = new Entities.high_Score
            //{


            //};


            OnForm2Closed(); // Trigger the Form2Closed event
            MessageBox.Show("Successfully added data!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

        }
        private string GetColumnName(int term)
        {
            switch (term)
            {
                case 1:
                    return "Quizzes";
                case 2:
                    return "Performance";
                case 3:
                    return "Oral";
                case 4:
                    return "Project";
                default:
                    return "unknown"; // Handle any other cases as needed
            }
        }

        private void gunaTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        protected virtual void OnForm2Closed()
        {
            Form2Closed?.Invoke(this, EventArgs.Empty);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
