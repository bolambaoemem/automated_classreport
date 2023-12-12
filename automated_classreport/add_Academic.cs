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
    public partial class add_Academic : Form
    {
        public event EventHandler Form2Closed;
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        int num;
        public add_Academic()
        {
            InitializeComponent();
        }

        public add_Academic(int id):this()
         
        {
            _id = id;
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OnForm2Closed();
            this.Close();
        }
        protected virtual void OnForm2Closed()
        {
            Form2Closed?.Invoke(this, EventArgs.Empty);
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
                   

                    semester sem = new semester();
                    sem.teach_id = _id;
                    sem.sem_Name = name;
                    sem.sem_Mean = num;
                    _context.semesters.Add(sem);
                    _context.SaveChanges(); 
                    OnForm2Closed();
                    this.Close();

                }


            }
            catch (Exception ex) { 
            
            }
        }

        private void add_Academic_Load(object sender, EventArgs e)
        {
            var check = _context.semesters
                   .Where(q => q.teach_id == _id)
                   .ToList();
            int sem_Count = check.Count;
            if (sem_Count <= 0)
            {
                num = 1;

            }
            if (sem_Count == 1)
            {
                num = 2;

            }
        }
    }
}
