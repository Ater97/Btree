using Laboratorio_3.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese el grado");
            int degree = int.Parse(Console.ReadLine());
            BTree<int, Guid> tree = new BTree<int, Guid>(degree);

            for (int i = 0; i < 10; i++)
            {
                tree.Insert(i, Guid.NewGuid());
            }
            Console.ReadLine();

        }
    }
}
