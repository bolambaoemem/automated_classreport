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


namespace automated_classreport.Report
{
    public partial class history_Report_frm : Form
    {

        gradingsysEntities _context = new gradingsysEntities();
        int _hisid;
        int _id;
        string _teacher_Name;
        public history_Report_frm()
        {
            InitializeComponent();
        }
        public history_Report_frm(int hisId, int id) : this()
        {
            _id = id;
            _hisid = hisId;
        }

        private void history_Report_frm_Load(object sender, EventArgs e)
        {
            var name_t = _context.user_account.Where(q => q.accId == _id).FirstOrDefault();
            _teacher_Name = name_t.fname + "  " + name_t.lname.ToString();

            setdata();
            List<ReportParameter> parameters = new List<ReportParameter>();

            parameters.Add(new ReportParameter("teacher", _teacher_Name));
            this.reportViewer1.LocalReport.SetParameters(parameters);
            this.reportViewer1.RefreshReport();
        }
        public void setdata()
        {
            var datas = (
             from cr in _context.class_Record
             join st in _context.Students on cr.stud_Id equals st.t_Id
             where cr.teach_Id == _id && cr.sem == _hisid.ToString()
             group new { cr, st } by st.t_Id into studentGroup
             select new classTermViewmodel
             {
                 stud_Id = studentGroup.Key,
                 lastname = studentGroup.Select(s => s.st.LastName + "," + s.st.FirstName + " " + s.st.Middlename.Substring(0, 1))
                        .FirstOrDefault(),
                 subject= studentGroup.Select(s=> s.cr.subject.ToString()).FirstOrDefault(),
                 course = studentGroup.Select(s => s.cr.course.ToString()).FirstOrDefault(),
                 column_1 =
                        (studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                            .Select(q => (q.total ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Quizzes" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                      +

                       studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Performance" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                            .Select(q => (q.total ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Performance" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                    +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Oral" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                           .Select(q => (q.total ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Oral" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                                          +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Project" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                           .Select(q => (q.total ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Final" && hs.typeof_column == "Project" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                        +
                        studentGroup.SelectMany(s => _context.class_Record
                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Final" && q.typeof_column == "Quizzes" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                            .Select(q => q.type_total ?? 0)
                            .DefaultIfEmpty(0)
                        ).FirstOrDefault()

                        ),

                 column_9 =
                        (studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                            .Select(q => (q.total ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Quizzes" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                      +

                       studentGroup.Select(s => _context.class_Record
                                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Performance" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                            .Select(q => (q.total ?? 0) * (_context.high_Score
                                                .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Performance" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                                .Select(hs => hs.wgt ?? 0)
                                                .DefaultIfEmpty()
                                                .FirstOrDefault() / 100))
                                            .DefaultIfEmpty()
                                            .FirstOrDefault())
                                        .FirstOrDefault()
                    +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Oral" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                           .Select(q => (q.total ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Oral" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                                         +
                       studentGroup.Select(s => _context.class_Record
                                           .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Project" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                           .Select(q => (q.total ?? 0) * (_context.high_Score
                                               .Where(hs => hs.term_exam == "Midterm" && hs.typeof_column == "Project" && q.sem == _hisid.ToString() && q.teach_Id == _id)
                                               .Select(hs => hs.wgt ?? 0)
                                               .DefaultIfEmpty()
                                               .FirstOrDefault() / 100))
                                           .DefaultIfEmpty()
                                           .FirstOrDefault())
                                        .FirstOrDefault()
                        +
                        studentGroup.SelectMany(s => _context.class_Record
                            .Where(q => q.stud_Id == s.st.t_Id && q.term_exam == "Midterm" && q.typeof_column == "Quizzes" && q.sem == _hisid.ToString() && q.teach_Id == _id)
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
        }
    }
}
