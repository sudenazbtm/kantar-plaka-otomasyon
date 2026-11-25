namespace Kantar_Plaka_Arge_Projesi
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblIp = new System.Windows.Forms.Label();
            this.txtCameraIP = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtCameraUser = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtCameraPass = new System.Windows.Forms.TextBox();
            this.btnGiris = new System.Windows.Forms.Button();
            this.btnOffline = new System.Windows.Forms.Button();
            this.lblHata = new System.Windows.Forms.Label();
            this.lblFooter = new System.Windows.Forms.Label();
            this.pnlCard = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            this.pnlCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(32)))), ((int)(((byte)(48)))));
            this.pnlHeader.Controls.Add(this.lblSubTitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(420, 90);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(145)))), ((int)(((byte)(170)))));
            this.lblSubTitle.Location = new System.Drawing.Point(95, 52);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(222, 17);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "HIKVISION KAMERA DOĞRULAMASI";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(230)))), ((int)(((byte)(118)))));
            this.lblTitle.Location = new System.Drawing.Point(62, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(296, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "KANTAR & PLAKA OTOMASYONU";
            // 
            // lblIp
            // 
            this.lblIp.AutoSize = true;
            this.lblIp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblIp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(225)))));
            this.lblIp.Location = new System.Drawing.Point(25, 18);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(107, 15);
            this.lblIp.TabIndex = 0;
            this.lblIp.Text = "Kamera IP Adresi:";
            // 
            // txtCameraIP
            // 
            this.txtCameraIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.txtCameraIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCameraIP.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCameraIP.ForeColor = System.Drawing.Color.White;
            this.txtCameraIP.Location = new System.Drawing.Point(28, 38);
            this.txtCameraIP.Name = "txtCameraIP";
            this.txtCameraIP.Size = new System.Drawing.Size(310, 25);
            this.txtCameraIP.TabIndex = 1;
            this.txtCameraIP.Text = "192.168.1.64";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(225)))));
            this.lblUser.Location = new System.Drawing.Point(25, 75);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(126, 15);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "Kamera Kullanıcı Adı:";
            // 
            // txtCameraUser
            // 
            this.txtCameraUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.txtCameraUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCameraUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCameraUser.ForeColor = System.Drawing.Color.White;
            this.txtCameraUser.Location = new System.Drawing.Point(28, 95);
            this.txtCameraUser.Name = "txtCameraUser";
            this.txtCameraUser.Size = new System.Drawing.Size(310, 25);
            this.txtCameraUser.TabIndex = 3;
            this.txtCameraUser.Text = "admin";
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(225)))));
            this.lblPass.Location = new System.Drawing.Point(25, 132);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(103, 15);
            this.lblPass.TabIndex = 4;
            this.lblPass.Text = "Kamera Parolası:";
            // 
            // txtCameraPass
            // 
            this.txtCameraPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.txtCameraPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCameraPass.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCameraPass.ForeColor = System.Drawing.Color.White;
            this.txtCameraPass.Location = new System.Drawing.Point(28, 152);
            this.txtCameraPass.Name = "txtCameraPass";
            this.txtCameraPass.PasswordChar = '*';
            this.txtCameraPass.Size = new System.Drawing.Size(310, 25);
            this.txtCameraPass.TabIndex = 5;
            this.txtCameraPass.Text = "";
            // 
            // btnGiris
            // 
            this.btnGiris.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(83)))));
            this.btnGiris.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGiris.FlatAppearance.BorderSize = 0;
            this.btnGiris.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGiris.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnGiris.ForeColor = System.Drawing.Color.Black;
            this.btnGiris.Location = new System.Drawing.Point(28, 200);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(310, 40);
            this.btnGiris.TabIndex = 6;
            this.btnGiris.Text = "SİSTEME GİRİŞ YAP & KAMERAYA BAĞLAN";
            this.btnGiris.UseVisualStyleBackColor = false;
            this.btnGiris.Click += new System.EventHandler(this.btnGiris_Click);
            // 
            // btnOffline
            // 
            this.btnOffline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(48)))), ((int)(((byte)(70)))));
            this.btnOffline.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOffline.FlatAppearance.BorderSize = 0;
            this.btnOffline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOffline.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOffline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(185)))), ((int)(((byte)(210)))));
            this.btnOffline.Location = new System.Drawing.Point(28, 250);
            this.btnOffline.Name = "btnOffline";
            this.btnOffline.Size = new System.Drawing.Size(310, 30);
            this.btnOffline.TabIndex = 7;
            this.btnOffline.Text = "Çevrimdışı (Demo / Simülasyon) Modunda Başlat";
            this.btnOffline.UseVisualStyleBackColor = false;
            this.btnOffline.Click += new System.EventHandler(this.btnOffline_Click);
            // 
            // lblHata
            // 
            this.lblHata.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblHata.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(82)))), ((int)(((byte)(82)))));
            this.lblHata.Location = new System.Drawing.Point(25, 290);
            this.lblHata.Name = "lblHata";
            this.lblHata.Size = new System.Drawing.Size(310, 35);
            this.lblHata.TabIndex = 8;
            this.lblHata.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblFooter
            // 
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(105)))), ((int)(((byte)(125)))));
            this.lblFooter.Location = new System.Drawing.Point(0, 445);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(420, 25);
            this.lblFooter.TabIndex = 2;
            this.lblFooter.Text = "Kantar & Plaka Tanıma Sistemi © 2026";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCard
            // 
            this.pnlCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(28)))), ((int)(((byte)(42)))));
            this.pnlCard.Controls.Add(this.lblIp);
            this.pnlCard.Controls.Add(this.txtCameraIP);
            this.pnlCard.Controls.Add(this.lblUser);
            this.pnlCard.Controls.Add(this.txtCameraUser);
            this.pnlCard.Controls.Add(this.lblPass);
            this.pnlCard.Controls.Add(this.txtCameraPass);
            this.pnlCard.Controls.Add(this.btnGiris);
            this.pnlCard.Controls.Add(this.btnOffline);
            this.pnlCard.Controls.Add(this.lblHata);
            this.pnlCard.Location = new System.Drawing.Point(27, 100);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(365, 335);
            this.pnlCard.TabIndex = 1;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnGiris;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(420, 470);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kamera Doğrulama & Giriş";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.TextBox txtCameraIP;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtCameraUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtCameraPass;
        private System.Windows.Forms.Button btnGiris;
        private System.Windows.Forms.Button btnOffline;
        private System.Windows.Forms.Label lblHata;
        private System.Windows.Forms.Label lblFooter;
    }
}
