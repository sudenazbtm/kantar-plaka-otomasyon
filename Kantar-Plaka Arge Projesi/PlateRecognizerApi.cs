using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Kantar_Plaka_Arge_Projesi
{
    public static class PlateRecognizerApi
    {
        public static string ApiToken = ""; // Plate Recognizer API Token (Kendi API anahtarınızı giriniz)
        public static string ApiUrl = "https://api.platerecognizer.com/v1/plate-reader/";

        public static string OkuPlakaApi(string resimYolu)
        {
            try
            {
                if (string.IsNullOrEmpty(ApiToken))
                {
                    return null;
                }

                if (!File.Exists(resimYolu)) return null;

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", ApiToken);

                    using (var content = new MultipartFormDataContent())
                    {
                        byte[] fileBytes = File.ReadAllBytes(resimYolu);
                        var byteContent = new ByteArrayContent(fileBytes);
                        byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        content.Add(byteContent, "upload", Path.GetFileName(resimYolu));
                        content.Add(new StringContent("tr"), "regions");

                        var response = client.PostAsync(ApiUrl, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = response.Content.ReadAsStringAsync().Result;
                            
                            Match m = Regex.Match(jsonResponse, @"(?:""plate""\s*:\s*"")([^""]+)""");
                            if (m.Success)
                            {
                                string plaka = m.Groups[1].Value.Trim().ToUpper();
                                return plaka;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return null;
        }
    }
}
