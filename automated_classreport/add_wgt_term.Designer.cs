
namespace automated_classreport
{
    partial class add_wgt_term

    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gunaElipse1 = new Guna.UI.WinForms.GunaElipse(this.components);
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.gunaLabel1 = new Guna.UI.WinForms.GunaLabel();
            this.wgt_term = new Guna.UI.WinForms.GunaTextBox();
            this.gunaLabel2 = new Guna.UI.WinForms.GunaLabel();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gunaElipse1
            // 
            this.gunaElipse1.Radius = 9;
            this.gunaElipse1.TargetControl = this;
            // 
            // guna2Button2
            // 
            this.guna2Button2.BorderRadius = 12;
            this.guna2Button2.CheckedState.Parent = this.guna2Button2;
            this.guna2Button2.CustomImages.Parent = this.guna2Button2;
            this.guna2Button2.FillColor = System.Drawing.Color.MediumSeaGreen;
            this.guna2Button2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.HoverState.Parent = this.guna2Button2;
            this.guna2Button2.Location = new System.Drawing.Point(311, 154);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.ShadowDecoration.Parent = this.guna2Button2;
            this.guna2Button2.Size = new System.Drawing.Size(65, 37);
            this.guna2Button2.TabIndex = 6;
            this.guna2Button2.Text = "Save";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Orange;
            this.guna2Panel1.Controls.Add(this.gunaLabel1);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.ShadowDecoration.Parent = this.guna2Panel1;
            this.guna2Panel1.Size = new System.Drawing.Size(395, 50);
            this.guna2Panel1.TabIndex = 7;
            // 
            // gunaLabel1
            // 
            this.gunaLabel1.AutoSize = true;
            this.gunaLabel1.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold);
            this.gunaLabel1.Location = new System.Drawing.Point(81, 9);
            this.gunaLabel1.Name = "gunaLabel1";
            this.gunaLabel1.Size = new System.Drawing.Size(218, 23);
            this.gunaLabel1.TabIndex = 8;
            this.gunaLabel1.Text = "ADD WGT ON TERM";
            // 
            // wgt_term
            // 
            this.wgt_term.BaseColor = System.Drawing.Color.White;
            this.wgt_term.BorderColor = System.Drawing.Color.Silver;
            this.wgt_term.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wgt_term.FocusedBaseColor = System.Drawing.Color.White;
            this.wgt_term.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.wgt_term.FocusedForeColor = System.Drawing.SystemColors.ControlText;
            this.wgt_term.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.wgt_term.Location = new System.Drawing.Point(131, 83);
            this.wgt_term.Name = "wgt_term";
            this.wgt_term.PasswordChar = '\0';
            this.wgt_term.SelectedText = "";
            this.wgt_term.Size = new System.Drawing.Size(245, 37);
            this.wgt_term.TabIndex = 8;
            this.wgt_term.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gunaTextBox1_KeyPress);
            // 
            // gunaLabel2
            // 
            this.gunaLabel2.AutoSize = true;
            this.gunaLabel2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.gunaLabel2.Location = new System.Drawing.Point(73, 93);
            this.gunaLabel2.Name = "gunaLabel2";
            this.gunaLabel2.Size = new System.Drawing.Size(57, 18);
            this.gunaLabel2.TabIndex = 9;
            this.gunaLabel2.Text = "WGT:";
            // 
            // add_wgt_term
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 202);
            this.Controls.Add(this.gunaLabel2);
            this.Controls.Add(this.wgt_term);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2Button2);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "add_wgt_term";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "add_Wgt";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI.WinForms.GunaElipse gunaElipse1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI.WinForms.GunaLabel gunaLabel2;
        private Guna.UI.WinForms.GunaTextBox wgt_term;
        private Guna.UI.WinForms.GunaLabel gunaLabel1;
    }
}