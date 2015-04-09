using System.IO;

namespace ACSharedMemory.Models.Car
{
    public class CarDesc
    {
        public CarInfo CarInfo { get; set; }

        private string carPath;

        public string CarPath
        {
            get { return carPath; }
        }

        public string Model { get; private set; }

        public static CarDesc FromModel(string ACPAth, string model)
        {
            CarDesc result = new CarDesc();
            result.carPath = System.IO.Path.Combine(ACPAth, "content", "cars", model);
            result.CarInfo = CarInfo.FromModel(ACPAth, model);
            result.Model = model;
            return result;
        }

        public byte[] GetCarBadge()
        {
            string path = System.IO.Path.Combine(carPath, "ui", "badge.png");
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllBytes(path);
            }
            return null;
        }

        public byte[] GetCarPreview()
        {
            string[] filenames = { "preview_small.png", "preview.png", "preview.jpg", "preview_small.jpg" };

            foreach (var filename in filenames)
            {
                string path = System.IO.Path.Combine(carPath, "ui", filename);
                if (System.IO.File.Exists(path))
                {
                    return System.IO.File.ReadAllBytes(path);
                }
            }

            foreach (var skin in Directory.GetDirectories(System.IO.Path.Combine(carPath, "skins")))
            {
                foreach (var filename in filenames)
                {
                    var path = Path.Combine(skin, filename);
                    if (System.IO.File.Exists(path))
                    {
                        return System.IO.File.ReadAllBytes(path);
                    }
                }
            }

            return null;
        }
    }
}