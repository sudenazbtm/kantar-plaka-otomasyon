using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;

namespace Kantar_Plaka_Arge_Projesi
{
    public class YetkiSorguSonucu
    {
        public string Durum { get; set; }
        public string Mesaj { get; set; }
    }

    public class KantarKayit
    {
        public string Plaka { get; set; }
        public DateTime GirisZamani { get; set; }
        public string GirisResimYolu { get; set; }
        public int? BosTartim { get; set; }
        public DateTime? BosTartimZamani { get; set; }
        public int? DoluTartim { get; set; }
        public DateTime? DoluTartimZamani { get; set; }
        public int? NetKilo { get; set; }
        public bool FaturaDurumu { get; set; }
        public DateTime? CikisZamani { get; set; }
        public string Durum { get; set; } 
        public string SoforAdi { get; set; }
        public string FirmaAdi { get; set; }
        public string MalzemeCinsi { get; set; }
    }

    public static class DatabaseManager
    {
        private static readonly string XmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kantar_db.xml");
        private static readonly object FileLock = new object();

        public static string SqlConnectionString = ""; 
        public static bool UseSqlServer => !string.IsNullOrEmpty(SqlConnectionString);

        public static readonly List<string> Whitelist = new List<string> { "34ABC123", "06XYZ456", "35DEF789", "16KLM012" };
        public static readonly List<string> Blacklist = new List<string> { "34BLK001", "34BLK002" };
        public static readonly Dictionary<string, DateTime> Guestlist = new Dictionary<string, DateTime>
        {
            { "34GST001", DateTime.Now.AddHours(2) },
            { "34GST002", DateTime.Now.AddHours(-1) }
        };

        public static YetkiSorguSonucu PlakaSorgula(string plaka)
        {
            if (string.IsNullOrEmpty(plaka))
                return new YetkiSorguSonucu { Durum = "UNKNOWN", Mesaj = "Bos Plaka" };

            plaka = plaka.Trim().ToUpper();

            if (Blacklist.Contains(plaka))
            {
                return new YetkiSorguSonucu { Durum = "BLACKLIST", Mesaj = "KARA LISTE: GEÇIS ENGEL" };
            }

            if (Whitelist.Contains(plaka))
            {
                return new YetkiSorguSonucu { Durum = "WHITELIST", Mesaj = "BEYAZ LISTE: YETKILI" };
            }

            if (Guestlist.ContainsKey(plaka))
            {
                DateTime bitis = Guestlist[plaka];
                if (DateTime.Now <= bitis)
                {
                    return new YetkiSorguSonucu { Durum = "GUEST_VALID", Mesaj = "GEÇICI IZIN: GEÇERLI" };
                }
                else
                {
                    return new YetkiSorguSonucu { Durum = "GUEST_EXPIRED", Mesaj = "GEÇICI IZIN: SURE DOLU" };
                }
            }

            return new YetkiSorguSonucu { Durum = "UNKNOWN", Mesaj = "TANIMSIZ ZIYARETÇI" };
        }

        static DatabaseManager()
        {
            
            lock (FileLock)
            {
                if (!File.Exists(XmlFilePath))
                {
                    try
                    {
                        KayitlariKaydetXml(new List<KantarKayit>());
                    }
                    catch { }
                }
            }
        }

        public static List<KantarKayit> TumKayitlariGetir()
        {
            if (UseSqlServer)
            {
                return TumKayitlariGetirSql();
            }

            lock (FileLock)
            {
                if (!File.Exists(XmlFilePath))
                    return new List<KantarKayit>();

                try
                {
                    var serializer = new XmlSerializer(typeof(List<KantarKayit>));
                    using (var reader = new StreamReader(XmlFilePath))
                    {
                        return (List<KantarKayit>)serializer.Deserialize(reader);
                    }
                }
                catch
                {
                    return new List<KantarKayit>();
                }
            }
        }

        private static void KayitlariKaydetXml(List<KantarKayit> list)
        {
            var serializer = new XmlSerializer(typeof(List<KantarKayit>));
            using (var writer = new StreamWriter(XmlFilePath))
            {
                serializer.Serialize(writer, list);
            }
        }

        public static KantarKayit AktifKayitBul(string plaka)
        {
            if (string.IsNullOrEmpty(plaka)) return null;
            plaka = plaka.Trim().ToUpper();

            if (UseSqlServer)
            {
                return AktifKayitBulSql(plaka);
            }

            var tumu = TumKayitlariGetir();
            return tumu.Find(x => x.Plaka == plaka && x.Durum == "AKTIF");
        }

        public static void YeniGirisKaydet(string plaka, string resimYolu, string sofor = "", string firma = "", string malzeme = "")
        {
            if (string.IsNullOrEmpty(plaka)) return;
            plaka = plaka.Trim().ToUpper();

            var mevcutAktif = AktifKayitBul(plaka);
            if (mevcutAktif != null) return;

            var yeni = new KantarKayit
            {
                Plaka = plaka,
                GirisZamani = DateTime.Now,
                GirisResimYolu = resimYolu,
                Durum = "AKTIF",
                FaturaDurumu = false,
                SoforAdi = sofor,
                FirmaAdi = firma,
                MalzemeCinsi = malzeme
            };

            if (UseSqlServer)
            {
                YeniGirisKaydetSql(yeni);
                return;
            }

            lock (FileLock)
            {
                var tumu = TumKayitlariGetir();
                tumu.Add(yeni);
                KayitlariKaydetXml(tumu);
            }
        }

        public static void BosTartimKaydet(string plaka, int kilo)
        {
            if (string.IsNullOrEmpty(plaka)) return;
            plaka = plaka.Trim().ToUpper();

            if (UseSqlServer)
            {
                BosTartimKaydetSql(plaka, kilo);
                return;
            }

            lock (FileLock)
            {
                var tumu = TumKayitlariGetir();
                var aktif = tumu.Find(x => x.Plaka == plaka && x.Durum == "AKTIF");
                if (aktif != null)
                {
                    aktif.BosTartim = kilo;
                    aktif.BosTartimZamani = DateTime.Now;
                    KayitlariKaydetXml(tumu);
                }
            }
        }

        public static void DoluTartimKaydet(string plaka, int kilo)
        {
            if (string.IsNullOrEmpty(plaka)) return;
            plaka = plaka.Trim().ToUpper();

            if (UseSqlServer)
            {
                DoluTartimKaydetSql(plaka, kilo);
                return;
            }

            lock (FileLock)
            {
                var tumu = TumKayitlariGetir();
                var aktif = tumu.Find(x => x.Plaka == plaka && x.Durum == "AKTIF");
                if (aktif != null)
                {
                    aktif.DoluTartim = kilo;
                    aktif.DoluTartimZamani = DateTime.Now;
                    if (aktif.BosTartim.HasValue)
                    {
                        aktif.NetKilo = Math.Abs(aktif.DoluTartim.Value - aktif.BosTartim.Value);
                    }
                    KayitlariKaydetXml(tumu);
                }
            }
        }

        public static void FaturaDurumuGuncelle(string plaka, bool faturaKesildi)
        {
            if (string.IsNullOrEmpty(plaka)) return;
            plaka = plaka.Trim().ToUpper();

            if (UseSqlServer)
            {
                FaturaDurumuGuncelleSql(plaka, faturaKesildi);
                return;
            }

            lock (FileLock)
            {
                var tumu = TumKayitlariGetir();
                var aktif = tumu.Find(x => x.Plaka == plaka && x.Durum == "AKTIF");
                if (aktif != null)
                {
                    aktif.FaturaDurumu = faturaKesildi;
                    KayitlariKaydetXml(tumu);
                }
            }
        }

        public static bool CikisYap(string plaka, out string hataMesaji)
        {
            hataMesaji = "";
            if (string.IsNullOrEmpty(plaka))
            {
                hataMesaji = "Geçersiz plaka.";
                return false;
            }
            plaka = plaka.Trim().ToUpper();

            var aktif = AktifKayitBul(plaka);
            if (aktif == null)
            {
                hataMesaji = "Bu plakaya ait aktif bir giriş kaydı bulunamadı.";
                return false;
            }

            if (!aktif.BosTartim.HasValue)
            {
                hataMesaji = "Aracın boş tartımı yapılmamış.";
                return false;
            }

            if (!aktif.DoluTartim.HasValue)
            {
                hataMesaji = "Aracın dolu tartımı yapılmamış.";
                return false;
            }

            if (!aktif.FaturaDurumu)
            {
                hataMesaji = "Aracın fatura/irsaliye kaydı onaylanmamış.";
                return false;
            }

            if (UseSqlServer)
            {
                return CikisYapSql(plaka, out hataMesaji);
            }

            lock (FileLock)
            {
                var tumu = TumKayitlariGetir();
                var kayit = tumu.Find(x => x.Plaka == plaka && x.Durum == "AKTIF");
                if (kayit != null)
                {
                    kayit.CikisZamani = DateTime.Now;
                    kayit.Durum = "TAMAMLANDI";
                    KayitlariKaydetXml(tumu);
                    return true;
                }
            }

            hataMesaji = "Sistemsel bir hata oluştu.";
            return false;
        }

        private static List<KantarKayit> TumKayitlariGetirSql()
        {
            var list = new List<KantarKayit>();
            try
            {
                using (var conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM KantarKayitlari ORDER BY GirisZamani DESC";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                list.Add(new KantarKayit
                                {
                                    Plaka = rdr["Plaka"].ToString(),
                                    GirisZamani = (DateTime)rdr["GirisZamani"],
                                    GirisResimYolu = rdr["GirisResimYolu"]?.ToString(),
                                    BosTartim = rdr["BosTartim"] == DBNull.Value ? (int?)null : (int)rdr["BosTartim"],
                                    BosTartimZamani = rdr["BosTartimZamani"] == DBNull.Value ? (DateTime?)null : (DateTime)rdr["BosTartimZamani"],
                                    DoluTartim = rdr["DoluTartim"] == DBNull.Value ? (int?)null : (int)rdr["DoluTartim"],
                                    DoluTartimZamani = rdr["DoluTartimZamani"] == DBNull.Value ? (DateTime?)null : (DateTime)rdr["DoluTartimZamani"],
                                    NetKilo = rdr["NetKilo"] == DBNull.Value ? (int?)null : (int)rdr["NetKilo"],
                                    FaturaDurumu = (bool)rdr["FaturaDurumu"],
                                    CikisZamani = rdr["CikisZamani"] == DBNull.Value ? (DateTime?)null : (DateTime)rdr["CikisZamani"],
                                    Durum = rdr["Durum"].ToString(),
                                    SoforAdi = rdr["SoforAdi"]?.ToString(),
                                    FirmaAdi = rdr["FirmaAdi"]?.ToString(),
                                    MalzemeCinsi = rdr["MalzemeCinsi"]?.ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch { }
            return list;
        }

        private static KantarKayit AktifKayitBulSql(string plaka)
        {
            try
            {
                using (var conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                    string query = "SELECT TOP 1 * FROM KantarKayitlari WHERE Plaka = @plaka AND Durum = 'AKTIF'";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                return new KantarKayit
                                {
                                    Plaka = rdr["Plaka"].ToString(),
                                    GirisZamani = (DateTime)rdr["GirisZamani"],
                                    GirisResimYolu = rdr["GirisResimYolu"]?.ToString(),
                                    BosTartim = rdr["BosTartim"] == DBNull.Value ? (int?)null : (int)rdr["BosTartim"],
                                    BosTartimZamani = rdr["BosTartimZamani"] == DBNull.Value ? (DateTime?)null : (DateTime)rdr["BosTartimZamani"],
                                    DoluTartim = rdr["DoluTartim"] == DBNull.Value ? (int?)null : (int)rdr["DoluTartim"],
                                    DoluTartimZamani = rdr["DoluTartimZamani"] == DBNull.Value ? (DateTime?)null : (DateTime)rdr["DoluTartimZamani"],
                                    NetKilo = rdr["NetKilo"] == DBNull.Value ? (int?)null : (int)rdr["NetKilo"],
                                    FaturaDurumu = (bool)rdr["FaturaDurumu"],
                                    CikisZamani = rdr["CikisZamani"] == DBNull.Value ? (DateTime?)null : (DateTime)rdr["CikisZamani"],
                                    Durum = rdr["Durum"].ToString(),
                                    SoforAdi = rdr["SoforAdi"]?.ToString(),
                                    FirmaAdi = rdr["FirmaAdi"]?.ToString(),
                                    MalzemeCinsi = rdr["MalzemeCinsi"]?.ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        private static void YeniGirisKaydetSql(KantarKayit yeni)
        {
            try
            {
                using (var conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                     string query = "INSERT INTO KantarKayitlari (Plaka, GirisZamani, GirisResimYolu, FaturaDurumu, Durum, SoforAdi, FirmaAdi, MalzemeCinsi) " +
                                   "VALUES (@plaka, @giris, @resim, @fatura, @durum, @sofor, @firma, @malzeme)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@plaka", yeni.Plaka);
                        cmd.Parameters.AddWithValue("@giris", yeni.GirisZamani);
                        cmd.Parameters.AddWithValue("@resim", (object)yeni.GirisResimYolu ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@fatura", yeni.FaturaDurumu);
                        cmd.Parameters.AddWithValue("@durum", yeni.Durum);
                        cmd.Parameters.AddWithValue("@sofor", (object)yeni.SoforAdi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@firma", (object)yeni.FirmaAdi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@malzeme", (object)yeni.MalzemeCinsi ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private static void BosTartimKaydetSql(string plaka, int kilo)
        {
            try
            {
                using (var conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                    string query = "UPDATE KantarKayitlari SET BosTartim = @kilo, BosTartimZamani = @tarih WHERE Plaka = @plaka AND Durum = 'AKTIF'";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kilo", kilo);
                        cmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private static void DoluTartimKaydetSql(string plaka, int kilo)
        {
            try
            {
                using (var conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                    string query = "UPDATE KantarKayitlari SET DoluTartim = @kilo, DoluTartimZamani = @tarih WHERE Plaka = @plaka AND Durum = 'AKTIF'";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kilo", kilo);
                        cmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        cmd.ExecuteNonQuery();
                    }

                    var aktif = AktifKayitBulSql(plaka);
                    if (aktif != null && aktif.BosTartim.HasValue)
                    {
                        int net = Math.Abs(kilo - aktif.BosTartim.Value);
                        string updateNet = "UPDATE KantarKayitlari SET NetKilo = @net WHERE Plaka = @plaka AND Durum = 'AKTIF'";
                        using (var cmd = new SqlCommand(updateNet, conn))
                        {
                            cmd.Parameters.AddWithValue("@net", net);
                            cmd.Parameters.AddWithValue("@plaka", plaka);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch { }
        }

        private static void FaturaDurumuGuncelleSql(string plaka, bool faturaKesildi)
        {
            try
            {
                using (var conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                    string query = "UPDATE KantarKayitlari SET FaturaDurumu = @fatura WHERE Plaka = @plaka AND Durum = 'AKTIF'";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fatura", faturaKesildi);
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        private static bool CikisYapSql(string plaka, out string hataMesaji)
        {
            hataMesaji = "";
            try
            {
                using (var conn = new SqlConnection(SqlConnectionString))
                {
                    conn.Open();
                    string query = "UPDATE KantarKayitlari SET CikisZamani = @cikis, Durum = 'TAMAMLANDI' WHERE Plaka = @plaka AND Durum = 'AKTIF'";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cikis", DateTime.Now);
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        int affected = cmd.ExecuteNonQuery();
                        if (affected > 0) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                hataMesaji = "SQL Hatası: " + ex.Message;
            }
            return false;
        }
    }
}
