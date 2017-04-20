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
            List<string> TimesI = new List<string>();
            List<string> TimesB = new List<string>();
            List<string> TimesE = new List<string>();
            bool flag = true;
            int gradoInicial = 5;
            int gradoFinal = 13;
            while (flag)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Ingrese el grado con el que desea iniciar la prueba:");
                    gradoInicial = int.Parse(Console.ReadLine());
                    Console.WriteLine("Ingrese el grado con el que desea terminar la prueba:");
                    gradoFinal = int.Parse(Console.ReadLine());
                    flag = false;
                }
                catch
                {
                    flag = true;
                }
            }
            for (int j = gradoInicial; j <= gradoFinal; j++)
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
                TimesI.Add(elapsedTime.ToString());

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
                TimesB.Add(Time);
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
                TimesE.Add(Time);
                Console.WriteLine("Time " + Time + "\n");
                registros = new List<Guid>();
            }

            Console.WriteLine("Ingrese el nombre del archivo CSV que desea: ");
            string FileName = Console.ReadLine();
            CrearCvs(TimesI, TimesB, TimesE, FileName, gradoInicial);
            Console.ReadKey();
        }

        private static void CrearCvs(List<string> timeI, List<string> timeb, List<string> timeE, string fileName, int gradoA)
        {
            int grado = gradoA;
            String newLine = "";
            //var varstream = File.CreateText(@"C:\Users\sebas\Desktop\BTree tests\" + fileName + ".csv");
            var varstream = File.CreateText(@"C:\Users\sebas\Desktop\" + fileName + ".csv");
            for (int i = 0; i < timeI.Count(); i++)
            {
                newLine = "Arbol grado" + grado +  ",Tiempo Incercion: " + timeI[i] + ",Timpo Busqueda :" + timeb[i] + ",Timpo Eliminacion :" + timeE[i];
                varstream.WriteLine(newLine);
                grado++;
            }
            varstream.Close();
        }
    }
}
