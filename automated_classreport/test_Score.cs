using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using automated_classreport.Entities;
using automated_classreport.ViewModel;
using automated_classreport.Report;

namespace automated_classreport
{
    public partial class test_Score : Form
    {
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        string _sem;
        string term;
        string mount;
        public test_Score()
        {
            InitializeComponent();
        }
        public test_Score(int id):this()
        {
            _id = id;
        }

 
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            term = "Midterm";
            guna2Button1.FillColor = Color.MediumTurquoise;
            guna2Button2.FillColor = Color.Orange;
            guna2HtmlLabel1.Text = "Midterm Examination";


            var semister_Name = guna2TextBox2.Text.Trim();
            var course = guna2ComboBox2.SelectedValue;
            var subjects = guna2ComboBox1.SelectedValue;

            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == semister_Name);

                if (semester != null)
                {
                    int name_Sem = semester.sem_Id;

                    var groupedStudents = _context.class_Record
                        .FirstOrDefault(q => q.teach_Id == _id && q.subject == subjects.ToString() && q.course == course.ToString() && q.sem == name_Sem.ToString() && q.term_exam == "Midterm" && q.term_Score != null && q.mount == mount);
                    if (groupedStudents != null)
                    {
                        setdata();
                    }
                    else
                    {
                        MessageBox.Show("No Data For Midterm Score.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please fill up the fields.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void acceptName(string sem) {
            _sem = sem;
        
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            term = "Final";
            guna2Button2.FillColor = Color.MediumTurquoise;
            guna2Button1.FillColor = Color.Orange;
            guna2HtmlLabel1.Text = "Final Examination";

            var semister_Name = guna2TextBox2.Text.Trim();
            var course = guna2ComboBox2.SelectedValue;
            var subjects = guna2ComboBox1.SelectedValue;

            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == semister_Name);

                if (semester != null)
                {
                    int name_Sem = semester.sem_Id;

                    var groupedStudents = _context.class_Record
                        .FirstOrDefault(q => q.teach_Id == _id && q.subject == subjects.ToString() && q.course == course.ToString() && q.sem == name_Sem.ToString() && q.term_exam == "Final" && q.term_Score != null && q.mount == mount);
                    if (groupedStudents != null)
                    {
                        setdatas();
                    }
                    else {
                        MessageBox.Show("No Data For Final Score.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    }
                else
                {
                    MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please fill up the fields.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void test_Score_Load(object sender, EventArgs e)
        {
            term = "Midterm";
            guna2ComboBox1.SelectedIndex = -1;
            guna2ComboBox2.SelectedIndex = -1;
            guna2TextBox2.Text = _sem;
            guna2Button2.FillColor = Color.Orange;
            get_datafromdb();
        }
        public void setdata()
        {
            var semister_Name = guna2TextBox2.Text.Trim();
            var course = guna2ComboBox2.SelectedValue;
            var subjects = guna2ComboBox1.SelectedValue;

            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == semister_Name);

                if (semester != null)
                {
                    int name_Sem = semester.sem_Id;

                    var groupedStudents = _context.class_Record
                        .Where(q => q.teach_Id == _id && q.subject == subjects.ToString() && q.course == course.ToString() && q.sem == name_Sem.ToString() && q.term_exam == "Midterm" && q.term_Score != null && q.mount == mount)
                        .GroupBy(q => q.stud_Id)
                        .ToList();

                    guna2TextBox3.Text = groupedStudents.Count.ToString();
                    guna2TextBox9.Text = ((int)_context.high_Score
                                .Where(q => q.teach_Id == _id && q.subject == subjects.ToString() && q.course == course.ToString() && q.sem == name_Sem.ToString() && q.term_exam == "Midterm" && q.term_Score != null && q.mount == mount)
                                .Select(s => (decimal?)s.term_Score ?? 0)
                                .DefaultIfEmpty(0)
                                .FirstOrDefault()
                                ).ToString();
                    decimal highestScore = decimal.MinValue; 
                    decimal lowestScore = decimal.MaxValue; 
                    decimal medianScore = 0; 

                    foreach (var studentGroup in groupedStudents)
                    {
                        var studentScores = studentGroup.Select(q => q.term_Score.Value).OrderBy(score => score).ToList();

                        // Update highest score if necessary
                        highestScore = Math.Max(highestScore, studentScores.Max());

                        // Update lowest score if necessary
                        lowestScore = Math.Min(lowestScore, studentScores.Min());

                        // Update median score
                        medianScore += GetMedian(studentScores);
                    }

                    // Calculate the final median score
                    medianScore /= groupedStudents.Count;

                    // Display highest score, median score, and lowest score outside the loop
                    guna2TextBox5.Text = $"{Math.Round(highestScore)}";
                    guna2TextBox7.Text = $"{Math.Round(medianScore)}";
                    guna2TextBox8.Text = $"{Math.Round(lowestScore)}";
                    settotable();


                }
                else
                {
                    MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Handle the case where semister_Name, subjects, or course is null or empty
            }
        }
        public void setdatas()
        {
            var semister_Name = guna2TextBox2.Text.Trim();
            var course = guna2ComboBox2.SelectedValue;
            var subjects = guna2ComboBox1.SelectedValue;

            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == semister_Name);

                if (semester != null)
                {
                    int name_Sem = semester.sem_Id;

                    var groupedStudents = _context.class_Record
                        .Where(q => q.teach_Id == _id && q.subject == subjects.ToString() && q.course == course.ToString() && q.sem == name_Sem.ToString() && q.term_exam == "Final" && q.term_Score != null && q.mount == mount)
                        .GroupBy(q => q.stud_Id)
                        .ToList();

                    guna2TextBox3.Text = groupedStudents.Count.ToString();
                    guna2TextBox9.Text =((int) _context.high_Score
                                .Where(q => q.teach_Id == _id && q.subject == subjects.ToString() && q.course == course.ToString() && q.sem == name_Sem.ToString() && q.term_exam == "Final" && q.term_Score != null && q.mount == mount)
                                .Select(s => (decimal?)s.term_Score ?? 0)
                                .DefaultIfEmpty(0)
                                .FirstOrDefault())
                                .ToString();
                    decimal highestScore = decimal.MinValue;
                    decimal lowestScore = decimal.MaxValue;
                    decimal medianScore = 0;

                    foreach (var studentGroup in groupedStudents)
                    {
                        var studentScores = studentGroup.Select(q => q.term_Score.Value).OrderBy(score => score).ToList();

                        // Update highest score if necessary
                        highestScore = Math.Max(highestScore, studentScores.Max());

                        // Update lowest score if necessary
                        lowestScore = Math.Min(lowestScore, studentScores.Min());

                        // Update median score
                        medianScore += GetMedian(studentScores);
                    }

                    // Calculate the final median score
                    medianScore /= groupedStudents.Count;

                    // Display highest score, median score, and lowest score outside the loop
                    guna2TextBox5.Text = $"{Math.Round(highestScore)}";
                    guna2TextBox7.Text = $"{Math.Round(medianScore)}";
                    guna2TextBox8.Text = $"{Math.Round(lowestScore)}";
                    settotables();


                }
                else
                {
                    MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Handle the case where semister_Name, subjects, or course is null or empty
            }
        }

        public static decimal GetMedian(List<decimal> scores)
        {
            scores.Sort();

            int count = scores.Count;
            int middle = (int)Math.Round((decimal)count / 2);

            if (count % 2 == 0)
            {
                decimal left = scores[middle];
                decimal right = scores[middle + 1];
                return Math.Round(left);
            }
            else
            {
                return scores[middle];
            }
        }
        public void get_datafromdb()
        {

            string sem = guna2TextBox2.Text.Trim();
            int getsemId = _context.semesters.Where(q => q.sem_Name == sem).Select(s => s.sem_Id).FirstOrDefault();
            var getdata = _context.Students.Where(q => q.teach_id == _id && q.sem_Id == getsemId).ToList();
            if (getdata.Count == 0)
            {

                MessageBox.Show("Please enroll students first .", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                setdatatosubject();
                setdatatocourse();

            }

        }
        public void setdatatosubject()
        {

            var sename = guna2TextBox2.Text.Trim();
            var subject = _context.semesters.Where(q => q.sem_Name == sename).FirstOrDefault();

            if (subject != null)
            {
                int name_Sem = subject.sem_Id;

                var groupedStudents = _context.Students
                    .Where(q => q.teach_id == _id && q.sem_Id == name_Sem)
                    .GroupBy(q => q.subject)
                    .Select(group => new
                    {
                        Subject = group.Key,
                        Students = group.ToList()
                    })
                    .ToList();

                if (groupedStudents.Count == 0)
                {
                    MessageBox.Show("Please enroll students first .", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    guna2ComboBox1.DataSource = groupedStudents;
                }
            }
            else
            {
                MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void setdatatocourse()
        {

            var sename = guna2TextBox2.Text.Trim();
            var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == sename);

            if (semester != null)
            {
                int name_Sem = semester.sem_Id;

                var groupedStudents = _context.Students
                    .Where(q => q.teach_id == _id && q.sem_Id == name_Sem)
                    .GroupBy(q => q.course_year)
                    .Select(group => new
                    {
                        course_year = group.Key,
                        Students = group.ToList()
                    })
                    .ToList();

                if (groupedStudents.Count == 0)
                {
                    MessageBox.Show("No students found for the selected semester and teacher. Please enroll students first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    guna2ComboBox2.DataSource = groupedStudents;
                }
            }
            else
            {
                MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setdata();

        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            setdata();
        }
        public void settotable() {
            var semister_Name = guna2TextBox2.Text.Trim();
            var course = guna2ComboBox2.SelectedValue;
            var subjects = guna2ComboBox1.SelectedValue;

            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == semister_Name);

                if (semester != null)
                {
                    int name_Sem = semester.sem_Id;

                    var datas = (
                                    from cr in _context.class_Record
                                    join st in _context.Students on cr.stud_Id equals st.t_Id
                                    where cr.teach_Id == _id && cr.subject == subjects.ToString() && cr.course == course.ToString() && cr.sem == name_Sem.ToString() && cr.term_exam == "Midterm" && cr.term_Score != null && cr.mount == mount
                                    group new { cr, st } by st.t_Id into studentGroup
                                    select new classTermViewmodel
                                    {
                                        stud_Id = studentGroup.Key,
                                        lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1) + ".").FirstOrDefault(),
                                        high_score = (int)Math.Round(studentGroup.Select(s => (decimal?)s.cr.term_Score ?? 0).FirstOrDefault())
                                    }
                                ).ToList();

                    var highScoreCounts = datas.GroupBy(item => item.high_score)
                                               .ToDictionary(group => group.Key, group => group.Count());

                    foreach (var item in datas)
                    {
 
                        if (highScoreCounts.TryGetValue(item.high_score, out var count))
                        {
                            item.type_total = count;
 
                            highScoreCounts.Remove(item.high_score);
                        }
                    }

                    classTermViewmodelBindingSource.DataSource = datas;
                }
                else
                {
                    MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Handle the case where semister_Name, subjects, or course is null or empty
            }




        }
        public void settotables()
        {
            var semister_Name = guna2TextBox2.Text.Trim();
            var course = guna2ComboBox2.SelectedValue;
            var subjects = guna2ComboBox1.SelectedValue;

            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == semister_Name);

                if (semester != null)
                {
                    int name_Sem = semester.sem_Id;

                    var datas = (
                                    from cr in _context.class_Record
                                    join st in _context.Students on cr.stud_Id equals st.t_Id
                                    where cr.teach_Id == _id && cr.subject == subjects.ToString() && cr.course == course.ToString() && cr.sem == name_Sem.ToString() && cr.term_exam == "Final" && cr.term_Score != null && cr.mount == mount
                                    group new { cr, st } by st.t_Id into studentGroup
                                    select new classTermViewmodel
                                    {
                                        stud_Id = studentGroup.Key,
                                        lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1) + ".").FirstOrDefault(),
                                        high_score = (int)Math.Round(studentGroup.Select(s => (decimal?)s.cr.term_Score ?? 0).FirstOrDefault())
                                    }
                                ).ToList();

                    var highScoreCounts = datas.GroupBy(item => item.high_score)
                                               .ToDictionary(group => group.Key, group => group.Count());

                    foreach (var item in datas)
                    {

                        if (highScoreCounts.TryGetValue(item.high_score, out var count))
                        {
                            item.type_total = count;

                            highScoreCounts.Remove(item.high_score);
                        }
                    }

                    classTermViewmodelBindingSource.DataSource = datas;
                }
                else
                {
                    MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Handle the case where semister_Name, subjects, or course is null or empty
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

            var subject = guna2ComboBox1.SelectedValue;
            var course = guna2ComboBox2.SelectedValue;
            var semister_Name = guna2TextBox2.Text.Trim();
            var test_given = guna2TextBox4.Text.Trim();
            var test_submit = guna2TextBox1.Text.Trim();
            var num_takingtest = guna2TextBox3.Text.Trim();
            var high_score = guna2TextBox5.Text.Trim();
            var med_score = guna2TextBox7.Text.Trim();
            var low_score = guna2TextBox8.Text.Trim();
            var score = guna2TextBox9.Text.Trim();

            test_form_print pr = new test_form_print(_id,term,subject.ToString(),course.ToString(),semister_Name,test_given,test_submit,num_takingtest,high_score,med_score,low_score,score,mount);
            pr.ShowDialog();



        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mon = guna2ComboBox3.SelectedIndex;
            if (mon == 0)
            {
                mount = "lec";
            }
            else {
                mount = "lab";

            }
        }
    }
}
