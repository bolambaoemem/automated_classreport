using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using automated_classreport.Entities;


namespace automated_classreport
{
    public partial class setting_Frm : Form
    {
        gradingsysEntities _context = new gradingsysEntities();
        int _id;
        int i = 0;
        public setting_Frm()
        {
            InitializeComponent();
        }
        public setting_Frm(int id):this()
        {

            _id = id;
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void setting_Frm_Load(object sender, EventArgs e)
        {
                settab1();
                settab2();
        }
        public void settab1() {

            var getinfo = _context.user_account.Where(q => q.accId == _id).FirstOrDefault();
            guna2HtmlLabel16.Text = getinfo.fname.ToUpper();
            guna2HtmlLabel17.Text = getinfo.lname.ToUpper();
            guna2HtmlLabel18.Text = getinfo.email.ToString();
            guna2HtmlLabel19.Text = getinfo.username.ToString();


        }
        public void settab2() {
            var getinfo = _context.user_account.Where(q => q.accId == _id).FirstOrDefault();
            guna2TextBox8.PlaceholderText = getinfo.fname.ToUpper();
            guna2TextBox9.PlaceholderText = getinfo.lname.ToUpper();
            guna2TextBox10.PlaceholderText = getinfo.email.ToString();
            guna2TextBox11.PlaceholderText = getinfo.username.ToString();

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

            resetdt();
        }
        public void resetdt() {

            int tabPageIndex = 1;
            guna2TextBox8.Text = string.Empty;
            guna2TextBox9.Text = string.Empty;
            guna2TextBox10.Text = string.Empty;
            guna2TextBox11.Text = string.Empty;

            tabControl1.TabPages[tabPageIndex].Invalidate();


        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {


            var userToUpdate = _context.user_account.FirstOrDefault(q => q.accId == _id);

            if (userToUpdate != null)
            {
                bool changesMade = false;

                if (!string.IsNullOrWhiteSpace(guna2TextBox8.Text))
                {
                    userToUpdate.fname = guna2TextBox8.Text.Trim();
                    changesMade = true;
                }
                else if (!string.IsNullOrWhiteSpace(guna2TextBox9.Text))
                {
                    userToUpdate.lname = guna2TextBox9.Text.Trim();
                    changesMade = true;
                }
                else if (!string.IsNullOrWhiteSpace(guna2TextBox10.Text))
                {
                    userToUpdate.email = guna2TextBox10.Text.Trim();
                    changesMade = true;
                }
                else if (!string.IsNullOrWhiteSpace(guna2TextBox11.Text))
                {
                    userToUpdate.username = guna2TextBox11.Text.Trim();
                    changesMade = true;
                }

                if (changesMade)
                {
                    _context.SaveChanges();
                    MessageBox.Show("Update successful!");
                    settab1();
                    resetdt();


                }
                else
                {
                    MessageBox.Show("No changes made. Update cannot be added.");
                    resetdt();
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

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox5.Enabled = true;
            string password = guna2TextBox6.Text;
            // Check if password is at least 8 characters long
            if (password.Length <= 0)
            {
                i = 0;
                errorlabel.Visible = false;
                errorlabel.Text = string.Empty;
                errorlabel2.Text = string.Empty;
            

            }
     
           
            

        }

        private void seeeye_Click(object sender, EventArgs e)
        {
            guna2TextBox6.PasswordChar = '●';
            seeeye.SendToBack();
            closeeye.BringToFront();
        }

        private void closeeye_Click(object sender, EventArgs e)
        {
            string pass = guna2TextBox6.Text;
            if (pass.Length > 0)
            {
                guna2TextBox6.PasswordChar = '\0';
                closeeye.SendToBack();
                seeeye.BringToFront();
            }
        }

        private void copeneye_Click(object sender, EventArgs e)
        {
            guna2TextBox5.PasswordChar = '●';
            copeneye.SendToBack();
            ccloseeye.BringToFront();
        }

        private void ccloseeye_Click(object sender, EventArgs e)
        {
            string pass = guna2TextBox5.Text;
            if (pass.Length > 0)
            {
                guna2TextBox5.PasswordChar = '\0';
                ccloseeye.SendToBack();
                copeneye.BringToFront();
            }
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
            gunaLabel2.Visible = false;
            guna2TextBox7.Enabled = true;
            string password = guna2TextBox6.Text;
            string cpassword = guna2TextBox5.Text;
            bool isValidPassword = CheckPasswordCriteria(password);

            if (password.Length < 8 || !isValidPassword)
            {

                    errorlabel.Visible = true;
                    errorlabel2.Visible = true;
                    errorlabel.Text = "Password must be a minimum of 8 characters in length, and it must contain at least one uppercase letter and one special character";
                    errorlabel2.Text = "letter and one special character";
                    guna2TextBox5.Text = "";
                     guna2TextBox5.Enabled = false;



            }
            else {
                guna2TextBox5.Enabled = true;
                errorlabel.Visible = false;
                errorlabel.Text = string.Empty;
                errorlabel2.Visible = false;
                errorlabel2.Text = string.Empty;

                if (password.Length == cpassword.Length)
                {

                    if (password != cpassword)
                    {
                        gunaLabel2.Visible = true;
                        gunaLabel2.Text = "The password you entered is didn't matched!";


                    }
                    else
                    {

                        gunaLabel2.Visible = false;
                        gunaLabel2.Text = string.Empty;
                    }

                }





            }


        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {
            gunaLabel1.Visible = false;
            string password = guna2TextBox6.Text;
            string cpassword = guna2TextBox5.Text;
            if (password != cpassword)
            {
                gunaLabel2.Visible = true;
                gunaLabel2.Text = "The password you entered is didn't matched!";
                guna2TextBox7.Text = "";
                guna2TextBox7.Enabled = false;
            }
            else
            {
                guna2TextBox7.Enabled = true;
                gunaLabel2.Visible = false;
                gunaLabel2.Text = string.Empty;


            }
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            guna2TextBox7.PasswordChar = '●';
            gunaButton2.SendToBack();
            gunaButton1.BringToFront();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            string pass = guna2TextBox7.Text;
            if (pass.Length > 0)
            {
                guna2TextBox7.PasswordChar = '\0';
                gunaButton1.SendToBack();
                gunaButton2.BringToFront();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var oldpass = guna2TextBox7.Text.Trim();
            string password = guna2TextBox6.Text.Trim();

            var checkpass = _context.user_account.FirstOrDefault(q => q.accId == _id && q.acc_password == oldpass);

            if (checkpass != null)
            {

                checkpass.acc_password = password;
                _context.SaveChanges();
                MessageBox.Show("Save successful! ,The changes will be made after you log out");
                cleartxt();
            }
            else {

                gunaLabel1.Visible = true;

            }
            
        }
        public void cleartxt() {
            int tabPageIndex = 2;
            guna2TextBox6.Text = string.Empty;
            guna2TextBox5.Text = string.Empty;
            guna2TextBox7.Text = string.Empty;
            closeeye.BringToFront();
            ccloseeye.BringToFront();
            gunaButton1.BringToFront();
            errorlabel.Visible = false;
            errorlabel2.Visible = false;
            gunaLabel1.Visible = false;
            gunaLabel2.Visible = false;
            tabControl1.TabPages[tabPageIndex].Invalidate();





        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            cleartxt();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            settab2();
            settab1();
        }
    }
}
