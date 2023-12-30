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
                    return;
                }
                else
                {
                    string userna = txtusername.Text.Trim();
                    string pass = txtpassword.Text.Trim();

                    var matchedUser = _context.user_account.Where(a => a.username == userna && a.acc_password == pass && a.brute_stat != "no").FirstOrDefault();

                     if (matchedUser != null)
                    {
                        matchedUser.brute_count = null;
                        _context.SaveChanges();
                        int Id = matchedUser.accId;
                        user_Dash user = new user_Dash(Id);
                        this.Hide();
                        user.ShowDialog();
                        //MessageBox.Show("Account is user type", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        var matchedUsers = _context.user_account.Where(a => a.username == userna && a.brute_stat != "no").FirstOrDefault();

                        if (matchedUsers != null)
                        {
                            if (matchedUsers.brute_count.ToString() == "NULL")
                            {
                                matchedUsers.brute_count = 1;
                                _context.SaveChanges();
                         
                            }
                            else {
                                int attempt = Convert.ToInt32(matchedUsers.brute_count);
                                if (attempt < 5)
                                {
                                    matchedUsers.brute_count = attempt + 1;
                                    _context.SaveChanges();

                                }
                                else {
                                    matchedUsers.brute_stat = "no";
                                    _context.SaveChanges();
                                    MessageBox.Show("You already attempted 5 times in these account, These account will be temporarily locked to open these used the forget password Thank you", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            MessageBox.Show("Wrong Password.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        else {
                            var matchedUserr = _context.user_account.Where(a => a.username == userna).FirstOrDefault();
                            if (matchedUserr == null)
                            {
                                MessageBox.Show("Username and Password is not registered.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;

                            }
                            else
                            {
                                MessageBox.Show("Youre account is temporarily locked, Please use the forget password to unlocked youre account.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                       
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
