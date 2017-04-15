using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Prueba_Arbol_B
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Guid> registros = new List<Guid>();
            BTree<string, Guid> arbol = null;

            Console.WriteLine("Se empezo la inserción.");

            int orden = 10;
            Guid guid;

            while (orden < 11)
            {
                arbol = new BTree<string, Guid>("ArbolB-" + orden.ToString() + ".txt", orden);

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
            sw.Stop();
            TimeSpan elapsedTime = sw.Elapsed;
            Console.WriteLine("Time" + elapsedTime);

            Console.ReadKey();

            Console.WriteLine("Inicia busqueda");

            Console.WriteLine("Dato Buscado: " + registros[0].ToString());
            Console.WriteLine("¿Encontrado? {0} ", arbol.Buscar(registros[0].ToString(), registros[0]));         
            Console.WriteLine("Se ha terminado la inserción.");

            Console.ReadKey();
        }
    }
}
