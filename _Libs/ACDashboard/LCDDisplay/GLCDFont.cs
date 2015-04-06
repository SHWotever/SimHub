using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ACDashboard.LCDDisplay
{
    [Serializable]
    public class FONTSIZE
    {
        [XmlAttribute("WIDTH", DataType = "int")]
        public int WIDTH { get; set; }

        [XmlAttribute("HEIGHT", DataType = "int")]
        public int HEIGHT { get; set; }

        [XmlAttribute("PROPORTIONAL", DataType = "string")]
        public string PROPORTIONAL { get; set; }

        [XmlAttribute("FONTKIND", DataType = "string")]
        public string FONTKIND { get; set; }
    }

    [Serializable]
    public class RANGE
    {
        [XmlAttribute("FROM", DataType = "int")]
        public int FROM { get; set; }

        [XmlAttribute("TO", DataType = "int")]
        public int TO { get; set; }
    }

    [Serializable]
    public class CHAR
    {
        [XmlAttribute("CODE", DataType = "int")]
        public int CODE { get; set; }

        [XmlAttribute("PIXELS", DataType = "string")]
        public string PIXELS { get; set; }
    }

    //[Serializable]
    //public class CHAR
    //{
    //    [XmlArray("Skills")]
    //    [XmlArrayItem("Skill")]
    //}
    [Serializable, XmlRoot("FONT")]
    public class FONT
    {
        public string FONTNAME { get; set; }

        public FONTSIZE FONTSIZE { get; set; }

        public RANGE RANGE { get; set; }

        [XmlArray("CHARS")]
        [XmlArrayItem("CHAR")]
        public List<CHAR> CHARS { get; set; }
    }

    [Serializable, XmlRoot("FONT")]
    public class GLCDFont
    {
        public FONT font { get; set; }
    }
}