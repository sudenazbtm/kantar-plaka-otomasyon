using PreviewDemo;
using System;
using System.Windows.Forms;

namespace Kantar_Plaka_Arge_Projesi
{
    public partial class LoginForm : Form
    {
        public static string CameraIP { get; set; } = "192.168.1.64";
        public static string CameraUser { get; set; } = "admin";
        public static string CameraPass { get; set; } = "";
        public static Int32 LoggedUserID { get; set; } = -1;
        public static bool IsOfflineMode { get; set; } = false;

        public LoginForm()
        {
            InitializeComponent();
            try
            {
                CHCNetSDK.NET_DVR_Init();
            }
            catch { }
        }

        private async void btnGiris_Click(object sender, EventArgs e)
        {
            lblHata.Text = "";
            string ip = txtCameraIP.Text.Trim();
            string user = txtCameraUser.Text.Trim();
            string pass = txtCameraPass.Text.Trim();

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                lblHata.Text = "Lütfen tüm kamera bağlantı alanlarını doldurunuz!";
                return;
            }

            btnGiris.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            Int32 lUserID = -1;
            uint err = 0;

            await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    CHCNetSDK.NET_DVR_DEVICEINFO_V30 struDeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
                    lUserID = CHCNetSDK.NET_DVR_Login_V30(ip, 8000, user, pass, ref struDeviceInfo);
                    if (lUserID < 0)
                    {
                        err = CHCNetSDK.NET_DVR_GetLastError();
                    }
                }
                catch { }
            });

            this.Cursor = Cursors.Default;
            btnGiris.Enabled = true;

            if (lUserID >= 0)
            {
                CameraIP = ip;
                CameraUser = user;
                CameraPass = pass;
                LoggedUserID = lUserID;
                IsOfflineMode = false;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblHata.Text = $"Kamera Doğrulaması Başarısız!\nIP/Kullanıcı/Parola hatalı veya kamera kapalı. (Hata Kodu: {err})";
            }
        }

        private void btnOffline_Click(object sender, EventArgs e)
        {
            CameraIP = txtCameraIP.Text.Trim();
            CameraUser = txtCameraUser.Text.Trim();
            CameraPass = txtCameraPass.Text.Trim();
            LoggedUserID = -1;
            IsOfflineMode = true;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
