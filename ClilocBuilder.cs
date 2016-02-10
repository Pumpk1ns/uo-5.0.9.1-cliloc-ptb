using System;
using System.IO;
using System.Text;

namespace ClilocBuilder
{
    class MainClass
    {
        public static void Main()
        {
            var fi = new FileInfo(string.Concat("Cliloc.ptb"));
            FileStream fs;

            try
            {
                fs = new FileStream(fi.FullName, FileMode.Create);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            var bw = new BinaryWriter(fs);
            bw.Write(new byte[] { 2, 0, 0, 0, 1, 0 });

            using (var sr = new StreamReader("CliLoc.csv"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#") || line.StartsWith("Number;"))
                        continue;
                    
                    try
                    {
                        string[] split = line.Split(';');

                        if (split.Length < 2)
                            continue;

                        int number = int.Parse(split[0].Trim());
                        string text = split[1].Trim();
                        byte[] textBytes = Encoding.UTF8.GetBytes(text);

                        bw.Write(Convert.ToUInt32(number));
                        bw.Write((byte)0);
                        bw.Write(Convert.ToUInt16(textBytes.Length));
                        bw.Write(textBytes);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            fs.Close();
            bw.Close();
        }
    }
}
