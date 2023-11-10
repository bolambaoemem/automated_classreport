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
    public partial class forgotpass: Form

    {

        gradingsysEntities _context = new gradingsysEntities();
        public forgotpass()
        {
            InitializeComponent();
        }

        public void clear()
        {

            txtemail.Text = "";
            txtfirstname.Text = "";
            txtlastname.Text = "";
            txtusername.Text = "";


        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtemail.Text == string.Empty || txtfirstname.Text == string.Empty || txtlastname.Text == string.Empty || txtusername.Text == string.Empty)
                {
                    MessageBox.Show("Please fill up the field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string email = txtemail.Text.Trim();
                    string fname = txtfirstname.Text.Trim();
                    string lname = txtlastname.Text.Trim();
                    string username = txtusername.Text.Trim();

                    var matchedUser = _context.user_account.Where(a => a.username == username && a.fname == fname && a.email == email && a.lname == lname).FirstOrDefault();

                     if (matchedUser != null)
                    {
                        // Assuming this code is inside an event handler or a method
                        DialogResult result = MessageBox.Show("Your password : " + matchedUser.acc_password + ". Please remember it.\n\nDo you want to proceed to the login page?", "Success!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            // User clicked "Yes", proceed to the login page
                            clear();
                           Form1 loginForm = new Form1(); 
                            loginForm.Show(); 
                            this.Close(); 
                        }
                        else
                        {
                            // User clicked "No", you can choose what to do here
                            // For example, show a message or perform some other action
                            MessageBox.Show("You chose not to proceed to the login page. You can perform some other action here.");
                        }



                    }
                    else
                    {
                        clear();
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
            Form1 back = new Form1();
            this.Close();
            back.Show();
        }
    }
}
