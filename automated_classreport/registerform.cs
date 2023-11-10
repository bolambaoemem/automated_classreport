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
using System.Text.RegularExpressions;

namespace automated_classreport
{
    public partial class registerform : Form
    {
        gradingsysEntities _context = new gradingsysEntities();
        int i=0;
        public registerform()
        {
            InitializeComponent();
        }

        public void clear()
        {
            fnametxt.Text = "";
            lnametxt.Text = "";
            emailtxt.Text = "";
            usernametxt.Text = "";
            passwordtxt.Text = "";
            cpasstxt.Text = "";

        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string password = passwordtxt.Text;
                if (password.Length < 8)
                {
                    MessageBox.Show("The password must meet the minimum requirements, it should be 8 characters ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (passwordtxt.Text.Trim() != cpasstxt.Text.Trim()) {
                    MessageBox.Show("The password didn't matched", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else {
                    if (fnametxt.Text == string.Empty || lnametxt.Text == string.Empty || emailtxt.Text == string.Empty || usernametxt.Text == string.Empty || passwordtxt.Text == string.Empty) {
                        MessageBox.Show("Please fill up the field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else {

                        //add the data to database
                        user_account user = new user_account();
                        user.fname = fnametxt.Text.Trim();
                        user.lname = lnametxt.Text.Trim();
                        user.email = emailtxt.Text.Trim();
                        user.username = usernametxt.Text.Trim();
                        user.acc_password = passwordtxt.Text.Trim();

                        _context.user_account.Add(user);
                        _context.SaveChanges();

                        // Assuming this code is inside an event handler or a method
                        DialogResult result = MessageBox.Show("Successfully Registered, You can now login.\n\nDo you want to proceed to the login page?", "Success!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            
                            clear();
                            closeeye.BringToFront();
                            ccloseeye.BringToFront();
                            Form1 loginForm = new Form1();
                            loginForm.Show();
                            this.Close();
                        }
                        else
                        {
                            clear();
                            closeeye.BringToFront();
                            ccloseeye.BringToFront();
                            MessageBox.Show("You chose not to proceed to the login page.\n\nYou can perform some other action here.");
                        }

                       

                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection!", ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            clear();
        }
        private void guna2ControlBox1_Click_1(object sender, EventArgs e)
        {
            Form1 flogin = new Form1();
            this.Close();
            flogin.Show();
        }

        private void passwordtxt_TextChanged(object sender, EventArgs e)
        {
           
            string password = passwordtxt.Text;
            // Check if password is at least 8 characters long
            if (password.Length <=0 )
            {
                i = 0;
                errorlabel.Visible = false;
                errorlabel.Text = string.Empty;
                errorlabel2.Text = string.Empty;
                this.guna2HtmlLabel7.Location = new System.Drawing.Point(399, 332);
                this.cpasstxt.Location = new System.Drawing.Point(399, 352);
                this.ccloseeye.Location = new System.Drawing.Point(786, 357);
                this.copeneye.Location = new System.Drawing.Point(786, 357);

            }
            if (password.Length == 8 || password.Length > 8)
            {

                bool isValidPassword = CheckPasswordCriteria(password);

                if (!isValidPassword)
                {

                    errorlabel.Visible = true;
                    errorlabel2.Visible = true;
                    errorlabel.Text = "Password must be a minimum of 8 characters in length, and it must contain at least one uppercase letter and one special character";
                    errorlabel2.Text = "letter and one special character";
                    //passwordtxt.BorderColor = Color.Red;
                    if (i == 0)
                    {
                        i = 1;
                        guna2HtmlLabel7.Location = new Point(guna2HtmlLabel7.Location.X + 0, guna2HtmlLabel7.Location.Y + 12);
                        cpasstxt.Location = new Point(cpasstxt.Location.X + 0, cpasstxt.Location.Y + 12);
                        ccloseeye.Location = new Point(ccloseeye.Location.X + 0, ccloseeye.Location.Y + 12);
                        copeneye.Location = new Point(copeneye.Location.X + 0, copeneye.Location.Y + 12);

                    }

                }
                

                else
                {
                    //this.guna2HtmlLabel7.Location = new System.Drawing.Point(100, 100);
                    //passwordtxt.BorderColor = Color.FromArgb(94, 148, 255);
                    i = 0;
                    this.guna2HtmlLabel7.Location = new System.Drawing.Point(399, 332);
                    this.cpasstxt.Location = new System.Drawing.Point(399, 352);
                    this.ccloseeye.Location = new System.Drawing.Point(786, 357);
                    this.copeneye.Location = new System.Drawing.Point(786, 357);
                    errorlabel.Visible = false;
                    errorlabel.Text = string.Empty;
                    errorlabel2.Visible = false;
                    errorlabel2.Text = string.Empty;
                    
                }
            }
           
           
        }

        private bool CheckPasswordCriteria(string password)
        {
            // Check if password contains at least one uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return false;
            }

            // Check if password contains at least one special character
            if (!Regex.IsMatch(password, @"[!@#\$%\^&\*\(\),\.\?\\\"":\{\}\|<>]"))
            {
                return false;
            }

            return true;
        }

        private void registerform_Load(object sender, EventArgs e)
        {
            closeeye.BringToFront();
            ccloseeye.BringToFront();
        }

        private void fnametxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void lnametxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void closeeye_Click(object sender, EventArgs e)
        {
            string pass = passwordtxt.Text;
            if (pass.Length > 0)
            {
                passwordtxt.PasswordChar = '\0';
                closeeye.SendToBack();
                seeeye.BringToFront();
            }
        }

        private void seeeye_Click(object sender, EventArgs e)
        {
           
                passwordtxt.PasswordChar = '●';
                seeeye.SendToBack();
                closeeye.BringToFront();
            
        }

        private void ccloseeye_Click(object sender, EventArgs e)
        {
            string pass = cpasstxt.Text;
            if (pass.Length > 0)
            {
                cpasstxt.PasswordChar = '\0';
                ccloseeye.SendToBack();
                copeneye.BringToFront();
            }
        }

        private void copeneye_Click(object sender, EventArgs e)
        {
          
        }

        private void copeneye_Click_1(object sender, EventArgs e)
        {
            cpasstxt.PasswordChar = '●';
            copeneye.SendToBack();
            ccloseeye.BringToFront();
        }

        private void cpasstxt_TextChanged(object sender, EventArgs e)
        {
            
        }
    }


}
