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
            TimeSpan elapsedTime;
            Console.WriteLine("Se empezo la inserción.");

            for (int j = 5; j< 13; j++)
            {               
                orden = j;

                Console.WriteLine("\n   Insertando Arbol de orden {0}", orden);

                sw.Start();
                arbol = new BTree<string, Guid>("ArbolB-" + orden.ToString() + ".btree", orden);

                for (int i = 0; i < 100000; i++)
                {
                    guid = Guid.NewGuid();
                    arbol.Insertar(guid.ToString(), guid);

                    if ((i % 1000) == 0)
                    {
                        registros.Add(guid);
                    }
                }

                Console.WriteLine("     Se ha terminado la inserción del árbol de orden {0} ", orden);
                sw.Stop();
                elapsedTime = sw.Elapsed;
                sw.Reset();
                Console.WriteLine("Time: " + elapsedTime);

                Console.WriteLine("Inicia busqueda");
                sw.Start();
                for (int i = 0; i < registros.Count(); i++)
                {
                    Console.WriteLine("Dato Buscado: " + registros[i].ToString());
                    Console.WriteLine("¿Encontrado? {0} ", arbol.Buscar(registros[i].ToString(), registros[i]));
                }
                sw.Stop();
                elapsedTime = sw.Elapsed;
                sw.Reset();
                Console.WriteLine("Time " + elapsedTime);

                Console.WriteLine("\nInicio Eliminacion");
                sw.Start();
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (arbol.Eliminar(registros[i].ToString()))
                    {
                        Console.WriteLine(registros[i].ToString() + " Eliminado");
                    }
                    else
                    {
                        Console.WriteLine(registros[i].ToString() + " Error");
                    }
                    
                }
                Console.WriteLine("Se ha terminado la eliminacion.");
                sw.Stop();
                elapsedTime = sw.Elapsed;
                sw.Reset();
                Console.WriteLine("Time " + elapsedTime + "\n");
                

            }

          
            Console.ReadKey();
        }
    }
}
