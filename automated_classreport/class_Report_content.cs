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
    public partial class class_Report_content : Form
    {
        private add_Wgt form3;
        private add_wgt_term form2;
        gradingsysEntities _context = new gradingsysEntities();
        List<int> editedRowIds = new List<int>();
        int high=1;
        String Semister_name;
        String mount;
        int _id;
        int sam = 0;
        int samp = 0;
        int o = 0;
        int mo = 0;
        int fo = 0;
        bool isquiz = false;
        bool isoral = false;
        bool isperformance = false;
        bool isproject = false;
        bool ismidterm = false;
        bool isfinal = false;
        string term_exas = "";
        string type_ofbtn = "";
        string term_Type;
        string term_exis;
        string type;
        public class_Report_content()
        {
            InitializeComponent();
        }

        public class_Report_content(int id) : this()
        {
            _id = id;
        }

        private void class_Report_content_Load(object sender, EventArgs e)
        {
            guna2ToggleSwitch1.Checked = false;
            guna2ComboBox1.SelectedIndex = -1;
            guna2ComboBox2.SelectedIndex = -1;
            sem_txtbx.Text = Semister_name;
            get_datafromdb();
            //gunaDataGridView1.ClearSelection();
            //guna2DataGridView1.ClearSelection();
            //guna2DataGridView2.ClearSelection();


        }
        public void accept_Name(String name_sem) {

            Semister_name = name_sem;
        
        }
        public void get_datafromdb() {

             string sem = sem_txtbx.Text.Trim();
            int getsemId = _context.semesters.Where(q=>q.sem_Name == sem).Select(s=> s.sem_Id).FirstOrDefault();
            var getdata = _context.Students.Where(q => q.teach_id == _id && q.sem_Id == getsemId).ToList();
            if (getdata.Count == 0)
            {

                MessageBox.Show("Please enroll students first .", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
 
                setdatatosubject();
                setdatatocourse();

            }

        }


        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (isquiz == false && isoral == false && isperformance == false && isproject == false)
            {
                MessageBox.Show("Please Select a button  of What type of a record");
            }
            else
            {

                var semister_Name = sem_txtbx.Text.Trim();
                var subjects = guna2ComboBox1.SelectedValue;
                var course = guna2ComboBox2.SelectedValue;
                if (ismidterm == true)
                {
                    term_exas = "Midterm";
                }
                if (isfinal == true)
                {
                    term_exas = "Final";
                }
                if (isquiz == true &&  isoral == false && isperformance == false && isproject == false) {
                    type_ofbtn = "Quizzes";
                }
                if (isquiz == false &&  isoral == true && isperformance == false && isproject == false)
                {
                    type_ofbtn = "Oral";
                }
                if (isquiz == false && isoral == false && isperformance == true && isproject == false)
                {
                    type_ofbtn = "Performance";
                }
                if (isquiz == false &&  isoral == false && isperformance == false && isproject == true)
                {
                    type_ofbtn = "Project";
                }
                int sem_Id = _context.semesters.Where(q => q.sem_Name == semister_Name).Select(s => s.sem_Id).FirstOrDefault();

                form3 = new add_Wgt(_id, sem_Id, subjects.ToString(), course.ToString(), term_exas,type_ofbtn,mount);
                form3.Form3Closed += Form3_FormClosed;
                form3.ShowDialog();
            }
        }

        private void record_table_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = record_Table.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Validate the input based on your criteria
                string inputValue = cell.Value?.ToString();

                // Check if the input is not null or empty
                if (!string.IsNullOrEmpty(inputValue))
                {
                    // Check if the input contains only numbers and does not exceed three digits
                    if (IsNumeric(inputValue) && inputValue.Length <= 3)
                    {
                        // Valid input (only numbers and not exceeding three digits)
                        // Proceed with updating the database
                        UpdateDatabase(e.RowIndex, inputValue);
                    }
                    else
                    {
                        // Show an alert message for invalid input
                        MessageBox.Show("Invalid input. Please enter a numeric value with at most three digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Handle empty or null input if needed
                    // Show an alert message or take appropriate action
                    MessageBox.Show("Invalid input. Please enter a numeric value with at most three digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }


        private void UpdateDatabase(int rowIndex, string inputValue)
        {
            try
            {

                    // Get the ID from the first cell assuming it's an integer
                    if (int.TryParse(record_Table.Rows[rowIndex].Cells[0].Value?.ToString(), out int rowId))
                    {
                        // Retrieve the existing entity from the database
                        var existingEntity = _context.high_Score.Find(rowId);

                        if (existingEntity != null)
                        {

                        existingEntity.column_1 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[5].Value ?? 0);
                        existingEntity.column_2 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[6].Value ?? 0);
                        existingEntity.column_3 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[7].Value ?? 0);
                        existingEntity.column_4 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[8].Value ?? 0);
                        existingEntity.column_5 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[9].Value ?? 0);
                        existingEntity.column_6 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[10].Value ?? 0);
                        existingEntity.column_7 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[11].Value ?? 0);
                        existingEntity.column_8 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[12].Value ?? 0);
                        existingEntity.column_9 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[13].Value ?? 0);
                        existingEntity.column_10 = Convert.ToDecimal(record_Table.Rows[rowIndex].Cells[14].Value ?? 0);
                        existingEntity.total = Convert.ToDecimal(existingEntity.column_1 + existingEntity.column_2 + existingEntity.column_3 + existingEntity.column_4 + existingEntity.column_5 + existingEntity.column_6 + existingEntity.column_7 + existingEntity.column_8 + existingEntity.column_9 + existingEntity.column_10 ?? 0);
                            
                        _context.SaveChanges();
                      
                        //highViewModelBindingSource.DataSource = _context.high_Score.Where(q => q.high_ID == rowId).FirstOrDefault();
                        MessageBox.Show("Successfully Added");
                        //highViewModelBindingSource.ResetBindings(true);
                        highViewModelBindingSource.DataSource = existingEntity;

                    }
                    else
                        {
                            // Handle the case when the entity is not found (if needed)
                            MessageBox.Show("Entity not found in the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    //highViewModelBindingSource.DataSource = _context.high_Score.Where(q => q.high_ID == rowId).FirstOrDefault();
                }

              
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error updating the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateDatabases(int rowIndex, string inputValue)
        {
            try
            {

                // Get the ID from the first cell assuming it's an integer
                if (int.TryParse(guna2DataGridView1.Rows[rowIndex].Cells[0].Value?.ToString(), out int rowId))
                {
                    // Retrieve the existing entity from the database
                    var existingEntity = _context.class_Record.Find(rowId);

                    if (existingEntity != null)
                    {

                        existingEntity.column_1 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[5].Value ?? 0);
                        existingEntity.column_2 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[6].Value ?? 0);
                        existingEntity.column_3 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[7].Value ?? 0);
                        existingEntity.column_4 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[8].Value ?? 0);
                        existingEntity.column_5 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[9].Value ?? 0);
                        existingEntity.column_6 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[10].Value ?? 0);
                        existingEntity.column_7 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[11].Value ?? 0);
                        existingEntity.column_8 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[12].Value ?? 0);
                        existingEntity.column_9 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[13].Value ?? 0);
                        existingEntity.column_10 = Convert.ToDecimal(guna2DataGridView1.Rows[rowIndex].Cells[14].Value ?? 0);
                        existingEntity.total = Convert.ToDecimal(existingEntity.column_1 + existingEntity.column_2 + existingEntity.column_3 + existingEntity.column_4 + existingEntity.column_5 + existingEntity.column_6 + existingEntity.column_7 + existingEntity.column_8 + existingEntity.column_9 + existingEntity.column_10 ?? 0);
                        var dta = _context.class_Record.FirstOrDefault(q => q.ID == rowId);

                        if (dta != null)
                        {
                            // Assuming the code to retrieve the weight is correct and the corresponding table is _context.high_Record
                            var wgt = _context.high_Score.FirstOrDefault(q => q.teach_Id == dta.teach_Id && q.course == dta.course && q.subject == dta.subject && q.term_exam == dta.term_exam && q.typeof_column == type && q.mount == dta.mount);

                            if (wgt != null)
                            {
                                if (wgt.typeof_column == "Performance" || wgt.typeof_column == "Project" && wgt.mount == "lab")
                                {
                                    if (wgt.typeof_column == "Performance")
                                    {
                                        var wgts = _context.class_Record.FirstOrDefault(q => q.teach_Id == dta.teach_Id && q.course == dta.course && q.subject == dta.subject && q.term_exam == dta.term_exam && q.typeof_column == "Project" && q.mount == "lab" && q.stud_Id == existingEntity.stud_Id );
                                        if (wgts != null)
                                        {
                                            existingEntity.term_total_wgt = Math.Round(Convert.ToDecimal(((existingEntity.total / wgt.total) * wgt.wgt ?? 0)), 2);
                                            _context.SaveChanges();
                                            SetGades(existingEntity.term_total_wgt, rowId,wgts.stud_Id,wgts.typeof_column);
                                          
                                        }
                                        else
                                        {
                                            existingEntity.term_total_wgt = Math.Round(Convert.ToDecimal((existingEntity.total / wgt.total) * wgt.wgt ?? 0), 2);
                                            _context.SaveChanges();
                                            SetGrade(existingEntity.total, rowId);
                                        }
                                    }
                                    else
                                    {
                                        var wgtss = _context.class_Record.FirstOrDefault(q => q.teach_Id == dta.teach_Id && q.course == dta.course && q.subject == dta.subject && q.term_exam == dta.term_exam && q.typeof_column == "Performance" && q.mount == "lab" && q.stud_Id == existingEntity.stud_Id);
                                        if (wgtss != null)
                                        {
                                            existingEntity.term_total_wgt = Math.Round(Convert.ToDecimal(((existingEntity.total / wgt.total) * wgt.wgt ?? 0)), 2);
                                            _context.SaveChanges();
                                            SetGades(existingEntity.term_total_wgt, rowId, wgtss.stud_Id, wgtss.typeof_column);
                                        }
                                        else {
                                            existingEntity.term_total_wgt = Math.Round(Convert.ToDecimal((existingEntity.total / wgt.total) * wgt.wgt ?? 0), 2);
                                            _context.SaveChanges();
                                            SetGrade(existingEntity.total, rowId);
                                        }
                                    }                            
                                    }
                                else
                                {
                                    existingEntity.term_total_wgt = Math.Round(Convert.ToDecimal((existingEntity.total / wgt.total) * wgt.wgt ?? 0), 2);
                                }
                            }
                            else
                            {
                                existingEntity.term_total_wgt = 0;
                            }
                        
                        }
                        _context.SaveChanges();
                        if (existingEntity.typeof_column !="Performance" || existingEntity.typeof_column != "Project" && existingEntity.mount != "lab") {
                            SetGrade(existingEntity.total, rowId);
                        
                        }
                       
                        MessageBox.Show("Successfully Added");
                        RefreshData();




                    }
                    else
                    {
                        // Handle the case when the entity is not found (if needed)
                        MessageBox.Show("Entity not found in the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //highViewModelBindingSource.DataSource = _context.high_Score.Where(q => q.high_ID == rowId).FirstOrDefault();
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error updating the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void SetGades(decimal? total, int IDrow, int? id, string columntype)
        {
            try
            {
                if (total.HasValue)
                {
                    
                    int gtotal = 0;
                    var dta = _context.class_Record.FirstOrDefault(q => q.ID == IDrow);
              
                    string targetType = (dta.typeof_column == "Project") ? "Performance" : "Project";

                    var dtas = _context.class_Record.FirstOrDefault(q =>
                        q.stud_Id == dta.stud_Id &&
                        q.typeof_column == targetType &&
                        q.term_exam == dta.term_exam &&
                        q.sem == dta.sem && 
                        q.mount == dta.mount);

                    
                    var hta = _context.high_Score.FirstOrDefault(q => q.typeof_column == "Performance" && q.term_exam == dta.term_exam && q.sem == dta.sem && q.mount == dta.mount);
                    var htas = _context.high_Score.FirstOrDefault(q => q.typeof_column == "Project" && q.term_exam == dta.term_exam && q.sem == dta.sem && q.mount == dta.mount);
                    gtotal = (int)Math.Round(((((dta.total + dtas.total) / (hta.total + htas.total)) * 100) ?? 0), 0);



                    MessageBox.Show(gtotal.ToString()) ;
                    if (gtotal == 100)
                    {
                        dta.set_Grade = 95;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 95;
                        }

                    }
                    else if (gtotal == 99)
                    {
                        dta.set_Grade = 94;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 94;
                        }
                    }
                    else if (gtotal == 98)
                    {
                        dta.set_Grade = 93;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 93;
                        }
                    }
                    else if (gtotal == 97)
                    {
                        dta.set_Grade = 92;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 92;
                        }
                    }
                    else if (gtotal == 96)
                    {
                        dta.set_Grade = 91;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 91;
                        }

                    }
                    else if (gtotal == 95)
                    {
                        dta.set_Grade = 90;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 90;
                        }

                    }

                    else if (gtotal >= 93 && gtotal <= 94)
                    {
                        dta.set_Grade = 89;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 89;
                        }


                    }
                    else if (gtotal >= 91 && gtotal <= 92)
                    {
                        dta.set_Grade = 88;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 88;
                        }

                    }

                    else if (gtotal >= 89 && gtotal <= 90)
                    {
                        dta.set_Grade = 87;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 87;
                        }

                    }
                    else if (gtotal >= 87 && gtotal <= 88)
                    {
                        dta.set_Grade = 86;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 86;
                        }
                    }

                    else if (gtotal >= 85 && gtotal <= 86)
                    {
                        dta.set_Grade = 85;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 85;
                        }


                    }
                    else if (gtotal >= 83 && gtotal <= 84)
                    {
                        dta.set_Grade = 84;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 84;
                        }

                    }

                    else if (gtotal >= 81 && gtotal <= 82)
                    {
                        dta.set_Grade = 83;

                        if (dtas != null)
                        {
                            dtas.set_Grade = 83;
                        }
                    }
                    else if (gtotal >= 79 && gtotal <= 80)
                    {
                        dta.set_Grade = 82;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 82;
                        }
                    }

                    else if (gtotal >= 77 && gtotal <= 78)
                    {
                        dta.set_Grade = 81;

                        if (dtas != null)
                        {
                            dtas.set_Grade = 81;
                        }
                    }
                    else if (gtotal >= 75 && gtotal <= 76)
                    {
                        dta.set_Grade = 80;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 80;
                        }
                    }

                    else if (gtotal >= 70 && gtotal <= 74)
                    {
                        dta.set_Grade = 79;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 79;
                        }

                    }
                    else if (gtotal >= 65 && gtotal <= 69)
                    {
                        dta.set_Grade = 78;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 78;
                        }
                    }

                    else if (gtotal >= 60 && gtotal <= 64)
                    {
                        dta.set_Grade = 77;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 77;
                        }

                    }
                    else if (gtotal >= 55 && gtotal <= 59)
                    {
                        dta.set_Grade = 76;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 76;
                        }
                    }

                    else if (gtotal >= 50 && gtotal <= 54)
                    {
                        dta.set_Grade = 75;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 75;
                        }

                    }
                    else if (gtotal >= 47 && gtotal <= 49)
                    {
                        dta.set_Grade = 74;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 74;
                        }
                    }

                    else if (gtotal >= 44 && gtotal <= 46)
                    {
                        dta.set_Grade = 73;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 73;
                        }

                    }
                    else if (gtotal >= 41 && gtotal <= 43)
                    {
                        dta.set_Grade = 72;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 72;
                        }
                    }

                    else if (gtotal >= 38 && gtotal <= 40)
                    {
                        dta.set_Grade = 71;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 71;
                        }

                    }
                    else if (gtotal >= 35 && gtotal <= 37)
                    {
                        dta.set_Grade = 70;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 70;
                        }
                    }

                    else if (gtotal >= 32 && gtotal <= 34)
                    {
                        dta.set_Grade = 69;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 69;
                        }

                    }
                    else if (gtotal >= 29 && gtotal <= 31)
                    {
                        dta.set_Grade = 68;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 68;
                        }

                    }

                    else if (gtotal >= 26 && gtotal <= 28)
                    {
                        dta.set_Grade = 67;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 67;
                        }

                    }
                    else if (gtotal >= 23 && gtotal <= 25)
                    {
                        dta.set_Grade = 66;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 66;
                        }
                    }

                    else if (gtotal >= 20 && gtotal <= 22)
                    {
                        dta.set_Grade = 65;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 65;
                        }

                    }
                    else if (gtotal >= 18 && gtotal <= 19)
                    {
                        dta.set_Grade = 64;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 64;
                        }
                    }

                    else if (gtotal >= 16 && gtotal <= 17)
                    {
                        dta.set_Grade = 63;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 63;
                        }

                    }
                    else if (gtotal >= 14 && gtotal <= 15)
                    {
                        dta.set_Grade = 62;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 62;
                        }
                    }

                    else if (gtotal >= 12 && gtotal <= 13)
                    {
                        dta.set_Grade = 61;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 61;
                        }

                    }
                    else if (gtotal >= 10 && gtotal <= 11)
                    {
                        dta.set_Grade = 60;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 60;
                        }
                    }

                    else if (gtotal >= 8 && gtotal <= 9)
                    {
                        dta.set_Grade = 59;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 59;
                        }

                    }
                    else if (gtotal >= 6 && gtotal <= 7)
                    {
                        dta.set_Grade = 58;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 58;
                        }
                    }
                    else if (gtotal >= 4 && gtotal <= 5)
                    {
                        dta.set_Grade = 57;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 57;
                        }
                    }

                    else if (gtotal >= 2 && gtotal <= 3)
                    {
                        dta.set_Grade = 56;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 56;
                        }

                    }
                    else if (gtotal >= 0 && gtotal <= 1)
                    {
                        dta.set_Grade = 55;
                        if (dtas != null)
                        {
                            dtas.set_Grade = 55;
                        }
                    }


                    _context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Total is null. Cannot set grade.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting grade: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetGrade(decimal? total, int IDrow)
        {
            try
            {
                if (total.HasValue)
                {
                 
                    var dta = _context.class_Record.FirstOrDefault(q => q.ID == IDrow);
                    var dtas = _context.high_Score.FirstOrDefault(q =>
                    q.typeof_column == dta.typeof_column &&
                    q.term_exam == dta.term_exam &&
                    q.sem == dta.sem &&
                    q.mount == dta.mount);

                    int gtotal = (int)Math.Round(((dta.total / dtas.total) * 100) ?? 0, 0);

                    if (gtotal == 100)
                    {
                        dta.set_Grade = 95;

                    }
                    else if (gtotal == 99)
                    {
                        dta.set_Grade = 94;
               
                    }
                    else if (gtotal == 98)
                    {
                        dta.set_Grade = 93;
 
                    }
                    else if (gtotal == 97)
                    {
                        dta.set_Grade = 92;
                    }
                    else if (gtotal == 96)
                    {
                        dta.set_Grade = 91;
                      
                    }
                    else if (gtotal == 95)
                    {
                        dta.set_Grade = 90;
                      
                    }

                    else if (gtotal >= 93 && gtotal <= 94)
                    {
                        dta.set_Grade = 89;
                     

                    }
                    else if (gtotal >= 91 && gtotal <= 92)
                    {
                        dta.set_Grade = 88;
                   
                    }

                    else if (gtotal >= 89 && gtotal <= 90)
                    {
                        dta.set_Grade = 87;

                    }
                    else if (gtotal >= 87 && gtotal <= 88)
                    {
                        dta.set_Grade = 86;
                    }

                    else if (gtotal >= 85 && gtotal <= 86)
                    {
                        dta.set_Grade = 85;
                      

                    }
                    else if (gtotal >= 83 && gtotal <= 84)
                    {
                        dta.set_Grade = 84;
    
                    }

                    else if (gtotal >= 81 && gtotal <= 82)
                    {
                        dta.set_Grade = 83;
               

                    }
                    else if (gtotal >= 79 && gtotal <= 80)
                    {
                        dta.set_Grade = 82;
              
                    }

                    else if (gtotal >= 77 && gtotal <= 78)
                    {
                        dta.set_Grade = 81;
                

                    }
                    else if (gtotal >= 75 && gtotal <= 76)
                    {
                        dta.set_Grade = 80;
                     
                    }

                    else if (gtotal >= 70 && gtotal <= 74)
                    {
                        dta.set_Grade = 79;
                 

                    }
                    else if (gtotal >= 65 && gtotal <= 69)
                    {
                        dta.set_Grade = 78;
                  
                    }

                    else if (gtotal >= 60 && gtotal <= 64)
                    {
                        dta.set_Grade = 77;
                   

                    }
                    else if (gtotal >= 55 && gtotal <= 59)
                    {
                        dta.set_Grade = 76;
                     
                    }

                    else if (gtotal >= 50 && gtotal <= 54)
                    {
                        dta.set_Grade = 75;
                     

                    }
                    else if (gtotal >= 47 && gtotal <= 49)
                    {
                        dta.set_Grade = 74;
                      
                    }

                    else if (gtotal >= 44 && gtotal <= 46)
                    {
                        dta.set_Grade = 73;
                       

                    }
                    else if (gtotal >= 41 && gtotal <= 43)
                    {
                        dta.set_Grade = 72;
                    
                    }

                    else if (gtotal >= 38 && gtotal <= 40)
                    {
                        dta.set_Grade = 71;
                  

                    }
                    else if (gtotal >= 35 && gtotal <= 37)
                    {
                        dta.set_Grade = 70;
                
                    }

                    else if (gtotal >= 32 && gtotal <= 34)
                    {
                        dta.set_Grade = 69;
                    

                    }
                    else if (gtotal >= 29 && gtotal <= 31)
                    {
                        dta.set_Grade = 68;
                      
                    }

                    else if (gtotal >= 26 && gtotal <= 28)
                    {
                        dta.set_Grade = 67;
                      

                    }
                    else if (gtotal >= 23 && gtotal <= 25)
                    {
                        dta.set_Grade = 66;
                      
                    }

                    else if (gtotal >= 20 && gtotal <= 22)
                    {
                        dta.set_Grade = 65;
                       

                    }
                    else if (gtotal >= 18 && gtotal <= 19)
                    {
                        dta.set_Grade = 64;
                        
                    }

                    else if (gtotal >= 16 && gtotal <= 17)
                    {
                        dta.set_Grade = 63;
                  

                    }
                    else if (gtotal >= 14 && gtotal <= 15)
                    {
                        dta.set_Grade = 62;
                    
                    }

                    else if (gtotal >= 12 && gtotal <= 13)
                    {
                        dta.set_Grade = 61;
                  

                    }
                    else if (gtotal >= 10 && gtotal <= 11)
                    {
                        dta.set_Grade = 60;
                      
                    }

                    else if (gtotal >= 8 && gtotal <= 9)
                    {
                        dta.set_Grade = 59;
                       

                    }
                    else if (gtotal >= 6 && gtotal <= 7)
                    {
                        dta.set_Grade = 58;
                     
                    }
                    else if (gtotal >= 4 && gtotal <= 5)
                    {
                        dta.set_Grade = 57;
                      
                    }

                    else if (gtotal >= 2 && gtotal <= 3)
                    {
                        dta.set_Grade = 56;
                       

                    }
                    else if (gtotal >= 0 && gtotal <= 1)
                    {
                        dta.set_Grade = 55;
                      
                    }


                    _context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Total is null. Cannot set grade.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting grade: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            showdata();
        }

        public void setdatatosubject() {

            var sename = sem_txtbx.Text.Trim();
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
                    gunaDataGridView1.Visible = false;
                    guna2DataGridView1.Visible = false;
                    guna2HtmlLabel3.Visible = false;
                    guna2DataGridView2.Visible = false;
                    guna2TextBox2.Visible = false;
                }
                else
                {
                    guna2ComboBox1.DataSource = groupedStudents;
                    gunaDataGridView1.Visible = true;
                    guna2DataGridView1.Visible = true;
                    guna2HtmlLabel3.Visible = true;
                    guna2DataGridView2.Visible = true;
                    //guna2TextBox2.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void setdatatocourse() {

            var sename = sem_txtbx.Text.Trim();
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

                    gunaDataGridView1.Visible = false;
                    guna2DataGridView1.Visible =false;
                    guna2HtmlLabel3.Visible =  false;
                    guna2DataGridView2.Visible = false;
                    guna2TextBox2.Visible = false;
                }
                else
                {
                    guna2ComboBox2.DataSource = groupedStudents;
                    gunaDataGridView1.Visible = true;
                    guna2DataGridView1.Visible = true;
                    guna2HtmlLabel3.Visible = true;
                    guna2DataGridView2.Visible = true;
                    //guna2TextBox2.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        public void showdata() {
            var semister_Name = sem_txtbx.Text.Trim();
            var subjects = guna2ComboBox1.SelectedValue; 
            var course = guna2ComboBox2.SelectedValue;   

            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                var semester = _context.semesters.FirstOrDefault(q => q.sem_Name == semister_Name);

                if (semester != null)
                {
                    int name_Sem = semester.sem_Id;

                    var student_name = _context.Students
                                      .Where(q => q.teach_id == _id && q.subject == subjects.ToString() && q.course_year == course.ToString() && q.sem_Id == name_Sem)
                                      .OrderBy(q => q.LastName)
                                      .Select(q => new studendtViewModel
                                      {
                                          ID = q.t_Id,
                                          Lastname = q.LastName,
                                          Firstname = q.FirstName,
                                          Middlename = !string.IsNullOrEmpty(q.Middlename) ? q.Middlename.Substring(0, 1) + "." : string.Empty,

                                          })
                                      .ToList();

                    gunaDataGridView1.DataSource = student_name;
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("No such semester found. Please create the semester first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
              
            }

        }
        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            showdata();
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ismidterm = true;
            isfinal = false;
            btn_term.SendToBack();
            guna2Button1.FillColor = Color.Orange;
            guna2Button2.FillColor = Color.FromArgb(128, 64, 64);

           var semister_Name = sem_txtbx.Text.Trim();
            var subjects = guna2ComboBox1.SelectedValue;
            var course = guna2ComboBox2.SelectedValue;
      
            string typec = "Quizzes";
            string term_ex = "Midterm";
   
            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                if (guna2ComboBox3.SelectedIndex != -1)
                {
                    int sem_Id = _context.semesters.Where(q => q.sem_Name == semister_Name).Select(s => s.sem_Id).FirstOrDefault();
                    var check_class = _context.class_Record
                        .Where(q => q.teach_Id == _id && q.subject == subjects.ToString() && q.course == course.ToString() && q.sem == sem_Id.ToString() && q.term_exam == term_ex && q.mount == mount)
                        .ToList();

                    if (check_class.Count > 0)
                    {
                        type = "Quizzes";
                        gunaDataGridView1.Visible = true;
                        guna2DataGridView1.Visible = true;
                        guna2HtmlLabel3.Visible = true;
                        guna2DataGridView2.Visible = true;
                        guna2TextBox2.Visible = true;
                        btn_term.Visible = true;
                        record_Table.Visible = true;
                        guna2Button4.Visible = true;
                        guna2Button3.Visible = true;
                        guna2Button6.Visible = true;
                        guna2Button5.Visible = true;
                        guna2Button7.Visible = true;
                        guna2TextBox2.Visible = true;
                        guna2Button4.FillColor = Color.Orange;
                        guna2Button3.FillColor = Color.DarkKhaki;
                        guna2Button5.FillColor = Color.DarkKhaki;
                        guna2Button6.FillColor = Color.DarkKhaki;
                        changedataonclickbtn(typec);
                        if (mount == "lec") {

                            if (mo == 0 && fo == 0 )
                            {
                                guna2Button3.Visible = false;
                                guna2Button6.Location = new Point(guna2Button6.Location.X - 69, guna2Button6.Location.Y + 0);
                                o = 1;
                                mo = 1;
                                fo = 0;
                            }
                            else {
                           
                                    guna2Button3.Visible = false;
                                    guna2Button6.Location = new Point(guna2Button6.Location.X + 0, guna2Button6.Location.Y + 0);
                                
                            }

                        }
                        if (mount == "lab") {
                            guna2Button3.Visible = true;
                            if (o == 1)
                            {
                                guna2Button6.Location = new Point(guna2Button6.Location.X +69, guna2Button6.Location.Y + 0);
                             
                                o = 0;
                                mo = 0;
                            }
                        }

                    }
                    else
                    {
                        // No data found, prompt the user
                        var result = MessageBox.Show("No data found. Do you want to create data?", "No Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            guna2DataGridView1.Visible = true;
                            record_Table.Visible = true;
                            btn_term.Visible = true;
                            guna2Button4.FillColor = Color.Orange;
                            guna2Button3.FillColor = Color.DarkKhaki;
                            guna2Button5.FillColor = Color.DarkKhaki;
                            guna2Button6.FillColor = Color.DarkKhaki;
                            guna2Button4.Visible = true;
                            guna2Button3.Visible = true;
                            guna2Button5.Visible = true;
                            guna2Button6.Visible = true;
                            guna2Button7.Visible = true;
                            guna2TextBox2.Visible = true;
                            type = "Quizzes";
                            guna2Button4.FillColor = Color.Orange;
                            guna2Button3.FillColor = Color.DarkKhaki;
                            guna2Button5.FillColor = Color.DarkKhaki;
                            guna2Button6.FillColor = Color.DarkKhaki;
                            changedataonclickbtn(typec);
                            form2 = new add_wgt_term(_id, sem_Id, subjects.ToString(), course.ToString(), term_ex,mount);
                            form2.Form2Closed += Form2_FormClosed;
                            form2.ShowDialog();
                        }
                        else
                        {
                            gunaDataGridView1.Visible = false;
                            guna2DataGridView1.Visible = false;
                            guna2HtmlLabel3.Visible = false;
                            guna2DataGridView2.Visible = false;
                            guna2TextBox2.Visible = false;
                            btn_term.Visible = false;
                            record_Table.Visible = false;
                            guna2Button4.Visible = false;
                            guna2Button3.Visible = false;
                            guna2Button6.Visible = false;
                            guna2Button5.Visible = false;
                            guna2Button7.Visible = false;
                            guna2TextBox2.Visible = false;
                        }
                    }
                }
                else {

                    MessageBox.Show("Please select what class record type it is", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            
            
            }
            else
            {
                MessageBox.Show("Please fill up the fields above first", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Form3_FormClosed(object sender, EventArgs e)
        {
            RefreshDatas();
        }
        private void Form2_FormClosed(object sender, EventArgs e)
        {
            RefreshData(); 
        }
        public void RefreshData()
        {
            if (mount == "lec") {
                if (samp == 0) {
                    guna2Button3.Visible = false;
                    guna2Button6.Location = new Point(guna2Button6.Location.X - 69, guna2Button6.Location.Y + 0);
                    samp = 1;
                }
                else {
                    guna2Button3.Visible = false;
                    guna2Button6.Location = new Point(guna2Button6.Location.X + 0, guna2Button6.Location.Y + 0);
                }
                sam = 0;
            }
            if (mount == "lab")
            {
                if (sam == 0) {
                    guna2Button3.Visible = true;
                    guna2Button6.Location = new Point(guna2Button6.Location.X + 69, guna2Button6.Location.Y + 0);
                    sam = 1;
                }
             
            else {

                    guna2Button3.Visible = true;
                    guna2Button6.Location = new Point(guna2Button6.Location.X + 0, guna2Button6.Location.Y + 0);


                }
                samp = 0;
            }

            guna2DataGridView1.Visible = true;
            gunaDataGridView1.Visible = true;
            guna2DataGridView1.Visible = true;
            guna2HtmlLabel3.Visible = true;
            guna2DataGridView2.Visible = true;
            refresh_datagrid();


        }
        public void refresh_datagrid() {

            if (ismidterm == true)
            {
                term_Type = "Midterm";
            }
            if (isfinal == true)
            {
                term_Type = "Final";
            }

            var semister_Name = sem_txtbx.Text.Trim();
            var subjects = guna2ComboBox1.SelectedValue?.ToString();
            var course = guna2ComboBox2.SelectedValue?.ToString();

            int sem_Id = _context.semesters
                .Where(q => q.sem_Name == semister_Name)
                .Select(s => s.sem_Id)
                .FirstOrDefault();

            var getdata = (from ct in _context.class_Record
                           join st in _context.Students on ct.stud_Id equals st.t_Id
                           where ct.teach_Id == _id &&
                                 ct.subject == subjects &&
                                 ct.course == course &&
                                 ct.sem == sem_Id.ToString() &&
                                 ct.term_exam == term_Type && 
                                 ct.typeof_column == type && 
                                 ct.mount == mount
                           orderby st.LastName
                           select new classViewModel
                           {
                               ID = ct.ID,
                               sem = ct.sem,
                               subject = ct.subject,
                               course = ct.course,
                               teach_Id = _id,
                               column_1 = ct.column_1 != null ? (decimal)ct.column_1 : 0,
                               column_2 = ct.column_2 != null ? (decimal)ct.column_2 : 0,
                               column_3 = ct.column_3 != null ? (decimal)ct.column_3 : 0,
                               column_4 = ct.column_4 != null ? (decimal)ct.column_4 : 0,
                               column_5 = ct.column_5 != null ? (decimal)ct.column_5 : 0,
                               column_6 = ct.column_6 != null ? (decimal)ct.column_6 : 0,
                               column_7 = ct.column_7 != null ? (decimal)ct.column_7 : 0,
                               column_8 = ct.column_8 != null ? (decimal)ct.column_8 : 0,
                               column_9 = ct.column_9 != null ? (decimal)ct.column_9 : 0,
                               column_10 = ct.column_10 != null ? (decimal)ct.column_10 : 0,
                               stud_Id = ct.stud_Id != null ? (decimal)ct.stud_Id : 0,
                               term_total_wgt = ct.term_total_wgt != null ? (decimal)ct.term_total_wgt : 0,
                               total = ct.total != null ? (decimal)ct.total : 0,
                               lastname = st.LastName,
                               firstname = st.FirstName,
                               Middlename = st.Middlename
                           }).ToList();

            classViewModelBindingSource.DataSource = getdata;

            var getdatas = _context.high_Score
                .Where(q => q.teach_Id == _id &&
                            q.subject == subjects &&
                            q.course == course &&
                            q.sem == sem_Id.ToString() &&
                            q.term_exam == term_Type &&
                            q.typeof_column == type && 
                            q.mount == mount)
                .ToList();

            highViewModelBindingSource.DataSource = getdatas;

            var getdatass = (from crs in _context.class_Record
                             join st in _context.Students on crs.stud_Id equals st.t_Id
                             where crs.teach_Id == _id &&
                                   crs.subject == subjects &&
                                   crs.course == course &&
                                   crs.sem == sem_Id.ToString() &&
                                   crs.term_exam == term_Type &&
                                   crs.typeof_column == "Quizzes" &&
                                   crs.mount == mount
                             orderby st.LastName
                             select new classTermViewmodel
                             {
                                 ID = crs.ID,
                                 sem = crs.sem,
                                 subject = crs.subject,
                                 course = crs.course,
                                 teach_Id = _id,
                                 column_1 = crs.column_1 != null ? (decimal)crs.column_1 : 0,
                                 column_2 = crs.column_2 != null ? (decimal)crs.column_2 : 0,
                                 column_3 = crs.column_3 != null ? (decimal)crs.column_3 : 0,
                                 column_4 = crs.column_4 != null ? (decimal)crs.column_4 : 0,
                                 column_5 = crs.column_5 != null ? (decimal)crs.column_5 : 0,
                                 column_6 = crs.column_6 != null ? (decimal)crs.column_6 : 0,
                                 column_7 = crs.column_7 != null ? (decimal)crs.column_7 : 0,
                                 column_8 = crs.column_8 != null ? (decimal)crs.column_8 : 0,
                                 column_9 = crs.column_9 != null ? (decimal)crs.column_9 : 0,
                                 column_10 = crs.column_10 != null ? (decimal)crs.column_10 : 0,
                                 stud_Id = crs.stud_Id != null ? (decimal)crs.stud_Id : 0,
                                 wgt = crs.wgt != null ? (decimal)crs.wgt : 0,
                                 total = crs.total != null ? (decimal)crs.total : 0,
                                 term_Score = crs.term_Score != null ? (decimal)crs.term_Score : 0,
                                 type_total = crs.type_total != null ? (decimal)crs.type_total : 0,
                                 lastname = st.LastName,
                                 firstname = st.FirstName,
                                 Middlename = st.Middlename
                             })
                    .ToList();

            classTermViewmodelBindingSource.DataSource = getdatass;

        }

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Validate the input based on your criteria
                string inputValue = cell.Value?.ToString();

                // Check if the input is not null or empty
                if (!string.IsNullOrEmpty(inputValue))
                {
                    // Check if the input contains only numbers and does not exceed three digits
                    if (IsNumeric(inputValue) && inputValue.Length <= 3)
                    {
                        // Valid input (only numbers and not exceeding three digits)s
                        // Proceed with updating the database
                        UpdateDatabases(e.RowIndex, inputValue);
                    }
                    else
                    {
                        // Show an alert message for invalid input
                        MessageBox.Show("Invalid input. Please enter a numeric value with at most three digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Handle empty or null input if needed
                    // Show an alert message or take appropriate action
                    MessageBox.Show("Invalid input. Please enter a numeric value with at most three digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void RefreshDatas()
        {
            if (ismidterm == true) {
               term_exis = "Midterm";

            }
            if (isfinal == true) {
                term_exis = "Final";

            }

            if (isquiz == true && isoral == false && isperformance == false && isproject == false)
            {
                type_ofbtn = "Quizzes";
            }
            if (isquiz == false && isoral == true && isperformance == false && isproject == false)
            {
                type_ofbtn = "Oral";
            }
            if (isquiz == false && isoral == false && isperformance == true && isproject == false)
            {
                type_ofbtn = "Performance";
            }
            if (isquiz == false && isoral == false && isperformance == false && isproject == true)
            {
                type_ofbtn = "Project";
            }
            var semister_Name = sem_txtbx.Text.Trim();
                var subjects = guna2ComboBox1.SelectedValue?.ToString();
                var course = guna2ComboBox2.SelectedValue?.ToString();

                int sem_Id = _context.semesters
                    .Where(q => q.sem_Name == semister_Name)
                    .Select(s => s.sem_Id)
                    .FirstOrDefault();

                var getdata =   _context.high_Score
                                .Where(q => q.teach_Id == _id &&
                                q.subject == subjects &&
                                q.course == course &&
                                q.sem == sem_Id.ToString() &&
                                q.term_exam == term_exis &&
                                q.typeof_column == type_ofbtn && 
                                q.mount == mount)
                                 .ToList();

            highViewModelBindingSource.DataSource = getdata;
            var getdatas = (from ct in _context.class_Record
                            join st in _context.Students on ct.stud_Id equals st.t_Id
                            where ct.teach_Id == _id &&
                                  ct.subject == subjects &&
                                  ct.course == course &&
                                  ct.sem == sem_Id.ToString() &&
                                  ct.term_exam == term_exis &&
                                  ct.typeof_column == type_ofbtn &&
                                  ct.mount == mount
                            orderby st.LastName
                            select new classViewModel
                            {
                                ID = ct.ID,
                                sem = ct.sem,
                                subject = ct.subject,
                                course = ct.course,
                                teach_Id = _id,
                                column_1 = ct.column_1 != null ? (decimal)ct.column_1 : 0,
                                column_2 = ct.column_2 != null ? (decimal)ct.column_2 : 0,
                                column_3 = ct.column_3 != null ? (decimal)ct.column_3 : 0,
                                column_4 = ct.column_4 != null ? (decimal)ct.column_4 : 0,
                                column_5 = ct.column_5 != null ? (decimal)ct.column_5 : 0,
                                column_6 = ct.column_6 != null ? (decimal)ct.column_6 : 0,
                                column_7 = ct.column_7 != null ? (decimal)ct.column_7 : 0,
                                column_8 = ct.column_8 != null ? (decimal)ct.column_8 : 0,
                                column_9 = ct.column_9 != null ? (decimal)ct.column_9 : 0,
                                column_10 = ct.column_10 != null ? (decimal)ct.column_10 : 0,
                                stud_Id = ct.stud_Id != null ? (decimal)ct.stud_Id : 0,
                                wgt = ct.wgt != null ? (decimal)ct.wgt : 0,
                                total = ct.total != null ? (decimal)ct.total : 0,
                                lastname = st.LastName,
                                firstname = st.FirstName,
                                Middlename = st.Middlename
                            }).ToList();

            classViewModelBindingSource.DataSource = getdatas;



        }

        private void changedataonclickbtn(string getbtn) {


            if (ismidterm == true && isfinal == false)
            {
                term_Type = "Midterm";
            }
            if (isfinal == true && ismidterm == false)
            {
                term_Type = "Final";
            }
            var semister_Name = sem_txtbx.Text.Trim();
            var subjects = guna2ComboBox1.SelectedValue?.ToString();
            var course = guna2ComboBox2.SelectedValue?.ToString();
            int sem_Id = _context.semesters
                .Where(q => q.sem_Name == semister_Name)
                .Select(s => s.sem_Id)
                .FirstOrDefault();



            var getdata = _context.high_Score
                .Where(q => q.teach_Id == _id &&
                            q.subject == subjects &&
                            q.course == course &&
                            q.sem == sem_Id.ToString() &&
                            q.term_exam == term_Type &&
                            q.typeof_column == getbtn &&
                            q.mount == mount)
                .ToList();

            highViewModelBindingSource.DataSource = getdata;

            var getdatas = (from crs in _context.class_Record
                           join st in _context.Students on crs.stud_Id equals st.t_Id
                           where crs.teach_Id == _id &&
                                 crs.subject == subjects &&
                                 crs.course == course &&
                                 crs.sem == sem_Id.ToString() &&
                                 crs.term_exam == term_Type &&
                                 crs.typeof_column == getbtn &&
                                 crs.mount == mount
                           orderby st.LastName
                           select new classViewModel
                           {
                               ID = crs.ID,
                               sem = crs.sem,
                               subject = crs.subject,
                               course = crs.course,
                               teach_Id = _id,
                               column_1 = crs.column_1 != null ? (decimal)crs.column_1 : 0, 
                               column_2 = crs.column_2 != null ? (decimal)crs.column_2 : 0,
                               column_3 = crs.column_3 != null ? (decimal)crs.column_3 : 0,
                               column_4 = crs.column_4 != null ? (decimal)crs.column_4 : 0,
                               column_5 = crs.column_5 != null ? (decimal)crs.column_5 : 0,
                               column_6 = crs.column_6 != null ? (decimal)crs.column_6 : 0,
                               column_7 = crs.column_7 != null ? (decimal)crs.column_7 : 0,
                               column_8 = crs.column_8 != null ? (decimal)crs.column_8 : 0,
                               column_9 = crs.column_9 != null ? (decimal)crs.column_9 : 0,
                               column_10 = crs.column_10 != null ? (decimal)crs.column_10 : 0,
                               stud_Id = crs.stud_Id != null ? (decimal)crs.stud_Id : 0,
                               term_total_wgt = crs.term_total_wgt != null ? (decimal)crs.term_total_wgt : 0,
                               total = crs.total != null ? (decimal)crs.total : 0,
                               lastname = st.LastName,
                               firstname = st.FirstName,
                               Middlename = st.Middlename

                           }).ToList();

            classViewModelBindingSource.DataSource = getdatas;

            var getdatass = (from cr in _context.class_Record
                             join st in _context.Students on cr.stud_Id equals st.t_Id
                             where cr.teach_Id == _id &&
                                   cr.subject == subjects &&
                                   cr.course == course &&
                                   cr.sem == sem_Id.ToString() &&
                                   cr.term_exam == term_Type &&
                                   cr.typeof_column == "Quizzes" &&
                                   cr.mount == mount    
                                   orderby st.LastName
                             select new classTermViewmodel
                             {
                                 ID = cr.ID,
                                 sem = cr.sem,
                                 subject = cr.subject,
                                 course = cr.course,
                                 teach_Id = _id,
                                 column_1 = cr.column_1 != null ? (decimal)cr.column_1 : 0,
                                 column_2 = cr.column_2 != null ? (decimal)cr.column_2 : 0,
                                 column_3 = cr.column_3 != null ? (decimal)cr.column_3 : 0,
                                 column_4 = cr.column_4 != null ? (decimal)cr.column_4 : 0,
                                 column_5 = cr.column_5 != null ? (decimal)cr.column_5 : 0,
                                 column_6 = cr.column_6 != null ? (decimal)cr.column_6 : 0,
                                 column_7 = cr.column_7 != null ? (decimal)cr.column_7 : 0,
                                 column_8 = cr.column_8 != null ? (decimal)cr.column_8 : 0,
                                 column_9 = cr.column_9 != null ? (decimal)cr.column_9 : 0,
                                 column_10 = cr.column_10 != null ? (decimal)cr.column_10 : 0,
                                 stud_Id = cr.stud_Id != null ? (decimal)cr.stud_Id : 0,
                                 wgt = cr.wgt != null ? (decimal)cr.wgt : 0,
                                 total = cr.total != null ? (decimal)cr.total : 0,
                                 term_Score = cr.term_Score != null ? (decimal)cr.term_Score : 0,
                                 type_total = cr.type_total != null ? (decimal)cr.type_total : 0,
                                 lastname = st.LastName,
                                 firstname = st.FirstName,
                                 Middlename = st.Middlename
                             })
                       .ToList();

            classTermViewmodelBindingSource.DataSource = getdatass;
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            guna2Button4.FillColor = Color.Orange;
            guna2Button3.FillColor = Color.DarkKhaki;
            guna2Button5.FillColor = Color.DarkKhaki;
            guna2Button6.FillColor = Color.DarkKhaki;
            isquiz = true;
             isoral = false;
             isperformance = false;
             isproject = false;

            string btn = "Quizzes";
            type = "Quizzes";
            changedataonclickbtn(btn);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

            guna2Button3.FillColor = Color.Orange;
            guna2Button4.FillColor = Color.DarkKhaki;
            guna2Button5.FillColor = Color.DarkKhaki;
            guna2Button6.FillColor = Color.DarkKhaki;
            isquiz = false;
            isoral = false;
            isperformance = true;
            isproject = false;

            type = "Performance";
            string btn = "Performance";
            changedataonclickbtn(btn);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            guna2Button6.FillColor = Color.Orange;
            guna2Button4.FillColor = Color.DarkKhaki;
            guna2Button5.FillColor = Color.DarkKhaki;
            guna2Button3.FillColor = Color.DarkKhaki;
            isquiz = false;
            isoral = true;
            isperformance = false;
            isproject = false;


            string btn = "Oral";
            type = "Oral";
            changedataonclickbtn(btn);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            guna2Button5.FillColor = Color.Orange;
            guna2Button4.FillColor = Color.DarkKhaki;
            guna2Button3.FillColor = Color.DarkKhaki;
            guna2Button6.FillColor = Color.DarkKhaki;
            isquiz = false;
            isoral = false;
            isperformance = false;
            isproject = true;


            string btn = "Project";
            type = "Project";
            changedataonclickbtn(btn);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ismidterm = false;
            isfinal = true;
            btn_term.BringToFront();
            guna2Button1.FillColor = Color.FromArgb(128, 64, 64);
            guna2Button2.FillColor = Color.Orange;
            var semister_Name = sem_txtbx.Text.Trim();
            var subjects = guna2ComboBox1.SelectedValue;
            var course = guna2ComboBox2.SelectedValue;
            string typec = "Quizzes";
            string term_ex = "Final";

            if (!string.IsNullOrEmpty(semister_Name) && subjects != null && course != null)
            {
                if (guna2ComboBox3.SelectedIndex != -1)
                {
                    int sem_Id = _context.semesters.Where(q => q.sem_Name == semister_Name).Select(s => s.sem_Id).FirstOrDefault();
                    var check_class = _context.class_Record
                        .Where(q => q.teach_Id == _id && q.subject == subjects.ToString() && q.course == course.ToString() && q.sem == sem_Id.ToString() && q.term_exam == term_ex && q.mount == mount)
                        .ToList();

                    if (check_class.Count > 0)
                    {
                        gunaDataGridView1.Visible = true;
                        guna2DataGridView1.Visible = true;
                        guna2HtmlLabel3.Visible = true;
                        guna2DataGridView2.Visible = true;
                        guna2TextBox2.Visible = true;
                        btn_term.Visible = true;
                        record_Table.Visible = true;
                        guna2Button4.Visible = true;
                        guna2Button3.Visible = true;
                        guna2Button6.Visible = true;
                        guna2Button5.Visible = true;
                        guna2Button7.Visible = true;
                        guna2TextBox2.Visible = true;
                        guna2Button4.FillColor = Color.Orange;
                        guna2Button3.FillColor = Color.DarkKhaki;
                        guna2Button5.FillColor = Color.DarkKhaki;
                        guna2Button6.FillColor = Color.DarkKhaki;
                        changedataonclickbtn(typec);
                        if (mount == "lec")
                        {

                            if (fo == 0 && mo==0)
                            {
                                guna2Button3.Visible = false;
                                guna2Button6.Location = new Point(guna2Button6.Location.X - 69, guna2Button6.Location.Y + 0);
                                o = 1;
                                fo = 1;
                            }
                            else
                            {
                                
                                    guna2Button3.Visible = false;
                                    guna2Button6.Location = new Point(guna2Button6.Location.X + 0, guna2Button6.Location.Y + 0);
                               
                            }
                        }
                        if (mount == "lab")
                        {
                            guna2Button3.Visible = true;
                            if (o == 1)
                            {
                                guna2Button6.Location = new Point(guna2Button6.Location.X + 69, guna2Button6.Location.Y + 0);

                                o = 0;
                                fo = 0;
                                mo = 0;
                            }
                        }
                    } 
                    else
                    {
                        // No data found, prompt the user
                        var result = MessageBox.Show("No data found. Do you want to create data?", "No Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            guna2DataGridView1.Visible = true;
                            record_Table.Visible = true;
                            btn_term.Visible = true;
                            guna2Button4.FillColor = Color.Orange;
                            guna2Button3.FillColor = Color.DarkKhaki;
                            guna2Button5.FillColor = Color.DarkKhaki;
                            guna2Button6.FillColor = Color.DarkKhaki;
                            guna2Button4.Visible = true;
                            guna2Button3.Visible = true;
                            guna2Button5.Visible = true;
                            guna2Button6.Visible = true;
                            guna2Button7.Visible = true;
                            guna2TextBox2.Visible = true;
                            type = "Quizzes";
                            changedataonclickbtn(typec);
                            form2 = new add_wgt_term(_id, sem_Id, subjects.ToString(), course.ToString(), term_ex, mount);
                            form2.Form2Closed += Form2_FormClosed;
                            form2.ShowDialog();

                        }
                        else
                        {
                            gunaDataGridView1.Visible = false;
                            guna2DataGridView1.Visible = false;
                            guna2HtmlLabel3.Visible = false;
                            guna2DataGridView2.Visible = false;
                            guna2TextBox2.Visible = false;
                            btn_term.Visible = false;
                            record_Table.Visible = false;
                            guna2Button4.Visible = false;
                            guna2Button3.Visible = false;
                            guna2Button6.Visible = false;
                            guna2Button5.Visible = false;
                            guna2Button7.Visible = false;
                            guna2TextBox2.Visible = false;
                        }
                    }
                }
                else {

                    MessageBox.Show("Please select what class record type it is", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            else
            {
                MessageBox.Show("Please fill up the fields above first", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = guna2DataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Validate the input based on your criteria
                string inputValue = cell.Value?.ToString();

                // Check if the input is not null or empty
                if (!string.IsNullOrEmpty(inputValue))
                {
                    // Check if the input contains only numbers and does not exceed three digits
                    if (IsNumeric(inputValue) && inputValue.Length <= 3)
                    {
                        // Valid input (only numbers and not exceeding three digits)s
                        // Proceed with updating the database
                        UpdateDatabasess(e.RowIndex, inputValue);
                    }
                    else
                    {
                        // Show an alert message for invalid input
                        MessageBox.Show("Invalid input. Please enter a numeric value with at most three digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Handle empty or null input if needed
                    // Show an alert message or take appropriate action
                    MessageBox.Show("Invalid input. Please enter a numeric value with at most three digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateDatabasess(int rowIndex, string inputValue)
        {
            try
            {

                // Get the ID from the first cell assuming it's an integer
                if (int.TryParse(guna2DataGridView2.Rows[rowIndex].Cells[0].Value?.ToString(), out int rowId))
                {
                    // Retrieve the existing entity from the database
                    var existingEntity = _context.class_Record.Find(rowId);

                    if (existingEntity != null)
                    {

                        existingEntity.term_Score = Convert.ToDecimal(guna2DataGridView2.Rows[rowIndex].Cells[22].Value ?? 0);
                        existingEntity.type_total = Math.Round(Convert.ToDecimal(existingEntity.term_Score * (existingEntity.wgt / 100)), 1);

                        _context.SaveChanges();

                        settermgrade(existingEntity.term_Score, rowId);
                        MessageBox.Show("Successfully Added");


                        refresh_datagrid();




                    }
                    else
                    {
                        // Handle the case when the entity is not found (if needed)
                        MessageBox.Show("Entity not found in the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //highViewModelBindingSource.DataSource = _context.high_Score.Where(q => q.high_ID == rowId).FirstOrDefault();
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error updating the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void settermgrade(decimal? total, int IDrow)
        {
            try
            {
                if (total.HasValue)
                {

                    var dta = _context.class_Record.FirstOrDefault(q => q.ID == IDrow);
                    int gtotal = (int)Math.Round(dta.term_Score ?? 0, 0);

                    if (gtotal == 100)
                    {
                        dta.trem_wgt = 95;

                    }
                    else if (gtotal == 99)
                    {
                        dta.trem_wgt = 94;

                    }
                    else if (gtotal == 98)
                    {
                        dta.trem_wgt = 93;

                    }
                    else if (gtotal == 97)
                    {
                        dta.trem_wgt = 92;
                    }
                    else if (gtotal == 96)
                    {
                        dta.trem_wgt = 91;

                    }
                    else if (gtotal == 95)
                    {
                        dta.trem_wgt = 90;

                    }

                    else if (gtotal >= 93 && gtotal <= 94)
                    {
                        dta.trem_wgt = 89;


                    }
                    else if (gtotal >= 91 && gtotal <= 92)
                    {
                        dta.trem_wgt = 88;

                    }

                    else if (gtotal >= 89 && gtotal <= 90)
                    {
                        dta.trem_wgt = 87;

                    }
                    else if (gtotal >= 87 && gtotal <= 88)
                    {
                        dta.trem_wgt = 86;
                    }

                    else if (gtotal >= 85 && gtotal <= 86)
                    {
                        dta.trem_wgt = 85;


                    }
                    else if (gtotal >= 83 && gtotal <= 84)
                    {
                        dta.trem_wgt = 84;

                    }

                    else if (gtotal >= 81 && gtotal <= 82)
                    {
                        dta.trem_wgt = 83;


                    }
                    else if (gtotal >= 79 && gtotal <= 80)
                    {
                        dta.trem_wgt = 82;

                    }

                    else if (gtotal >= 77 && gtotal <= 78)
                    {
                        dta.trem_wgt = 81;


                    }
                    else if (gtotal >= 75 && gtotal <= 76)
                    {
                        dta.trem_wgt = 80;

                    }

                    else if (gtotal >= 70 && gtotal <= 74)
                    {
                        dta.trem_wgt = 79;


                    }
                    else if (gtotal >= 65 && gtotal <= 69)
                    {
                        dta.trem_wgt = 78;

                    }

                    else if (gtotal >= 60 && gtotal <= 64)
                    {
                        dta.trem_wgt = 77;


                    }
                    else if (gtotal >= 55 && gtotal <= 59)
                    {
                        dta.trem_wgt = 76;

                    }

                    else if (gtotal >= 50 && gtotal <= 54)
                    {
                        dta.trem_wgt = 75;


                    }
                    else if (gtotal >= 47 && gtotal <= 49)
                    {
                        dta.trem_wgt = 74;

                    }

                    else if (gtotal >= 44 && gtotal <= 46)
                    {
                        dta.trem_wgt = 73;


                    }
                    else if (gtotal >= 41 && gtotal <= 43)
                    {
                        dta.trem_wgt = 72;

                    }

                    else if (gtotal >= 38 && gtotal <= 40)
                    {
                        dta.trem_wgt = 71;


                    }
                    else if (gtotal >= 35 && gtotal <= 37)
                    {
                        dta.trem_wgt = 70;

                    }

                    else if (gtotal >= 32 && gtotal <= 34)
                    {
                        dta.trem_wgt = 69;


                    }
                    else if (gtotal >= 29 && gtotal <= 31)
                    {
                        dta.trem_wgt = 68;

                    }

                    else if (gtotal >= 26 && gtotal <= 28)
                    {
                        dta.trem_wgt = 67;


                    }
                    else if (gtotal >= 23 && gtotal <= 25)
                    {
                        dta.trem_wgt = 66;

                    }

                    else if (gtotal >= 20 && gtotal <= 22)
                    {
                        dta.trem_wgt = 65;


                    }
                    else if (gtotal >= 18 && gtotal <= 19)
                    {
                        dta.trem_wgt = 64;

                    }

                    else if (gtotal >= 16 && gtotal <= 17)
                    {
                        dta.trem_wgt = 63;


                    }
                    else if (gtotal >= 14 && gtotal <= 15)
                    {
                        dta.trem_wgt = 62;

                    }

                    else if (gtotal >= 12 && gtotal <= 13)
                    {
                        dta.trem_wgt = 61;


                    }
                    else if (gtotal >= 10 && gtotal <= 11)
                    {
                        dta.trem_wgt = 60;

                    }

                    else if (gtotal >= 8 && gtotal <= 9)
                    {
                        dta.trem_wgt = 59;


                    }
                    else if (gtotal >= 6 && gtotal <= 7)
                    {
                        dta.trem_wgt = 58;

                    }
                    else if (gtotal >= 4 && gtotal <= 5)
                    {
                        dta.trem_wgt = 57;

                    }

                    else if (gtotal >= 2 && gtotal <= 3)
                    {
                        dta.trem_wgt = 56;


                    }
                    else if (gtotal >= 0 && gtotal <= 1)
                    {
                        dta.trem_wgt = 55;

                    }


                    _context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Total is null. Cannot set grade.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting grade: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void gunaDataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            guna2DataGridView1.FirstDisplayedScrollingRowIndex = gunaDataGridView1.FirstDisplayedScrollingRowIndex;
            guna2DataGridView2.FirstDisplayedScrollingRowIndex = gunaDataGridView1.FirstDisplayedScrollingRowIndex;
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            String sem__Name = sem_txtbx.Text.Trim();

            final_Ratings rate = new final_Ratings(_id,sem__Name);
            rate.ShowDialog();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            String name = sem_txtbx.Text.Trim();
            test_Score rate = new test_Score(_id);
            rate.acceptName(name);
            rate.ShowDialog();
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            bool isActive = guna2ToggleSwitch1.Checked;

            if (isActive)
            {
                record_Table.ReadOnly = false;
                gunaDataGridView1.ReadOnly = true;
                guna2DataGridView1.ReadOnly = false;
                guna2DataGridView2.ReadOnly = false;
                guna2Button1.Enabled = true;
                guna2Button2.Enabled = true;
                guna2Button3.Enabled = true;
                guna2Button4.Enabled = true;
                guna2Button5.Enabled = true;
                guna2Button6.Enabled = true;
                guna2Button7.Enabled = true;
                sem_txtbx.ReadOnly = true;
                guna2ComboBox1.Enabled = true;
                guna2ComboBox2.Enabled = true;
                guna2TextBox2.ReadOnly = true;
            }
            else
            {
                guna2TextBox2.ReadOnly = true;
                record_Table.ReadOnly = true;
                gunaDataGridView1.ReadOnly = true;
                guna2DataGridView1.ReadOnly = true;
                guna2DataGridView2.ReadOnly = true;
                guna2Button1.Enabled = false;
                guna2Button2.Enabled = false;
                guna2Button3.Enabled = false;
                guna2Button4.Enabled = false;
                guna2Button5.Enabled = false;
                guna2Button6.Enabled = false;
                guna2Button7.Enabled = false;
                sem_txtbx.ReadOnly = false;
                guna2ComboBox1.Enabled = false;
                guna2ComboBox2.Enabled = false;


            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            gunaDataGridView1.FirstDisplayedScrollingRowIndex = guna2DataGridView1.FirstDisplayedScrollingRowIndex;
            guna2DataGridView2.FirstDisplayedScrollingRowIndex = guna2DataGridView1.FirstDisplayedScrollingRowIndex;
        }

        private void guna2DataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            gunaDataGridView1.FirstDisplayedScrollingRowIndex = guna2DataGridView2.FirstDisplayedScrollingRowIndex;
            guna2DataGridView1.FirstDisplayedScrollingRowIndex = guna2DataGridView2.FirstDisplayedScrollingRowIndex;
        }


        private void gunaDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (gunaDataGridView1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = gunaDataGridView1.SelectedCells[0].RowIndex;

                guna2DataGridView1.ClearSelection();

                if (selectedRowIndex < guna2DataGridView1.Rows.Count)
                {
                    guna2DataGridView1.Rows[selectedRowIndex].Selected = true;
                }

           
                guna2DataGridView2.ClearSelection();

                if (selectedRowIndex < guna2DataGridView2.Rows.Count)
                {
                    guna2DataGridView2.Rows[selectedRowIndex].Selected = true;
                }
            }
        }

        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var moun = guna2ComboBox3.SelectedIndex;
            if (moun == 0)
            {
                mount = "lec";
            }
            else {

                mount = "lab";
            }
        }
    }
}
