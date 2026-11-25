# 🚛 Kantar & Plaka Entegreli Otomasyon Sistemi

> **Kantar & Plaka Tanıma Sistemi Kapsamlı Dokümantasyonu**  
> **Platform:** C# .NET Framework 4.7.2 (WinForms) | Hikvision HCNetSDK v6.1 | Hikvision ISAPI | Tesseract OCR 5.2 | Plate Recognizer AI Cloud API | ONNX | XML & SQL Server  

---

## 🔑 1. Sistem Giriş Bilgileri & Varsayılan Ayarlar

Uygulama başlatıldığında donanım doğrulamalı bağımsız **Kamera Giriş Ekranı (`LoginForm.cs`)** açılır. Kamera bağlantısı veya sistem testi için parametreler:

| Parametre | Değer / Örnek | Açıklama |
| :--- | :--- | :--- |
| **Kamera IP Adresi** | `192.168.1.64` | Tesis / Saha Hikvision IP Kamera IP'si |
| **Kamera Portu** | `8000` | Hikvision SDK Varsayılan İletişim Portu |
| **Kamera Kullanıcı Adı** | `admin` | Kamera Yönetici Kullanıcı Adı |
| **Kamera Parolası** | `******` | Kamera Erişim Parolası |
| **API Token** | `YOUR_API_TOKEN` | Plate Recognizer Bulut Yapay Zeka Erişim Anahtarı |
| **Çevrimdışı Mod** | *"Çevrimdışı Modda Başlat"* | Kamerasız testler ve kantar simülasyonu için |

---

## 📝 2. Projede Uygulanan Mimari, SDK ve Teknolojiler

Bu proje geliştirilirken endüstriyel kantar ve plaka tanıma alanında en hızlı ve kararlı mimariyi kurmak için birçok farklı kütüphane, donanım protokolü ve yapay zeka yöntemi entegre edilmiştir. Karşılaşılan mühendislik zorlukları ve geliştirilen çözümler aşağıdadır:

### 1. Hikvision C# SDK (`CHCNetSDK.cs` - HCNetSDK v6.1) Entegrasyonu
*   **Nasıl Çalışır?** Hikvision IP kameralarla soket seviyesinde haberleşmek için resmi `HCNetSDK.dll` kütüphanesi C# P/Invoke katmanı (`CHCNetSDK.cs`) ile projeye dahil edilmiştir. Oturum açmak için `NET_DVR_Login_V30`, canlı görüntü yayın akışı için `NET_DVR_RealPlay_V40`, anlık fotoğraf yakalamak için `NET_DVR_CapturePicture` ve oturum kapatma için `NET_DVR_Logout` metotları bağlanmıştır.
*   **Mühendislik Zorluğu 1 (x86 / x64 DLL Çakışması):** SDK kütüphanelerinin yüklenmesi esnasında 32-bit ve 64-bit bellek adreslemesi çakıştığı için `BadImageFormatException` ve `DllNotFoundException` hatalarıyla karşılaşılmıştır.
*   **Çözüm:** Proje derleme hedefi Visual Studio üzerinde kesin olarak **`x64`** platformuna sabitlenmiştir.
*   **Mühendislik Zorluğu 2 (Arayüz Donma Problemi):** Canlı yayın başlarken SDK'nın `bBlocked = true` modu formu 3-5 saniye kilitliyordu.
*   **Çözüm:** `struPreviewInfo.bBlocked = false;` yapılarak canlı yayın asenkron iş parçacığına (`Task.Run` ve `BeginInvoke`) taşınmıştır. Açılış süresi **0.05 saniyeye** düşürülmüştür.

### 2. Hikvision ISAPI (HTTP REST Endpoints) İncelemesi
*   **Mimari:** SDK kullanmaksızın doğrudan HTTP isteği atarak kameradan anlık fotoğraf çekme (`http://<KAMERA_IP>/ISAPI/Streaming/channels/101/picture`) yapısı incelenmiştir.
*   **Karşılaştırma ve Tercih:** ISAPI protokolü `HTTP Digest Authentication` gerektirdiğinden ve ağ paketlerinde ekstra oturum yönetimi yükü getirdiğinden, SDK üzerinden soket seviyesinde haberleşmenin daha hızlı olduğu gözlemlenip ana yapı SDK ile tamamlanmıştır.

### 3. OpenCV / EmguCV ve C# Native `LockBits` Bellek Mimarisi Karşılaştırması
*   **Yöntem:** Plaka bölgesi tespiti için OpenCV ve C# yerel bellek yönetim yöntemleri karşılaştırılmıştır.
*   **Neden Native C# Tercih Edildi?** Ağır harici C++ wrapper bağımlılıklarının projeye ek yük getirmemesi için C#'ın `Bitmap.LockBits` bellek adresleme tekniği kullanılmıştır.
*   **Performans İyileştirmesi:** Standart `GetPixel/SetPixel` metotları RAM'i yorduğu için doğrudan bellek adresi kilitlenip (`Bitmap.LockBits` & `Marshal.Copy`) 1D byte dizisi üzerinden 8-bit Gri Tonlama (`Gri = 0.299*R + 0.587*G + 0.114*B`) kodlanmıştır. İşleme süresi **120 ms'den 12 ms'ye (10 kat hız)** düşürülmüştür.

### 4. C# ile Adaptif Otsu Binarizasyon ve Aday Bölge (ROI) Tespiti
*   **Otsu Binarizasyon:** Güneş yansıması, gece çekimi ve gölgeli plakaları netleştirmek için piksel histogram varyansını maximize eden `Otsu Eşikleme` algoritması C# ile geliştirilmiştir.
*   **Aday Bölge (ROI) Süzgeci:** Görüntüdeki araç tamponu ve yansımaları elemek için plaka geometrisine uygun Genişlik/Yükseklik oranı **3.0 ile 6.0** arasındaki dikdörtgen bölgeleri (ROI) tarayan BoundingBox algoritması geliştirilmiştir.
*   **Çoklu Eşik Taraması (Multi-Threshold Scanning):** Kirli ve yıpranmış plakaları kaçırmamak adına `Otsu`, `Otsu + 40` ve `200` sabit eşik değerleriyle 3 farklı binarizasyon katmanı uygulanmıştır. ROI bulunamadığı durumlarda ise sistemin resmi kaçırmaması için **Tüm Resim (Full-Image)** ve **Merkez %70 Bölge (Center-Region)** yedek tarama katmanları eklenmiştir.

### 5. Google Tesseract OCR Motoru Entegrasyonu ve Karakter Optimizasyonu
*   **Entegrasyon:** `Tesseract 5.2.0` NuGet paketi ve `tur.traineddata` dil verisi entegre edilmiştir.
*   **Sözlük Tahminlerini Kapatma:** Tesseract'ın plakaları kelimelere benzetmesini önlemek için DAWG sözlükleri kapatılmıştır (`load_system_dawg = 0`, `load_freq_dawg = 0`).
*   **Karakter Whitelist:** Karakter kümesi yalnızca plaka karakterlerine kısıtlanmıştır (`whitelist = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"`).
*   **Metin Düzeltme & Regex:** Karakter karışıklıklarını ('O'->'0', 'I'->'1', 'B'->'8') düzelten `PlakaMetniniDuzelt()` metodu kodlanmış ve çıktılar plaka Regex formatı (`\d{2}[A-Z]{1,5}\d{2,4}`) ile doğrulanmıştır.

### 6. Bulut Yapay Zeka Entegrasyonu (Plate Recognizer REST API)
*   **Çalışma Prensibi:** `PlateRecognizerApi.cs` sınıfı üzerinden `HttpClient` ile fotoğraflar multipart/form-data olarak bulut derin öğrenme (CNN) sunucusuna gönderilir.
*   **Bağımsız JSON Parsing:** Dönen yanıt ekstra bağımlılık gerektirmeden C# Regex deseni `(?:""plate""\s*:\s*"")([^""]+)""` ile ayrıştırılarak **%99.9 doğrulukta** plaka verisi elde edilir.

### 7. Üç Kademeli Hibrit Yapay Zeka Mimarisi
Sistemi tek noktaya bağımlı kılmamak için yedekli mimari kurulmuştur:
1.  **1. Kademe (Bulut AI API):** İnternet ve API Token varsa resmi derin öğrenme sunucusunda okur (%99.9 doğruluk).
2.  **2. Kademe (Özel Model - `CustomAiModel.cs`):** Projede `models/plaka_model.onnx` dosyası varsa özel eğitilmiş YOLOv8/ONNX modellerini çalıştırır.
3.  **3. Kademe (Lokal Tesseract Fallback):** İnternet veya kota bittiğinde sistem kesintisiz olarak bellek içi Tesseract motoruna düşer.

### 8. Veritabanı ve Kalıcılık Katmanı (`DatabaseManager.cs`)
*   **XML Serileştirme & Yerel Kalıcılık (`kantar_db.xml`):** Ekstra veritabanı sunucusu gerektirmeyen, taşınabilir C# `XmlSerializer` veri saklama mimarisi.
*   **SQL Server Kurumsal Desteği:** `DatabaseManager` sınıfı kurumsal SQL Server veritabanı bağlantı dizgilerine (Connection String) doğrudan geçiş yapabilecek modüler yapıda kodlanmıştır.
*   **Thread Safety (`lock` mekanizması):** Eşzamanlı okuma/yazma kilitlenmelerini önlemek için `lock(FileLock)` senkronizasyonu uygulanmıştır.
*   **Plaka Sorgu Mantığı:** `PlakaSorgula()` metodu ile Beyaz Liste (Whitelist - Otomatik Geçiş), Kara Liste (Blacklist - Bariyer Kilitleme), Misafir ve Tanımsız Ziyaretçi Onay Akışı sağlanmıştır.

---

## 🛠️ 3. Tesis ve Kantar İş Akışı Mantığı

Sistem endüstriyel kantar ve tesis işleyişine uygun tasarlanmıştır:

1.  **Giriş & Geçiş İzni Kontrolü:**
    *   Araç geldiğinde kameradan veya bilgisayardaki resimden plaka okunur.
    *   `DatabaseManager.PlakaSorgula()` fonksiyonu plakayı sorgular:
        *   **Beyaz Liste (Whitelist):** Otomatik onaylanır, yeşil ışık yanar ve bariyer açılır.
        *   **Kara Liste (Blacklist):** Kırmızı ışık yanar, bariyer kilitlenir ve sesli/görsel uyarı verilir.
        *   **Misafir (Guestlist):** Süresi geçerliyse onaylanır.
        *   **Tanımsız Ziyaretçi:** Operatörün ekranına *"Giriş onaylansın mı?"* onay penceresi düşer.
2.  **Kantar Tartım Süreci:**
    *   **Boş Tartım (Dara):** Tesis girişinde boş kantar kilosu kaydedilir.
    *   **Dolu Tartım (Brüt):** Yük yüklendikten sonra dolu kilo kaydedilir.
    *   **Net Malzeme Hesaplama:** Sistem otomatik olarak `Net Kilo = Dolu - Boş` hesabını yapar.
3.  **Fatura Zorunluluğu & Çıkış İzni:**
    *   Fatura/İrsaliye onayı verilmeden araç çıkış bariyerinden geçemez.
4.  **Endüstriyel Sensörler & LED Panel:**
    *   Araç kantar platformuna hizalanmadığında (ön/arka sensör kesildiğinde) tartım kilitlenir ve LED panelde uyarı mesajı yayınlanır.

---

## 📊 4. Yazdırma, Raporlama ve Dosya Özellikleri

*   **📁 Dosyadan Resim Seç ve Oku (`btnDosyadanOku`):** Kameradan bağımsız olarak bilgisayardaki herhangi bir plaka resmini (`.jpg`, `.png`, `.bmp`) seçip anında test etmeyi sağlayan buton.
*   **🖨️ Termal Tartım Fişi:** Seçilen kaydın yazıcı çıktısını veya önizlemesini alma (`PrintDocument`).
*   **📊 Excel / CSV İhracatı:** Tüm veritabanı geçmişini tek tıkla Excel uyumlu `.csv` formatında kaydetme.
*   **🎨 Modern Arayüz (Dark UI):** Modern ve ergonomik operatör kontrol ekranı.

---

## 🚀 5. Projeyi Çalıştırma Adımları

Proje tak-çalıştır mimaridedir. Çalıştırmak için şu adımları izleyin:

1. **Projeyi Açın:**  
   `Kantar-Plaka Arge Projesi.slnx` (veya `Kantar-Plaka Arge Projesi.csproj`) dosyasını **Visual Studio** ile açın.

2. **Derleme Platformunu `x64` Yapın:**  
   Visual Studio üst araç çubuğundaki Platform seçeneğini **`x64`** olarak ayarlayın. (Hikvision SDK kütüphaneleri 64-bit çalıştığı için `x64` seçimi önerilir).

3. **Projeyi Başlatın:**  
   Klavyeden **`F5`** tuşuna basın veya **`Başlat (Start)`** butonuna tıklayın.

4. **Giriş Ekranında Seçim Yapın:**  
   * **Kamera İle Test:** Kameranız takılıysa IP ve yetkili kullanıcı adı / parola bilgileriyle *"SİSTEME GİRİŞ YAP & KAMERAYA BAĞLAN"* butonuna basın.
   * **Kamerasız / Resim İle Test:** *"Çevrimdışı Modda Başlat"* butonuna tıklayarak doğrudan kantar ekranına geçin. Açılan ekranda mavi **"📁 Dosyadan Resim Seç ve Oku"** butonuna basarak bilgisayarınızdaki herhangi bir plaka resmini seçip yapay zeka okumasını test edin.

---
*Kantar & Plaka Otomasyon Sistemi © 2026*
