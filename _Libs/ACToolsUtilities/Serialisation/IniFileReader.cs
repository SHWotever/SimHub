using IniParser;
using IniParser.Exceptions;
using IniParser.Model;
using System;
using System.IO;
using System.Text;

namespace ACToolsUtilities.Serialisation
{
    /// <summary>
    ///     Represents an INI data parser for files.
    /// </summary>
    public class IniFileReader : StreamIniDataParser
    {
        /// <summary>
        ///     Implements reading ini data from a file.
        /// </summary>
        /// <remarks>
        ///     Uses <see cref="Encoding.Default"/> codification for the file.
        /// </remarks>
        /// <param name="filePath">
        ///     Path to the file
        /// </param>
        public IniData ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.Default);
        }

        /// <summary>
        ///     Implements reading ini data from a file.
        /// </summary>
        /// <param name="filePath">
        ///     Path to the file
        /// </param>
        /// <param name="fileEncoding">
        ///     File's encoding.
        /// </param>
        public IniData ReadFile(string filePath, Encoding fileEncoding)
        {
            if (filePath == string.Empty)
                throw new ArgumentException("Bad filename.");

            try
            {
                // (FileAccess.Read) we want to open the ini only for reading
                // (FileShare.ReadWrite) any other process should still have access to the ini file
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs, fileEncoding))
                    {
                        return ReadData(sr);
                    }

                    fs.Close();
                }
            }
            catch (IOException ex)
            {
                throw new ParsingException(String.Format("Could not parse file {0}", filePath), ex);
            }
        }
    }
}