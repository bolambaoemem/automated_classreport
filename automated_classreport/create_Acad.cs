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



namespace automated_classreport
{
    public partial class create_Acad : Form
    {
        private add_Academic form2;
        private update_Academic form3;
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        int num;
        public create_Acad()
        {
            InitializeComponent();
        }

        public create_Acad(int Id):this()
        {
            _id = Id;
        }

        private void create_Acad_Load(object sender, EventArgs e)
        {
            refresh();
        }
        public void refresh() {

            var check = _context.semesters
                  .Where(q => q.teach_id == _id)
                  .ToList();
            int sem_Count = check.Count;
            if (sem_Count <= 0)
            {
                guna2Button1.Visible = true;
                num = 0;
                guna2TextBox1.Text = num.ToString();
                gunaPictureBox1.Visible = false;
                gunaPictureBox2.Visible = false;
                guna2HtmlLabel2.Visible = false;
                guna2HtmlLabel3.Visible = false;
                first_Sem_name.Visible = false;
                second_Sem_name.Visible = false;
                guna2Button2.Visible = false;
            }
            if (sem_Count == 1)
            {

              
                guna2Button1.Visible = true;
                var semname = _context.semesters
                    .Where(q => q.teach_id == _id)
                    .FirstOrDefault();
                    num = 1;
                if (semname.sem_Mean == 1) {
                    guna2TextBox1.Text = num.ToString();
                    first_Sem_name.Text = string.Join(", ", semname.sem_Name);
                    guna2Button1.Visible = true;
                    gunaPictureBox1.Visible = true;
                    guna2HtmlLabel2.Visible = true;
                    first_Sem_name.Visible = true;
                    gunaPictureBox2.Visible = false;
                    guna2HtmlLabel3.Visible = false;
                    second_Sem_name.Visible = false;
                }
                if (semname.sem_Mean == '2') {
                    guna2TextBox1.Text = num.ToString();
                    second_Sem_name.Text = string.Join(", ", semname.sem_Name);
                    guna2Button1.Visible = true;
                    gunaPictureBox1.Visible = false;
                    guna2HtmlLabel2.Visible = false;
                    first_Sem_name.Visible = false;
                    gunaPictureBox2.Visible = true;
                    guna2HtmlLabel3.Visible = true;
                    second_Sem_name.Visible = true;


                }
            }
            if (sem_Count == 2)
            {
                guna2Button2.Visible = true;
                guna2Button1.Visible = false;
                var semesters = _context.semesters
                  .Where(q => q.teach_id == _id)
                  .OrderBy(s => s.sem_Mean)
                  .Select(v => v.sem_Name)
                  .ToList();

                if (semesters.Count == 2)
                {
                    first_Sem_name.Text = semesters[0];
                    second_Sem_name.Text = semesters[1];
                    gunaPictureBox1.Visible = true;
                    guna2HtmlLabel2.Visible = true;
                    first_Sem_name.Visible = true;
                    gunaPictureBox2.Visible = true;
                    guna2HtmlLabel3.Visible = true;
                    second_Sem_name.Visible = true;


                }

            }



        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            form2 = new add_Academic(_id);
            form2.Form2Closed += Form2_FormClosed;
            form2.ShowDialog();
        }
        public void change_num(int _num) {
            num = _num;
        }

   
        private void Form2_FormClosed(object sender, EventArgs e)
        {
            refresh(); 
        }
        private void Form3_FormClosed(object sender, EventArgs e)
        {
            refresh();
        }

        private void gunaPictureBox1_Click(object sender, EventArgs e)
        {
            string sem_Name = first_Sem_name.Text.Trim();
            class_Report_content dsh = new class_Report_content(_id);
            dsh.TopLevel = false;
            acad_panel.Controls.Clear();
            acad_panel.Controls.Add(dsh);
            dsh.accept_Name(sem_Name);
            dsh.Show();
        }

        private void gunaPictureBox1_MouseHover(object sender, EventArgs e)
        {
            first_btn.Visible = true;
        }
        private void pass_content_MouseHover(object sender, EventArgs e)
        {
            first_btn.Visible = false;
            guna2Button3.Visible = false;
        }

        private void first_btn_Click(object sender, EventArgs e)
        {
            gunaContextMenuStrip1.Show(first_btn, first_btn.Width,1);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string senName = first_Sem_name.Text.Trim();
            form3 = new update_Academic(_id,senName);
            form3.Form3Closed += Form3_FormClosed;
            form3.ShowDialog();
        }

        private void gunaPictureBox2_MouseHover(object sender, EventArgs e)
        {
            guna2Button3.Visible = true;
        }

        private void renameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string senName = second_Sem_name.Text.Trim();
            form3 = new update_Academic(_id, senName);
            form3.Form3Closed += Form3_FormClosed;
            form3.ShowDialog();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            gunaContextMenuStrip2.Show(guna2Button3, guna2Button3.Width, 1);
        }

        public void deletedata() {
            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var semName = first_Sem_name.Text.Trim();
            var semesterToDelete = _context.semesters.FirstOrDefault(q => q.teach_id == _id && q.sem_Name == semName);

            if (semesterToDelete != null)
            {

                var result = MessageBox.Show("Are you sure you want to delete this semester?", "Confirm Deletion", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {

                    _context.semesters.Remove(semesterToDelete);
                    _context.SaveChanges();

                    MessageBox.Show("Semester deleted successfully!");
                    refresh();
                }

            }
            else
            {
                MessageBox.Show("Semester not found for ID: " + _id);
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var semName = second_Sem_name.Text.Trim();
            var semesterToDelete = _context.semesters.FirstOrDefault(q => q.teach_id == _id && q.sem_Name == semName);

            if (semesterToDelete != null)
            {

                var result = MessageBox.Show("Are you sure you want to delete this semester?", "Confirm Deletion", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                
                    var relatedDataToDelete1 = _context.class_Record.Where(o => o.teach_Id== _id && o.sem==semesterToDelete.sem_Id.ToString());
                    _context.class_Record.RemoveRange(relatedDataToDelete1);


                    var relatedDataToDelete2 = _context.high_Score.Where(o => o.teach_Id == _id && o.sem == semesterToDelete.sem_Id.ToString()) ;
                    _context.high_Score.RemoveRange(relatedDataToDelete2);

                    _context.semesters.Remove(semesterToDelete);
                    _context.SaveChanges();

                    MessageBox.Show("Semester deleted successfully!");
                    refresh();
                }

            }
            else
            {
                MessageBox.Show("Semester not found for ID: " + _id);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to move all of these to history?", "Moving Confirmation", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                DateTime currentDate = DateTime.Now;
                var getdalldata = _context.semesters.Where(q => q.teach_id == _id).ToList();

                foreach (var data in getdalldata)
                {
                    history his = new history
                    {
                        sem_Id = data.sem_Id,
                        teach_id = _id,
                        semName = data.sem_Name,
                        sem_mean = data.sem_Mean.ToString(),
                        history1 = currentDate,
                    };

                    _context.histories.Add(his);
                }
                foreach (var data in getdalldata)
                {
                    _context.semesters.Remove(data);
                }

                _context.SaveChanges();


                MessageBox.Show("Successfully move to history!");
                refresh();
            }
            else { 
            
            
            
            }

        }

        private void gunaPictureBox2_Click(object sender, EventArgs e)
        {
            string sem_Name = second_Sem_name.Text.Trim();
            class_Report_content dsh = new class_Report_content(_id);
            dsh.TopLevel = false;
            acad_panel.Controls.Clear();
            acad_panel.Controls.Add(dsh);
            dsh.accept_Name(sem_Name);
            dsh.Show();
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}
