using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Prueba_Arbol_B
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Guid> registros = new List<Guid>();
            BTree<string, Guid> arbol = null;
            BTree<string, Guid> otro = null;

            Console.WriteLine("Se empezo la inserción.");

            int orden = 10;
            Guid guid;

            while (orden < 11)
            {
                arbol = new BTree<string, Guid>("ArbolB-" + orden.ToString() + ".btree", orden);

                for (int i = 0; i < 10000; i++)
                {
                    guid = Guid.NewGuid();
                    arbol.Insertar(guid.ToString(), guid);

                    if (i == 2500)
                    {
                        registros.Add(guid);
                    }
                }

                Console.WriteLine("Se ha terminado la inserción del árbol de orden " + orden.ToString());
                orden++;
            }


            Console.ReadKey();

            Console.WriteLine("Inicia busqueda");

            Console.WriteLine("Dato Buscado: " + registros[0].ToString());
            Console.WriteLine("¿Encontrado? {0} ", arbol.Buscar(registros[0].ToString(), registros[0]));         
            Console.WriteLine("Se ha terminado la inserción.");

            //prueba.Insertar(56, 56);
            //prueba.Insertar(25, 25);
            //prueba.Insertar(20, 20);
            //prueba.Insertar(105, 105);
            //prueba.Insertar(19, 19);
            //prueba.Insertar(8, 8);
            //prueba.Insertar(75, 75);
            //prueba.Insertar(110, 110);
            //prueba.Insertar(200, 200);
            //prueba.Insertar(30, 30);
            //prueba.Insertar(70, 70);
            //prueba.Insertar(10, 10);
            //prueba.Insertar(4, 4);
            //prueba.Insertar(5, 5);
            //prueba.Insertar(250, 250);
            //prueba.Insertar(225, 225);
            //prueba.Insertar(300, 300);
            //prueba.Insertar(275, 275);
            //prueba.Insertar(310, 310);
            //prueba.Insertar(350, 350);
            //prueba.Insertar(400, 400);
            //prueba.Insertar(415, 415);
            //prueba.Insertar(450, 450);
            //prueba.Insertar(320, 320);
            //prueba.Insertar(325, 325);
            //prueba.Insertar(500, 500);
            //prueba.Insertar(480, 480);
            //prueba.Insertar(460, 460);
            //prueba.Insertar(420, 420);
            //prueba.Insertar(417, 417);

            Console.ReadKey();
        }
    }
}
