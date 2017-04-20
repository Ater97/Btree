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

                for (int i = 0; i < 1000; i++)
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
               
                for (int i = 0; i < registros.Count(); i++)
                {
                    sw.Start();
                    Console.WriteLine("Dato Buscado: " + registros[i].ToString());
                    Console.WriteLine("¿Encontrado? {0} ", arbol.Buscar(registros[i].ToString(), registros[i]));
                    sw.Stop();
                    elapsedTime = +sw.Elapsed;
                    sw.Reset();
                }
                sw.Stop();
                
                string Time = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds,
                elapsedTime.Milliseconds / registros.Count());
              //  sw.Reset();
                Console.WriteLine("Time " + Time);

                Console.WriteLine("\nInicio Eliminacion");
                for (int i = 0; i < registros.Count(); i++)
                {
                    sw.Start();
                    if (arbol.Eliminar(registros[i].ToString()))
                    {
                        Console.WriteLine(registros[i].ToString() + " Eliminado");
                    }
                    else
                    {
                        Console.WriteLine(registros[i].ToString() + " Error");
                    }
                    sw.Stop();
                    elapsedTime = +sw.Elapsed;
                    sw.Reset();
                }
                Console.WriteLine("Se ha terminado la eliminacion.");
                Time = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                              elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds,
                              elapsedTime.Milliseconds / registros.Count());
                sw.Reset();
                Console.WriteLine("Time " + Time + "\n");
                registros = new List<Guid>();
            }

          
            Console.ReadKey();
        }
    }
}
