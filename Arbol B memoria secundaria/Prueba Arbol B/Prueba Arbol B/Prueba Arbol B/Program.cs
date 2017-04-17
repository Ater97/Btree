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
            List<Guid> registros = new List<Guid>();
            BTree<string, Guid> arbol = null;
            Guid guid;
            int orden;
            Console.WriteLine("Se empezo la inserción.");
            for (int j = 3; j<6; j++)
            {
                sw.Start();
                orden = j;

                arbol = new BTree<string, Guid>("ArbolB-" + orden.ToString() + ".txt", orden);

                for (int i = 0; i < 100; i++)
                {
                    guid = Guid.NewGuid();
                    arbol.Insertar(guid.ToString(), guid);

                    if ((i % 50) == 0)
                    {
                        registros.Add(guid);
                    }
                }

                Console.WriteLine("Se ha terminado la inserción del árbol de orden " + orden.ToString());
                sw.Stop();
                TimeSpan elapsedTime = sw.Elapsed;
                sw.Reset();

                Console.WriteLine("Time" + elapsedTime);
                for (int i = 0; i < registros.Count(); i++)
                {
                    arbol.Eliminar(registros[i].ToString());
                }
                Console.WriteLine("Se ha terminado la eliminacion.");

                Console.WriteLine("Inicia busqueda");

                for (int i = 0; i < registros.Count(); i++)
                {
                    Console.WriteLine("Dato Buscado: " + registros[i].ToString());
                    Console.WriteLine("¿Encontrado? {0} ", arbol.Buscar(registros[i].ToString(), registros[i]));
                }
            }

          
            Console.ReadKey();
        }
    }
}
