using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automated_classreport
{
    public partial class class_Record : Form
    {
        public class_Record()
        {
            InitializeComponent();
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gunaLabel1_Click(object sender, EventArgs e)
        {

        }

        private void gunaDataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            class_Report_content dsh = new class_Report_content();
            dsh.TopLevel = false;
            panel_content.Controls.Clear();
            panel_content.Controls.Add(dsh);

            dsh.Show();
        }

        private void class_Record_Load(object sender, EventArgs e)
        {
            class_Report_content dsh = new class_Report_content();
            dsh.TopLevel = false;
            panel_content.Controls.Clear();
            panel_content.Controls.Add(dsh);

            dsh.Show();
       
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            create_Acad dsh = new create_Acad();
            dsh.TopLevel = false;
            record_panel.Controls.Clear();
            record_panel.Controls.Add(dsh);
            dsh.Show();
        }
    }
}
