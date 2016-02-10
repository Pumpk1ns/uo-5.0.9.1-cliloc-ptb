using System;
using System.IO;
using System.Text;

namespace ClilocToTsv
{
    class MainClass
    {
        public static void Main()
        {
            unsafe
            {
                var delimiter = "\t";

                var csv = new StringBuilder();
                csv.AppendLine(string.Concat("Number", delimiter, "Text"));

                var fi = new FileInfo("Cliloc.enu");

                FileStream fs;

                try
                {
                    fs = new FileStream(fi.FullName, FileMode.Open);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                var br = new BinaryReader(fs, Encoding.UTF8);
                br.BaseStream.Seek((long)6, SeekOrigin.Begin);

                while (br.BaseStream.Length != br.BaseStream.Position)
                {
                    uint number = br.ReadUInt32();
                    br.ReadByte();
                    uint length = br.ReadUInt16();
                    byte[] textBytes = new byte[length];
                    textBytes = br.ReadBytes(Convert.ToInt32(length));
                    var text = Encoding.UTF8.GetString(textBytes);

                    if (text.Contains(delimiter))
                        text = text.Replace(delimiter, " ");

                    csv.AppendLine(string.Concat(number.ToString(), delimiter, text));
                }

                fs.Close();
                br.Close();

                File.WriteAllText("Cliloc.enu.tsv", csv.ToString());
            }
        }
    }
}
