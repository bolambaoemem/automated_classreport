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
    public partial class add_Wgt : Form
    {
        public event EventHandler Form3Closed;
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        int _sem;
        string _subject;
        string _course;
        string _termname;
        string type ;
        public add_Wgt()
        {
            InitializeComponent();
        }
        public add_Wgt(int id, int sem, string subject, string course, string termname,string _type) :this()
        {
            _id = id;
            _sem = sem;
            _subject = subject;
            _course = course;
            _termname = termname;
            type = _type;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {


            high_Score high = new high_Score
            {
                teach_Id = _id,
                sem = _sem.ToString(),
                course = _course,
                term_exam = _termname,
                subject = _subject,
                typeof_column = type,
                wgt = Convert.ToDecimal(guna2TextBox2.Text.Trim()),
            };

            _context.high_Score.Add(high);
            _context.SaveChanges();


            OnForm3Closed(); // Trigger the Form2Closed event
            MessageBox.Show("Successfully added data!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        protected virtual void OnForm3Closed()
        {
            Form3Closed?.Invoke(this, EventArgs.Empty);
        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
    }
}
