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
    public partial class update_Academic : Form
    {
        public event EventHandler Form3Closed;
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        int semID;
        string _name;
        int num;
        public update_Academic()
        {
            InitializeComponent();
        }

        public update_Academic(int id,String name):this()
         
        {
            _id = id;
            _name = name;
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OnForm3Closed();
            this.Close();
        }
        protected virtual void OnForm3Closed()
        {
            Form3Closed?.Invoke(this, EventArgs.Empty);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try {
                string name = sem_Name.Text;
            
                if (name.Length < 0 || name.Length == 0)
                {
                    MessageBox.Show("Please fill Up the fields");
                }
                else {


                    var existingSemester = _context.semesters.FirstOrDefault(q => q.sem_Id == semID);

                    if (existingSemester != null)
                    {
 
                        existingSemester.sem_Name = name;

                        _context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Semester not found for ID: " + semID);
                    }
                    OnForm3Closed();
                    this.Close();

                }


            }
            catch (Exception ex) { 
            
            }
        }

        private void add_Academic_Load(object sender, EventArgs e)
        {
            var check = _context.semesters
                   .Where(q => q.teach_id == _id && q.sem_Name == _name)
                   .FirstOrDefault();
            sem_Name.Text = check.sem_Name;
            semID = check.sem_Id;
        }
    }
}
