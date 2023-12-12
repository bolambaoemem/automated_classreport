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
    public partial class user_Dash : Form
    {
        int _id;
     

        public user_Dash()
        {
            InitializeComponent();
        }
        public user_Dash(int Id):this()  
        {
            _id = Id;
        }


        private void gunaPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            academic_btn.FillColor = Color.White;
            academic_btn.ForeColor = Color.Gray;
            academic_btn.Size = new Size(188, 53);


            guna2Button1.FillColor = Color.Gray;
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Size = new Size(160, 53);

            guna2Button6.FillColor = Color.Gray;
            guna2Button6.ForeColor = Color.White;
            guna2Button6.Size = new Size(160, 53);

            guna2Button3.FillColor = Color.Gray;
            guna2Button3.ForeColor = Color.White;
            guna2Button3.Size = new Size(160, 53);


            create_Acad dsh = new create_Acad(_id);
            dsh.TopLevel = false;
            plContent.Controls.Clear();

            plContent.Controls.Add(dsh);
            dsh.Show();

        }

        private void user_Dash_Load(object sender, EventArgs e)
        {

        }



        private void guna2Button1_Click_1(object sender, EventArgs e)
        {

            guna2Button1.FillColor = Color.White;
            guna2Button1.ForeColor = Color.Gray;
            guna2Button1.Size = new Size(188, 53);


            academic_btn.FillColor = Color.Gray;
            academic_btn.ForeColor = Color.White;
            academic_btn.Size = new Size(160, 53);

            guna2Button6.FillColor = Color.Gray;
            guna2Button6.ForeColor = Color.White;
            guna2Button6.Size = new Size(160, 53);

            guna2Button3.FillColor = Color.Gray;
            guna2Button3.ForeColor = Color.White;
            guna2Button3.Size = new Size(160, 53);


            enroll_Student dsh = new enroll_Student(_id);
            dsh.TopLevel = false;
            plContent.Controls.Clear();
            plContent.Controls.Add(dsh);
            dsh.Show();

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            guna2Button6.FillColor = Color.White;
            guna2Button6.ForeColor = Color.Gray;
            guna2Button6.Size = new Size(188, 53);

            academic_btn.FillColor = Color.Gray;
            academic_btn.ForeColor = Color.White;
            academic_btn.Size = new Size(160, 53);

            guna2Button1.FillColor = Color.Gray;
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Size = new Size(160, 53);

            guna2Button3.FillColor = Color.Gray;
            guna2Button3.ForeColor = Color.White;
            guna2Button3.Size = new Size(160, 53);


            history_Frm dsh = new history_Frm(_id);
            dsh.TopLevel = false;
            plContent.Controls.Clear();
            plContent.Controls.Add(dsh);
            dsh.Show();


        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2Button3.FillColor = Color.White;
            guna2Button3.ForeColor = Color.Gray;
            guna2Button3.Size = new Size(188, 53);

            academic_btn.FillColor = Color.Gray;
            academic_btn.ForeColor = Color.White;
            academic_btn.Size = new Size(160, 53);

            guna2Button1.FillColor = Color.Gray;
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Size = new Size(160, 53);

            guna2Button6.FillColor = Color.Gray;
            guna2Button6.ForeColor = Color.White;
            guna2Button6.Size = new Size(160, 53);

           setting_Frm dsh = new setting_Frm(_id);
            dsh.TopLevel = false;
            plContent.Controls.Clear();
            plContent.Controls.Add(dsh);
            dsh.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Log Out Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

      
            if (result == DialogResult.Yes)
            {
                Form1 flog = new Form1();
                this.Close();
                flog.Show();
            }
            else
            {
    
            }
        }
    }
}
