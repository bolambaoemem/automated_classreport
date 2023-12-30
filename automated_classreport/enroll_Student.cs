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
    public partial class enroll_Student : Form
    {

        gradingsysEntities _context = new gradingsysEntities();
        private List<int> editedRowIds = new List<int>();
        int _id;
        public enroll_Student()
        {
            InitializeComponent();
       
        }
        public enroll_Student(int id):this()
        {
            _id = id;

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            int SEm = Convert.ToInt32(semister.SelectedValue);
            var course_tb = course.Text.Trim();
            var subject_tb = subject.Text.Trim();
            gunaDataGridView1.DataSource = _context.Students.Where(q => q.teach_id == _id && q.course_year == course_tb && q.subject == subject_tb && q.sem_Id == SEm).OrderBy(q => q.LastName).ToList();


            gunaDataGridView1.ReadOnly = false;
            guna2Button3.Visible = true;
            update.Visible = true;
            save.Visible = false;
            guna2Button1.Visible = true;
            guna2Button2.Enabled = false;
            guna2Button1.Enabled = true;

        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var course_tb = course.Text.Trim();
            var subject_tb = subject.Text.Trim();

            if (course_tb == "" || subject_tb == "")
            {

                MessageBox.Show("Please fill up the form", "Cannot add to unknown field");

            }
            else
            {

                guna2Button1.Enabled = false;
                guna2Button2.Enabled = true;
                gunaDataGridView1.ReadOnly = false;
                gunaDataGridView1.DataSource = null;
                guna2Button3.Visible = true;
                update.Visible = false;
                save.Visible = true;
                gunaDataGridView1.ClearSelection();
            }
        
        }

        private void enroll_Student_Load(object sender, EventArgs e)
        {
            //var student = _context.Students.ToList();
            //gunaDataGridView1.DataSource = student;

            var semesters = _context.semesters
                  .Where(q => q.teach_id == _id)
                  .DefaultIfEmpty()
                  .ToList();

            semister.DataSource = semesters;


        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in gunaDataGridView1.Rows)
                {
                    if (!row.IsNewRow )
                    {
                        Student entity = new Student
                        {
                            StudentID = Convert.ToInt32(row.Cells[1].Value),
                            teach_id = _id, 
                            FirstName = row.Cells[2].Value.ToString(),
                            LastName = row.Cells[3].Value.ToString(),
                            Middlename = row.Cells[4].Value.ToString(),
                            subject = subject.Text.Trim(),
                            course_year = course.Text.Trim(), 
                            sem_Id = Convert.ToInt32(semister.SelectedValue),
                                                                         
                        };
                        _context.Students.Add(entity);
              

                    }
                }

                // Save changes to the database
                _context.SaveChanges();
                //passdata_ClassRecord();
                MessageBox.Show("Successfully Added");
                gunaDataGridView1.ReadOnly = true;
                guna2Button1.Enabled = true;
                guna2Button2.Enabled = true;
                save.Visible = false;
                guna2Button3.Visible = false;
                int SEm = Convert.ToInt32(semister.SelectedValue);
                var course_tb = course.Text.Trim();
                var subject_tb = subject.Text.Trim();
                gunaDataGridView1.ClearSelection();
                gunaDataGridView1.DataSource = _context.Students.Where(q => q.teach_id == _id && q.course_year == course_tb && q.subject == subject_tb && q.sem_Id == SEm).OrderBy(q => q.LastName).ToList();


            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                guna2Button1.Enabled = true;
                guna2Button2.Enabled = true;
                save.Visible = false;
                guna2Button3.Visible = false;
            }
}

        public void passdata_ClassRecord()
        {
            //try {
            //    int semId = Convert.ToInt32(semister.SelectedValue);
            //    var student_info = _context.Students.Where(q => q.teach_id ==_id && q.course_year == course.Text.Trim() && q.subject == subject.Text.Trim() && q.sem_Id == semId).ToList();

            //    foreach (var studentId in student_info)
            //    {
            //        bool recordExists = _context.class_Record.Any(o => o.stud_Id == studentId.t_Id );

            //        if (!recordExists)
            //        { 
            //            Entities.class_Record record = new Entities.class_Record
            //            {

            //                stud_Id = studentId.t_Id,
            //                teach_Id = studentId.teach_id,
            //                course = studentId.course_year,
            //                sem = Convert.ToString(studentId.sem_Id),
            //                subject = studentId.subject,


            //            };
            //            _context.class_Record.Add(record);
            //        }
            //    }
            //    _context.SaveChanges();
            //    MessageBox.Show("Successfully Added");
            //    gunaDataGridView1.ReadOnly = true;
            //    int SEm = Convert.ToInt32(semister.SelectedValue);
            //    var course_tb = course.Text.Trim();
            //    var subject_tb = subject.Text.Trim();

            //    gunaDataGridView1.DataSource = _context.Students.Where(q => q.teach_id == _id && q.course_year == course_tb && q.subject == subject_tb && q.sem_Id == SEm).ToList();
            //    guna2Button1.Enabled = true;
            //    guna2Button2.Enabled = true;
            //    save.Visible = false;
            //    guna2Button3.Visible = false;
            //    gunaDataGridView1.ClearSelection();

            //}
            //catch (Exception ex) {
            //    MessageBox.Show($"Error: {ex.Message}");

            //}
        
        
        
        
        }



        private bool AreAllRowValuesEmpty(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    return false; // At least one non-empty cell found
                }
            }

            return true; // All cells are empty or null
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

            try
            {
                // Assuming you have a DbContext (e.g., _context) available
                foreach (int rowId in editedRowIds)
                {
                    var existingEntity = _context.Students.Find(rowId);

                    if (existingEntity != null)
                    {
                        // Update existing entity in the DbContext
                        // Perform the necessary updates here
                    }
                    else
                    {
                        // Handle the case when the entity is not found (if needed)
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Changes Saved");

                editedRowIds.Clear();
                gunaDataGridView1.ReadOnly = true;
                guna2Button1.Enabled = true;
                guna2Button2.Enabled = true;
                update.Visible = false;
                guna2Button3.Visible = false;



            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                guna2Button1.Enabled = true;
                guna2Button1.Visible = true;
                guna2Button2.Enabled = true;
                update.Visible = false;
                guna2Button3.Visible = false;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            int SEm = Convert.ToInt32(semister.SelectedValue);
            var course_tb = course.Text.Trim();
            var subject_tb = subject.Text.Trim();
            var hasData = _context.Students.Any(q => q.teach_id == _id && q.course_year == course_tb && q.subject == subject_tb && q.sem_Id == SEm);

            if (hasData)
            {
                gunaDataGridView1.DataSource = _context.Students.Where(q => q.teach_id == _id && q.course_year == course_tb && q.subject == subject_tb && q.sem_Id == SEm).ToList();
                gunaDataGridView1.ReadOnly = true;
                save.Visible = false;
                update.Visible = false;
                guna2Button3.Visible = false;
                guna2Button1.Enabled = true;
                guna2Button2.Enabled = true;
                guna2Button1.Visible = true;
                gunaDataGridView1.ClearSelection();
            }
            else
            {
                save.Visible = false;
                update.Visible = false;
                guna2Button3.Visible = false;
                gunaDataGridView1.DataSource = null;
                guna2Button1.Visible = true;
                guna2Button1.Enabled = true;
                gunaDataGridView1.ClearSelection();
            }
         

        }

        private void gunaDataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = gunaDataGridView1.Rows[e.RowIndex];

                if (!row.IsNewRow)
                {
                    // Get the ID from the first cell assuming it's an integer
                    if (int.TryParse(row.Cells[0].Value?.ToString(), out int rowId))
                    {
                        // Check if the ID is not already in the list
                        if (!editedRowIds.Contains(rowId))
                        {
                            // Add the ID to the list of edited rows
                            editedRowIds.Add(rowId);
                        }
                    }
                }
            }

        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                var students = _context.Students;

                if (gunaDataGridView1.SelectedRows.Count > 0)
                {


                    DialogResult result = MessageBox.Show("Are you sure you want to delete the selected row(s)?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in gunaDataGridView1.SelectedRows)
                        {
                            var studentIdCellValue = row.Cells[1].Value;
                            int studentId = Convert.ToInt32(studentIdCellValue);

                            var entityToDelete = _context.Students.SingleOrDefault(q => q.StudentID == studentId);

                            if (entityToDelete != null)
                            {
                                _context.Students.Remove(entityToDelete);
                            }
                        }


                        _context.SaveChanges();

                        int SEm = Convert.ToInt32(semister.SelectedValue);
                        var course_tb = course.Text.Trim();
                        var subject_tb = subject.Text.Trim();
                        gunaDataGridView1.ClearSelection();
                        gunaDataGridView1.DataSource = _context.Students.Where(q => q.teach_id == _id && q.course_year == course_tb && q.subject == subject_tb && q.sem_Id == SEm).ToList();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }


        }

        private void semister_Click(object sender, EventArgs e)
        {

            var semesters = _context.semesters
                  .Where(q => q.teach_id == _id)
                  .ToList();

         
            if (semesters.Count == 0)
            {
                MessageBox.Show("No semesters found. Please create a semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                semister.DataSource = semesters;
            }
        }



        public void check_ondata() {

            int SEm = Convert.ToInt32(semister.SelectedValue);
            var course_tb = course.Text.Trim();
            var subject_tb = subject.Text.Trim();

            if (course_tb != "" && subject_tb != "" && SEm != 0)
            {
                var checking_data = _context.Students.Where(q => q.teach_id == _id && q.course_year == course_tb && q.subject == subject_tb && q.sem_Id == SEm).ToList();
                if (checking_data.Count > 0)
                {
                    gunaDataGridView1.DataSource = checking_data;
                    gunaDataGridView1.Visible = true;
                    gunaDataGridView1.ClearSelection();
                    guna2Button1.Visible = true;


                }
                else
                {
                    DialogResult result = MessageBox.Show("No student data found.Do you want to add a new student?", "Confirmation", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        guna2Button1.Enabled = true;
                        guna2Button1.Visible = true;
                        guna2Button2.Visible = false;
                        guna2Button4.Visible = false;
                        gunaDataGridView1.ReadOnly = false;
                        gunaDataGridView1.DataSource = null;
                        guna2Button3.Visible = false;
                        update.Visible = false;
                        save.Visible = false;
                        gunaDataGridView1.ClearSelection();


                    }
                    else
                    {
                        guna2Button1.Visible = false;
                        guna2Button2.Visible = false;
                        guna2Button4.Visible = false;
                        guna2Button3.Visible = true;
                        save.Visible = true;
                        gunaDataGridView1.ClearSelection();
                 
                    }

                }
            }
            else
            {
                MessageBox.Show("Please fill up the form first", "No data Found");
                guna2Button1.Visible = false;
                guna2Button2.Visible = false;
                guna2Button4.Visible = false;
                gunaDataGridView1.ReadOnly = false;
                gunaDataGridView1.DataSource = null;
                guna2Button3.Visible = false;
                update.Visible = false;
                save.Visible = false;
                gunaDataGridView1.ClearSelection();

            }

        }

       

        private void subject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                check_ondata();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

        }

        private void course_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                check_ondata();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
         
        }

        private void gunaDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gunaDataGridView1.SelectedRows)
            {
                var checkdata = row.Cells[1].Value;
                if (Convert.ToInt32(checkdata) != 0) {
                    guna2Button1.Visible = true;
                    guna2Button2.Visible = true;
                    guna2Button4.Visible = true;

   
                }
                else
                {
                    guna2Button1.Visible = true;
                    guna2Button2.Visible = false;
                    guna2Button4.Visible = false;

                }

            }

     
           
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
