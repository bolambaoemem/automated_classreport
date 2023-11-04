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
                        user_account user = new user_account();
                        user.fname = fnametxt.Text.Trim();
                        user.lname = lnametxt.Text.Trim();
                        user.email = emailtxt.Text.Trim();
                        user.username = usernametxt.Text.Trim();
                        user.acc_password = passwordtxt.Text.Trim();

                        _context.user_account.Add(user);
                        _context.SaveChanges();

                        MessageBox.Show("Successfully Registered, You can now login", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        closeeye.BringToFront();
                        ccloseeye.BringToFront();

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
                errorlabel.Visible = false;
                errorlabel.Text = string.Empty; // Clear the error label if password is valid

            }
            if (password.Length == 8 || password.Length > 8)
            {

                bool isValidPassword = CheckPasswordCriteria(password);

                if (!isValidPassword)
                {
                    errorlabel.Visible = true;
                    errorlabel.Text = "Password should be 8 characters with at least one uppercase letter and one special character";
                    passwordtxt.BorderColor = Color.Red;
                }
   
                else
                {
                    errorlabel.Visible = false;
                    errorlabel.Text = string.Empty; // Clear the error label if password is valid
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
