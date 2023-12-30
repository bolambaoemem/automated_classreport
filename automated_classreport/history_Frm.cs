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
using automated_classreport.Report;

namespace automated_classreport
{
    public partial class history_Frm : Form
    {
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        string monthname;
        string mount;
        private DataView originalDataView;
        public history_Frm()
        {
            InitializeComponent();
        }
        public history_Frm(int id):this()
        {
            _id = id;
        }

        private void history_Frm_Load(object sender, EventArgs e)
        {

            originalDataView = GetOriginalDataView();
            guna2DataGridView1.DataSource = originalDataView;
            guna2DataGridView1.ClearSelection();
       

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
            
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "view")
                {
                 
                    var idValue = guna2DataGridView1.Rows[e.RowIndex].Cells[1].Value;

                    history_Report_frm pnl = new history_Report_frm(Convert.ToInt32(idValue), _id,mount);
                    pnl.ShowDialog();
                   
                }

           
                else if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "delete")
                {
                    var idValue = guna2DataGridView1.Rows[e.RowIndex].Cells[1].Value;
                    var hisIDs = (int)guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value;
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        var deleteinclassrecord = _context.class_Record.Where(q => q.teach_Id == _id && q.sem == idValue.ToString()).ToList();
                        if (deleteinclassrecord.Any())
                        {
                            foreach (var classrec in deleteinclassrecord)
                            {
                                _context.class_Record.Remove(classrec);
                            }

                            _context.SaveChanges();
                        }
                        var deleteinhigh = _context.high_Score.Where(q => q.teach_Id == _id && q.sem == idValue.ToString()).ToList();
                        if (deleteinhigh.Any())
                        {
                            foreach (var clashigh in deleteinhigh)
                            {

                                _context.high_Score.Remove(clashigh);

                            }
                            _context.SaveChanges();
                        }
                        int ids = Convert.ToInt32(idValue);
                        var deleteinstud = _context.Students.Where(q => q.teach_id == _id && q.sem_Id == ids).ToList();
                        if (deleteinhigh.Any())
                        {
                            foreach (var classtud in deleteinstud)
                            {

                                _context.Students.Remove(classtud);

                            }
                            _context.SaveChanges();
                        }
                        var historyToDelete = _context.histories.SingleOrDefault(h => h.his_Id == hisIDs);
                        if (historyToDelete != null)
                        {
                          
                          
                            _context.histories.Remove(historyToDelete);

                            _context.SaveChanges();
                        }
                        MessageBox.Show("Successfully Deleted");

                        originalDataView = GetOriginalDataView();
                        guna2DataGridView1.DataSource = originalDataView;
                        guna2DataGridView1.ClearSelection();
                    }

                }
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentYear = DateTime.Now.Year;
            int selectedin = guna2ComboBox1.SelectedIndex;
            if (selectedin == 0) {
                monthname = "January";
                filtertable(monthname,currentYear);
            }
            if (selectedin == 1)
            {
                monthname = "February";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 2)
            {
                monthname = "March";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 3)
            {
                monthname = "April";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 4)
            {
                monthname = "May";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 5)
            {
                monthname = "June";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 6)
            {
                monthname = "July";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 7)
            {
                monthname = "August";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 8)
            {
                monthname = "September";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 9)
            {
                monthname = "October";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 10)
            {
                monthname = "November";
                filtertable(monthname, currentYear);
            }
            if (selectedin == 11)
            {
                monthname = "December";
                filtertable(monthname, currentYear);
            }
        }

        public void filtertable(string selectedmonthname, int cyear) {

            string filterExpression = $"date LIKE '{selectedmonthname}%' AND date LIKE '%{cyear}'";
            originalDataView.RowFilter = filterExpression;
            guna2DataGridView1.ClearSelection();

        }
        private DataView GetOriginalDataView()
        {
            var data = _context.histories
                .Where(q => q.teach_id == _id)
                .AsEnumerable()
                .Select(q => new collegiatemfViewmodel
                {
                    ID = q.his_Id,
                    sem_ID = q.sem_Id.ToString(),
                    semName = q.semName,
                    sem_mean = q.sem_mean == "1" ? "1st" : (q.sem_mean == "2" ? "2nd" : q.sem_mean.ToString()),
                    date = DateTime.Parse(q.history1.ToString()).ToString("MMMM dd, yyyy")
                })
                .ToList();

            DataTable dataTable = ConvertToDataTable(data);

            return new DataView(dataTable);
        }
        private DataTable ConvertToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mon = guna2ComboBox2.SelectedIndex;
            if (mon == 0)
            {
                mount = "lec";
            }
            else {
                mount = "lec";
            }
        }
    }
}
