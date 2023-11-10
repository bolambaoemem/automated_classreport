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
    public partial class Form1 : Form

    {

        gradingsysEntities _context = new gradingsysEntities();
        public Form1()
        {
            InitializeComponent();
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {
           registerform admin = new registerform();
            this.Hide();
            admin.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            registerform admin = new registerform();
            this.Hide();
            admin.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtusername.Text == string.Empty && txtpassword.Text == string.Empty)
                {
                    MessageBox.Show("Please fill up the field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string userna = txtusername.Text.Trim();
                    string pass = txtpassword.Text.Trim();

                    var matchedUser = _context.user_account.Where(a => a.username == userna && a.acc_password == pass).FirstOrDefault();

                     if (matchedUser != null)
                    {
                        user_Dash user = new user_Dash();
                        this.Hide();
                        user.ShowDialog();
                        //MessageBox.Show("Account is user type", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("Username and Password is not registered.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection!", ex.Message);
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
           forgotpass admin = new forgotpass();
            this.Hide();
            admin.Show();
        }
    }
}
