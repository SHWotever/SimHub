using System.Collections.Generic;

namespace TimingClient.Plugins.OutputPlugins.Dash
{
    public class SerialDashPluginSettings
    {
        public bool ReverseScreen0 { get; set; }

        public bool ReverseScreen1 { get; set; }

        public bool ReverseScreen2 { get; set; }

        public bool ReverseScreen3 { get; set; }

        public Dictionary<string, Screen> Screens { get; set; }

        public List<string> GameIdleScreens { get; set; }

        public List<string> GameRunnningScreens { get; set; }
    }

    public class Screen
    {
        public string ScreenName { get; set; }

        public List<ScreenPart> Scren0Parts { get; set; }

        public List<ScreenPart> Scren1Parts { get; set; }

        public List<ScreenPart> Scren2Parts { get; set; }

        public List<ScreenPart> Scren3Parts { get; set; }
    }

    public class ScreenPart
    {
        public int FixedLength { get; set; }
        public bool RightAlign { get; set; }

        public string Text { get; set; }

        public string Expression { get; set; }

        public string FormatString { get; set; }

        public string Script { get; set; }

        public string Description
        {
            get
            {
                return (this.Expression == null ? "TEXT : \"" + this.Text : "EXPR : \"" + Expression) + "\"";
            }
        }
    }
}