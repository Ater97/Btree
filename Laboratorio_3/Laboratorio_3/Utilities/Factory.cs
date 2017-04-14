using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio_3.Utilities
{
    class Factory
    {
        int Degree;
        string path;

        public bool createFile(int degre)
        {
            path = @"C:\Users\sebas\Desktop\BTree tests";
            Directory.CreateDirectory(path);
            string fileName = "Example_degre" + degre.ToString() + ".txt";
            path = Path.Combine(path, fileName);

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }
                Degree = degre;
                File.WriteAllText(path, "");
                return true;
            }
            return false;
        }

        public bool SetHeader<P>(P root, P lastFree, int size, int height)
        {
            string[] lines = File.ReadAllLines(path);
            File.WriteAllText(path, "");
            int n = 5;
            using (StreamWriter file = new StreamWriter(path, true))
            {
                file.WriteLine(VerifyLenght(root));
                file.WriteLine(VerifyLenght(lastFree));
                file.WriteLine(VerifyLenght(Degree));
                file.WriteLine(VerifyLenght(size));
                file.WriteLine(VerifyLenght(height));
                if (lines.Length < 5)
                    n = 0;
                for (int i = n; i < lines.Length; i++)
                {
                    file.WriteLine(lines[i]);
                }
            }
            return true;
        }
        #region Verify
        public string VerifyLenght<P>( P pointer)
        {
            if (pointer.ToString() != "0")
            {


                double size = Math.Floor(Math.Log10(int.Parse(pointer.ToString())) + 1);
                string asn = pointer.ToString();
                if (size < 12)
                {
                    for (int i = 0; i < 12 - size; i++)
                    {
                        asn = "0" + asn;
                    }
                    return asn;
                }
                else
                {
                    return asn;
                }
            }
            return "000000000000";
        }

        public string VerifyLenght(int number)
        {
            if (number == 0)
                number = 1;
            double size = Math.Floor(Math.Log10(int.Parse(number.ToString())) + 1);
            string asn = number.ToString();
            if (size < 12)
            {
                for (int i = 0; i < 12 - size; i++)
                {
                    asn = "0" + asn;
                }
                return asn;
            }
            else
            {
                return asn;
            }
        }
        #endregion
        public bool SetNodes<T, P>(P instantP, P fatherP, List<BNode<T, P>> children, List<Entry<T, P>> entries)
        {
            string[] lines = File.ReadAllLines(path);
            File.WriteAllText(path, "");
            using (StreamWriter file = new StreamWriter(path, true))
            {
                if (lines.Length != 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        file.WriteLine(lines[i]);
                    }

                    for (int i = 5; i < lines.Length; i++)
                    {
                        //if (!lines[i].Contains(VerifyLenght(instantP) + "|" + VerifyLenght(fatherP)))
                            file.WriteLine(lines[i]);
                       // else
                          //  file.WriteLine(strgetNode(instantP, fatherP, children, entries));
                    }
                    file.WriteLine(strgetNode(instantP, fatherP, children, entries));
                }
                else
                    file.WriteLine(strgetNode(instantP, fatherP, children, entries));

            }
            return true;
        }
        public string strgetNode<T, P>(P instantP, P fatherP, List<BNode<T, P>> children, List<Entry<T, P>> entries)
        {
            string strChildren = "";
            string strEntries = "";
            for (int i = 0; i < children.Count; i++)
            {
                strChildren = strChildren + "|" + children.First().Entries.First().Pointer;
                children.Remove(children.First());
            }
            for (int i = 0; i < entries.Count; i++)
            {
                strEntries = strEntries + "|" + entries.First().Key;
                entries.Remove(entries.First());
            }
            return (VerifyLenght(instantP) + "|" + VerifyLenght(fatherP)+ "||" + strChildren + "||" + strEntries);
        }

    }
}
