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
using Microsoft.Reporting.WinForms;
using automated_classreport.Report;

namespace automated_classreport
{
    public partial class final_Ratings : Form
    {
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        string _sem;
        string mount;
        public final_Ratings()
        {
            InitializeComponent();
        }
        public final_Ratings(int id,string sem):this()
        {
            _id = id;
            _sem = sem;
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }


        private void final_Ratings_Load(object sender, EventArgs e)
        {

            var teachname = _context.user_account.Where(q => q.accId == _id).FirstOrDefault();
            guna2TextBox2.Text = teachname.fname+ " " + teachname.lname.ToString();
            var setsemName = _context.semesters.Where(q => q.teach_id==_id && q.sem_Name==_sem.ToString()).FirstOrDefault();
            guna2HtmlLabel4.Text = setsemName.sem_Name;
            int term_num = Convert.ToInt32(setsemName.sem_Mean);
            if (term_num == 1) {
                guna2HtmlLabel1.Text = "1st";


            }
             if (term_num == 2)
                {
                    guna2HtmlLabel1.Text = "2nd";


                }


            setdatatosubject();
            setdatatocourse();

            //this.reportViewer1.RefreshReport();
            //this.reportViewer2.RefreshReport();
        }
        public void setdatatosubject()
        {
            var subject = _context.semesters.Where(q => q.sem_Name == _sem).FirstOrDefault();

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
                    guna2ComboBox2.DataSource = groupedStudents;
                }
            }
            else
            {
                MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void setdatatocourse()
        {

           
            var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == _sem);

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
                    guna2ComboBox1.DataSource = groupedStudents;

                }
            }
            else
            {
                MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        public void getdataz()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
            var datas = (
            from cr in _context.class_Record
            join st in _context.Students on cr.stud_Id equals st.t_Id
            where cr.teach_Id == _id && cr.sem == sem_Id.ToString() && cr.course == courses.ToString() && cr.subject == subjects.ToString() && cr.mount == mount
            group new { cr, st } by st.t_Id into studentGroup
            select new classTermViewmodel
            {

                stud_Id = studentGroup.Key,
                lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1))
                        .FirstOrDefault(),

                column_1 = studentGroup.SelectMany(s => _context.class_Record
                                                                             .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                                                .Select(q => Math.Round((q.set_Grade ?? 0), 1))
                                                                                .DefaultIfEmpty(0))
                                             .FirstOrDefault(),

                column_3 = studentGroup.Select(s => _context.class_Record
                                                        .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                        .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                            .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                            .Select(hs => hs.wgt ?? 0)
                                                            .DefaultIfEmpty()
                                                            .FirstOrDefault() / 100))
                                                        .DefaultIfEmpty()
                                                        .FirstOrDefault())
                                        .FirstOrDefault(),


                column_2 = studentGroup.SelectMany(s => _context.class_Record
                                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Performance" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                            .Select(q => (decimal?)q.set_Grade ?? 0)
                                                            .DefaultIfEmpty(0))
                                                .FirstOrDefault(),


                column_4 = studentGroup.Select(s => _context.class_Record
                                                        .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Performance" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                        .Select(q => (q.set_Grade ?? 0) * (((_context.high_Score
                                                            .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Performance" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                            .Select(hs => hs.wgt ?? 0)
                                                            .DefaultIfEmpty()
                                                            .FirstOrDefault())+(_context.high_Score
                                                            .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                            .Select(hs => hs.wgt ?? 0)
                                                            .DefaultIfEmpty()
                                                            .FirstOrDefault()) )/ 100))
                                                        .DefaultIfEmpty()
                                                        .FirstOrDefault())
                                        .FirstOrDefault(),
                column_5 = studentGroup.SelectMany(s => _context.class_Record
                                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                            .Select(q => (decimal?)q.set_Grade ?? 0)
                                                            .DefaultIfEmpty(0))
                                                .FirstOrDefault(),

                column_6 = studentGroup.Select(s => _context.class_Record
                                                       .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                       .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                           .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                           .Select(hs => hs.wgt ?? 0)
                                                           .DefaultIfEmpty()
                                                           .FirstOrDefault() / 100))
                                                       .DefaultIfEmpty()
                                                       .FirstOrDefault())
                                        .FirstOrDefault(),

                //term_Score = studentGroup.SelectMany(s => _context.class_Record
                //                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                //                                            .Select(q => (decimal?)q.set_Grade ?? 0)
                //                                            .DefaultIfEmpty(0))
                //                                            .FirstOrDefault(),

                //type_total = studentGroup.Select(s => _context.class_Record
                //                                       .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                //                                       .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                //                                           .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                //                                           .Select(hs => hs.wgt ?? 0)
                //                                           .DefaultIfEmpty()
                //                                           .FirstOrDefault() / 100))
                //                                       .DefaultIfEmpty()
                //                                       .FirstOrDefault())
                //                        .FirstOrDefault(),

                column_7 = studentGroup.SelectMany(s => _context.class_Record
                                              .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                              .Select(q => (decimal?)q.term_Score ?? 0)
                                              .DefaultIfEmpty(0))
                                              .FirstOrDefault(),

                column_8 = Math.Round(
                 studentGroup.SelectMany(s => _context.class_Record
                                                     .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                     .Select(q => ((decimal?)q.type_total ?? 0))
                                                     .DefaultIfEmpty(0))
                                                     .FirstOrDefault(),
                 1
             ),
                column_9 =
                        (studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                      +

                       studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Performance" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (((_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Performance" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault())+(_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault())) / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                    +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                        +
                        studentGroup.SelectMany(s => _context.class_Record
                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                            .Select(q => q.type_total ?? 0)
                            .DefaultIfEmpty(0)
                        ).FirstOrDefault()

                        ),

                column_10 = 0,
                term_total_wgt = 0,

            }
                        ).ToList();

            foreach (var data in datas)
            {
                data.column_1 = (int)Math.Round(data.column_1);
                data.column_9 = (int)Math.Round(data.column_9);

                if (data.column_9 == 100)
                {
                    data.column_10 = 95;
                    data.term_total_wgt = (decimal)1.0;
                }
                else if (data.column_9 == 99)
                {
                    data.column_10 = 94;
                    data.term_total_wgt = (decimal)1.1;
                }
                else if (data.column_9 == 98)
                {
                    data.column_10 = 93;
                    data.term_total_wgt = (decimal)1.2;
                }
                else if (data.column_9 == 97)
                {
                    data.column_10 = 92;
                    data.term_total_wgt = (decimal)1.3;
                }
                else if (data.column_9 == 96)
                {
                    data.column_10 = 91;
                    data.term_total_wgt = (decimal)1.4;
                }
                else if (data.column_9 == 95)
                {
                    data.column_10 = 90;
                    data.term_total_wgt = (decimal)1.5;
                }

                else if (data.column_9 >= 93 && data.column_9 <= 94)
                {
                    data.column_10 = 89;
                    data.term_total_wgt = (decimal)1.6;

                }
                else if (data.column_9 >= 91 && data.column_9 <= 92)
                {
                    data.column_10 = 88;
                    data.term_total_wgt = (decimal)1.7;
                }

                else if (data.column_9 >= 89 && data.column_9 <= 90)
                {
                    data.column_10 = 87;
                    data.term_total_wgt = (decimal)1.8;

                }
                else if (data.column_9 >= 87 && data.column_9 <= 88)
                {
                    data.column_10 = 86;
                    data.term_total_wgt = (decimal)1.9;
                }

                else if (data.column_9 >= 85 && data.column_9 <= 86)
                {
                    data.column_10 = 85;
                    data.term_total_wgt = (decimal)2.0;

                }
                else if (data.column_9 >= 83 && data.column_9 <= 84)
                {
                    data.column_10 = 84;
                    data.term_total_wgt = (decimal)2.1;
                }

                else if (data.column_9 >= 81 && data.column_9 <= 82)
                {
                    data.column_10 = 83;
                    data.term_total_wgt = (decimal)2.2;

                }
                else if (data.column_9 >= 79 && data.column_9 <= 80)
                {
                    data.column_10 = 82;
                    data.term_total_wgt = (decimal)2.3;
                }

                else if (data.column_9 >= 77 && data.column_9 <= 78)
                {
                    data.column_10 = 81;
                    data.term_total_wgt = (decimal)2.4;

                }
                else if (data.column_9 >= 75 && data.column_9 <= 76)
                {
                    data.column_10 = 80;
                    data.term_total_wgt = (decimal)2.5;
                }

                else if (data.column_9 >= 70 && data.column_9 <= 74)
                {
                    data.column_10 = 79;
                    data.term_total_wgt = (decimal)2.6;

                }
                else if (data.column_9 >= 65 && data.column_9 <= 69)
                {
                    data.column_10 = 78;
                    data.term_total_wgt = (decimal)2.7;
                }

                else if (data.column_9 >= 60 && data.column_9 <= 64)
                {
                    data.column_10 = 77;
                    data.term_total_wgt = (decimal)2.8;

                }
                else if (data.column_9 >= 55 && data.column_9 <= 59)
                {
                    data.column_10 = 76;
                    data.term_total_wgt = (decimal)2.9;
                }

                else if (data.column_9 >= 50 && data.column_9 <= 54)
                {
                    data.column_10 = 75;
                    data.term_total_wgt = (decimal)3.0;

                }
                else if (data.column_9 >= 47 && data.column_9 <= 49)
                {
                    data.column_10 = 74;
                    data.term_total_wgt = (decimal)3.1;
                }

                else if (data.column_9 >= 44 && data.column_9 <= 46)
                {
                    data.column_10 = 73;
                    data.term_total_wgt = (decimal)3.2;

                }
                else if (data.column_9 >= 41 && data.column_9 <= 43)
                {
                    data.column_10 = 72;
                    data.term_total_wgt = (decimal)3.3;
                }

                else if (data.column_9 >= 38 && data.column_9 <= 40)
                {
                    data.column_10 = 71;
                    data.term_total_wgt = (decimal)3.4;

                }
                else if (data.column_9 >= 35 && data.column_9 <= 37)
                {
                    data.column_10 = 70;
                    data.term_total_wgt = (decimal)3.5;
                }

                else if (data.column_9 >= 32 && data.column_9 <= 34)
                {
                    data.column_10 = 69;
                    data.term_total_wgt = (decimal)3.6;

                }
                else if (data.column_9 >= 29 && data.column_9 <= 31)
                {
                    data.column_10 = 68;
                    data.term_total_wgt = (decimal)3.7;
                }

                else if (data.column_9 >= 26 && data.column_9 <= 28)
                {
                    data.column_10 = 67;
                    data.term_total_wgt = (decimal)3.8;

                }
                else if (data.column_9 >= 23 && data.column_9 <= 25)
                {
                    data.column_10 = 66;
                    data.term_total_wgt = (decimal)3.9;
                }

                else if (data.column_9 >= 20 && data.column_9 <= 22)
                {
                    data.column_10 = 65;
                    data.term_total_wgt = (decimal)4.0;

                }
                else if (data.column_9 >= 18 && data.column_9 <= 19)
                {
                    data.column_10 = 64;
                    data.term_total_wgt = (decimal)4.1;
                }

                else if (data.column_9 >= 16 && data.column_9 <= 17)
                {
                    data.column_10 = 63;
                    data.term_total_wgt = (decimal)4.2;

                }
                else if (data.column_9 >= 14 && data.column_9 <= 15)
                {
                    data.column_10 = 62;
                    data.term_total_wgt = (decimal)4.3;
                }

                else if (data.column_9 >= 12 && data.column_9 <= 13)
                {
                    data.column_10 = 61;
                    data.term_total_wgt = (decimal)4.4;

                }
                else if (data.column_9 >= 10 && data.column_9 <= 11)
                {
                    data.column_10 = 60;
                    data.term_total_wgt = (decimal)4.5;
                }

                else if (data.column_9 >= 8 && data.column_9 <= 9)
                {
                    data.column_10 = 59;
                    data.term_total_wgt = (decimal)4.6;

                }
                else if (data.column_9 >= 6 && data.column_9 <= 7)
                {
                    data.column_10 = 58;
                    data.term_total_wgt = (decimal)4.7;
                }
                else if (data.column_9 >= 4 && data.column_9 <= 5)
                {
                    data.column_10 = 57;
                    data.term_total_wgt = (decimal)4.8;
                }

                else if (data.column_9 >= 2 && data.column_9 <= 3)
                {
                    data.column_10 = 56;
                    data.term_total_wgt = (decimal)4.9;

                }
                else if (data.column_9 >= 0 && data.column_9 <= 1)
                {
                    data.column_10 = 55;
                    data.term_total_wgt = (decimal)5.0;
                }

            }

            classTermViewmodelBindingSource.DataSource = datas;
            setparatz();
        }

        public void getdata() {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
            var datas = (
            from cr in _context.class_Record
            join st in _context.Students on cr.stud_Id equals st.t_Id
            where cr.teach_Id ==_id && cr.sem ==sem_Id.ToString() && cr.course == courses.ToString() && cr.subject == subjects.ToString() && cr.mount == mount
            group new { cr, st } by st.t_Id into studentGroup
            select new classTermViewmodel
{

    stud_Id = studentGroup.Key,
    lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1))
                        .FirstOrDefault(),

    column_1 = studentGroup.SelectMany(s => _context.class_Record
                                                                 .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                                    .Select(q => Math.Round((q.set_Grade ?? 0), 1))
                                                                    .DefaultIfEmpty(0))
                                             .FirstOrDefault(),

    column_3 = studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault(),


    column_2 = studentGroup.SelectMany(s => _context.class_Record
                                                .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                .Select(q => (decimal?)q.set_Grade ?? 0)
                                                .DefaultIfEmpty(0))
                                                .FirstOrDefault(),


    column_4 = studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault(),
    column_5 = studentGroup.SelectMany(s => _context.class_Record
                                                .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                .Select(q => (decimal?)q.set_Grade ?? 0)
                                                .DefaultIfEmpty(0))
                                                .FirstOrDefault(),

    column_6 = studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault(),


                column_7 = studentGroup.SelectMany(s => _context.class_Record
                                              .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                              .Select(q => (decimal?)q.term_Score ?? 0)
                                              .DefaultIfEmpty(0))
                                              .FirstOrDefault(),

    column_8 = Math.Round(
                 studentGroup.SelectMany(s => _context.class_Record
                                                     .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                     .Select(q => ((decimal?)q.type_total ?? 0))
                                                     .DefaultIfEmpty(0))
                                                     .FirstOrDefault(),
                 1
             ),
    column_9 = 
                        (studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                  
                    +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                     +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                        +
                        studentGroup.SelectMany(s => _context.class_Record
                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                            .Select(q => q.type_total ?? 0)
                            .DefaultIfEmpty(0)
                        ).FirstOrDefault()

                        ),
                 
                             column_10 = 0,
                            term_total_wgt = 0,

}
                        ).ToList();

            foreach (var data in datas)
            {
                data.column_1 = (int)Math.Round(data.column_1);
                data.column_9 = (int)Math.Round(data.column_9);

                if (data.column_9 == 100)
                {
                    data.column_10 = 95;
                    data.term_total_wgt = (decimal)1.0;
                }
                else if (data.column_9 == 99)
                {
                    data.column_10 = 94;
                    data.term_total_wgt = (decimal)1.1;
                }
                else if (data.column_9 == 98)
                {
                    data.column_10 = 93;
                    data.term_total_wgt = (decimal)1.2;
                }
                else if (data.column_9 == 97)
                {
                    data.column_10 = 92;
                    data.term_total_wgt = (decimal)1.3;
                }
               else if (data.column_9 == 96)
                {
                    data.column_10 = 91;
                    data.term_total_wgt = (decimal)1.4;
                }
                else if (data.column_9 == 95)
                {
                    data.column_10 = 90;
                    data.term_total_wgt = (decimal)1.5;
                }

                else if (data.column_9 >= 93 && data.column_9 <= 94)
                {
                    data.column_10 = 89;
                    data.term_total_wgt = (decimal)1.6;

                }
                else if (data.column_9 >= 91 && data.column_9 <= 92)
                {
                    data.column_10 = 88;
                    data.term_total_wgt = (decimal)1.7;
                }

                else if (data.column_9 >= 89 && data.column_9 <= 90)
                {
                    data.column_10 = 87;
                    data.term_total_wgt = (decimal)1.8;

                }
                else if (data.column_9 >= 87 && data.column_9 <= 88)
                {
                    data.column_10 = 86;
                    data.term_total_wgt = (decimal)1.9;
                }

                else if (data.column_9 >= 85 && data.column_9 <= 86)
                {
                    data.column_10 = 85;
                    data.term_total_wgt = (decimal)2.0;

                }
                else if (data.column_9 >= 83 && data.column_9 <= 84)
                {
                    data.column_10 = 84;
                    data.term_total_wgt = (decimal)2.1;
                }

                else if (data.column_9 >= 81 && data.column_9 <= 82)
                {
                    data.column_10 = 83;
                    data.term_total_wgt = (decimal)2.2;

                }
                else if (data.column_9 >= 79 && data.column_9 <= 80)
                {
                    data.column_10 = 82;
                    data.term_total_wgt = (decimal)2.3;
                }

                else if (data.column_9 >= 77 && data.column_9 <= 78)
                {
                    data.column_10 = 81;
                    data.term_total_wgt = (decimal)2.4;

                }
                else if (data.column_9 >= 75 && data.column_9 <= 76)
                {
                    data.column_10 = 80;
                    data.term_total_wgt = (decimal)2.5;
                }

                else if (data.column_9 >= 70 && data.column_9 <= 74)
                {
                    data.column_10 = 79;
                    data.term_total_wgt = (decimal)2.6;

                }
                else if (data.column_9 >= 65 && data.column_9 <= 69)
                {
                    data.column_10 = 78;
                    data.term_total_wgt = (decimal)2.7;
                }

                else if (data.column_9 >= 60 && data.column_9 <= 64)
                {
                    data.column_10 = 77;
                    data.term_total_wgt = (decimal)2.8;

                }
                else if (data.column_9 >= 55 && data.column_9 <= 59)
                {
                    data.column_10 = 76;
                    data.term_total_wgt = (decimal)2.9;
                }

                else if (data.column_9 >= 50 && data.column_9 <= 54)
                {
                    data.column_10 = 75;
                    data.term_total_wgt = (decimal)3.0;

                }
                else if (data.column_9 >= 47 && data.column_9 <= 49)
                {
                    data.column_10 =74;
                    data.term_total_wgt = (decimal)3.1;
                }

                else if (data.column_9 >= 44 && data.column_9 <= 46)
                {
                    data.column_10 = 73;
                    data.term_total_wgt = (decimal)3.2;

                }
                else if (data.column_9 >= 41 && data.column_9 <= 43)
                {
                    data.column_10 = 72;
                    data.term_total_wgt = (decimal)3.3;
                }

                else if (data.column_9 >= 38 && data.column_9 <= 40)
                {
                    data.column_10 = 71;
                    data.term_total_wgt = (decimal)3.4;

                }
                else if (data.column_9 >= 35 && data.column_9 <= 37)
                {
                    data.column_10 = 70;
                    data.term_total_wgt = (decimal)3.5;
                }

                else if (data.column_9 >= 32 && data.column_9 <= 34)
                {
                    data.column_10 = 69;
                    data.term_total_wgt = (decimal)3.6;

                }
                else if (data.column_9 >= 29 && data.column_9 <= 31)
                {
                    data.column_10 = 68;
                    data.term_total_wgt = (decimal)3.7;
                }

                else if (data.column_9 >= 26 && data.column_9 <= 28)
                {
                    data.column_10 = 67;
                    data.term_total_wgt = (decimal)3.8;

                }
                else if (data.column_9 >= 23 && data.column_9 <= 25)
                {
                    data.column_10 = 66;
                    data.term_total_wgt = (decimal)3.9;
                }

                else if (data.column_9 >= 20 && data.column_9 <= 22)
                {
                    data.column_10 = 65;
                    data.term_total_wgt = (decimal)4.0;

                }
                else if (data.column_9 >= 18 && data.column_9 <= 19)
                {
                    data.column_10 = 64;
                    data.term_total_wgt = (decimal)4.1;
                }

                else if (data.column_9 >= 16 && data.column_9 <= 17)
                {
                    data.column_10 = 63;
                    data.term_total_wgt = (decimal)4.2;

                }
                else if (data.column_9 >= 14 && data.column_9 <= 15)
                {
                    data.column_10 = 62;
                    data.term_total_wgt = (decimal)4.3;
                }

                else if (data.column_9 >= 12 && data.column_9 <= 13)
                {
                    data.column_10 = 61;
                    data.term_total_wgt = (decimal)4.4;

                }
                else if (data.column_9 >= 10 && data.column_9 <= 11)
                {
                    data.column_10 = 60;
                    data.term_total_wgt = (decimal)4.5;
                }

                else if (data.column_9 >= 8 && data.column_9 <= 9)
                {
                    data.column_10 = 59;
                    data.term_total_wgt = (decimal)4.6;

                }
                else if (data.column_9 >= 6 && data.column_9 <= 7)
                {
                    data.column_10 = 58;
                    data.term_total_wgt = (decimal)4.7;
                }
                else if (data.column_9 >= 4 && data.column_9 <= 5)
                {
                    data.column_10 = 57;
                    data.term_total_wgt = (decimal)4.8;
                }

                else if (data.column_9 >= 2 && data.column_9 <= 3)
                {
                    data.column_10 = 56;
                    data.term_total_wgt = (decimal)4.9;

                }
                else if (data.column_9 >= 0 && data.column_9 <= 1)
                {
                    data.column_10 = 55;
                    data.term_total_wgt = (decimal)5.0;
                }

            }

        classTermViewmodelBindingSource.DataSource = datas;
            setparat();
        }

        public void getdatazz()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
            var datas = (
                from cr in _context.class_Record
                join st in _context.Students on cr.stud_Id equals st.t_Id
                where cr.teach_Id == _id && cr.sem == sem_Id.ToString() && cr.course == courses.ToString() && cr.subject == subjects.ToString() && cr.mount == mount
                group new { cr, st } by st.t_Id into studentGroup
                select new classTermViewmodel
                {

                    stud_Id = studentGroup.Key,


                    lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1))
                      .FirstOrDefault(),

                    column_1 = studentGroup.SelectMany(s => _context.class_Record
                                                                                    .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                                                    .Select(q => Math.Round((q.set_Grade ?? 0), 1))
                                                                                    .DefaultIfEmpty(0))
                                           .FirstOrDefault(),

                    column_3 = studentGroup.Select(s => _context.class_Record
                                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                                .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                                .Select(hs => hs.wgt ?? 0)
                                                                .DefaultIfEmpty()
                                                                .FirstOrDefault() / 100))
                                                            .DefaultIfEmpty()
                                                            .FirstOrDefault())
                                      .FirstOrDefault(),


                    column_2 = studentGroup.SelectMany(s => _context.class_Record
                                                                .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Performance" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                                .Select(q => (decimal?)q.set_Grade ?? 0)
                                                                .DefaultIfEmpty(0))
                      .FirstOrDefault(),


                    column_4 = studentGroup.Select(s => _context.class_Record
                                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Performance" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                            .Select(q => (q.set_Grade ?? 0) *(( (_context.high_Score
                                                                .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Performance" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                                .Select(hs => hs.wgt ?? 0)
                                                                .DefaultIfEmpty()
                                                                .FirstOrDefault())+ (_context.high_Score
                                                                .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                                .Select(hs => hs.wgt ?? 0)
                                                                .DefaultIfEmpty()
                                                                .FirstOrDefault()) )/ 100))
                                                            .DefaultIfEmpty()
                                                            .FirstOrDefault())
                                      .FirstOrDefault(),
                    column_5 = studentGroup.SelectMany(s => _context.class_Record
                                                                .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                                .Select(q => (decimal?)q.set_Grade ?? 0)
                                                                .DefaultIfEmpty(0))
                                              .FirstOrDefault(),

                    column_6 = studentGroup.Select(s => _context.class_Record
                                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                               .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                               .Select(hs => hs.wgt ?? 0)
                                                               .DefaultIfEmpty()
                                                               .FirstOrDefault() / 100))
                                                           .DefaultIfEmpty()
                                                           .FirstOrDefault())
                                      .FirstOrDefault(),


                    column_7 = studentGroup.SelectMany(s => _context.class_Record
                                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                            .Select(q => (decimal?)q.term_Score ?? 0)
                                                            .DefaultIfEmpty(0))
                                              .FirstOrDefault(),

                    column_8 = Math.Round(
               studentGroup.SelectMany(s => _context.class_Record
                                                   .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                   .Select(q => ((decimal?)q.type_total ?? 0))
                                                   .DefaultIfEmpty(0))
                                                   .FirstOrDefault(),
               1
           ),
                    column_9 =
                      (studentGroup.Select(s => _context.class_Record
                                          .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                          .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                              .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                              .Select(hs => hs.wgt ?? 0)
                                              .DefaultIfEmpty()
                                              .FirstOrDefault() / 100))
                                          .DefaultIfEmpty()
                                          .FirstOrDefault())
                                      .FirstOrDefault()

                  +
                     studentGroup.Select(s => _context.class_Record
                                         .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                         .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                             .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                             .Select(hs => hs.wgt ?? 0)
                                             .DefaultIfEmpty()
                                             .FirstOrDefault() / 100))
                                         .DefaultIfEmpty()
                                         .FirstOrDefault())
                                      .FirstOrDefault()
                                        +
                     studentGroup.Select(s => _context.class_Record
                                         .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Performance" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                         .Select(q => (q.set_Grade ?? 0) * (((_context.high_Score
                                             .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Performance" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                             .Select(hs => hs.wgt ?? 0)
                                             .DefaultIfEmpty()
                                             .FirstOrDefault())+ (_context.high_Score
                                             .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                             .Select(hs => hs.wgt ?? 0)
                                             .DefaultIfEmpty()
                                             .FirstOrDefault()) )/ 100))
                                         .DefaultIfEmpty()
                                         .FirstOrDefault())
                                      .FirstOrDefault()
                      +
                      studentGroup.SelectMany(s => _context.class_Record
                          .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                          .Select(q => q.type_total ?? 0)
                          .DefaultIfEmpty(0)
                      ).FirstOrDefault()

                      ),

                    column_10 = 0,
                    term_total_wgt = 0,

                }
                      ).ToList();

            foreach (var data in datas)
            {
                data.column_9 = (int)Math.Round(data.column_9);
                if (data.column_9 == 100)
                {
                    data.column_10 = 95;
                    data.term_total_wgt = (decimal)1.0;
                }
                else if (data.column_9 == 99)
                {
                    data.column_10 = 94;
                    data.term_total_wgt = (decimal)1.1;
                }
                else if (data.column_9 == 98)
                {
                    data.column_10 = 93;
                    data.term_total_wgt = (decimal)1.2;
                }
                else if (data.column_9 == 97)
                {
                    data.column_10 = 92;
                    data.term_total_wgt = (decimal)1.3;
                }
                else if (data.column_9 == 96)
                {
                    data.column_10 = 91;
                    data.term_total_wgt = (decimal)1.4;
                }
                else if (data.column_9 == 95)
                {
                    data.column_10 = 90;
                    data.term_total_wgt = (decimal)1.5;
                }

                else if (data.column_9 >= 93 && data.column_9 <= 94)
                {
                    data.column_10 = 89;
                    data.term_total_wgt = (decimal)1.6;

                }
                else if (data.column_9 >= 91 && data.column_9 <= 92)
                {
                    data.column_10 = 88;
                    data.term_total_wgt = (decimal)1.7;
                }

                else if (data.column_9 >= 89 && data.column_9 <= 90)
                {
                    data.column_10 = 87;
                    data.term_total_wgt = (decimal)1.8;

                }
                else if (data.column_9 >= 87 && data.column_9 <= 88)
                {
                    data.column_10 = 86;
                    data.term_total_wgt = (decimal)1.9;
                }

                else if (data.column_9 >= 85 && data.column_9 <= 86)
                {
                    data.column_10 = 85;
                    data.term_total_wgt = (decimal)2.0;

                }
                else if (data.column_9 >= 83 && data.column_9 <= 84)
                {
                    data.column_10 = 84;
                    data.term_total_wgt = (decimal)2.1;
                }

                else if (data.column_9 >= 81 && data.column_9 <= 82)
                {
                    data.column_10 = 83;
                    data.term_total_wgt = (decimal)2.2;

                }
                else if (data.column_9 >= 79 && data.column_9 <= 80)
                {
                    data.column_10 = 82;
                    data.term_total_wgt = (decimal)2.3;
                }

                else if (data.column_9 >= 77 && data.column_9 <= 78)
                {
                    data.column_10 = 81;
                    data.term_total_wgt = (decimal)2.4;

                }
                else if (data.column_9 >= 75 && data.column_9 <= 76)
                {
                    data.column_10 = 80;
                    data.term_total_wgt = (decimal)2.5;
                }

                else if (data.column_9 >= 70 && data.column_9 <= 74)
                {
                    data.column_10 = 79;
                    data.term_total_wgt = (decimal)2.6;

                }
                else if (data.column_9 >= 65 && data.column_9 <= 69)
                {
                    data.column_10 = 78;
                    data.term_total_wgt = (decimal)2.7;
                }

                else if (data.column_9 >= 60 && data.column_9 <= 64)
                {
                    data.column_10 = 77;
                    data.term_total_wgt = (decimal)2.8;

                }
                else if (data.column_9 >= 55 && data.column_9 <= 59)
                {
                    data.column_10 = 76;
                    data.term_total_wgt = (decimal)2.9;
                }

                else if (data.column_9 >= 50 && data.column_9 <= 54)
                {
                    data.column_10 = 75;
                    data.term_total_wgt = (decimal)3.0;

                }
                else if (data.column_9 >= 47 && data.column_9 <= 49)
                {
                    data.column_10 = 74;
                    data.term_total_wgt = (decimal)3.1;
                }

                else if (data.column_9 >= 44 && data.column_9 <= 46)
                {
                    data.column_10 = 73;
                    data.term_total_wgt = (decimal)3.2;

                }
                else if (data.column_9 >= 41 && data.column_9 <= 43)
                {
                    data.column_10 = 72;
                    data.term_total_wgt = (decimal)3.3;
                }

                else if (data.column_9 >= 38 && data.column_9 <= 40)
                {
                    data.column_10 = 71;
                    data.term_total_wgt = (decimal)3.4;

                }
                else if (data.column_9 >= 35 && data.column_9 <= 37)
                {
                    data.column_10 = 70;
                    data.term_total_wgt = (decimal)3.5;
                }

                else if (data.column_9 >= 32 && data.column_9 <= 34)
                {
                    data.column_10 = 69;
                    data.term_total_wgt = (decimal)3.6;

                }
                else if (data.column_9 >= 29 && data.column_9 <= 31)
                {
                    data.column_10 = 68;
                    data.term_total_wgt = (decimal)3.7;
                }

                else if (data.column_9 >= 26 && data.column_9 <= 28)
                {
                    data.column_10 = 67;
                    data.term_total_wgt = (decimal)3.8;

                }
                else if (data.column_9 >= 23 && data.column_9 <= 25)
                {
                    data.column_10 = 66;
                    data.term_total_wgt = (decimal)3.9;
                }

                else if (data.column_9 >= 20 && data.column_9 <= 22)
                {
                    data.column_10 = 65;
                    data.term_total_wgt = (decimal)4.0;

                }
                else if (data.column_9 >= 18 && data.column_9 <= 19)
                {
                    data.column_10 = 64;
                    data.term_total_wgt = (decimal)4.1;
                }

                else if (data.column_9 >= 16 && data.column_9 <= 17)
                {
                    data.column_10 = 63;
                    data.term_total_wgt = (decimal)4.2;

                }
                else if (data.column_9 >= 14 && data.column_9 <= 15)
                {
                    data.column_10 = 62;
                    data.term_total_wgt = (decimal)4.3;
                }

                else if (data.column_9 >= 12 && data.column_9 <= 13)
                {
                    data.column_10 = 61;
                    data.term_total_wgt = (decimal)4.4;

                }
                else if (data.column_9 >= 10 && data.column_9 <= 11)
                {
                    data.column_10 = 60;
                    data.term_total_wgt = (decimal)4.5;
                }

                else if (data.column_9 >= 8 && data.column_9 <= 9)
                {
                    data.column_10 = 59;
                    data.term_total_wgt = (decimal)4.6;

                }
                else if (data.column_9 >= 6 && data.column_9 <= 7)
                {
                    data.column_10 = 58;
                    data.term_total_wgt = (decimal)4.7;
                }
                else if (data.column_9 >= 4 && data.column_9 <= 5)
                {
                    data.column_10 = 57;
                    data.term_total_wgt = (decimal)4.8;
                }

                else if (data.column_9 >= 2 && data.column_9 <= 3)
                {
                    data.column_10 = 56;
                    data.term_total_wgt = (decimal)4.9;

                }
                else if (data.column_9 >= 0 && data.column_9 <= 1)
                {
                    data.column_10 = 55;
                    data.term_total_wgt = (decimal)5.0;
                }

            }

            classTermViewmodelBindingSource.DataSource = datas;
            setparatszz();
        }
        public void getdatas()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
            var datas = (
                from cr in _context.class_Record
                join st in _context.Students on cr.stud_Id equals st.t_Id
                where cr.teach_Id == _id && cr.sem == sem_Id.ToString() && cr.course == courses.ToString() && cr.subject == subjects.ToString() && cr.mount == mount
                group new { cr, st } by st.t_Id into studentGroup
                select new classTermViewmodel
{

  stud_Id = studentGroup.Key,


  lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1))
                      .FirstOrDefault(),

  column_1 = studentGroup.SelectMany(s => _context.class_Record
                                                                  .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                                  .Select(q => Math.Round((q.set_Grade ?? 0), 1))
                                                                  .DefaultIfEmpty(0))
                                           .FirstOrDefault(),

  column_3 = studentGroup.Select(s => _context.class_Record
                                          .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                          .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                              .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                              .Select(hs => hs.wgt ?? 0)
                                              .DefaultIfEmpty()
                                              .FirstOrDefault() / 100))
                                          .DefaultIfEmpty()
                                          .FirstOrDefault())
                                      .FirstOrDefault(),


  column_2 = studentGroup.SelectMany(s => _context.class_Record
                                              .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                              .Select(q => (decimal?)q.set_Grade ?? 0)
                                              .DefaultIfEmpty(0))
                      .FirstOrDefault(),


  column_4 = studentGroup.Select(s => _context.class_Record
                                          .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                          .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                              .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                              .Select(hs => hs.wgt ?? 0)
                                              .DefaultIfEmpty()
                                              .FirstOrDefault() / 100))
                                          .DefaultIfEmpty()
                                          .FirstOrDefault())
                                      .FirstOrDefault(),
  column_5 = studentGroup.SelectMany(s => _context.class_Record
                                              .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                              .Select(q => (decimal?)q.set_Grade ?? 0)
                                              .DefaultIfEmpty(0))
                                              .FirstOrDefault(),

  column_6 = studentGroup.Select(s => _context.class_Record
                                         .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                         .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                             .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                             .Select(hs => hs.wgt ?? 0)
                                             .DefaultIfEmpty()
                                             .FirstOrDefault() / 100))
                                         .DefaultIfEmpty()
                                         .FirstOrDefault())
                                      .FirstOrDefault(),


    column_7 = studentGroup.SelectMany(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (decimal?)q.term_Score ?? 0)
                                            .DefaultIfEmpty(0))
                                              .FirstOrDefault(),

  column_8 = Math.Round(
               studentGroup.SelectMany(s => _context.class_Record
                                                   .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                                   .Select(q => ((decimal?)q.type_total ?? 0))
                                                   .DefaultIfEmpty(0))
                                                   .FirstOrDefault(),
               1
           ),
  column_9 =
                      (studentGroup.Select(s => _context.class_Record
                                          .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                          .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                              .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                              .Select(hs => hs.wgt ?? 0)
                                              .DefaultIfEmpty()
                                              .FirstOrDefault() / 100))
                                          .DefaultIfEmpty()
                                          .FirstOrDefault())
                                      .FirstOrDefault()
              
                  +
                     studentGroup.Select(s => _context.class_Record
                                         .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                         .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                             .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                             .Select(hs => hs.wgt ?? 0)
                                             .DefaultIfEmpty()
                                             .FirstOrDefault() / 100))
                                         .DefaultIfEmpty()
                                         .FirstOrDefault())
                                      .FirstOrDefault()
                                        +
                     studentGroup.Select(s => _context.class_Record
                                         .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                         .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                             .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                             .Select(hs => hs.wgt ?? 0)
                                             .DefaultIfEmpty()
                                             .FirstOrDefault() / 100))
                                         .DefaultIfEmpty()
                                         .FirstOrDefault())
                                      .FirstOrDefault()
                      +
                      studentGroup.SelectMany(s => _context.class_Record
                          .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                          .Select(q => q.type_total ?? 0)
                          .DefaultIfEmpty(0)
                      ).FirstOrDefault()

                      ),

  column_10 = 0,
  term_total_wgt = 0,

}
                      ).ToList();

            foreach (var data in datas)
            {
                data.column_9 = (int)Math.Round(data.column_9);
                if (data.column_9 == 100)
                {
                    data.column_10 = 95;
                    data.term_total_wgt = (decimal)1.0;
                }
                else if (data.column_9 == 99)
                {
                    data.column_10 = 94;
                    data.term_total_wgt = (decimal)1.1;
                }
                else if (data.column_9 == 98)
                {
                    data.column_10 = 93;
                    data.term_total_wgt = (decimal)1.2;
                }
                else if (data.column_9 == 97)
                {
                    data.column_10 = 92;
                    data.term_total_wgt = (decimal)1.3;
                }
                else if (data.column_9 == 96)
                {
                    data.column_10 = 91;
                    data.term_total_wgt = (decimal)1.4;
                }
                else if (data.column_9 == 95)
                {
                    data.column_10 = 90;
                    data.term_total_wgt = (decimal)1.5;
                }

                else if (data.column_9 >= 93 && data.column_9 <= 94)
                {
                    data.column_10 = 89;
                    data.term_total_wgt = (decimal)1.6;

                }
                else if (data.column_9 >= 91 && data.column_9 <= 92)
                {
                    data.column_10 = 88;
                    data.term_total_wgt = (decimal)1.7;
                }

                else if (data.column_9 >= 89 && data.column_9 <= 90)
                {
                    data.column_10 = 87;
                    data.term_total_wgt = (decimal)1.8;

                }
                else if (data.column_9 >= 87 && data.column_9 <= 88)
                {
                    data.column_10 = 86;
                    data.term_total_wgt = (decimal)1.9;
                }

                else if (data.column_9 >= 85 && data.column_9 <= 86)
                {
                    data.column_10 = 85;
                    data.term_total_wgt = (decimal)2.0;

                }
                else if (data.column_9 >= 83 && data.column_9 <= 84)
                {
                    data.column_10 = 84;
                    data.term_total_wgt = (decimal)2.1;
                }

                else if (data.column_9 >= 81 && data.column_9 <= 82)
                {
                    data.column_10 = 83;
                    data.term_total_wgt = (decimal)2.2;

                }
                else if (data.column_9 >= 79 && data.column_9 <= 80)
                {
                    data.column_10 = 82;
                    data.term_total_wgt = (decimal)2.3;
                }

                else if (data.column_9 >= 77 && data.column_9 <= 78)
                {
                    data.column_10 = 81;
                    data.term_total_wgt = (decimal)2.4;

                }
                else if (data.column_9 >= 75 && data.column_9 <= 76)
                {
                    data.column_10 = 80;
                    data.term_total_wgt = (decimal)2.5;
                }

                else if (data.column_9 >= 70 && data.column_9 <= 74)
                {
                    data.column_10 = 79;
                    data.term_total_wgt = (decimal)2.6;

                }
                else if (data.column_9 >= 65 && data.column_9 <= 69)
                {
                    data.column_10 = 78;
                    data.term_total_wgt = (decimal)2.7;
                }

                else if (data.column_9 >= 60 && data.column_9 <= 64)
                {
                    data.column_10 = 77;
                    data.term_total_wgt = (decimal)2.8;

                }
                else if (data.column_9 >= 55 && data.column_9 <= 59)
                {
                    data.column_10 = 76;
                    data.term_total_wgt = (decimal)2.9;
                }

                else if (data.column_9 >= 50 && data.column_9 <= 54)
                {
                    data.column_10 = 75;
                    data.term_total_wgt = (decimal)3.0;

                }
                else if (data.column_9 >= 47 && data.column_9 <= 49)
                {
                    data.column_10 = 74;
                    data.term_total_wgt = (decimal)3.1;
                }

                else if (data.column_9 >= 44 && data.column_9 <= 46)
                {
                    data.column_10 = 73;
                    data.term_total_wgt = (decimal)3.2;

                }
                else if (data.column_9 >= 41 && data.column_9 <= 43)
                {
                    data.column_10 = 72;
                    data.term_total_wgt = (decimal)3.3;
                }

                else if (data.column_9 >= 38 && data.column_9 <= 40)
                {
                    data.column_10 = 71;
                    data.term_total_wgt = (decimal)3.4;

                }
                else if (data.column_9 >= 35 && data.column_9 <= 37)
                {
                    data.column_10 = 70;
                    data.term_total_wgt = (decimal)3.5;
                }

                else if (data.column_9 >= 32 && data.column_9 <= 34)
                {
                    data.column_10 = 69;
                    data.term_total_wgt = (decimal)3.6;

                }
                else if (data.column_9 >= 29 && data.column_9 <= 31)
                {
                    data.column_10 = 68;
                    data.term_total_wgt = (decimal)3.7;
                }

                else if (data.column_9 >= 26 && data.column_9 <= 28)
                {
                    data.column_10 = 67;
                    data.term_total_wgt = (decimal)3.8;

                }
                else if (data.column_9 >= 23 && data.column_9 <= 25)
                {
                    data.column_10 = 66;
                    data.term_total_wgt = (decimal)3.9;
                }

                else if (data.column_9 >= 20 && data.column_9 <= 22)
                {
                    data.column_10 = 65;
                    data.term_total_wgt = (decimal)4.0;

                }
                else if (data.column_9 >= 18 && data.column_9 <= 19)
                {
                    data.column_10 = 64;
                    data.term_total_wgt = (decimal)4.1;
                }

                else if (data.column_9 >= 16 && data.column_9 <= 17)
                {
                    data.column_10 = 63;
                    data.term_total_wgt = (decimal)4.2;

                }
                else if (data.column_9 >= 14 && data.column_9 <= 15)
                {
                    data.column_10 = 62;
                    data.term_total_wgt = (decimal)4.3;
                }

                else if (data.column_9 >= 12 && data.column_9 <= 13)
                {
                    data.column_10 = 61;
                    data.term_total_wgt = (decimal)4.4;

                }
                else if (data.column_9 >= 10 && data.column_9 <= 11)
                {
                    data.column_10 = 60;
                    data.term_total_wgt = (decimal)4.5;
                }

                else if (data.column_9 >= 8 && data.column_9 <= 9)
                {
                    data.column_10 = 59;
                    data.term_total_wgt = (decimal)4.6;

                }
                else if (data.column_9 >= 6 && data.column_9 <= 7)
                {
                    data.column_10 = 58;
                    data.term_total_wgt = (decimal)4.7;
                }
                else if (data.column_9 >= 4 && data.column_9 <= 5)
                {
                    data.column_10 = 57;
                    data.term_total_wgt = (decimal)4.8;
                }

                else if (data.column_9 >= 2 && data.column_9 <= 3)
                {
                    data.column_10 = 56;
                    data.term_total_wgt = (decimal)4.9;

                }
                else if (data.column_9 >= 0 && data.column_9 <= 1)
                {
                    data.column_10 = 55;
                    data.term_total_wgt = (decimal)5.0;
                }

            }

            classTermViewmodelBindingSource.DataSource = datas;
            setparats();
        }

        public void setparat()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
            var jyz = _context.class_Record
                .Where(q => q.term_exam == "Midterm" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                .Select(s => s.wgt)
                .FirstOrDefault() ?? 0; // If jyz is null, default to 0

            var jy = _context.high_Score.Where(q => q.term_exam == "Midterm" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount).ToList();

            List<ReportParameter> parameters = new List<ReportParameter>();

            bool hasOral = false; // Flag to check if "Oral" is present in the database
            bool hasquiz = false;
            bool hasproject = false;
            bool hasperformance = false;
            foreach (var j in jy)
            {
                if (j.typeof_column == "Quizzes")
                {
                    hasquiz = true;
                    parameters.Add(new ReportParameter("quiz", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
            
                else if (j.typeof_column == "Oral")
                {
                    hasOral = true;
                    parameters.Add(new ReportParameter("oral", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
                else if (j.typeof_column == "Project")
                {
                    hasOral = true;
                    parameters.Add(new ReportParameter("performance", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
            }

       
            if (!hasOral)
            {
                parameters.Add(new ReportParameter("oral", "N/A")); 
            }
            if (!hasquiz)
            {
                parameters.Add(new ReportParameter("quiz", "N/A"));
            }
            if (!hasproject)
            {
                parameters.Add(new ReportParameter("performance", "N/A"));
            }
            parameters.Add(new ReportParameter("term", $"{jyz.ToString("N0")}%"));
            parameters.Add(new ReportParameter("project", "Project"));
            this.reportViewer1.LocalReport.SetParameters(parameters.ToArray());
            this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            this.reportViewer1.Width = 915;
            this.reportViewer1.RefreshReport();
        }

        public void setparatz()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
            var jyz = _context.class_Record
                .Where(q => q.term_exam == "Midterm" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                .Select(s => s.wgt)
                .FirstOrDefault() ?? 0; // If jyz is null, default to 0

            var jy = _context.high_Score.Where(q => q.term_exam == "Midterm" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount).ToList();

            List<ReportParameter> parameters = new List<ReportParameter>();

            bool hasOral = false; // Flag to check if "Oral" is present in the database
            bool hasquiz = false;
            bool hasproject = false;
            bool hasperformance = false;

            decimal combinedPerformanceWgt = 0;
            decimal combinedProjectWgt = 0;
            foreach (var j in jy)
            {
                if (j.typeof_column == "Quizzes")
                {
                    hasquiz = true;
                    parameters.Add(new ReportParameter("quiz", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }

                else if (j.typeof_column == "Oral")
                {
                    hasOral = true;
                    parameters.Add(new ReportParameter("oral", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
                else if (j.typeof_column == "Project"  )
                {
                    hasproject = true;
                    combinedProjectWgt += j.wgt ?? 0;
                }
                else if (j.typeof_column == "Performance")
                {
                    hasperformance = true;
                    combinedPerformanceWgt += j.wgt ?? 0;
                }

            }


            if (hasproject && hasperformance)
            {
                decimal combinedWgt = Math.Round((combinedPerformanceWgt + combinedProjectWgt),0);
                parameters.Add(new ReportParameter("performance", $"{combinedWgt.ToString("N0") ?? "0"}%"));
            }
            if (!hasquiz)
            {
                parameters.Add(new ReportParameter("quiz", "N/A"));
            }
            if (!hasOral)
            {
                parameters.Add(new ReportParameter("oral", "N/A"));
            }

            parameters.Add(new ReportParameter("term", $"{jyz.ToString("N0")}%"));
            parameters.Add(new ReportParameter("project", "Performance"));
            this.reportViewer1.LocalReport.SetParameters(parameters.ToArray());
            this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            this.reportViewer1.Width = 915;
            this.reportViewer1.RefreshReport();
        }
        public void setparats()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
            var jyz = _context.class_Record
                .Where(q => q.term_exam == "Final" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                .Select(s => s.wgt)
                .FirstOrDefault() ?? 0; // If jyz is null, default to 0

            var jy = _context.high_Score.Where(q => q.term_exam == "Final" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount).ToList();

            List<ReportParameter> parameters = new List<ReportParameter>();

            bool hasOral = false; // Flag to check if "Oral" is present in the database
            bool hasquiz = false;
            bool hasproject = false;
            bool hasperformance = false;
            foreach (var j in jy)
            {
                if (j.typeof_column == "Quizzes")
                {
                    hasquiz = true;
                    parameters.Add(new ReportParameter("quiz", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
                else if (j.typeof_column == "Oral")
                {
                    hasOral = true;
                    parameters.Add(new ReportParameter("oral", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
                else if (j.typeof_column == "Project")
                {
                    hasOral = true;
                    parameters.Add(new ReportParameter("performance", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
            }


            if (!hasOral)
            {
                parameters.Add(new ReportParameter("oral", "N/A"));
            }
            if (!hasquiz)
            {
                parameters.Add(new ReportParameter("quiz", "N/A"));
            }
            if (!hasproject)
            {
                parameters.Add(new ReportParameter("performance", "N/A"));
            }
            parameters.Add(new ReportParameter("project", "Project"));
            parameters.Add(new ReportParameter("term", $"{jyz.ToString("N0")}%"));

            this.reportViewer1.LocalReport.SetParameters(parameters.ToArray());
            this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            this.reportViewer1.Width = 915;
            this.reportViewer1.RefreshReport();
        }
        public void setparatszz()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
            var jyz = _context.class_Record
                .Where(q => q.term_exam == "Final" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                .Select(s => s.wgt)
                .FirstOrDefault() ?? 0; // If jyz is null, default to 0

            var jy = _context.high_Score.Where(q => q.term_exam == "Final" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount).ToList();

            List<ReportParameter> parameters = new List<ReportParameter>();

            bool hasOral = false; // Flag to check if "Oral" is present in the database
            bool hasquiz = false;
            bool hasproject = false;
            bool hasperformance = false;
            decimal combinedPerformanceWgt = 0;
            decimal combinedProjectWgt = 0;
            foreach (var j in jy)
            {
                if (j.typeof_column == "Quizzes")
                {
                    hasquiz = true;
                    parameters.Add(new ReportParameter("quiz", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
                else if (j.typeof_column == "Oral")
                {
                    hasOral = true;
                    parameters.Add(new ReportParameter("oral", (j.wgt?.ToString("N0") ?? "0") + "%"));
                }
                else if (j.typeof_column == "Project")
                {
                    hasproject = true;
                    combinedProjectWgt += j.wgt ?? 0;
                }
                else if (j.typeof_column == "Performance")
                {
                    hasperformance = true;
                    combinedPerformanceWgt += j.wgt ?? 0;
                }
            }



            if (hasproject && hasperformance)
            {
                decimal combinedWgt = Math.Round((combinedPerformanceWgt + combinedProjectWgt), 0);
                parameters.Add(new ReportParameter("performance", $"{combinedWgt.ToString("N0") ?? "0"}%"));
            }
            if (!hasquiz)
            {
                parameters.Add(new ReportParameter("quiz", "N/A"));
            }
            if (!hasOral)
            {
                parameters.Add(new ReportParameter("oral", "N/A"));
            }
            parameters.Add(new ReportParameter("project", "Performance"));
            parameters.Add(new ReportParameter("term", $"{jyz.ToString("N0")}%"));

            this.reportViewer1.LocalReport.SetParameters(parameters.ToArray());
            this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            this.reportViewer1.Width = 915;
            this.reportViewer1.RefreshReport();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
 
            guna2TextBox3.Text = "Midterm";
            guna2Button3.FillColor = Color.MediumTurquoise;
            guna2Button1.FillColor = Color.MediumTurquoise;
            guna2Button2.FillColor = Color.Orange;
            reportViewer1.Visible = true;
            reportViewer2.Visible = false;

            if (mount == "lec")
            {
                getdata();
            }
            if (mount == "lab") {
                getdataz();
            
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2TextBox3.Text = "Final";
            guna2Button3.FillColor = Color.MediumTurquoise;
            guna2Button1.FillColor = Color.Orange;
            guna2Button2.FillColor = Color.MediumTurquoise;
            reportViewer1.Visible = true;
            reportViewer2.Visible = false;
            if (mount == "lec")
            {
                getdatas();
            }
            if (mount == "lab")
            {
                getdatazz();

            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2TextBox3.Text = "Final Ratings"; ;
            guna2Button3.FillColor =Color.Orange;
            guna2Button1.FillColor = Color.MediumTurquoise;
            guna2Button2.FillColor = Color.MediumTurquoise;
            reportViewer1.Visible = false;
            reportViewer2.Visible = true;
            if (mount == "lec")
            {
                getdatass();
            }
            if (mount == "lab") {

                getdatassz();
            }
        }



        //for final Rating
        public void getdatassz()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();

            var datas = (
            from cr in _context.class_Record
            join st in _context.Students on cr.stud_Id equals st.t_Id
            where cr.teach_Id == _id && cr.sem == sem_Id.ToString() && cr.course == courses.ToString() && cr.subject == subjects.ToString() && cr.mount == mount
            group new { cr, st } by st.t_Id into studentGroup
            select new classTermViewmodel
            {

                stud_Id = studentGroup.Key,
                lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1))
                        .FirstOrDefault(),
                column_1 =
                        (studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                    +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                                          +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Performance" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (((_context.high_Score
                                               .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Performance" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault())+ (_context.high_Score
                                               .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault()) )/ 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                        +
                        studentGroup.SelectMany(s => _context.class_Record
                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                            .Select(q => q.type_total ?? 0)
                            .DefaultIfEmpty(0)
                        ).FirstOrDefault()

                        ),

                column_9 =
                        (studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                    +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                                         +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (((_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault())+ (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault()))/ 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                        +
                        studentGroup.SelectMany(s => _context.class_Record
                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                            .Select(q => q.type_total ?? 0)
                            .DefaultIfEmpty(0)
                        ).FirstOrDefault()

                        ),
                column_2 = 0,
                Middlename = "",
                column_10 = 0,
                term_total_wgt = 0,

            }
                        ).ToList();

            foreach (var data in datas)
            {
                data.column_9 = (int)Math.Round(data.column_9);
                data.column_1 = (int)Math.Round(data.column_1);
                //check the column_9
                if (data.column_9 == 100)
                {

                    data.term_total_wgt = (decimal)1.0;
                }

                else if (data.column_9 == 99)
                {

                    data.term_total_wgt = (decimal)1.1;
                }
                else if (data.column_9 == 98)
                {

                    data.term_total_wgt = (decimal)1.2;
                }
                else if (data.column_9 == 97)
                {

                    data.term_total_wgt = (decimal)1.3;
                }
                else if (data.column_9 == 96)
                {

                    data.term_total_wgt = (decimal)1.4;
                }
                else if (data.column_9 == 95)
                {

                    data.term_total_wgt = (decimal)1.5;
                }

                else if (data.column_9 >= 93 && data.column_9 <= 94)
                {

                    data.term_total_wgt = (decimal)1.6;

                }
                else if (data.column_9 >= 91 && data.column_9 <= 92)
                {

                    data.term_total_wgt = (decimal)1.7;
                }

                else if (data.column_9 >= 89 && data.column_9 <= 90)
                {

                    data.term_total_wgt = (decimal)1.8;

                }
                else if (data.column_9 >= 87 && data.column_9 <= 88)
                {

                    data.term_total_wgt = (decimal)1.9;
                }

                else if (data.column_9 >= 85 && data.column_9 <= 86)
                {

                    data.term_total_wgt = (decimal)2.0;

                }
                else if (data.column_9 >= 83 && data.column_9 <= 84)
                {

                    data.term_total_wgt = (decimal)2.1;
                }

                else if (data.column_9 >= 81 && data.column_9 <= 82)
                {

                    data.term_total_wgt = (decimal)2.2;

                }
                else if (data.column_9 >= 79 && data.column_9 <= 80)
                {
                    data.term_total_wgt = (decimal)2.3;
                }

                else if (data.column_9 >= 77 && data.column_9 <= 78)
                {
                    data.term_total_wgt = (decimal)2.4;

                }
                else if (data.column_9 >= 75 && data.column_9 <= 76)
                {

                    data.term_total_wgt = (decimal)2.5;
                }

                else if (data.column_9 >= 70 && data.column_9 <= 74)
                {

                    data.term_total_wgt = (decimal)2.6;

                }
                else if (data.column_9 >= 65 && data.column_9 <= 69)
                {

                    data.term_total_wgt = (decimal)2.7;
                }

                else if (data.column_9 >= 60 && data.column_9 <= 64)
                {

                    data.term_total_wgt = (decimal)2.8;

                }
                else if (data.column_9 >= 55 && data.column_9 <= 59)
                {

                    data.term_total_wgt = (decimal)2.9;
                }

                else if (data.column_9 >= 50 && data.column_9 <= 54)
                {

                    data.term_total_wgt = (decimal)3.0;

                }
                else if (data.column_9 >= 47 && data.column_9 <= 49)
                {

                    data.term_total_wgt = (decimal)3.1;
                }

                else if (data.column_9 >= 44 && data.column_9 <= 46)
                {

                    data.term_total_wgt = (decimal)3.2;

                }
                else if (data.column_9 >= 41 && data.column_9 <= 43)
                {

                    data.term_total_wgt = (decimal)3.3;
                }

                else if (data.column_9 >= 38 && data.column_9 <= 40)
                {

                    data.term_total_wgt = (decimal)3.4;

                }
                else if (data.column_9 >= 35 && data.column_9 <= 37)
                {

                    data.term_total_wgt = (decimal)3.5;
                }

                else if (data.column_9 >= 32 && data.column_9 <= 34)
                {

                    data.term_total_wgt = (decimal)3.6;

                }
                else if (data.column_9 >= 29 && data.column_9 <= 31)
                {

                    data.term_total_wgt = (decimal)3.7;
                }

                else if (data.column_9 >= 26 && data.column_9 <= 28)
                {

                    data.term_total_wgt = (decimal)3.8;

                }
                else if (data.column_9 >= 23 && data.column_9 <= 25)
                {

                    data.term_total_wgt = (decimal)3.9;
                }

                else if (data.column_9 >= 20 && data.column_9 <= 22)
                {

                    data.term_total_wgt = (decimal)4.0;

                }
                else if (data.column_9 >= 18 && data.column_9 <= 19)
                {

                    data.term_total_wgt = (decimal)4.1;
                }

                else if (data.column_9 >= 16 && data.column_9 <= 17)
                {

                    data.term_total_wgt = (decimal)4.2;

                }
                else if (data.column_9 >= 14 && data.column_9 <= 15)
                {

                    data.term_total_wgt = (decimal)4.3;
                }

                else if (data.column_9 >= 12 && data.column_9 <= 13)
                {

                    data.term_total_wgt = (decimal)4.4;

                }
                else if (data.column_9 >= 10 && data.column_9 <= 11)
                {

                    data.term_total_wgt = (decimal)4.5;
                }

                else if (data.column_9 >= 8 && data.column_9 <= 9)
                {

                    data.term_total_wgt = (decimal)4.6;

                }
                else if (data.column_9 >= 6 && data.column_9 <= 7)
                {

                    data.term_total_wgt = (decimal)4.7;
                }
                else if (data.column_9 >= 4 && data.column_9 <= 5)
                {

                    data.term_total_wgt = (decimal)4.8;
                }

                else if (data.column_9 >= 2 && data.column_9 <= 3)
                {

                    data.term_total_wgt = (decimal)4.9;

                }
                else if (data.column_9 >= 0 && data.column_9 <= 1)
                {

                    data.term_total_wgt = (decimal)5.0;
                }

                //check the column_1
                if (data.column_1 == 100)
                {

                    data.column_10 = (decimal)1.0;
                }

                else if (data.column_1 == 99)
                {
                    data.column_10 = (decimal)1.1;

                }
                else if (data.column_1 == 98)
                {
                    data.column_10 = (decimal)1.2;

                }
                else if (data.column_1 == 97)
                {

                    data.column_10 = (decimal)1.3;
                }
                else if (data.column_1 == 96)
                {

                    data.column_10 = (decimal)1.4;
                }
                else if (data.column_1 == 95)
                {

                    data.column_10 = (decimal)1.5;
                }

                else if (data.column_1 >= 93 && data.column_1 <= 94)
                {

                    data.column_10 = (decimal)1.6;

                }
                else if (data.column_1 >= 91 && data.column_1 <= 92)
                {

                    data.column_10 = (decimal)1.7;
                }

                else if (data.column_1 >= 89 && data.column_1 <= 90)
                {

                    data.column_10 = (decimal)1.8;

                }
                else if (data.column_1 >= 87 && data.column_1 <= 88)
                {

                    data.column_10 = (decimal)1.9;
                }

                else if (data.column_1 >= 85 && data.column_1 <= 86)
                {

                    data.column_10 = (decimal)2.0;

                }
                else if (data.column_1 >= 83 && data.column_1 <= 84)
                {

                    data.column_10 = (decimal)2.1;
                }

                else if (data.column_1 >= 81 && data.column_1 <= 82)
                {

                    data.column_10 = (decimal)2.2;

                }
                else if (data.column_1 >= 79 && data.column_1 <= 80)
                {

                    data.column_10 = (decimal)2.3;
                }

                else if (data.column_1 >= 77 && data.column_1 <= 78)
                {

                    data.column_10 = (decimal)2.4;

                }
                else if (data.column_1 >= 75 && data.column_1 <= 76)
                {

                    data.column_10 = (decimal)2.5;
                }

                else if (data.column_1 >= 70 && data.column_1 <= 74)
                {

                    data.column_10 = (decimal)2.6;

                }
                else if (data.column_1 >= 65 && data.column_1 <= 69)
                {

                    data.column_10 = (decimal)2.7;
                }

                else if (data.column_1 >= 60 && data.column_1 <= 64)
                {

                    data.column_10 = (decimal)2.8;

                }
                else if (data.column_1 >= 55 && data.column_1 <= 59)
                {

                    data.column_10 = (decimal)2.9;
                }

                else if (data.column_1 >= 50 && data.column_1 <= 54)
                {

                    data.column_10 = (decimal)3.0;

                }
                else if (data.column_1 >= 47 && data.column_1 <= 49)
                {

                    data.column_10 = (decimal)3.1;
                }

                else if (data.column_1 >= 44 && data.column_1 <= 46)
                {

                    data.column_10 = (decimal)3.2;

                }
                else if (data.column_1 >= 41 && data.column_1 <= 43)
                {

                    data.column_10 = (decimal)3.3;
                }

                else if (data.column_1 >= 38 && data.column_1 <= 40)
                {

                    data.column_10 = (decimal)3.4;

                }
                else if (data.column_1 >= 35 && data.column_1 <= 37)
                {

                    data.column_10 = (decimal)3.5;
                }

                else if (data.column_1 >= 32 && data.column_1 <= 34)
                {

                    data.column_10 = (decimal)3.6;

                }
                else if (data.column_1 >= 29 && data.column_1 <= 31)
                {

                    data.column_10 = (decimal)3.7;
                }

                else if (data.column_1 >= 26 && data.column_1 <= 28)
                {

                    data.column_10 = (decimal)3.8;

                }
                else if (data.column_1 >= 23 && data.column_1 <= 25)
                {

                    data.column_10 = (decimal)3.9;
                }

                else if (data.column_1 >= 20 && data.column_1 <= 22)
                {

                    data.column_10 = (decimal)4.0;

                }
                else if (data.column_1 >= 18 && data.column_1 <= 19)
                {

                    data.column_10 = (decimal)4.1;
                }

                else if (data.column_1 >= 16 && data.column_1 <= 17)
                {

                    data.column_10 = (decimal)4.2;

                }
                else if (data.column_1 >= 14 && data.column_1 <= 15)
                {

                    data.column_10 = (decimal)4.3;
                }

                else if (data.column_1 >= 12 && data.column_1 <= 13)
                {

                    data.column_10 = (decimal)4.4;

                }
                else if (data.column_1 >= 10 && data.column_1 <= 11)
                {

                    data.column_10 = (decimal)4.5;
                }

                else if (data.column_1 >= 8 && data.column_1 <= 9)
                {

                    data.column_10 = (decimal)4.6;

                }
                else if (data.column_1 >= 6 && data.column_1 <= 7)
                {

                    data.column_10 = (decimal)4.7;
                }
                else if (data.column_1 >= 4 && data.column_1 <= 5)
                {

                    data.column_10 = (decimal)4.8;
                }

                else if (data.column_1 >= 2 && data.column_1 <= 3)
                {

                    data.column_10 = (decimal)4.9;

                }
                else if (data.column_1 >= 0 && data.column_1 <= 1)
                {

                    data.column_10 = (decimal)5.0;
                }
                data.column_2 = (data.column_10 + data.term_total_wgt) / 2;
                if (data.column_2 >= 1M && data.column_2 <= 3.0M)
                {
                    data.Middlename = "Passed";
                }
                else if (data.column_2 >= 3.1M && data.column_2 <= 4.0M)
                {

                    data.Middlename = "Conditional";
                }
                else if (data.column_2 >= 4.1M)
                {

                    data.Middlename = "Failed";

                }

            }

            classTermViewmodelBindingSource.DataSource = datas;
            this.reportViewer2.ZoomMode = ZoomMode.PageWidth;
            this.reportViewer2.Width = 915;
            this.reportViewer2.RefreshReport();
        }
        public void getdatass()
        {
            var courses = guna2ComboBox1.SelectedValue;
            var subjects = guna2ComboBox2.SelectedValue;
            int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();

            var datas = (
            from cr in _context.class_Record
            join st in _context.Students on cr.stud_Id equals st.t_Id
            where cr.teach_Id == _id && cr.sem == sem_Id.ToString() && cr.course == courses.ToString() && cr.subject == subjects.ToString() && cr.mount == mount
            group new { cr, st } by st.t_Id into studentGroup
            select new classTermViewmodel
            {

                stud_Id = studentGroup.Key,
                lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1))
                        .FirstOrDefault(),
                column_1 =
                        (studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                    +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                                          +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                        +
                        studentGroup.SelectMany(s => _context.class_Record
                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                            .Select(q => q.type_total ?? 0)
                            .DefaultIfEmpty(0)
                        ).FirstOrDefault()

                        ),

                column_9 =
                        (studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                            .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Quizzes" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                    +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Oral" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                                         +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Project" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                                           .Select(q => (q.set_Grade ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && hs.course == courses.ToString() && hs.subject == subjects.ToString() && hs.sem == sem_Id.ToString() && hs.teach_Id == _id && hs.mount == mount)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                        +
                        studentGroup.SelectMany(s => _context.class_Record
                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.course == courses.ToString() && q.subject == subjects.ToString() && q.sem == sem_Id.ToString() && q.teach_Id == _id && q.mount == mount)
                            .Select(q => q.type_total ?? 0)
                            .DefaultIfEmpty(0)
                        ).FirstOrDefault()

                        ),
                column_2 = 0,
                Middlename="",
                column_10 = 0,
                term_total_wgt = 0,

            }
                        ).ToList();

            foreach (var data in datas)
            {
                data.column_9 = (int)Math.Round(data.column_9);
                data.column_1 = (int)Math.Round(data.column_1);
                //check the column_9
                if (data.column_9 == 100 )
                {
                  
                    data.term_total_wgt = (decimal)1.0;
                }
            
                else if (data.column_9 == 99)
                {
                 
                    data.term_total_wgt = (decimal)1.1;
                }
                else if (data.column_9 == 98 )
                {
                   
                    data.term_total_wgt = (decimal)1.2;
                }
                else if (data.column_9 == 97)
                {
                
                    data.term_total_wgt = (decimal)1.3;
                }
                else if (data.column_9 == 96)
                {
                 
                    data.term_total_wgt = (decimal)1.4;
                }
                else if (data.column_9 == 95)
                {
           
                    data.term_total_wgt = (decimal)1.5;
                }

                else if (data.column_9 >= 93 && data.column_9 <= 94)
                {
                  
                    data.term_total_wgt = (decimal)1.6;

                }
                else if (data.column_9 >= 91 && data.column_9 <= 92)
                {
            
                    data.term_total_wgt = (decimal)1.7;
                }

                else if (data.column_9 >= 89 && data.column_9 <= 90)
                {
               
                    data.term_total_wgt = (decimal)1.8;

                }
                else if (data.column_9 >= 87 && data.column_9 <= 88)
                {
               
                    data.term_total_wgt = (decimal)1.9;
                }

                else if (data.column_9 >= 85 && data.column_9 <= 86)
                {
                
                    data.term_total_wgt = (decimal)2.0;

                }
                else if (data.column_9 >= 83 && data.column_9 <= 84)
                {
                 
                    data.term_total_wgt = (decimal)2.1;
                }

                else if (data.column_9 >= 81 && data.column_9 <= 82)
                {
                 
                    data.term_total_wgt = (decimal)2.2;

                }
                else if (data.column_9 >= 79 && data.column_9 <= 80)
                {
                    data.term_total_wgt = (decimal)2.3;
                }

                else if (data.column_9 >= 77 && data.column_9 <= 78)
                {
                    data.term_total_wgt = (decimal)2.4;

                }
                else if (data.column_9 >= 75 && data.column_9 <= 76)
                {
              
                    data.term_total_wgt = (decimal)2.5;
                }

                else if (data.column_9 >= 70 && data.column_9 <= 74)
                {
                  
                    data.term_total_wgt = (decimal)2.6;

                }
                else if (data.column_9 >= 65 && data.column_9 <= 69)
                {
              
                    data.term_total_wgt = (decimal)2.7;
                }

                else if (data.column_9 >= 60 && data.column_9 <= 64)
                {
                
                    data.term_total_wgt = (decimal)2.8;

                }
                else if (data.column_9 >= 55 && data.column_9 <= 59)
                {
               
                    data.term_total_wgt = (decimal)2.9;
                }

                else if (data.column_9 >= 50 && data.column_9 <= 54)
                {
   
                    data.term_total_wgt = (decimal)3.0;

                }
                else if (data.column_9 >= 47 && data.column_9 <= 49)
                {
               
                    data.term_total_wgt = (decimal)3.1;
                }

                else if (data.column_9 >= 44 && data.column_9 <= 46)
                {
         
                    data.term_total_wgt = (decimal)3.2;

                }
                else if (data.column_9 >= 41 && data.column_9 <= 43)
                {
              
                    data.term_total_wgt = (decimal)3.3;
                }

                else if (data.column_9 >= 38 && data.column_9 <= 40)
                {
            
                    data.term_total_wgt = (decimal)3.4;

                }
                else if (data.column_9 >= 35 && data.column_9 <= 37)
                {
          
                    data.term_total_wgt = (decimal)3.5;
                }

                else if (data.column_9 >= 32 && data.column_9 <= 34)
                {
               
                    data.term_total_wgt = (decimal)3.6;

                }
                else if (data.column_9 >= 29 && data.column_9 <= 31)
                {
              
                    data.term_total_wgt = (decimal)3.7;
                }

                else if (data.column_9 >= 26 && data.column_9 <= 28)
                {
            
                    data.term_total_wgt = (decimal)3.8;

                }
                else if (data.column_9 >= 23 && data.column_9 <= 25)
                {
             
                    data.term_total_wgt = (decimal)3.9;
                }

                else if (data.column_9 >= 20 && data.column_9 <= 22)
                {
         
                    data.term_total_wgt = (decimal)4.0;

                }
                else if (data.column_9 >= 18 && data.column_9 <= 19)
                {
            
                    data.term_total_wgt = (decimal)4.1;
                }

                else if (data.column_9 >= 16 && data.column_9 <= 17)
                {
                   
                    data.term_total_wgt = (decimal)4.2;

                }
                else if (data.column_9 >= 14 && data.column_9 <= 15)
                {
         
                    data.term_total_wgt = (decimal)4.3;
                }

                else if (data.column_9 >= 12 && data.column_9 <= 13)
                {
            
                    data.term_total_wgt = (decimal)4.4;

                }
                else if (data.column_9 >= 10 && data.column_9 <= 11)
                {
      
                    data.term_total_wgt = (decimal)4.5;
                }

                else if (data.column_9 >= 8 && data.column_9 <= 9)
                {
         
                    data.term_total_wgt = (decimal)4.6;

                }
                else if (data.column_9 >= 6 && data.column_9 <= 7)
                {
         
                    data.term_total_wgt = (decimal)4.7;
                }
                else if (data.column_9 >= 4 && data.column_9 <= 5)
                {
                  
                    data.term_total_wgt = (decimal)4.8;
                }

                else if (data.column_9 >= 2 && data.column_9 <= 3)
                {
               
                    data.term_total_wgt = (decimal)4.9;

                }
                else if (data.column_9 >= 0 && data.column_9 <= 1)
                {
              
                    data.term_total_wgt = (decimal)5.0;
                }

                //check the column_1
                if (data.column_1 == 100)
                {
                 
                    data.column_10= (decimal)1.0;
                }

                else if (data.column_1 == 99)
                {
                    data.column_10 = (decimal)1.1;
                 
                }
                else if (data.column_1 == 98)
                {
                    data.column_10 = (decimal)1.2;
          
                }
                else if (data.column_1 == 97)
                {

                    data.column_10 = (decimal)1.3;
                }
                else if (data.column_1 == 96)
                {

                    data.column_10 = (decimal)1.4;
                }
                else if (data.column_1 == 95)
                {

                    data.column_10 = (decimal)1.5;
                }

                else if (data.column_1 >= 93 && data.column_1 <= 94)
                {

                    data.column_10 = (decimal)1.6;

                }
                else if (data.column_1 >= 91 && data.column_1 <= 92)
                {

                    data.column_10 = (decimal)1.7;
                }

                else if (data.column_1 >= 89 && data.column_1 <= 90)
                {

                    data.column_10 = (decimal)1.8;

                }
                else if (data.column_1 >= 87 && data.column_1 <= 88)
                {

                    data.column_10 = (decimal)1.9;
                }

                else if (data.column_1 >= 85 && data.column_1 <= 86)
                {

                    data.column_10  = (decimal)2.0;

                }
                else if (data.column_1 >= 83 && data.column_1 <= 84)
                {

                    data.column_10 = (decimal)2.1;
                }

                else if (data.column_1 >= 81 && data.column_1 <= 82)
                {

                    data.column_10 = (decimal)2.2;

                }
                else if (data.column_1 >= 79 && data.column_1 <= 80)
                {

                    data.column_10 = (decimal)2.3;
                }

                else if (data.column_1 >= 77 && data.column_1 <= 78)
                {

                    data.column_10 = (decimal)2.4;

                }
                else if (data.column_1 >= 75 && data.column_1 <= 76)
                {

                    data.column_10 = (decimal)2.5;
                }

                else if (data.column_1 >= 70 && data.column_1 <= 74)
                {

                    data.column_10 = (decimal)2.6;

                }
                else if (data.column_1 >= 65 && data.column_1 <= 69)
                {

                    data.column_10 = (decimal)2.7;
                }

                else if (data.column_1 >= 60 && data.column_1 <= 64)
                {

                    data.column_10 = (decimal)2.8;

                }
                else if (data.column_1 >= 55 && data.column_1 <= 59)
                {

                    data.column_10 = (decimal)2.9;
                }

                else if (data.column_1 >= 50 && data.column_1 <= 54)
                {

                    data.column_10 = (decimal)3.0;

                }
                else if (data.column_1 >= 47 && data.column_1 <= 49)
                {

                    data.column_10 = (decimal)3.1;
                }

                else if (data.column_1 >= 44 && data.column_1 <= 46)
                {

                    data.column_10 = (decimal)3.2;

                }
                else if (data.column_1 >= 41 && data.column_1 <= 43)
                {

                    data.column_10 = (decimal)3.3;
                }

                else if (data.column_1 >= 38 && data.column_1 <= 40)
                {

                    data.column_10 = (decimal)3.4;

                }
                else if (data.column_1 >= 35 && data.column_1 <= 37)
                {

                    data.column_10 = (decimal)3.5;
                }

                else if (data.column_1 >= 32 && data.column_1 <= 34)
                {

                    data.column_10 = (decimal)3.6;

                }
                else if (data.column_1 >= 29 && data.column_1 <= 31)
                {

                    data.column_10 = (decimal)3.7;
                }

                else if (data.column_1 >= 26 && data.column_1 <= 28)
                {

                    data.column_10 = (decimal)3.8;

                }
                else if (data.column_1 >= 23 && data.column_1 <= 25)
                {

                    data.column_10 = (decimal)3.9;
                }

                else if (data.column_1 >= 20 && data.column_1 <= 22)
                {

                    data.column_10 = (decimal)4.0;

                }
                else if (data.column_1 >= 18 && data.column_1 <= 19)
                {

                    data.column_10 = (decimal)4.1;
                }

                else if (data.column_1 >= 16 && data.column_1 <= 17)
                {

                    data.column_10 = (decimal)4.2;

                }
                else if (data.column_1 >= 14 && data.column_1 <= 15)
                {

                    data.column_10 = (decimal)4.3;
                }

                else if (data.column_1 >= 12 && data.column_1 <= 13)
                {

                    data.column_10 = (decimal)4.4;

                }
                else if (data.column_1 >= 10 && data.column_1 <= 11)
                {

                    data.column_10 = (decimal)4.5;
                }

                else if (data.column_1 >= 8 && data.column_1 <= 9)
                {

                    data.column_10 = (decimal)4.6;

                }
                else if (data.column_1 >= 6 && data.column_1 <= 7)
                {

                    data.column_10 = (decimal)4.7;
                }
                else if (data.column_1 >= 4 && data.column_1 <= 5)
                {

                    data.column_10 = (decimal)4.8;
                }

                else if (data.column_1 >= 2 && data.column_1 <= 3)
                {

                    data.column_10 = (decimal)4.9;

                }
                else if (data.column_1 >= 0 && data.column_1 <= 1)
                {

                    data.column_10 = (decimal)5.0;
                }
                data.column_2 = (data.column_10 + data.term_total_wgt) / 2;
                if (data.column_2 >= 1M && data.column_2 <= 3.0M)
                {
                    data.Middlename = "Passed";
                }
                else if (data.column_2 >= 3.1M && data.column_2 <= 4.0M)
                {

                    data.Middlename = "Conditional";
                }
                else if (data.column_2 >=4.1M)
                {

                    data.Middlename = "Failed";

                }

            }

            classTermViewmodelBindingSource.DataSource = datas;
            this.reportViewer2.ZoomMode = ZoomMode.PageWidth;
            this.reportViewer2.Width = 915;
            this.reportViewer2.RefreshReport();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            var getthevalue = guna2TextBox3.Text.Trim();
            if (string.IsNullOrEmpty(getthevalue))
            {
                MessageBox.Show("Please select term", "Empty Value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var courses = guna2ComboBox1.SelectedValue;
                var subjects = guna2ComboBox2.SelectedValue;
                int sem_Id = _context.semesters.Where(q => q.sem_Name == _sem).Select(s => s.sem_Id).FirstOrDefault();
                String semmean = guna2HtmlLabel1.Text.Trim();
                String semNames = guna2HtmlLabel4.Text.Trim();
                String term = guna2TextBox3.Text.Trim();

                midterm_Report_frm frm = new midterm_Report_frm(_id, courses.ToString(), subjects.ToString(), sem_Id, semmean, semNames, term,mount);
                frm.ShowDialog();
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            guna2TextBox3.Text = String.Empty;
            guna2Button1.FillColor = Color.MediumTurquoise;
            guna2Button2.FillColor = Color.MediumTurquoise;
            guna2Button3.FillColor = Color.MediumTurquoise;

        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mon = guna2ComboBox3.SelectedIndex;
            if (mon == 0)
            {
                mount = "lec";
            }
            else
            {
                mount = "lab";
            }
        }
    }
}
