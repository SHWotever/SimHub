using ACToolsUtilities.Serialisation;
using IniParser;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACSharedMemory.Models.Car
{
    [ImplementPropertyChanged]
    public class CarDesc
    {
        public bool IsMissing { get; set; }

        public CarInfo CarInfo { get; set; }

        private string carPath;

        public string CarPath
        {
            get { return carPath; }
        }

        public string Model
        {
            get;
            set;
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

            if (!ACToolsUtilities.FileOP.Exists(raceIniPath))
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
            if (ACToolsUtilities.FileOP.Exists(path))
            {
                return System.IO.File.ReadAllBytes(path);
            }
            return null;
        }

        public string CarBadgePath
        {
            get
            {
                if (carPath == null) { return null; }
                string path = System.IO.Path.Combine(carPath, "ui", "badge.png");
                if (ACToolsUtilities.FileOP.Exists(path))
                {
                    return path;
                }
                return null;
            }
        }

        public IEnumerable<Skin> Skins
        {
            get
            {
                return GetSkins();
            }
        }

        private Skin _CurrentSkin = null;

        public Skin CurrentSkin
        {
            get
            {
                if (_CurrentSkin == null)
                {
                    _CurrentSkin = Skins.FirstOrDefault();
                }
                return _CurrentSkin;
            }
            set
            {
                _CurrentSkin = value;
            }
        }

        public IEnumerable<Skin> GetSkins()
        {
            //get
            {
                if (carPath != null)
                {
                    string skinspath = System.IO.Path.Combine(carPath, "skins");

                    if (Directory.Exists(skinspath))
                    {
                        foreach (var skin in Directory.EnumerateDirectories(skinspath))
                        {
                            string path = System.IO.Path.Combine(skin, "preview.jpg");
                            if (ACToolsUtilities.FileOP.Exists(path))
                            {
                                Skin result;
                                try
                                {
                                    var skinobject = JsonExtensions.FromJsonFile<Skin>(System.IO.Path.Combine(skin, "ui_skin.json"), a =>
                                    {
                                        string[] lines = a.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                                        for (int i = 0; i < lines.Length; i++)
                                        {
                                            if (lines[i].ToLower().Contains("number"))
                                            {
                                                lines[i] = "";
                                                return string.Join("\r", lines);
                                            }
                                        }
                                        return a;
                                    });
                                    skinobject = skinobject ?? new Skin();
                                    skinobject.Name = Path.GetFileName(skin);
                                    skinobject.PreviewImage = path;
                                    result = skinobject;
                                }
                                catch
                                {
                                    result = new Skin { Name = Path.GetFileName(skin), PreviewImage = path };
                                }
                                yield return result;
                            }
                        }
                    }
                }
            }
        }

        public string CarPreviewPath
        {
            get
            {
                if (carPath == null) { return null; }
                string skinspath = System.IO.Path.Combine(carPath, "skins");

                if (Directory.Exists(skinspath))
                {
                    foreach (var skin in Directory.EnumerateDirectories(skinspath))
                    {
                        string path = System.IO.Path.Combine(skin, "preview.jpg");
                        if (ACToolsUtilities.FileOP.Exists(path))
                        {
                            return path;
                        }
                    }
                }

                string[] filenames = { "preview_small.png", "preview.png", "preview.jpg", "preview_small.jpg" };

                foreach (var filename in filenames)
                {
                    string path = System.IO.Path.Combine(carPath, "ui", filename);
                    if (ACToolsUtilities.FileOP.Exists(path))
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
                            if (ACToolsUtilities.FileOP.Exists(path))
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
            if (ACToolsUtilities.FileOP.Exists(path))
            {
                return System.IO.File.ReadAllBytes(path);
            }
            return null;
        }
    }
}