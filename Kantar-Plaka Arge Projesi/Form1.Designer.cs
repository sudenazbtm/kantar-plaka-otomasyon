namespace Kantar_Plaka_Arge_Projesi
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btnBaglan = new System.Windows.Forms.Button();
            this.picKamera = new System.Windows.Forms.PictureBox();
            this.btnFotoCek = new System.Windows.Forms.Button();
            this.btnDosyadanOku = new System.Windows.Forms.Button();
            this.lblPlaka = new System.Windows.Forms.Label();
            this.txtPlaka = new System.Windows.Forms.TextBox();
            this.grpKantar = new System.Windows.Forms.GroupBox();
            this.chkArkaSensor = new System.Windows.Forms.CheckBox();
            this.chkOnSensor = new System.Windows.Forms.CheckBox();
            this.btnDoluKaydet = new System.Windows.Forms.Button();
            this.btnBosKaydet = new System.Windows.Forms.Button();
            this.txtAgirlik = new System.Windows.Forms.TextBox();
            this.lblAgirlik = new System.Windows.Forms.Label();
            this.grpCikis = new System.Windows.Forms.GroupBox();
            this.pnlTrafikLambasi = new System.Windows.Forms.Panel();
            this.btnCikisKontrol = new System.Windows.Forms.Button();
            this.chkFatura = new System.Windows.Forms.CheckBox();
            this.dgvKayitlar = new System.Windows.Forms.DataGridView();
            this.btnYenile = new System.Windows.Forms.Button();
            this.btnGirisKaydet = new System.Windows.Forms.Button();
            this.lblLedPanel = new System.Windows.Forms.Label();
            this.grpAyarlar = new System.Windows.Forms.GroupBox();
            this.btnPortAc = new System.Windows.Forms.Button();
            this.cmbBariyerPort = new System.Windows.Forms.ComboBox();
            this.lblBariyerCom = new System.Windows.Forms.Label();
            this.chkSeriPort = new System.Windows.Forms.CheckBox();
            this.cmbComPort = new System.Windows.Forms.ComboBox();
            this.lblCom = new System.Windows.Forms.Label();
            this.btnFisYazdir = new System.Windows.Forms.Button();
            this.grpDetaylar = new System.Windows.Forms.GroupBox();
            this.cmbMalzeme = new System.Windows.Forms.ComboBox();
            this.lblMalzeme = new System.Windows.Forms.Label();
            this.cmbFirma = new System.Windows.Forms.ComboBox();
            this.lblFirma = new System.Windows.Forms.Label();
            this.txtSofor = new System.Windows.Forms.TextBox();
            this.lblSofor = new System.Windows.Forms.Label();
            this.btnExcelAktar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picKamera)).BeginInit();
            this.grpKantar.SuspendLayout();
            this.grpCikis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKayitlar)).BeginInit();
            this.grpAyarlar.SuspendLayout();
            this.grpDetaylar.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kamera IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Kullanıcı Adı:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(415, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Şifre:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(95, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "192.168.1.64";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(300, 22);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 22);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "admin";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(460, 22);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 22);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "";
            this.textBox3.UseSystemPasswordChar = true;
            // 
            // btnBaglan
            // 
            this.btnBaglan.Location = new System.Drawing.Point(580, 18);
            this.btnBaglan.Name = "btnBaglan";
            this.btnBaglan.Size = new System.Drawing.Size(130, 30);
            this.btnBaglan.TabIndex = 6;
            this.btnBaglan.Text = "Kameraya Bağlan";
            this.btnBaglan.UseVisualStyleBackColor = true;
            this.btnBaglan.Click += new System.EventHandler(this.button1_Click);
            // 
            // picKamera
            // 
            this.picKamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picKamera.Location = new System.Drawing.Point(18, 120);
            this.picKamera.Name = "picKamera";
            this.picKamera.Size = new System.Drawing.Size(430, 240);
            this.picKamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picKamera.TabIndex = 8;
            this.picKamera.TabStop = false;
            // 
            // btnFotoCek
            // 
            this.btnFotoCek.Location = new System.Drawing.Point(18, 370);
            this.btnFotoCek.Name = "btnFotoCek";
            this.btnFotoCek.Size = new System.Drawing.Size(150, 40);
            this.btnFotoCek.TabIndex = 9;
            this.btnFotoCek.Text = "Fotoğraf Çek / Oku";
            this.btnFotoCek.UseVisualStyleBackColor = true;
            this.btnFotoCek.Click += new System.EventHandler(this.btnFotoCek_Click_1);
            // 
            // btnDosyadanOku
            // 
            this.btnDosyadanOku.Location = new System.Drawing.Point(180, 370);
            this.btnDosyadanOku.Name = "btnDosyadanOku";
            this.btnDosyadanOku.Size = new System.Drawing.Size(260, 40);
            this.btnDosyadanOku.TabIndex = 22;
            this.btnDosyadanOku.Text = "📁 Dosyadan Resim Seç ve Oku";
            this.btnDosyadanOku.UseVisualStyleBackColor = true;
            this.btnDosyadanOku.Click += new System.EventHandler(this.btnDosyadanOku_Click);
            // 
            // lblPlaka
            // 
            this.lblPlaka.AutoSize = true;
            this.lblPlaka.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPlaka.Location = new System.Drawing.Point(470, 120);
            this.lblPlaka.Name = "lblPlaka";
            this.lblPlaka.Size = new System.Drawing.Size(117, 18);
            this.lblPlaka.TabIndex = 10;
            this.lblPlaka.Text = "Okunan Plaka:";
            // 
            // txtPlaka
            // 
            this.txtPlaka.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtPlaka.Location = new System.Drawing.Point(473, 142);
            this.txtPlaka.Name = "txtPlaka";
            this.txtPlaka.Size = new System.Drawing.Size(150, 30);
            this.txtPlaka.TabIndex = 11;
            // 
            // grpKantar
            // 
            this.grpKantar.Controls.Add(this.chkArkaSensor);
            this.grpKantar.Controls.Add(this.chkOnSensor);
            this.grpKantar.Controls.Add(this.btnDoluKaydet);
            this.grpKantar.Controls.Add(this.btnBosKaydet);
            this.grpKantar.Controls.Add(this.txtAgirlik);
            this.grpKantar.Controls.Add(this.lblAgirlik);
            this.grpKantar.Location = new System.Drawing.Point(473, 305);
            this.grpKantar.Name = "grpKantar";
            this.grpKantar.Size = new System.Drawing.Size(262, 125);
            this.grpKantar.TabIndex = 12;
            this.grpKantar.TabStop = false;
            this.grpKantar.Text = "Kantar Simülasyonu";
            // 
            // chkArkaSensor
            // 
            this.chkArkaSensor.AutoSize = true;
            this.chkArkaSensor.Location = new System.Drawing.Point(125, 55);
            this.chkArkaSensor.Name = "chkArkaSensor";
            this.chkArkaSensor.Size = new System.Drawing.Size(104, 20);
            this.chkArkaSensor.TabIndex = 5;
            this.chkArkaSensor.Text = "Arka Sensör";
            this.chkArkaSensor.UseVisualStyleBackColor = true;
            this.chkArkaSensor.CheckedChanged += new System.EventHandler(this.chkSensor_CheckedChanged);
            // 
            // chkOnSensor
            // 
            this.chkOnSensor.AutoSize = true;
            this.chkOnSensor.Location = new System.Drawing.Point(15, 55);
            this.chkOnSensor.Name = "chkOnSensor";
            this.chkOnSensor.Size = new System.Drawing.Size(94, 20);
            this.chkOnSensor.TabIndex = 4;
            this.chkOnSensor.Text = "Ön Sensör";
            this.chkOnSensor.UseVisualStyleBackColor = true;
            this.chkOnSensor.CheckedChanged += new System.EventHandler(this.chkSensor_CheckedChanged);
            // 
            // btnDoluKaydet
            // 
            this.btnDoluKaydet.Location = new System.Drawing.Point(135, 82);
            this.btnDoluKaydet.Name = "btnDoluKaydet";
            this.btnDoluKaydet.Size = new System.Drawing.Size(120, 35);
            this.btnDoluKaydet.TabIndex = 3;
            this.btnDoluKaydet.Text = "Dolu (Brüt) Kaydet";
            this.btnDoluKaydet.UseVisualStyleBackColor = true;
            this.btnDoluKaydet.Click += new System.EventHandler(this.btnDoluKaydet_Click);
            // 
            // btnBosKaydet
            // 
            this.btnBosKaydet.Location = new System.Drawing.Point(9, 82);
            this.btnBosKaydet.Name = "btnBosKaydet";
            this.btnBosKaydet.Size = new System.Drawing.Size(120, 35);
            this.btnBosKaydet.TabIndex = 2;
            this.btnBosKaydet.Text = "Boş (Dara) Kaydet";
            this.btnBosKaydet.UseVisualStyleBackColor = true;
            this.btnBosKaydet.Click += new System.EventHandler(this.btnBosKaydet_Click);
            // 
            // txtAgirlik
            // 
            this.txtAgirlik.Location = new System.Drawing.Point(95, 27);
            this.txtAgirlik.Name = "txtAgirlik";
            this.txtAgirlik.Size = new System.Drawing.Size(130, 22);
            this.txtAgirlik.TabIndex = 1;
            this.txtAgirlik.Text = "15000";
            // 
            // lblAgirlik
            // 
            this.lblAgirlik.AutoSize = true;
            this.lblAgirlik.Location = new System.Drawing.Point(10, 30);
            this.lblAgirlik.Name = "lblAgirlik";
            this.lblAgirlik.Size = new System.Drawing.Size(77, 16);
            this.lblAgirlik.TabIndex = 0;
            this.lblAgirlik.Text = "Ağırlık (kg):";
            // 
            // grpCikis
            // 
            this.grpCikis.Controls.Add(this.pnlTrafikLambasi);
            this.grpCikis.Controls.Add(this.btnCikisKontrol);
            this.grpCikis.Controls.Add(this.chkFatura);
            this.grpCikis.Location = new System.Drawing.Point(473, 436);
            this.grpCikis.Name = "grpCikis";
            this.grpCikis.Size = new System.Drawing.Size(262, 100);
            this.grpCikis.TabIndex = 13;
            this.grpCikis.TabStop = false;
            this.grpCikis.Text = "Fatura & Çıkış Kontrolü";
            // 
            // pnlTrafikLambasi
            // 
            this.pnlTrafikLambasi.BackColor = System.Drawing.Color.Red;
            this.pnlTrafikLambasi.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlTrafikLambasi.Location = new System.Drawing.Point(195, 20);
            this.pnlTrafikLambasi.Name = "pnlTrafikLambasi";
            this.pnlTrafikLambasi.Size = new System.Drawing.Size(50, 22);
            this.pnlTrafikLambasi.TabIndex = 2;
            // 
            // btnCikisKontrol
            // 
            this.btnCikisKontrol.Location = new System.Drawing.Point(10, 52);
            this.btnCikisKontrol.Name = "btnCikisKontrol";
            this.btnCikisKontrol.Size = new System.Drawing.Size(245, 38);
            this.btnCikisKontrol.TabIndex = 1;
            this.btnCikisKontrol.Text = "Çıkış İzni Sorgula & Bariyer Aç";
            this.btnCikisKontrol.UseVisualStyleBackColor = true;
            this.btnCikisKontrol.Click += new System.EventHandler(this.btnCikisKontrol_Click);
            // 
            // chkFatura
            // 
            this.chkFatura.AutoSize = true;
            this.chkFatura.Location = new System.Drawing.Point(15, 22);
            this.chkFatura.Name = "chkFatura";
            this.chkFatura.Size = new System.Drawing.Size(175, 20);
            this.chkFatura.TabIndex = 0;
            this.chkFatura.Text = "Fatura / İrsaliye Onaylı";
            this.chkFatura.UseVisualStyleBackColor = true;
            this.chkFatura.CheckedChanged += new System.EventHandler(this.chkFatura_CheckedChanged);
            // 
            // dgvKayitlar
            // 
            this.dgvKayitlar.AllowUserToAddRows = false;
            this.dgvKayitlar.AllowUserToDeleteRows = false;
            this.dgvKayitlar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvKayitlar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKayitlar.Location = new System.Drawing.Point(18, 550);
            this.dgvKayitlar.Name = "dgvKayitlar";
            this.dgvKayitlar.ReadOnly = true;
            this.dgvKayitlar.RowHeadersWidth = 51;
            this.dgvKayitlar.RowTemplate.Height = 24;
            this.dgvKayitlar.Size = new System.Drawing.Size(950, 150);
            this.dgvKayitlar.TabIndex = 14;
            // 
            // btnYenile
            // 
            this.btnYenile.Location = new System.Drawing.Point(755, 490);
            this.btnYenile.Name = "btnYenile";
            this.btnYenile.Size = new System.Drawing.Size(212, 40);
            this.btnYenile.TabIndex = 15;
            this.btnYenile.Text = "Listeyi Yenile";
            this.btnYenile.UseVisualStyleBackColor = true;
            this.btnYenile.Click += new System.EventHandler(this.btnYenile_Click);
            // 
            // btnGirisKaydet
            // 
            this.btnGirisKaydet.Location = new System.Drawing.Point(630, 142);
            this.btnGirisKaydet.Name = "btnGirisKaydet";
            this.btnGirisKaydet.Size = new System.Drawing.Size(105, 30);
            this.btnGirisKaydet.TabIndex = 16;
            this.btnGirisKaydet.Text = "Giriş Yap";
            this.btnGirisKaydet.UseVisualStyleBackColor = true;
            this.btnGirisKaydet.Click += new System.EventHandler(this.btnGirisKaydet_Click);
            // 
            // lblLedPanel
            // 
            this.lblLedPanel.BackColor = System.Drawing.Color.Black;
            this.lblLedPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLedPanel.Font = new System.Drawing.Font("Courier New", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLedPanel.ForeColor = System.Drawing.Color.Lime;
            this.lblLedPanel.Location = new System.Drawing.Point(18, 65);
            this.lblLedPanel.Name = "lblLedPanel";
            this.lblLedPanel.Size = new System.Drawing.Size(950, 45);
            this.lblLedPanel.TabIndex = 17;
            this.lblLedPanel.Text = "SİSTEM ÇALIŞMAYA HAZIR / LÜTFEN ARACI KANTARA ALINIZ";
            this.lblLedPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpAyarlar
            // 
            this.grpAyarlar.Controls.Add(this.btnPortAc);
            this.grpAyarlar.Controls.Add(this.cmbBariyerPort);
            this.grpAyarlar.Controls.Add(this.lblBariyerCom);
            this.grpAyarlar.Controls.Add(this.chkSeriPort);
            this.grpAyarlar.Controls.Add(this.cmbComPort);
            this.grpAyarlar.Controls.Add(this.lblCom);
            this.grpAyarlar.Location = new System.Drawing.Point(755, 120);
            this.grpAyarlar.Name = "grpAyarlar";
            this.grpAyarlar.Size = new System.Drawing.Size(212, 190);
            this.grpAyarlar.TabIndex = 18;
            this.grpAyarlar.TabStop = false;
            this.grpAyarlar.Text = "Seri Port & Donanım";
            // 
            // btnPortAc
            // 
            this.btnPortAc.Location = new System.Drawing.Point(13, 145);
            this.btnPortAc.Name = "btnPortAc";
            this.btnPortAc.Size = new System.Drawing.Size(185, 30);
            this.btnPortAc.TabIndex = 5;
            this.btnPortAc.Text = "Portları Başlat";
            this.btnPortAc.UseVisualStyleBackColor = true;
            this.btnPortAc.Click += new System.EventHandler(this.btnPortAc_Click);
            // 
            // cmbBariyerPort
            // 
            this.cmbBariyerPort.FormattingEnabled = true;
            this.cmbBariyerPort.Location = new System.Drawing.Point(90, 110);
            this.cmbBariyerPort.Name = "cmbBariyerPort";
            this.cmbBariyerPort.Size = new System.Drawing.Size(108, 24);
            this.cmbBariyerPort.TabIndex = 4;
            // 
            // lblBariyerCom
            // 
            this.lblBariyerCom.AutoSize = true;
            this.lblBariyerCom.Location = new System.Drawing.Point(10, 113);
            this.lblBariyerCom.Name = "lblBariyerCom";
            this.lblBariyerCom.Size = new System.Drawing.Size(53, 16);
            this.lblBariyerCom.TabIndex = 3;
            this.lblBariyerCom.Text = "Bariyer:";
            // 
            // chkSeriPort
            // 
            this.chkSeriPort.AutoSize = true;
            this.chkSeriPort.Location = new System.Drawing.Point(13, 25);
            this.chkSeriPort.Name = "chkSeriPort";
            this.chkSeriPort.Size = new System.Drawing.Size(155, 20);
            this.chkSeriPort.TabIndex = 2;
            this.chkSeriPort.Text = "Seri Port Ağırlık Oku";
            this.chkSeriPort.UseVisualStyleBackColor = true;
            this.chkSeriPort.CheckedChanged += new System.EventHandler(this.chkSeriPort_CheckedChanged);
            // 
            // cmbComPort
            // 
            this.cmbComPort.FormattingEnabled = true;
            this.cmbComPort.Location = new System.Drawing.Point(90, 65);
            this.cmbComPort.Name = "cmbComPort";
            this.cmbComPort.Size = new System.Drawing.Size(108, 24);
            this.cmbComPort.TabIndex = 1;
            // 
            // lblCom
            // 
            this.lblCom.AutoSize = true;
            this.lblCom.Location = new System.Drawing.Point(10, 68);
            this.lblCom.Name = "lblCom";
            this.lblCom.Size = new System.Drawing.Size(47, 16);
            this.lblCom.TabIndex = 0;
            this.lblCom.Text = "Kantar:";
            // 
            // btnFisYazdir
            // 
            this.btnFisYazdir.Location = new System.Drawing.Point(755, 320);
            this.btnFisYazdir.Name = "btnFisYazdir";
            this.btnFisYazdir.Size = new System.Drawing.Size(212, 40);
            this.btnFisYazdir.TabIndex = 19;
            this.btnFisYazdir.Text = "Tartım Fişi Yazdır";
            this.btnFisYazdir.UseVisualStyleBackColor = true;
            this.btnFisYazdir.Click += new System.EventHandler(this.btnFisYazdir_Click);
            // 
            // grpDetaylar
            // 
            this.grpDetaylar.Controls.Add(this.cmbMalzeme);
            this.grpDetaylar.Controls.Add(this.lblMalzeme);
            this.grpDetaylar.Controls.Add(this.cmbFirma);
            this.grpDetaylar.Controls.Add(this.lblFirma);
            this.grpDetaylar.Controls.Add(this.txtSofor);
            this.grpDetaylar.Controls.Add(this.lblSofor);
            this.grpDetaylar.Location = new System.Drawing.Point(473, 180);
            this.grpDetaylar.Name = "grpDetaylar";
            this.grpDetaylar.Size = new System.Drawing.Size(262, 120);
            this.grpDetaylar.TabIndex = 20;
            this.grpDetaylar.TabStop = false;
            this.grpDetaylar.Text = "Yük Detayları (Giriş Yetkisi)";
            // 
            // cmbMalzeme
            // 
            this.cmbMalzeme.FormattingEnabled = true;
            this.cmbMalzeme.Items.AddRange(new object[] {
            "Demir",
            "Çimento",
            "Mıcır / Kum",
            "Asfalt",
            "Kömür",
            "Hazır Beton"});
            this.cmbMalzeme.Location = new System.Drawing.Point(90, 85);
            this.cmbMalzeme.Name = "cmbMalzeme";
            this.cmbMalzeme.Size = new System.Drawing.Size(155, 24);
            this.cmbMalzeme.TabIndex = 5;
            // 
            // lblMalzeme
            // 
            this.lblMalzeme.AutoSize = true;
            this.lblMalzeme.Location = new System.Drawing.Point(10, 88);
            this.lblMalzeme.Name = "lblMalzeme";
            this.lblMalzeme.Size = new System.Drawing.Size(65, 16);
            this.lblMalzeme.TabIndex = 4;
            this.lblMalzeme.Text = "Malzeme:";
            // 
            // cmbFirma
            // 
            this.cmbFirma.FormattingEnabled = true;
            this.cmbFirma.Items.AddRange(new object[] {
            "Örnek Lojistik A.Ş.",
            "Örnek İnşaat Ltd.",
            "Örnek Madencilik Sanayi",
            "Örnek Çimento A.Ş.",
            "Örnek Nakliyat",
            "Örnek Kamu / Kurum"});
            this.cmbFirma.Location = new System.Drawing.Point(90, 52);
            this.cmbFirma.Name = "cmbFirma";
            this.cmbFirma.Size = new System.Drawing.Size(155, 24);
            this.cmbFirma.TabIndex = 3;
            // 
            // lblFirma
            // 
            this.lblFirma.AutoSize = true;
            this.lblFirma.Location = new System.Drawing.Point(10, 55);
            this.lblFirma.Name = "lblFirma";
            this.lblFirma.Size = new System.Drawing.Size(44, 16);
            this.lblFirma.TabIndex = 2;
            this.lblFirma.Text = "Firma:";
            // 
            // txtSofor
            // 
            this.txtSofor.Location = new System.Drawing.Point(90, 20);
            this.txtSofor.Name = "txtSofor";
            this.txtSofor.Size = new System.Drawing.Size(155, 22);
            this.txtSofor.TabIndex = 1;
            // 
            // lblSofor
            // 
            this.lblSofor.AutoSize = true;
            this.lblSofor.Location = new System.Drawing.Point(10, 23);
            this.lblSofor.Name = "lblSofor";
            this.lblSofor.Size = new System.Drawing.Size(42, 16);
            this.lblSofor.TabIndex = 0;
            this.lblSofor.Text = "Şoför:";
            // 
            // btnExcelAktar
            // 
            this.btnExcelAktar.Location = new System.Drawing.Point(755, 440);
            this.btnExcelAktar.Name = "btnExcelAktar";
            this.btnExcelAktar.Size = new System.Drawing.Size(212, 40);
            this.btnExcelAktar.TabIndex = 21;
            this.btnExcelAktar.Text = "Excel / CSV Aktar";
            this.btnExcelAktar.UseVisualStyleBackColor = true;
            this.btnExcelAktar.Click += new System.EventHandler(this.btnExcelAktar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 713);
            this.Controls.Add(this.btnDosyadanOku);
            this.Controls.Add(this.btnExcelAktar);
            this.Controls.Add(this.grpDetaylar);
            this.Controls.Add(this.btnFisYazdir);
            this.Controls.Add(this.grpAyarlar);
            this.Controls.Add(this.lblLedPanel);
            this.Controls.Add(this.btnGirisKaydet);
            this.Controls.Add(this.btnYenile);
            this.Controls.Add(this.dgvKayitlar);
            this.Controls.Add(this.grpCikis);
            this.Controls.Add(this.grpKantar);
            this.Controls.Add(this.txtPlaka);
            this.Controls.Add(this.lblPlaka);
            this.Controls.Add(this.btnFotoCek);
            this.Controls.Add(this.picKamera);
            this.Controls.Add(this.btnBaglan);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(1000, 760);
            this.Name = "Form1";
            this.Text = "Kantar Plaka Entegreli Otomasyon Sistemi";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picKamera)).EndInit();
            this.grpKantar.ResumeLayout(false);
            this.grpKantar.PerformLayout();
            this.grpCikis.ResumeLayout(false);
            this.grpCikis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKayitlar)).EndInit();
            this.grpAyarlar.ResumeLayout(false);
            this.grpAyarlar.PerformLayout();
            this.grpDetaylar.ResumeLayout(false);
            this.grpDetaylar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btnBaglan;
        private System.Windows.Forms.PictureBox picKamera;
        private System.Windows.Forms.Button btnFotoCek;
        private System.Windows.Forms.Button btnDosyadanOku;
        private System.Windows.Forms.Label lblPlaka;
        private System.Windows.Forms.TextBox txtPlaka;
        private System.Windows.Forms.GroupBox grpKantar;
        private System.Windows.Forms.Button btnDoluKaydet;
        private System.Windows.Forms.Button btnBosKaydet;
        private System.Windows.Forms.TextBox txtAgirlik;
        private System.Windows.Forms.Label lblAgirlik;
        private System.Windows.Forms.GroupBox grpCikis;
        private System.Windows.Forms.Button btnCikisKontrol;
        private System.Windows.Forms.CheckBox chkFatura;
        private System.Windows.Forms.DataGridView dgvKayitlar;
        private System.Windows.Forms.Button btnYenile;
        private System.Windows.Forms.Button btnGirisKaydet;
        private System.Windows.Forms.Label lblLedPanel;
        private System.Windows.Forms.GroupBox grpAyarlar;
        private System.Windows.Forms.Button btnPortAc;
        private System.Windows.Forms.ComboBox cmbBariyerPort;
        private System.Windows.Forms.Label lblBariyerCom;
        private System.Windows.Forms.CheckBox chkSeriPort;
        private System.Windows.Forms.ComboBox cmbComPort;
        private System.Windows.Forms.Label lblCom;
        private System.Windows.Forms.Button btnFisYazdir;
        private System.Windows.Forms.CheckBox chkArkaSensor;
        private System.Windows.Forms.CheckBox chkOnSensor;
        private System.Windows.Forms.Panel pnlTrafikLambasi;
        private System.Windows.Forms.GroupBox grpDetaylar;
        private System.Windows.Forms.ComboBox cmbMalzeme;
        private System.Windows.Forms.Label lblMalzeme;
        private System.Windows.Forms.ComboBox cmbFirma;
        private System.Windows.Forms.Label lblFirma;
        private System.Windows.Forms.TextBox txtSofor;
        private System.Windows.Forms.Label lblSofor;
        private System.Windows.Forms.Button btnExcelAktar;
    }
}
