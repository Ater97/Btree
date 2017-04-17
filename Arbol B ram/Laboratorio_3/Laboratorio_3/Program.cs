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
        //𐤁𐤀𐤋  
       
        static void Main(string[] args)
        {
            //for (int j = 3; j < 11; j++)
            //{
            //    BTree<Guid, int> tree = new BTree<Guid, int>(j);
            //    //1000000
            //    for (int i = 0; i < 10; i++)
            //    {
            //        tree.Insert(Guid.NewGuid(), i);
            //    }
            //}
            Guid guid = Guid.NewGuid();
            BTree<Guid, Guid> tree = new BTree<Guid, Guid>(3);
            for (int i = 0; i < 1000; i++)
            {
                Guid nguid = Guid.NewGuid();
                if (i==100)
                {
                    guid = nguid;
                }
                tree.Insert(nguid, nguid);
            }
            tree.Delete(guid);
        Console.WriteLine("good.");
            Console.ReadLine();

        }
    }
}
