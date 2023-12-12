using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using automated_classreport.Entities;
using automated_classreport.ViewModel;
using Microsoft.Reporting.WinForms;

namespace automated_classreport.Report
{
    public partial class test_form_print : Form
    {
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        string _term;
        string _subject;
        string _courses;
        string _semister_Name;
        string _test_given;
        string _test_submit;
        string _num_takingtest;
        string _high_score;
        string _med_score;
        string _low_score;
         string _score;
        string _mean;
        public test_form_print()
        {
            InitializeComponent();
        }
        public test_form_print(int id,string term,string subject,string course,string semister_Name,string test_given,string test_submit,string num_takingtest,string high_score,string med_score,string low_score,string score):this()
        {
             _id = id;
            _term = term;
            _subject = subject;
             _courses = course;
            _semister_Name = semister_Name;
            _test_given = test_given;
            _test_submit = test_submit;
            _num_takingtest = num_takingtest;
            _high_score = high_score;
            _med_score = med_score;
            _low_score = low_score;
            _score = score;
        }


        private void test_form_print_Load(object sender, EventArgs e)
        {
            var getmean = _context.semesters.Where(q => q.sem_Name == _semister_Name && q.teach_id == _id).FirstOrDefault();
            if (getmean.sem_Mean == 1) {
                _mean = "1st";
            
            }
            if (getmean.sem_Mean == 2)
            {
                _mean = "2nd";

            }
            setdata();
            var user = _context.user_account.Where(q => q.accId == _id).FirstOrDefault();
                     ReportParameter[] para = new ReportParameter[] {

                             new ReportParameter("subject", (_subject != null) ? _subject.ToString() : string.Empty),
                            new ReportParameter("course", (_courses != null) ? _courses.ToString() : string.Empty),
                                 new ReportParameter("sem", (_semister_Name != null) ? _semister_Name.ToString() : string.Empty),
                                new ReportParameter("exam", (_term != null) ? _term.ToString() : string.Empty + "Examination"),
                                  new ReportParameter("dg", (_test_given != null) ? _test_given.ToString() : string.Empty),
                                  new ReportParameter("ds", (_test_submit != null) ? _test_submit.ToString() : string.Empty),
                                  new ReportParameter("nums", (_num_takingtest != null) ? _num_takingtest.ToString() : string.Empty),
                                   new ReportParameter("high", ( _high_score != null) ?  _high_score.ToString() : string.Empty),
                                   new ReportParameter("median", ( _med_score != null) ?  _med_score.ToString() : string.Empty),
                                   new ReportParameter("low", ( _low_score != null) ?  _low_score.ToString() : string.Empty),
                                   new ReportParameter("score", ( _score != null) ?  _score.ToString() : string.Empty),
                                   new ReportParameter("term_mean", (_mean != null) ?  _mean.ToString() : string.Empty),
                                   new ReportParameter("semister", (_semister_Name != null) ? _semister_Name.ToString() : string.Empty),
                                   new ReportParameter("teacher", (user.fname != null && user.lname !=null) ? user.fname.ToString()+" "+user.lname.ToString() : string.Empty),
            };
            this.reportViewer1.LocalReport.SetParameters(para);
            this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            this.reportViewer1.Width = 915;
            this.reportViewer1.RefreshReport();

        }
     
        public void setdata() {


         var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == _semister_Name);

                if (semester != null)
                {
                    int name_Sem = semester.sem_Id;

                    var data = (
                                    from cr in _context.class_Record
                                    join st in _context.Students on cr.stud_Id equals st.t_Id
                                    where cr.teach_Id == _id && cr.subject == _subject && cr.course == _courses && cr.sem == name_Sem.ToString() && cr.term_exam == _term 
                                    group new { cr, st } by st.t_Id into studentGroup
                                    select new classTermViewmodel
                                    {
                                        stud_Id = studentGroup.Key,
                                        lastname = studentGroup.Select(s => s.st.LastName + " , " + s.st.FirstName + "  " + s.st.Middlename.Substring(0, 1) + ".").FirstOrDefault(),
                                        high_score = (int)Math.Round(studentGroup.Select(s => (decimal?)s.cr.term_Score ?? 0).FirstOrDefault())
                                    }
                                ).ToList();

                                var highScoreCounts = data.GroupBy(item => item.high_score)
                                 .ToDictionary(group => group.Key, group => group.Count());

                                foreach (var item in data)
                                {

                                    if (highScoreCounts.TryGetValue(item.high_score, out var count))
                                    {
                                        item.type_total = count;

                                        highScoreCounts.Remove(item.high_score);
                                    }
                                }

                classTermViewmodelBindingSource.DataSource = data;
                }
                else
                {
                    MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        
        }
    }
}
