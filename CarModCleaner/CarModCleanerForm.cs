using System.IO;
using System.Windows.Forms;

namespace CarModCleaner
{
    public partial class CarModCleanerForm : Form
    {
        public CarModCleanerForm()
        {
            string carsPath = @"D:\Program Files (x86)\Assetto Corsa\content\cars\";

            foreach (var cars in System.IO.Directory.GetDirectories(carsPath))
            {
                var areoFilePath = Path.Combine(cars, "data\\aero.ini");
                if (File.Exists(areoFilePath))
                {
                    var parser = new IniParser.FileIniDataParser();
                    parser.Parser.Configuration.AssigmentSpacer = "";
                    parser.Parser.Configuration.CommentString = ";";
                    parser.Parser.Configuration.CommentRegex = new System.Text.RegularExpressions.Regex(";.*");

                    var fileContet = parser.ReadFile(areoFilePath);
                    if (fileContet.Sections.ContainsSection("DATA"))
                    {
                        fileContet.Sections.RemoveSection("DATA");

                        System.IO.File.Copy(areoFilePath, areoFilePath + ".bak", true);

                        parser.WriteFile(areoFilePath + "", fileContet, new System.Text.UTF8Encoding(false));
                    }
                }
            }
        }
    }
}