using PreviewDemo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tesseract;
namespace Kantar_Plaka_Arge_Projesi
{
    public partial class Form1 : Form
    {
        private Int32 m_lUserID = -1;       
        private Int32 m_lRealHandle = -1;   
        private bool m_bInitSDK = false;

        private System.IO.Ports.SerialPort serialPortKantar;
        private System.IO.Ports.SerialPort serialPortBariyer;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private string kantarBuffer = "";
        private Timer lightResetTimer;

        public Form1()
        {
            InitializeComponent();
            ApplyDarkTheme();
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);

            serialPortKantar = new System.IO.Ports.SerialPort();
            serialPortBariyer = new System.IO.Ports.SerialPort();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();

            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            printPreviewDialog1.Document = printDocument1;

            lightResetTimer = new Timer();
            lightResetTimer.Interval = 4000; 
            lightResetTimer.Tick += (s, ev) => {
                SetTrafficLight(System.Drawing.Color.Red);
                SetLedMessage("SİSTEM ÇALIŞMAYA HAZIR / LÜTFEN ARACI KANTARA ALINIZ", System.Drawing.Color.Lime);
                lightResetTimer.Stop();
            };

            try
            {
                
                m_bInitSDK = CHCNetSDK.NET_DVR_Init();
                if (m_bInitSDK == false)
                {
                    MessageBox.Show("Hikvision SDK Başlatılamadı! Cihaz hatası veya eksik dosya.");
                }
            }
            catch (DllNotFoundException dllEx)
            {
                MessageBox.Show($"[HATA] Gerekli bir SDK bileşeni bulunamadı!\n\nDetay: {dllEx.Message}", "DLL Eksik", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Genel bir hata oluştu: {ex.Message}", "Sistem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBaglan_Click(object sender, EventArgs e)
        {
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
                m_lUserID = -1;
            }

            CHCNetSDK.NET_DVR_DEVICEINFO_V30 struDeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

            string sIP = textBox1.Text.Trim();
            string sUser = textBox2.Text.Trim();
            string sPass = textBox3.Text.Trim();
            Int16 wPort = 8000;

            m_lUserID = CHCNetSDK.NET_DVR_Login_V30(sIP, wPort, sUser, sPass, ref struDeviceInfo);

            if (m_lUserID < 0)
            {
                uint iErrCode = CHCNetSDK.NET_DVR_GetLastError();
                MessageBox.Show("Kameraya bağlanılamadı! Hata Kodu: " + iErrCode);
                return;
            }

            CHCNetSDK.NET_DVR_PREVIEWINFO struPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            struPreviewInfo.hPlayWnd = picKamera.Handle;
            struPreviewInfo.lChannel = 1;
            struPreviewInfo.dwStreamType = 0;
            struPreviewInfo.dwLinkMode = 0;
            struPreviewInfo.bBlocked = true;

            m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref struPreviewInfo, null, IntPtr.Zero);

            if (m_lRealHandle < 0)
            {
                uint iErrCode = CHCNetSDK.NET_DVR_GetLastError();
                MessageBox.Show("Canlı görüntü başlatılamadı! Hata Kodu: " + iErrCode);
            }
            else
            {
                MessageBox.Show("Kameraya başarıyla bağlandınız ve canlı yayın başladı!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = LoginForm.CameraIP;
            textBox2.Text = LoginForm.CameraUser;
            textBox3.Text = LoginForm.CameraPass;

            ListeyiYenile();
            try
            {
                string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                cmbComPort.Items.Clear();
                cmbBariyerPort.Items.Clear();
                if (ports.Length > 0)
                {
                    cmbComPort.Items.AddRange(ports);
                    cmbBariyerPort.Items.AddRange(ports);
                    cmbComPort.SelectedIndex = 0;
                    if (ports.Length > 1) cmbBariyerPort.SelectedIndex = 1;
                    else cmbBariyerPort.SelectedIndex = 0;
                }
            }
            catch { }
            SetTrafficLight(System.Drawing.Color.Red);

            if (LoginForm.LoggedUserID >= 0)
            {
                m_lUserID = LoginForm.LoggedUserID;

                this.BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
                        CHCNetSDK.NET_DVR_PREVIEWINFO struPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                        struPreviewInfo.hPlayWnd = picKamera.Handle;
                        struPreviewInfo.lChannel = 1;
                        struPreviewInfo.dwStreamType = 0;
                        struPreviewInfo.dwLinkMode = 0;
                        struPreviewInfo.bBlocked = false;

                        m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref struPreviewInfo, null, IntPtr.Zero);

                        if (m_lRealHandle >= 0)
                        {
                            SetLedMessage($"KAMERAYA BAĞLANDI ({LoginForm.CameraIP}) / LÜTFEN ARACI KANTARA ALINIZ", System.Drawing.Color.Lime);
                        }
                    }
                    catch { }
                });
            }
            else if (LoginForm.IsOfflineMode)
            {
                SetLedMessage("ÇEVRİMDİŞİ (DEMO) MODU / LÜTFEN ARACI KANTARA ALINIZ", System.Drawing.Color.Yellow);
            }
        }

        private void ListeyiYenile()
        {
            try
            {
                dgvKayitlar.DataSource = null;
                dgvKayitlar.DataSource = DatabaseManager.TumKayitlariGetir();
                if (dgvKayitlar.Columns.Count > 0)
                {
                    dgvKayitlar.Columns["Plaka"].HeaderText = "Araç Plakası";
                    dgvKayitlar.Columns["GirisZamani"].HeaderText = "Giriş Zamanı";
                    dgvKayitlar.Columns["GirisResimYolu"].HeaderText = "Görsel Dosya Yolu";
                    dgvKayitlar.Columns["BosTartim"].HeaderText = "Boş Tartım (kg)";
                    dgvKayitlar.Columns["BosTartimZamani"].HeaderText = "Boş Tartım Zamanı";
                    dgvKayitlar.Columns["DoluTartim"].HeaderText = "Dolu Tartım (kg)";
                    dgvKayitlar.Columns["DoluTartimZamani"].HeaderText = "Dolu Tartım Zamanı";
                    dgvKayitlar.Columns["NetKilo"].HeaderText = "Net Malzeme (kg)";
                    dgvKayitlar.Columns["FaturaDurumu"].HeaderText = "Fatura Onaylı mı?";
                    dgvKayitlar.Columns["CikisZamani"].HeaderText = "Çıkış Zamanı";
                    dgvKayitlar.Columns["Durum"].HeaderText = "Kayıt Durumu";
                    dgvKayitlar.Columns["SoforAdi"].HeaderText = "Şoför Adı";
                    dgvKayitlar.Columns["FirmaAdi"].HeaderText = "Firma Adı";
                    dgvKayitlar.Columns["MalzemeCinsi"].HeaderText = "Malzeme Cinsi";

                    dgvKayitlar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt listesi yenilenirken hata oluştu: " + ex.Message);
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            ListeyiYenile();
        }

        private void chkSeriPort_CheckedChanged(object sender, EventArgs e)
        {
            txtAgirlik.ReadOnly = chkSeriPort.Checked;
        }

        private void chkSensor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOnSensor.Checked || chkArkaSensor.Checked)
            {
                btnBosKaydet.Enabled = false;
                btnDoluKaydet.Enabled = false;
                SetLedMessage("HATA: ARAÇ KANTAR DIŞINA TAŞMAKTADIR! HİZALAYINIZ", System.Drawing.Color.Red);
                SetTrafficLight(System.Drawing.Color.Red);
            }
            else
            {
                btnBosKaydet.Enabled = true;
                btnDoluKaydet.Enabled = true;
                SetLedMessage("ARAÇ KANTARDA / TARTIMA HAZIR", System.Drawing.Color.Lime);
            }
        }

        private void btnPortAc_Click(object sender, EventArgs e)
        {
            if (btnPortAc.Text == "Portları Başlat")
            {
                try
                {
                    if (chkSeriPort.Checked)
                    {
                        if (cmbComPort.SelectedItem == null)
                        {
                            MessageBox.Show("Lütfen önce kantar COM portunu seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        serialPortKantar.PortName = cmbComPort.SelectedItem.ToString();
                        serialPortKantar.BaudRate = 9600;
                        serialPortKantar.DataBits = 8;
                        serialPortKantar.Parity = System.IO.Ports.Parity.None;
                        serialPortKantar.StopBits = System.IO.Ports.StopBits.One;
                        serialPortKantar.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPortKantar_DataReceived);
                        serialPortKantar.Open();
                    }

                    if (cmbBariyerPort.SelectedItem != null)
                    {
                        serialPortBariyer.PortName = cmbBariyerPort.SelectedItem.ToString();
                        serialPortBariyer.BaudRate = 9600;
                        serialPortBariyer.Open();
                    }

                    btnPortAc.Text = "Portları Kapat";
                    SetLedMessage("SERİ PORT BAĞLANTILARI ETKİNLEŞTİRİLDİ", System.Drawing.Color.Lime);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Portlar açılamadı: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    if (serialPortKantar.IsOpen) serialPortKantar.Close();
                    if (serialPortBariyer.IsOpen) serialPortBariyer.Close();
                    btnPortAc.Text = "Portları Başlat";
                    SetLedMessage("PORT BAĞLANTILARI KAPATILDI", System.Drawing.Color.Yellow);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Portlar kapatılırken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void serialPortKantar_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string incoming = serialPortKantar.ReadExisting();
                kantarBuffer += incoming;

                if (kantarBuffer.Contains("\r") || kantarBuffer.Contains("\n"))
                {
                    string[] lines = kantarBuffer.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length > 0)
                    {
                        string lastLine = lines[lines.Length - 1];
                        Match m = Regex.Match(lastLine, @"\d+");
                        if (m.Success)
                        {
                            string rawVal = m.Value;
                            this.Invoke(new MethodInvoker(delegate {
                                txtAgirlik.Text = rawVal;
                            }));
                        }
                    }
                    kantarBuffer = "";
                }
            }
            catch { }
        }

        private void SetLedMessage(string msg, System.Drawing.Color color)
        {
            if (lblLedPanel.InvokeRequired)
            {
                lblLedPanel.Invoke(new MethodInvoker(delegate {
                    lblLedPanel.Text = msg;
                    lblLedPanel.ForeColor = color;
                }));
            }
            else
            {
                lblLedPanel.Text = msg;
                lblLedPanel.ForeColor = color;
            }
        }

        private void SetTrafficLight(System.Drawing.Color color)
        {
            if (pnlTrafikLambasi.InvokeRequired)
            {
                pnlTrafikLambasi.Invoke(new MethodInvoker(delegate {
                    pnlTrafikLambasi.BackColor = color;
                }));
            }
            else
            {
                pnlTrafikLambasi.BackColor = color;
            }
        }

        private void btnGirisKaydet_Click(object sender, EventArgs e)
        {
            string plaka = txtPlaka.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(plaka))
            {
                MessageBox.Show("Lütfen önce bir plaka girin veya kameradan okutun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string sofor = txtSofor.Text.Trim();
            string firma = cmbFirma.SelectedItem?.ToString() ?? cmbFirma.Text.Trim();
            string malzeme = cmbMalzeme.SelectedItem?.ToString() ?? cmbMalzeme.Text.Trim();

            DatabaseManager.YeniGirisKaydet(plaka, "", sofor, firma, malzeme);
            ListeyiYenile();
            SetLedMessage($"GİRİŞ KAYDI YAPILDI: {plaka}", System.Drawing.Color.Lime);
            MessageBox.Show($"'{plaka}' plakalı araç için Giriş Kaydı oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBosKaydet_Click(object sender, EventArgs e)
        {
            string plaka = txtPlaka.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(plaka))
            {
                MessageBox.Show("Plaka alanı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int kilo;
            if (!int.TryParse(txtAgirlik.Text, out kilo))
            {
                MessageBox.Show("Lütfen geçerli bir ağırlık (kg) değeri girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var aktif = DatabaseManager.AktifKayitBul(plaka);
            if (aktif == null)
            {
                MessageBox.Show("Bu araç için önce giriş kaydı yapılmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DatabaseManager.BosTartimKaydet(plaka, kilo);
            ListeyiYenile();
            SetLedMessage($"BOŞ TARTIM YAPILDI: {kilo} KG", System.Drawing.Color.Lime);
            MessageBox.Show($"'{plaka}' için Boş Tartım ({kilo} kg) kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDoluKaydet_Click(object sender, EventArgs e)
        {
            string plaka = txtPlaka.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(plaka))
            {
                MessageBox.Show("Plaka alanı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int kilo;
            if (!int.TryParse(txtAgirlik.Text, out kilo))
            {
                MessageBox.Show("Lütfen geçerli bir ağırlık (kg) değeri girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var aktif = DatabaseManager.AktifKayitBul(plaka);
            if (aktif == null)
            {
                MessageBox.Show("Bu araç için önce giriş kaydı yapılmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!aktif.BosTartim.HasValue)
            {
                MessageBox.Show("Dolu tartımdan önce aracın Boş Tartımı (Dara) yapılmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DatabaseManager.DoluTartimKaydet(plaka, kilo);
            ListeyiYenile();
            int net = Math.Abs(kilo - aktif.BosTartim.Value);
            SetLedMessage($"DOLU TARTIM YAPILDI: {kilo} KG / NET: {net} KG", System.Drawing.Color.Cyan);
            MessageBox.Show($"'{plaka}' için Dolu Tartım ({kilo} kg) kaydedildi.\nNet Kilo: {net} kg", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chkFatura_CheckedChanged(object sender, EventArgs e)
        {
            string plaka = txtPlaka.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(plaka)) return;
            DatabaseManager.FaturaDurumuGuncelle(plaka, chkFatura.Checked);
            ListeyiYenile();
        }

        private void btnCikisKontrol_Click(object sender, EventArgs e)
        {
            string plaka = txtPlaka.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(plaka))
            {
                MessageBox.Show("Plaka alanı boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string hata;
            if (DatabaseManager.CikisYap(plaka, out hata))
            {
                ListeyiYenile();
                SetLedMessage("ÇIKIŞ ONAYLANDI / BARİYER AÇILIYOR", System.Drawing.Color.Lime);
                SetTrafficLight(System.Drawing.Color.Lime);
                lightResetTimer.Start();

                if (serialPortBariyer.IsOpen)
                {
                    try
                    {
                        serialPortBariyer.WriteLine("OPEN");
                    }
                    catch { }
                }

                MessageBox.Show($"[BARİYER AÇILDI] '{plaka}' plakalı aracın çıkış izinleri eksiksiz onaylandı. Bariyer açılıyor!", "Geçiş Onaylandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SetLedMessage("GEÇİŞ ENGELLENDİ: " + hata.ToUpper(), System.Drawing.Color.Red);
                MessageBox.Show($"[GEÇİŞ ENGELLENDİ]\n\nHata: {hata}", "Çıkış İzni Reddedildi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFisYazdir_Click(object sender, EventArgs e)
        {
            if (dgvKayitlar.SelectedRows.Count == 0 && dgvKayitlar.CurrentRow == null)
            {
                MessageBox.Show("Lütfen çıktısını almak istediğiniz kantar kaydını aşağıdaki tablodan seçin!", "Seçim Eksik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                printPreviewDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yazıcı önizleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcelAktar_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV Dosyası (*.csv)|*.csv";
                sfd.FileName = $"Kantar_Tartim_Raporu_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine("Plaka;Giris Zamani;Bos Tartim (kg);Bos Tartim Zamani;Dolu Tartim (kg);Dolu Tartim Zamani;Net Malzeme (kg);Fatura Durumu;Cikis Zamani;Sofor Adi;Firma Adi;Malzeme Cinsi;Durum");

                        var list = DatabaseManager.TumKayitlariGetir();
                        foreach (var item in list)
                        {
                            sw.WriteLine($"{item.Plaka};" +
                                         $"{item.GirisZamani:yyyy-MM-dd HH:mm:ss};" +
                                         $"{(item.BosTartim.HasValue ? item.BosTartim.ToString() : "")};" +
                                         $"{(item.BosTartimZamani.HasValue ? item.BosTartimZamani.Value.ToString("yyyy-MM-dd HH:mm:ss") : "")};" +
                                         $"{(item.DoluTartim.HasValue ? item.DoluTartim.ToString() : "")};" +
                                         $"{(item.DoluTartimZamani.HasValue ? item.DoluTartimZamani.Value.ToString("yyyy-MM-dd HH:mm:ss") : "")};" +
                                         $"{(item.NetKilo.HasValue ? item.NetKilo.ToString() : "")};" +
                                         $"{item.FaturaDurumu};" +
                                         $"{(item.CikisZamani.HasValue ? item.CikisZamani.Value.ToString("yyyy-MM-dd HH:mm:ss") : "")};" +
                                         $"{item.SoforAdi};" +
                                         $"{item.FirmaAdi};" +
                                         $"{item.MalzemeCinsi};" +
                                         $"{item.Durum}");
                        }
                    }
                    MessageBox.Show("Tartım geçmişi başarıyla Excel/CSV formatında dışa aktarıldı!", "Rapor Hazır", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Rapor ihraç edilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                var row = dgvKayitlar.SelectedRows.Count > 0 ? dgvKayitlar.SelectedRows[0] : dgvKayitlar.CurrentRow;
                if (row == null) return;

                string plaka = row.Cells["Plaka"].Value?.ToString();
                string giris = row.Cells["GirisZamani"].Value?.ToString();
                string cikis = row.Cells["CikisZamani"].Value?.ToString() ?? "---";
                string bos = row.Cells["BosTartim"].Value?.ToString() ?? "0";
                string dolu = row.Cells["DoluTartim"].Value?.ToString() ?? "0";
                string net = row.Cells["NetKilo"].Value?.ToString() ?? "0";
                string durum = row.Cells["Durum"].Value?.ToString();
                string sofor = row.Cells["SoforAdi"].Value?.ToString() ?? "---";
                string firma = row.Cells["FirmaAdi"].Value?.ToString() ?? "---";
                string malzeme = row.Cells["MalzemeCinsi"].Value?.ToString() ?? "---";

                var fontBaslik = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
                var fontGovde = new System.Drawing.Font("Arial", 11, System.Drawing.FontStyle.Regular);
                var fontKalin = new System.Drawing.Font("Arial", 11, System.Drawing.FontStyle.Bold);
                var firca = System.Drawing.Brushes.Black;

                float x = 50;
                float y = 50;
                float satirYuksekligi = 25;

                e.Graphics.DrawString("*** ENDÜSTRİYEL KANTAR TARTIM FİŞİ ***", fontBaslik, firca, x, y);
                y += 40;

                e.Graphics.DrawLine(System.Drawing.Pens.Black, x, y, x + 500, y);
                y += 15;

                e.Graphics.DrawString($"Araç Plakası : {plaka}", fontKalin, firca, x, y);
                y += satirYuksekligi;
                e.Graphics.DrawString($"Şoför Adı    : {sofor}", fontGovde, firca, x, y);
                y += satirYuksekligi;
                e.Graphics.DrawString($"Firma Adı    : {firma}", fontGovde, firca, x, y);
                y += satirYuksekligi;
                e.Graphics.DrawString($"Malzeme Cinsi: {malzeme}", fontGovde, firca, x, y);
                y += satirYuksekligi;
                e.Graphics.DrawString($"Giriş Zamanı : {giris}", fontGovde, firca, x, y);
                y += satirYuksekligi;
                e.Graphics.DrawString($"Çıkış Zamanı : {cikis}", fontGovde, firca, x, y);
                y += satirYuksekligi;
                e.Graphics.DrawString($"Kayıt Durumu : {durum}", fontGovde, firca, x, y);
                y += 20;

                e.Graphics.DrawLine(System.Drawing.Pens.Black, x, y, x + 500, y);
                y += 15;

                e.Graphics.DrawString($"Boş Ağırlık (Dara) : {bos} kg", fontGovde, firca, x, y);
                y += satirYuksekligi;
                e.Graphics.DrawString($"Dolu Ağırlık (Brüt): {dolu} kg", fontGovde, firca, x, y);
                y += satirYuksekligi;
                e.Graphics.DrawString($"Net Malzeme Kilosu : {net} kg", fontKalin, firca, x, y);
                y += 25;

                e.Graphics.DrawLine(System.Drawing.Pens.Black, x, y, x + 500, y);
                y += 20;

                e.Graphics.DrawString("||| | |||| | ||| || ||| ||||", new System.Drawing.Font("Courier New", 20, System.Drawing.FontStyle.Bold), firca, x + 100, y);
                y += 30;
                e.Graphics.DrawString($"KTR-{plaka}-{DateTime.Now:yyyyMMdd}", fontGovde, firca, x + 150, y);
            }
            catch (Exception ex)
            {
                e.Graphics.DrawString("Fiş tasarlanamadı: " + ex.Message, new System.Drawing.Font("Arial", 12), System.Drawing.Brushes.Red, 50, 50);
            }
        }

        private void button1_Click(object sender, EventArgs e) { btnBaglan_Click(sender, e); }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (serialPortKantar.IsOpen) serialPortKantar.Close();
                if (serialPortBariyer.IsOpen) serialPortBariyer.Close();
            }
            catch { }

            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
            }
            if (m_bInitSDK)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
        }

        private void btnFotoCek_Click(object sender, EventArgs e)
        {
            if (m_lUserID < 0 || m_lRealHandle < 0)
            {
                MessageBox.Show("Önce kameraya başarılı bir şekilde bağlanmalısınız!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string klasorYolu = System.IO.Path.Combine(Application.StartupPath, "plaka_resimleri");
                if (!System.IO.Directory.Exists(klasorYolu))
                {
                    System.IO.Directory.CreateDirectory(klasorYolu);
                }

                string dosyaAdi = $"plaka_{DateTime.Now:yyyyMMdd_HHmmss}.bmp";
                string tamDosyaYolu = System.IO.Path.Combine(klasorYolu, dosyaAdi);

                bool bCapture = CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle, tamDosyaYolu);

                if (bCapture)
                {
                    string bulunanPlaka = PlakaOkuOCR(tamDosyaYolu);
                    txtPlaka.Text = bulunanPlaka;

                    if (bulunanPlaka.Length >= 5 && !bulunanPlaka.Contains(" ") && !bulunanPlaka.Contains(":") && !bulunanPlaka.Contains("Algılanamadı"))
                    {
                        var aktif = DatabaseManager.AktifKayitBul(bulunanPlaka);
                        if (aktif == null)
                        {
                            string sofor = txtSofor.Text.Trim();
                            string firma = cmbFirma.SelectedItem?.ToString() ?? cmbFirma.Text.Trim();
                            string malzeme = cmbMalzeme.SelectedItem?.ToString() ?? cmbMalzeme.Text.Trim();

                            var sorgu = DatabaseManager.PlakaSorgula(bulunanPlaka);
                            if (sorgu.Durum == "WHITELIST" || sorgu.Durum == "GUEST_VALID")
                            {
                                DatabaseManager.YeniGirisKaydet(bulunanPlaka, tamDosyaYolu, sofor, firma, malzeme);
                                SetLedMessage($"GEÇİŞ ONAYLANDI: {bulunanPlaka}", System.Drawing.Color.Lime);
                                SetTrafficLight(System.Drawing.Color.Lime);
                                lightResetTimer.Start();
                                MessageBox.Show($"{sorgu.Mesaj} Girişi Algılandı ve Kapı Açılıyor!\n\nPlaka: {bulunanPlaka}", "Giriş Yetkisi Onaylandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (sorgu.Durum == "BLACKLIST")
                            {
                                SetLedMessage($"YASAKLI ARAÇ: {bulunanPlaka}", System.Drawing.Color.Red);
                                SetTrafficLight(System.Drawing.Color.Red);
                                MessageBox.Show($"[GEÇİŞ ENGELLENDİ] '{bulunanPlaka}' plakalı araç KARA LİSTEDE kayıtlıdır. Bariyer açılmayacaktır!", "Giriş Reddedildi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string ekBilgi = sorgu.Durum == "GUEST_EXPIRED" ? " (Süresi Dolmuş İzin)" : " (Tanımsız Ziyaretçi)";
                                SetLedMessage($"ONAY BEKLENİYOR: {bulunanPlaka}{ekBilgi}", System.Drawing.Color.Yellow);
                                SetTrafficLight(System.Drawing.Color.Red);

                                DialogResult dr = MessageBox.Show($"Okunan plaka ('{bulunanPlaka}'){ekBilgi} otomatik giriş yetkisine sahip değil.\n\nGiriş onaylansın ve kapı açılsın mı?", "Operatör Geçiş Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dr == DialogResult.Yes)
                                {
                                    DatabaseManager.YeniGirisKaydet(bulunanPlaka, tamDosyaYolu, sofor, firma, malzeme);
                                    SetLedMessage($"GİRİŞ ONAYLANDI / BARİYER AÇILIYOR", System.Drawing.Color.Lime);
                                    SetTrafficLight(System.Drawing.Color.Lime);
                                    lightResetTimer.Start();
                                }
                                else
                                {
                                    SetLedMessage($"ARAÇ GİRİŞİ REDDEDİLDİ", System.Drawing.Color.Red);
                                }
                            }
                        }
                        else
                        {
                            chkFatura.Checked = aktif.FaturaDurumu;
                            SetLedMessage($"ARAÇ İÇERİDE: {bulunanPlaka}", System.Drawing.Color.Yellow);
                            MessageBox.Show($"Araç Zaten İçeride!\n\nPlaka: {bulunanPlaka}\nGiriş Zamanı: {aktif.GirisZamani}\n\nBoş/Dolu tartımlarını yapıp fatura durumunu güncelledikten sonra Çıkış İzni Sorgula butonuna basabilirsiniz.", "Sistem Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        ListeyiYenile();
                    }
                    else
                    {
                        MessageBox.Show($"Okunan Metin: {bulunanPlaka}\n(Plaka formatına uymadığı için veritabanına otomatik giriş kaydı oluşturulmadı. Gerekirse elle plaka kutusuna yazıp 'Giriş Yap' butonuna basabilirsiniz.)", "Plaka Tanıma Sonucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    uint iErrCode = CHCNetSDK.NET_DVR_GetLastError();
                    MessageBox.Show($"Fotoğraf çekilemedi! SDK Hata Kodu: {iErrCode}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fotoğraf çekme işleminde sistemsel hata: {ex.Message}", "Sistem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFotoCek_Click_1(object sender, EventArgs e)
        {
            btnFotoCek_Click(sender, e);
        }

        private void btnDosyadanOku_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Görsel Dosyaları (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Test İçin Plaka Fotoğrafı Seçin";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string tamDosyaYolu = ofd.FileName;
                    picKamera.ImageLocation = tamDosyaYolu;

                    string bulunanPlaka = PlakaOkuOCR(tamDosyaYolu);
                    txtPlaka.Text = bulunanPlaka;

                    if (bulunanPlaka.Length >= 5 && !bulunanPlaka.Contains(" ") && !bulunanPlaka.Contains(":") && !bulunanPlaka.Contains("Algılanamadı"))
                    {
                        var aktif = DatabaseManager.AktifKayitBul(bulunanPlaka);
                        if (aktif == null)
                        {
                            string sofor = txtSofor.Text.Trim();
                            string firma = cmbFirma.SelectedItem?.ToString() ?? cmbFirma.Text.Trim();
                            string malzeme = cmbMalzeme.SelectedItem?.ToString() ?? cmbMalzeme.Text.Trim();

                            var sorgu = DatabaseManager.PlakaSorgula(bulunanPlaka);
                            if (sorgu.Durum == "WHITELIST" || sorgu.Durum == "GUEST_VALID")
                            {
                                DatabaseManager.YeniGirisKaydet(bulunanPlaka, tamDosyaYolu, sofor, firma, malzeme);
                                SetLedMessage($"GEÇİŞ ONAYLANDI: {bulunanPlaka}", System.Drawing.Color.Lime);
                                SetTrafficLight(System.Drawing.Color.Lime);
                                lightResetTimer.Start();
                                MessageBox.Show($"{sorgu.Mesaj} Girişi Algılandı ve Kapı Açılıyor!\n\nPlaka: {bulunanPlaka}", "Giriş Yetkisi Onaylandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (sorgu.Durum == "BLACKLIST")
                            {
                                SetLedMessage($"YASAKLI ARAÇ: {bulunanPlaka}", System.Drawing.Color.Red);
                                SetTrafficLight(System.Drawing.Color.Red);
                                MessageBox.Show($"[GEÇİŞ ENGELLENDİ] '{bulunanPlaka}' plakalı araç KARA LİSTEDE kayıtlıdır. Bariyer açılmayacaktır!", "Giriş Reddedildi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string ekBilgi = sorgu.Durum == "GUEST_EXPIRED" ? " (Süresi Dolmuş İzin)" : " (Tanımsız Ziyaretçi)";
                                SetLedMessage($"ONAY BEKLENİYOR: {bulunanPlaka}{ekBilgi}", System.Drawing.Color.Yellow);
                                SetTrafficLight(System.Drawing.Color.Red);

                                DialogResult dr = MessageBox.Show($"Okunan plaka ('{bulunanPlaka}'){ekBilgi} otomatik giriş yetkisine sahip değil.\n\nGiriş onaylansın ve kapı açılsın mı?", "Operatör Geçiş Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dr == DialogResult.Yes)
                                {
                                    DatabaseManager.YeniGirisKaydet(bulunanPlaka, tamDosyaYolu, sofor, firma, malzeme);
                                    SetLedMessage($"GİRİŞ ONAYLANDI / BARİYER AÇILIYOR", System.Drawing.Color.Lime);
                                    SetTrafficLight(System.Drawing.Color.Lime);
                                    lightResetTimer.Start();
                                }
                                else
                                {
                                    SetLedMessage($"ARAÇ GİRİŞİ REDDEDİLDİ", System.Drawing.Color.Red);
                                }
                            }
                        }
                        else
                        {
                            chkFatura.Checked = aktif.FaturaDurumu;
                            SetLedMessage($"ARAÇ İÇERİDE: {bulunanPlaka}", System.Drawing.Color.Yellow);
                            MessageBox.Show($"Araç Zaten İçeride!\n\nPlaka: {bulunanPlaka}\nGiriş Zamanı: {aktif.GirisZamani}\n\nBoş/Dolu tartımlarını yapıp fatura durumunu güncelledikten sonra Çıkış İzni Sorgula butonuna basabilirsiniz.", "Sistem Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        ListeyiYenile();
                    }
                    else
                    {
                        MessageBox.Show($"Okunan Metin: {bulunanPlaka}\n(Plaka formatına uymadığı için veritabanına otomatik giriş kaydı oluşturulmadı. Gerekirse elle plaka kutusuna yazıp 'Giriş Yap' butonuna basabilirsiniz.)", "Plaka Tanıma Sonucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fotoğraf seçme işleminde hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static readonly Regex PlakaDeseni = new Regex(@"\d{2}[A-Z]{1,5}\d{2,4}");

        private struct PlakaAday
        {
            public System.Drawing.Rectangle Bolge;
            public double AciDerece;
        }

        public static string PlakaOkuOCR(string resimYolu)
        {
            try
            {
                string apiPlaka = PlateRecognizerApi.OkuPlakaApi(resimYolu);
                if (!string.IsNullOrEmpty(apiPlaka) && apiPlaka.Length >= 5)
                {
                    string duzeltilmisApi = PlakaMetniniDuzelt(apiPlaka);
                    Match matchApi = PlakaDeseni.Match(duzeltilmisApi);
                    if (matchApi.Success)
                    {
                        return matchApi.Value;
                    }
                    return apiPlaka;
                }

                string kendiModelPlaka = CustomAiModel.OkuPlakaKendiModelimiz(resimYolu);
                if (!string.IsNullOrEmpty(kendiModelPlaka) && kendiModelPlaka.Length >= 5)
                {
                    string duzeltilmisKendi = PlakaMetniniDuzelt(kendiModelPlaka);
                    Match matchKendi = PlakaDeseni.Match(duzeltilmisKendi);
                    if (matchKendi.Success)
                    {
                        return matchKendi.Value;
                    }
                    return kendiModelPlaka;
                }

                string veriKlasoru = System.IO.Path.Combine(Application.StartupPath, "tessdata");
                string debugKlasoru = System.IO.Path.Combine(Application.StartupPath, "plaka_debug");
                if (!System.IO.Directory.Exists(debugKlasoru))
                {
                    System.IO.Directory.CreateDirectory(debugKlasoru);
                }

                System.Drawing.Bitmap orjinalResim = null;
                int retries = 10;
                while (retries > 0)
                {
                    try
                    {
                        orjinalResim = new System.Drawing.Bitmap(resimYolu);
                        break;
                    }
                    catch (ArgumentException)
                    {
                        retries--;
                        if (retries == 0) throw;
                        System.Threading.Thread.Sleep(100);
                    }
                    catch (System.Runtime.InteropServices.ExternalException)
                    {
                        retries--;
                        if (retries == 0) throw;
                        System.Threading.Thread.Sleep(100);
                    }
                }

                using (orjinalResim)
                {
                    int genislik, yukseklik;

                    byte[] gri = GriDiziyeCevir(orjinalResim, out genislik, out yukseklik);

                    int otsu = OtsuEsikHesapla(gri);
                    var tumAdaylar = new List<PlakaAday>();
                    foreach (int esik in new int[] { otsu, otsu + 40, 200 })
                    {
                        if (esik > 250) continue;

                        foreach (var aday in KarakterGrubuAdaylari(gri, genislik, yukseklik, esik))
                        {
                            tumAdaylar.Add(aday);
                        }
                    }

                    tumAdaylar.Sort((a, b) => b.Bolge.Width.CompareTo(a.Bolge.Width));

                    var adaylar = new List<PlakaAday>();
                    foreach (var aday in tumAdaylar)
                    {
                        bool tekrar = false;
                        foreach (var mevcut in adaylar)
                        {
                            var kesisim = System.Drawing.Rectangle.Intersect(aday.Bolge, mevcut.Bolge);
                            if (!kesisim.IsEmpty && kesisim.Width * kesisim.Height * 2 > aday.Bolge.Width * aday.Bolge.Height)
                            {
                                tekrar = true;
                                break;
                            }
                        }
                        if (!tekrar) adaylar.Add(aday);
                    }

                    if (adaylar.Count == 0)
                    {
                        adaylar.Add(new PlakaAday { Bolge = new System.Drawing.Rectangle(0, 0, genislik, yukseklik), AciDerece = 0 });
                        adaylar.Add(new PlakaAday { Bolge = new System.Drawing.Rectangle(genislik / 6, yukseklik / 4, genislik * 2 / 3, yukseklik / 2), AciDerece = 0 });
                    }

                    string ilkOkunan = "";
                    using (var motor = new TesseractEngine(veriKlasoru, "tur", EngineMode.Default))
                    {
                        motor.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                        motor.SetVariable("load_system_dawg", "0");
                        motor.SetVariable("load_freq_dawg", "0");
                        motor.SetVariable("load_punc_dawg", "0");
                        motor.SetVariable("load_number_dawg", "0");
                        motor.SetVariable("load_unambig_dawg", "0");
                        motor.SetVariable("load_bigram_dawg", "0");
                        motor.SetVariable("load_fixed_length_dawgs", "0");

                        var eslesenPlakalar = new List<Tuple<string, int>>();

                        for (int i = 0; i < adaylar.Count && i < 6; i++)
                        {
                            foreach (int targetHeight in new int[] { 70, 100, 120 })
                            {
                                string kesitDosyasi = System.IO.Path.Combine(debugKlasoru, $"aday_{i + 1}_{targetHeight}.png");
                                using (var kesit = PlakaKesitiHazirla(orjinalResim, adaylar[i].Bolge, adaylar[i].AciDerece, targetHeight))
                                {
                                    kesit.Save(kesitDosyasi, System.Drawing.Imaging.ImageFormat.Png);
                                }

                                using (var pixResim = Pix.LoadFromFile(kesitDosyasi))
                                using (var sayfa = motor.Process(pixResim, PageSegMode.SingleLine))
                                {
                                    string hamMetin = Regex.Replace(sayfa.GetText(), @"[^A-Z0-9]", "").Trim().ToUpper();
                                    string duzeltilmis = PlakaMetniniDuzelt(hamMetin);
                                    Match plakaMaci = PlakaDeseni.Match(duzeltilmis);

                                    if (ilkOkunan.Length == 0 && hamMetin.Length > 0)
                                    {
                                        ilkOkunan = hamMetin;
                                    }

                                    if (plakaMaci.Success)
                                    {
                                        string bulgu = plakaMaci.Value;
                                        int skor = PlakaSkorunuHesapla(bulgu);
                                        eslesenPlakalar.Add(new Tuple<string, int>(bulgu, skor));
                                    }
                                }
                            }
                        }

                        if (eslesenPlakalar.Count > 0)
                        {
                            
                            var sirali = System.Linq.Enumerable.ToList(
                                System.Linq.Enumerable.OrderByDescending(eslesenPlakalar, x => x.Item2)
                            );
                            return sirali[0].Item1;
                        }
                    }

                    return ilkOkunan.Length > 3 ? $"Format dışı okuma: {PlakaMetniniDuzelt(ilkOkunan)}" : "Plaka Algılanamadı";
                }
            }
            catch (Exception ex)
            {
                return $"[Plaka Okuma Hatası: {ex.Message}\nStack: {ex.StackTrace}]";
            }
        }

        private static int PlakaSkorunuHesapla(string plaka)
        {
            if (string.IsNullOrEmpty(plaka)) return 0;

            Match m = PlakaDeseni.Match(plaka);
            if (!m.Success) return 0;

            string val = m.Value;
            string ilKodu = val.Substring(0, 2);

            int harfSayisi = 0;
            for (int i = 2; i < val.Length; i++)
            {
                if (char.IsLetter(val[i])) harfSayisi++;
                else break;
            }

            int skor = 0;

            if (harfSayisi >= 1 && harfSayisi <= 3)
            {
                skor += 20;
            }
            else
            {
                skor += 5; 
            }

            int ilNo;
            if (int.TryParse(ilKodu, out ilNo))
            {
                if (ilNo >= 1 && ilNo <= 81)
                {
                    skor += 15;
                }
                else
                {
                    skor -= 20; 
                }
            }

            if (ilKodu == "34")
            {
                skor += 10;
            }

            if (val.Length == 7 || val.Length == 8)
            {
                skor += 5;
            }

            return skor;
        }

        private static string PlakaMetniniDuzelt(string hamMetin)
        {
            if (string.IsNullOrEmpty(hamMetin))
                return hamMetin;

            if (hamMetin.Length < 5)
                return hamMetin;

            char[] arr = hamMetin.ToCharArray();

            for (int i = 0; i < Math.Min(2, arr.Length); i++)
            {
                arr[i] = KoyuKarakterSayıYap(arr[i]);
            }

            int sayiGrupUzunlugu = 0;
            for (int i = arr.Length - 1; i >= 2; i--)
            {
                char c = arr[i];
                if (char.IsDigit(c) || KarakterSayıyaDonusurMu(c))
                {
                    arr[i] = KoyuKarakterSayıYap(c);
                    sayiGrupUzunlugu++;
                    if (sayiGrupUzunlugu >= 4)
                        break;
                }
                else
                {
                    break;
                }
            }

            int ortaBitis = arr.Length - sayiGrupUzunlugu;
            for (int i = 2; i < ortaBitis; i++)
            {
                arr[i] = KoyuKarakterHarfYap(arr[i]);
            }

            return new string(arr);
        }

        private static bool KarakterSayıyaDonusurMu(char c)
        {
            return c == 'O' || c == 'I' || c == 'L' || c == 'T' || c == 'J' || c == 'Z' || 
                   c == 'S' || c == 'B' || c == 'G' || c == 'E' || c == 'A' || c == 'D' || 
                   c == 'U' || c == 'M' || c == 'H' || c == 'R';
        }

        private static char KoyuKarakterSayıYap(char c)
        {
            switch (c)
            {
                case 'O': case 'D': case 'U': case 'M': return '0';
                case 'I': case 'L': case 'T': case 'J': return '1';
                case 'Z': case 'R': return '2';
                case 'S': return '5';
                case 'B': case 'E': case 'H': return '8';
                case 'G': return '6';
                case 'A': return '4';
                default: return c;
            }
        }

        private static char KoyuKarakterHarfYap(char c)
        {
            switch (c)
            {
                case '0': return 'O';
                case '1': return 'I';
                case '2': return 'Z';
                case '5': return 'S';
                case '8': return 'B';
                default: return c;
            }
        }

        private static byte[] GriDiziyeCevir(System.Drawing.Bitmap kaynak, out int genislik, out int yukseklik)
        {
            genislik = kaynak.Width;
            yukseklik = kaynak.Height;
            byte[] gri = new byte[genislik * yukseklik];

            using (var resim24 = new System.Drawing.Bitmap(genislik, yukseklik, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                using (var g = System.Drawing.Graphics.FromImage(resim24))
                {
                    g.DrawImage(kaynak, 0, 0, genislik, yukseklik);
                }

                var veri = resim24.LockBits(
                    new System.Drawing.Rectangle(0, 0, genislik, yukseklik),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                try
                {
                    byte[] ham = new byte[veri.Stride * yukseklik];
                    Marshal.Copy(veri.Scan0, ham, 0, ham.Length);

                    for (int y = 0; y < yukseklik; y++)
                    {
                        int satirBasi = y * veri.Stride;
                        for (int x = 0; x < genislik; x++)
                        {
                            int p = satirBasi + x * 3; 
                            gri[y * genislik + x] = (byte)((ham[p] * 11 + ham[p + 1] * 59 + ham[p + 2] * 30) / 100);
                        }
                    }
                }
                finally
                {
                    resim24.UnlockBits(veri);
                }
            }
            return gri;
        }

        private static int OtsuEsikHesapla(byte[] gri)
        {
            int[] histogram = new int[256];
            foreach (byte b in gri) histogram[b]++;

            long toplamPiksel = gri.Length;
            long agirlikliToplam = 0;
            for (int i = 0; i < 256; i++) agirlikliToplam += (long)i * histogram[i];

            long arkaToplam = 0, arkaSayi = 0;
            double enIyiVaryans = -1;
            int enIyiEsik = 130;

            for (int t = 0; t < 256; t++)
            {
                arkaSayi += histogram[t];
                if (arkaSayi == 0) continue;
                long onSayi = toplamPiksel - arkaSayi;
                if (onSayi == 0) break;

                arkaToplam += (long)t * histogram[t];
                double arkaOrtalama = (double)arkaToplam / arkaSayi;
                double onOrtalama = (double)(agirlikliToplam - arkaToplam) / onSayi;
                double fark = arkaOrtalama - onOrtalama;
                double varyans = (double)arkaSayi * onSayi * fark * fark;

                if (varyans > enIyiVaryans)
                {
                    enIyiVaryans = varyans;
                    enIyiEsik = t;
                }
            }
            return enIyiEsik;
        }

        private static byte[] AdaptiveThreshold(byte[] gri, int w, int h)
        {
            byte[] sonuc = new byte[gri.Length];
            int S = Math.Max(9, w / 16); 
            int C = 7; 

            int[,] intImg = new int[w, h];
            for (int y = 0; y < h; y++)
            {
                int sum = 0;
                for (int x = 0; x < w; x++)
                {
                    sum += gri[y * w + x];
                    if (y == 0)
                        intImg[x, y] = sum;
                    else
                        intImg[x, y] = intImg[x, y - 1] + sum;
                }
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int x1 = Math.Max(0, x - S / 2);
                    int x2 = Math.Min(w - 1, x + S / 2);
                    int y1 = Math.Max(0, y - S / 2);
                    int y2 = Math.Min(h - 1, y + S / 2);

                    int count = (x2 - x1 + 1) * (y2 - y1 + 1);
                    int sum = intImg[x2, y2];
                    if (x1 > 0) sum -= intImg[x1 - 1, y2];
                    if (y1 > 0) sum -= intImg[x2, y1 - 1];
                    if (x1 > 0 && y1 > 0) sum += intImg[x1 - 1, y1 - 1];

                    if (gri[y * w + x] * count < sum - C * count)
                        sonuc[y * w + x] = 0;
                    else
                        sonuc[y * w + x] = 255;
                }
            }
            return sonuc;
        }

        private static List<PlakaAday> KarakterGrubuAdaylari(byte[] gri, int genislik, int yukseklik, int esik)
        {
            var karakterler = new List<System.Drawing.Rectangle>();
            bool[] ziyaret = new bool[gri.Length];
            var kuyruk = new Queue<int>();

            int minKarYuk = Math.Max(10, yukseklik / 100);
            int maxKarYuk = yukseklik * 15 / 100;

            for (int baslangic = 0; baslangic < gri.Length; baslangic++)
            {
                if (ziyaret[baslangic] || gri[baslangic] >= esik) continue; 

                ziyaret[baslangic] = true;
                kuyruk.Enqueue(baslangic);
                int minX = int.MaxValue, maxX = -1, minY = int.MaxValue, maxY = -1, pikselSayisi = 0;

                while (kuyruk.Count > 0)
                {
                    int p = kuyruk.Dequeue();
                    int x = p % genislik, y = p / genislik;
                    pikselSayisi++;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;

                    if (x > 0 && !ziyaret[p - 1] && gri[p - 1] < esik) { ziyaret[p - 1] = true; kuyruk.Enqueue(p - 1); }
                    if (x < genislik - 1 && !ziyaret[p + 1] && gri[p + 1] < esik) { ziyaret[p + 1] = true; kuyruk.Enqueue(p + 1); }
                    if (y > 0 && !ziyaret[p - genislik] && gri[p - genislik] < esik) { ziyaret[p - genislik] = true; kuyruk.Enqueue(p - genislik); }
                    if (y < yukseklik - 1 && !ziyaret[p + genislik] && gri[p + genislik] < esik) { ziyaret[p + genislik] = true; kuyruk.Enqueue(p + genislik); }
                }

                int w = maxX - minX + 1;
                int h = maxY - minY + 1;
                if (h < minKarYuk || h > maxKarYuk) continue;
                if (w > h * 3 / 2 || w < 2) continue;              
                double doluluk = (double)pikselSayisi / (w * h);
                if (doluluk < 0.15 || doluluk > 0.95) continue;    

                karakterler.Add(new System.Drawing.Rectangle(minX, minY, w, h));
            }

            karakterler.Sort((a, b) => a.X.CompareTo(b.X));
            bool[] kullanildi = new bool[karakterler.Count];
            var adaylar = new List<PlakaAday>();

            for (int i = 0; i < karakterler.Count; i++)
            {
                if (kullanildi[i]) continue;
                var grup = new List<System.Drawing.Rectangle> { karakterler[i] };
                kullanildi[i] = true;
                var son = karakterler[i];

                for (int j = i + 1; j < karakterler.Count; j++)
                {
                    if (kullanildi[j]) continue;
                    var k = karakterler[j];

                    double boyOran = (double)k.Height / son.Height;
                    if (boyOran < 0.6 || boyOran > 1.7) continue;

                    int aralik = k.X - (son.X + son.Width);
                    if (aralik > son.Height * 3 / 2) break; 
                    if (aralik < -son.Width / 2) continue;

                    double merkezFark = Math.Abs((k.Y + k.Height / 2.0) - (son.Y + son.Height / 2.0));
                    if (merkezFark > son.Height * 0.7) continue;

                    grup.Add(k);
                    kullanildi[j] = true;
                    son = k;
                }

                if (grup.Count < 4) continue; 

                bool grupDegisti = true;
                while (grupDegisti && grup.Count >= 4)
                {
                    grupDegisti = false;
                    
                    double toplamBoy = 0;
                    foreach (var k in grup) toplamBoy += k.Height;
                    double ortalamaH = toplamBoy / grup.Count;

                    int enKotuBoyIndex = -1;
                    double enBuyukBoyHata = 0;
                    for (int j = 0; j < grup.Count; j++)
                    {
                        double hata = Math.Abs(grup[j].Height - ortalamaH);
                        if (hata > ortalamaH * 0.35 && hata > enBuyukBoyHata)
                        {
                            enBuyukBoyHata = hata;
                            enKotuBoyIndex = j;
                        }
                    }

                    if (enKotuBoyIndex != -1)
                    {
                        grup.RemoveAt(enKotuBoyIndex);
                        grupDegisti = true;
                        continue;
                    }

                    double gn = grup.Count, gtx = 0, gty = 0, gtxy = 0, gtxx = 0;
                    foreach (var k in grup)
                    {
                        double cx = k.X + k.Width / 2.0, cy = k.Y + k.Height / 2.0;
                        gtx += cx; gty += cy; gtxy += cx * cy; gtxx += cx * cx;
                    }
                    double gpayda = gn * gtxx - gtx * gtx;
                    double gegim = gpayda != 0 ? (gn * gtxy - gtx * gty) / gpayda : 0;
                    double gkesim = (gty - gegim * gtx) / gn;

                    int enKotuHizIndex = -1;
                    double enBuyukHizHata = 0;
                    for (int j = 0; j < grup.Count; j++)
                    {
                        var k = grup[j];
                        double cx = k.X + k.Width / 2.0, cy = k.Y + k.Height / 2.0;
                        double yBeklenen = gegim * cx + gkesim;
                        double hata = Math.Abs(cy - yBeklenen);
                        if (hata > k.Height * 0.40 && hata > enBuyukHizHata)
                        {
                            enBuyukHizHata = hata;
                            enKotuHizIndex = j;
                        }
                    }

                    if (enKotuHizIndex != -1)
                    {
                        grup.RemoveAt(enKotuHizIndex);
                        grupDegisti = true;
                    }
                }

                if (grup.Count < 4) continue;

                int gMinX = int.MaxValue, gMaxX = -1, gMinY = int.MaxValue, gMaxY = -1, toplamYuk = 0;
                foreach (var k in grup)
                {
                    if (k.X < gMinX) gMinX = k.X;
                    if (k.X + k.Width > gMaxX) gMaxX = k.X + k.Width;
                    if (k.Y < gMinY) gMinY = k.Y;
                    if (k.Y + k.Height > gMaxY) gMaxY = k.Y + k.Height;
                    toplamYuk += k.Height;
                }
                int ortYuk = toplamYuk / grup.Count;
                int gw = gMaxX - gMinX, gh = gMaxY - gMinY;
                if ((double)gw / gh < 1.5) continue; 

                double n = grup.Count, tx = 0, ty = 0, txy = 0, txx = 0;
                foreach (var k in grup)
                {
                    double cx = k.X + k.Width / 2.0, cy = k.Y + k.Height / 2.0;
                    tx += cx; ty += cy; txy += cx * cy; txx += cx * cx;
                }
                double payda = n * txx - tx * tx;
                double egim = payda != 0 ? (n * txy - tx * ty) / payda : 0;
                double aci = Math.Atan(egim) * 180.0 / Math.PI;

                int payX = ortYuk * 180 / 100; 
                int payY = ortYuk * 50 / 100;  
                adaylar.Add(new PlakaAday
                {
                    Bolge = new System.Drawing.Rectangle(gMinX - payX, gMinY - payY, gw + 2 * payX, gh + 2 * payY),
                    AciDerece = aci
                });
            }
            return adaylar;
        }

        private static System.Drawing.Bitmap PlakaKesitiHazirla(System.Drawing.Bitmap kaynak, System.Drawing.Rectangle bolge, double aciDerece, int targetHeight)
        {
            if (bolge.Width <= 0 || bolge.Height <= 0)
            {
                return new System.Drawing.Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }

            double ortYuk = bolge.Height / 2.0;

            double cropW = bolge.Width;
            double cropH = bolge.Height;

            float merkezX = bolge.X + bolge.Width / 2f + (float)(0.20 * ortYuk);
            float merkezY = bolge.Y + bolge.Height / 2f;

            double olcek = Math.Max(1.0, (double)targetHeight / (ortYuk * 1.5));
            int yeniGen = (int)(cropW * olcek);
            int yeniYuk = (int)(cropH * olcek);

            var kesit = new System.Drawing.Bitmap(yeniGen, yeniYuk, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var g = System.Drawing.Graphics.FromImage(kesit))
            {
                g.Clear(System.Drawing.Color.White);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                g.TranslateTransform(yeniGen / 2f, yeniYuk / 2f);
                if (Math.Abs(aciDerece) > 1.5)
                {
                    g.RotateTransform((float)(-aciDerece));
                }
                g.ScaleTransform((float)olcek, (float)olcek);
                g.TranslateTransform(-merkezX, -merkezY);

                g.DrawImage(kaynak, 0, 0, kaynak.Width, kaynak.Height);
            }

            int kw = kesit.Width, kh = kesit.Height;
            byte[] kesitGri = GriDiziyeCevir(kesit, out kw, out kh);
            int kesitEsik = OtsuEsikHesapla(kesitGri);

            byte[] bin = new byte[kw * kh];
            for (int i = 0; i < bin.Length; i++)
            {
                bin[i] = kesitGri[i] >= kesitEsik ? (byte)255 : (byte)0;
            }

            int sinirYUst = (int)(kh * 0.14);
            int sinirYAlt = (int)(kh * 0.15);
            int sinirXSol = (int)(kw * 0.14); 
            int sinirXSag = (int)(kw * 0.05); 

            for (int y = 0; y < kh; y++)
            {
                for (int x = 0; x < kw; x++)
                {
                    if (y < sinirYUst || y >= kh - sinirYAlt || x < sinirXSol || x >= kw - sinirXSag)
                    {
                        bin[y * kw + x] = 255;
                    }
                }
            }

            bool[] viz = new bool[bin.Length];
            var components = new System.Collections.Generic.List<System.Collections.Generic.List<int>>();
            var bboxes = new System.Collections.Generic.List<System.Drawing.Rectangle>();
            var q = new System.Collections.Generic.Queue<int>();

            for (int start = 0; start < bin.Length; start++)
            {
                if (viz[start] || bin[start] == 255) continue;

                viz[start] = true;
                q.Enqueue(start);
                var component = new System.Collections.Generic.List<int> { start };
                int minX = start % kw, maxX = minX, minY = start / kw, maxY = minY;

                while (q.Count > 0)
                {
                    int p = q.Dequeue();
                    int x = p % kw, y = p / kw;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;

                    int[] dx = { -1, 1, 0, 0 };
                    int[] dy = { 0, 0, -1, 1 };
                    for (int d = 0; d < 4; d++)
                    {
                        int nx = x + dx[d], ny = y + dy[d];
                        if (nx >= 0 && nx < kw && ny >= 0 && ny < kh)
                        {
                            int np = ny * kw + nx;
                            if (!viz[np] && bin[np] == 0)
                            {
                                viz[np] = true;
                                q.Enqueue(np);
                                component.Add(np);
                            }
                        }
                    }
                }

                int compW = maxX - minX + 1;
                int compH = maxY - minY + 1;
                bool temizle = false;

                if ((minY == 0 || maxY == kh - 1) && compW > kw * 0.40) temizle = true;
                else if ((minX == 0 || maxX == kw - 1) && compH > kh * 0.45) temizle = true;
                else if (component.Count < (kw * kh) / 600) temizle = true;

                if (temizle)
                {
                    foreach (int idx in component) bin[idx] = 255;
                }
                else
                {
                    components.Add(component);
                    bboxes.Add(new System.Drawing.Rectangle(minX, minY, compW, compH));
                }
            }

            int enSagIndex = -1;
            int enSagX = -1;
            for (int i = 0; i < bboxes.Count; i++)
            {
                if (bboxes[i].X > enSagX)
                {
                    enSagX = bboxes[i].X;
                    enSagIndex = i;
                }
            }

            if (enSagIndex != -1)
            {
                var box = bboxes[enSagIndex];
                
                if (box.Width > 21)
                {
                    int cutX = box.X + 17;
                    int cutY = box.Y + box.Height - 14;
                    for (int y = cutY; y < box.Y + box.Height; y++)
                    {
                        for (int x = cutX; x < box.X + box.Width; x++)
                        {
                            if (x >= 0 && x < kw && y >= 0 && y < kh)
                            {
                                bin[y * kw + x] = 255;
                            }
                        }
                    }
                }
            }

            var veri = kesit.LockBits(
                new System.Drawing.Rectangle(0, 0, kw, kh),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            try
            {
                byte[] ham = new byte[veri.Stride * kh];
                for (int y = 0; y < kh; y++)
                {
                    int satirBasi = y * veri.Stride;
                    for (int x = 0; x < kw; x++)
                    {
                        byte deger = bin[y * kw + x];
                        int p = satirBasi + x * 3;
                        ham[p] = deger;
                        ham[p + 1] = deger;
                        ham[p + 2] = deger;
                    }
                }
                Marshal.Copy(ham, 0, veri.Scan0, ham.Length);
            }
            finally
            {
                kesit.UnlockBits(veri);
            }

            int paddingX = 30;
            int paddingY = 15;
            var dolguluKesit = new System.Drawing.Bitmap(kw + 2 * paddingX, kh + 2 * paddingY, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var g = System.Drawing.Graphics.FromImage(dolguluKesit))
            {
                g.Clear(System.Drawing.Color.White);
                g.DrawImage(kesit, paddingX, paddingY);
            }
            kesit.Dispose();

            return dolguluKesit;
        }

        private void ApplyDarkTheme()
        {
            this.BackColor = System.Drawing.Color.FromArgb(18, 20, 30);
            this.ForeColor = System.Drawing.Color.White;

            ApplyDarkThemeRecursive(this);

            if (dgvKayitlar != null)
            {
                dgvKayitlar.EnableHeadersVisualStyles = false;
                dgvKayitlar.BackgroundColor = System.Drawing.Color.FromArgb(26, 28, 42);
                dgvKayitlar.GridColor = System.Drawing.Color.FromArgb(45, 48, 70);
                
                dgvKayitlar.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(35, 38, 58);
                dgvKayitlar.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(0, 230, 118);
                dgvKayitlar.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
                
                dgvKayitlar.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(22, 24, 36);
                dgvKayitlar.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(220, 225, 240);
                dgvKayitlar.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(0, 200, 83);
                dgvKayitlar.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

                dgvKayitlar.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(28, 30, 46);
                dgvKayitlar.AlternatingRowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(220, 225, 240);
                dgvKayitlar.AlternatingRowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(0, 200, 83);
                dgvKayitlar.AlternatingRowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            }

            if (lblLedPanel != null)
            {
                lblLedPanel.BackColor = System.Drawing.Color.FromArgb(10, 12, 18);
                lblLedPanel.BorderStyle = BorderStyle.Fixed3D;
            }

            if (btnGirisKaydet != null) { btnGirisKaydet.BackColor = System.Drawing.Color.FromArgb(0, 200, 83); btnGirisKaydet.ForeColor = System.Drawing.Color.Black; btnGirisKaydet.FlatStyle = FlatStyle.Flat; }
            if (btnFotoCek != null) { btnFotoCek.BackColor = System.Drawing.Color.FromArgb(0, 176, 255); btnFotoCek.ForeColor = System.Drawing.Color.Black; btnFotoCek.FlatStyle = FlatStyle.Flat; }
            if (btnBaglan != null) { btnBaglan.BackColor = System.Drawing.Color.FromArgb(41, 121, 255); btnBaglan.ForeColor = System.Drawing.Color.White; btnBaglan.FlatStyle = FlatStyle.Flat; }
            if (btnCikisKontrol != null) { btnCikisKontrol.BackColor = System.Drawing.Color.FromArgb(255, 61, 0); btnCikisKontrol.ForeColor = System.Drawing.Color.White; btnCikisKontrol.FlatStyle = FlatStyle.Flat; }
            if (btnFisYazdir != null) { btnFisYazdir.BackColor = System.Drawing.Color.FromArgb(156, 39, 176); btnFisYazdir.ForeColor = System.Drawing.Color.White; btnFisYazdir.FlatStyle = FlatStyle.Flat; }
            if (btnExcelAktar != null) { btnExcelAktar.BackColor = System.Drawing.Color.FromArgb(0, 150, 136); btnExcelAktar.ForeColor = System.Drawing.Color.White; btnExcelAktar.FlatStyle = FlatStyle.Flat; }
        }

        private void ApplyDarkThemeRecursive(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is GroupBox gb)
                {
                    gb.BackColor = System.Drawing.Color.FromArgb(26, 28, 42);
                    gb.ForeColor = System.Drawing.Color.FromArgb(0, 230, 118);
                    ApplyDarkThemeRecursive(gb);
                }
                else if (c is Panel pnl && pnl != pnlTrafikLambasi)
                {
                    pnl.BackColor = System.Drawing.Color.FromArgb(26, 28, 42);
                    ApplyDarkThemeRecursive(pnl);
                }
                else if (c is TextBox tb)
                {
                    tb.BackColor = System.Drawing.Color.FromArgb(18, 20, 30);
                    tb.ForeColor = System.Drawing.Color.White;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (c is ComboBox cb)
                {
                    cb.BackColor = System.Drawing.Color.FromArgb(18, 20, 30);
                    cb.ForeColor = System.Drawing.Color.White;
                    cb.FlatStyle = FlatStyle.Flat;
                }
                else if (c is Label lbl && lbl != lblLedPanel)
                {
                    lbl.ForeColor = System.Drawing.Color.FromArgb(210, 215, 230);
                }
                else if (c is CheckBox chk)
                {
                    chk.ForeColor = System.Drawing.Color.FromArgb(210, 215, 230);
                }
                else if (c is Button btn && btn != btnGirisKaydet && btn != btnFotoCek && btn != btnBaglan && btn != btnCikisKontrol && btn != btnFisYazdir && btn != btnExcelAktar)
                {
                    btn.BackColor = System.Drawing.Color.FromArgb(38, 42, 62);
                    btn.ForeColor = System.Drawing.Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                }

                if (c.HasChildren)
                {
                    ApplyDarkThemeRecursive(c);
                }
            }
        }
    }
}