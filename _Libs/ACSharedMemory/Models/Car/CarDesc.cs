using IniParser;
using System;
using System.Collections.Generic;
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

        public string Model
        {
            get;
            private set;
        }

        public static CarDesc FromModel(string ACPAth, string model)
        {
            CarDesc result = new CarDesc();
            result.carPath = System.IO.Path.Combine(ACPAth, "content", "cars", model);
            result.CarInfo = CarInfo.FromModel(ACPAth, model);
            result.Model = model;
            return result;
        }

        public static CarDesc GetFromGameSettings(string acpath)
        {
            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = "";

            var raceIniPath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Assetto Corsa", "cfg", "race.ini");

            if (!File.Exists(raceIniPath))
            {
                return null;
            }
            var data = parser.ReadFile(raceIniPath);

            string Model = data["RACE"]["MODEL"];

            return CarDesc.FromModel(acpath, Model);
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

        public IEnumerable<Skin> Skins
        {
            get
            {
                string skinspath = System.IO.Path.Combine(carPath, "skins");

                if (Directory.Exists(skinspath))
                {
                    foreach (var skin in Directory.GetDirectories(skinspath))
                    {
                        string path = System.IO.Path.Combine(skin, "preview.jpg");
                        if (System.IO.File.Exists(path))
                        {
                            yield return new Skin { Name = Path.GetFileName(skin), PreviewImage = path };
                        }
                    }
                }
            }
        }

        public string CarPreviewPath
        {
            get
            {
                string skinspath = System.IO.Path.Combine(carPath, "skins");
                if (Directory.Exists(skinspath))
                {
                    foreach (var skin in Directory.GetDirectories(skinspath))
                    {
                        string path = System.IO.Path.Combine(skin, "preview.jpg");
                        if (System.IO.File.Exists(path))
                        {
                            return path;
                        }
                    }
                }

                string[] filenames = { "preview_small.png", "preview.png", "preview.jpg", "preview_small.jpg" };

                foreach (var filename in filenames)
                {
                    string path = System.IO.Path.Combine(carPath, "ui", filename);
                    if (System.IO.File.Exists(path))
                    {
                        return path;
                    }
                }
                if (System.IO.Directory.Exists(System.IO.Path.Combine(carPath, "skins")))
                    foreach (var skin in Directory.GetDirectories(System.IO.Path.Combine(carPath, "skins")))
                    {
                        foreach (var filename in filenames)
                        {
                            var path = Path.Combine(skin, filename);
                            if (System.IO.File.Exists(path))
                            {
                                return path;
                            }
                        }
                    }

                return null;
            }
        }

        public byte[] GetCarPreview()
        {
            var path = CarPreviewPath;
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllBytes(path);
            }
            return null;
        }
    }
}