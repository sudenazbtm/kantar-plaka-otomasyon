using System;
using System.IO;

namespace Kantar_Plaka_Arge_Projesi
{
    public static class CustomAiModel
    {
        public static string ModelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models", "plaka_model.onnx");

        public static string OkuPlakaKendiModelimiz(string resimYolu)
        {
            try
            {
                if (!File.Exists(ModelPath))
                {
                    return null;
                }

                // İleride eğitilecek YOLO/ONNX modeli buraya bağlanır
            }
            catch
            {
            }
            return null;
        }
    }
}
