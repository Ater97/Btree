using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Prueba_Arbol_B
{
    public class Fabrica<TLlave, T> where TLlave : IComparable<TLlave> where T : IComparable<T>
    {
        //Generar mi archivo 

        private string nombreArchivo;
        private string path;
        private int grado;
        private int altura;
        private int tamaño;
        private int posicionLibre;
        private string dataNull;
        string direccion;

        //Variables necesarias para abrir el archivo...

        FileStream stream;
        StreamReader reader;
        StreamWriter writer;



        public void AbrirArchivo()
        {
            stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
        }

        public void crearFolder()
        {
            direccion = @"C:\Users\bryan\Desktop\BTree tests\";
           // direccion = @"Archivo\";
            Directory.CreateDirectory(direccion);
            direccion = Path.Combine(direccion, nombreArchivo);
        }

        public Fabrica(string NombreArchivo, int Grado)
        {
            nombreArchivo = NombreArchivo;
            crearFolder();
            grado = Grado;
            altura = 0;
            tamaño = 0;
            posicionLibre = 0;
            path = direccion;
            AbrirArchivo();
            GenerarArbol();
        }

        public Fabrica(string NombreArchivo)
        {
            nombreArchivo = NombreArchivo;
            path = direccion;
            dataNull = "####################################"; 
            CargarEncabezado();
        }

        public void CargarEncabezado()
        {
            reader.BaseStream.Seek(13, SeekOrigin.Begin);
            posicionLibre = int.Parse(reader.ReadLine());
            reader.DiscardBufferedData();

            reader.BaseStream.Seek(26, SeekOrigin.Begin);
            tamaño = int.Parse(reader.ReadLine());
            reader.DiscardBufferedData();

            reader.BaseStream.Seek(39, SeekOrigin.Begin);
            grado = int.Parse(reader.ReadLine());
            reader.DiscardBufferedData();

            reader.BaseStream.Seek(52, SeekOrigin.Begin);
            altura = int.Parse(reader.ReadLine());
            reader.DiscardBufferedData();
        }

        //Genera el archivo que contendra al árbol
        public void GenerarArbol()
        {
            if (File.Exists(path))
            {
                //StreamWriter writer = File.CreateText(Path.GetFullPath(path));
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                writer.WriteLine(int.MinValue.ToString()); // Raiz
                writer.WriteLine(posicionLibre.ToString("D11")); // Posición libre
                writer.WriteLine(tamaño.ToString("D11")); // Tamaño
                writer.WriteLine(grado.ToString("D11")); // Orden 
                writer.WriteLine(altura.ToString("D11")); // Altura
                writer.Flush();
            }
        }

        public void NodoDeFabrica()
        {
            string nuevoNodo = string.Empty;
            nuevoNodo += posicionLibre.ToString("D11");
            nuevoNodo += "|" + int.MinValue.ToString();

            //Hijos
            nuevoNodo += "|||";

            for (int i = 0; i < grado; i++)
            {
                nuevoNodo += int.MinValue.ToString() + "|";
            }

            //Llaves del nodo
            nuevoNodo += "||";
            dataNull = "####################################";
            for (int i = 0; i < grado - 1; i++)
            {
                nuevoNodo += dataNull + "|";
            }

            //Contenido nulo del nodo

            nuevoNodo += "||";         
            for (int i = 0; i < grado - 1; i++)
            {
                nuevoNodo += dataNull + "|";
            }

            //Se almacena el nodo en el archivo
            writer.BaseStream.Seek(PosicionEnArchivo(posicionLibre), SeekOrigin.Begin);
            writer.WriteLine(nuevoNodo);
            writer.Flush();

            //Se cambia la posicion libre
            posicionLibre++;

            int j = nuevoNodo.Length;

            writer.BaseStream.Seek(13, SeekOrigin.Begin);
            writer.Write(posicionLibre.ToString("D11"));
            writer.Flush();
        }

        public BNode<TLlave, T> TraerNodo(int NodoActual)
        {
            if(NodoActual == int.MinValue)
            {
                return null;
            }
            else
            {
                reader.BaseStream.Seek(PosicionEnArchivo(NodoActual), SeekOrigin.Begin);
                string linea = reader.ReadLine();
                linea = linea.Remove(linea.Length - 1, 1);
                string[] componentes = linea.Split(new char[] { '|', '|', '|' });
                reader.DiscardBufferedData();
                BNode<TLlave, T> nodo = new BNode<TLlave, T>(grado, componentes);
                return nodo;
            }
                            
        }

        public void GuardarNodo(string[] nodo)
        {
            string guardar = string.Empty;
            
            for(int i = 0; i < nodo.Length; i++)
            {
                if(nodo[i] == "")
                {
                    guardar += "|";
                }
                else
                {
                    guardar += nodo[i] + "|";
                }             
            }
            int j = guardar.Length;
            int posicion = PosicionEnArchivo(int.Parse(nodo[0]));

            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            writer.BaseStream.Seek(posicion, SeekOrigin.Begin);
            writer.Write(guardar);
            writer.Flush();
        }

        private int PosicionEnArchivo(int NodoBuscado)
        {
            //Ignoramos el encabezado
            int posicion = 65;

            for (int i = 0; i < NodoBuscado; i++)
            {
                //Posicion y padre
                posicion += int.MinValue.ToString().Length * 2 + 1;
                posicion += 9;
                //Hijos
                posicion += int.MinValue.ToString().Length * grado;
                posicion += grado - 1;
                //LLaves
                posicion += dataNull.Length * (grado - 1);
                //Datos
                posicion += dataNull.Length * (grado - 1);
                //Simbolos |
                posicion += (grado - 2) * 2;
                // /n
                posicion += 3;
            }

            return posicion;
        } 

        public bool Empty()
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            string raiz = reader.ReadLine();
            reader.DiscardBufferedData();

            return raiz == int.MinValue.ToString();
        }

        public int ObtenerRaiz()
        {
            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //stream.Seek(0, SeekOrigin.Begin);
            //StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            string raiz = reader.ReadLine();
            reader.DiscardBufferedData();

            return int.Parse(raiz);
        }

        public int ObtenerPosicionLibre()
        {
            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //stream.Seek(13, SeekOrigin.Begin);
            //StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(13, SeekOrigin.Begin);
            string posicion = reader.ReadLine();
            reader.DiscardBufferedData();

            return int.Parse(posicion);
        }

        public void CambiarRaiz(int nuevaRaiz)
        {
            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            //stream.Seek(0, SeekOrigin.Begin);
            //StreamWriter writer = new StreamWriter(stream);
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write(nuevaRaiz.ToString("D11"));
            writer.Flush();
        }

        public void CambiarAltura(int agregar)
        {
            int posicion = 52;
            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            //stream.Seek(, SeekOrigin.Begin);
            //StreamWriter writer = new StreamWriter(stream);
            writer.BaseStream.Seek(posicion, SeekOrigin.Begin);
            writer.Write(agregar.ToString("D11"));
            writer.Flush();
        }

        public int ObtenerAltura()
        {
            int posicion = 52;
            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //stream.Seek(posicion, SeekOrigin.Begin);
            //StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(posicion, SeekOrigin.Begin);
            int altura = int.Parse(reader.ReadLine());
            reader.DiscardBufferedData();

            return altura;
        }

        public void CambiarTamaño(int agregar)
        {
            int posicion = 26;
            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            //stream.Seek(posicion, SeekOrigin.Begin);
            //StreamWriter writer = new StreamWriter(stream);
            writer.BaseStream.Seek(posicion, SeekOrigin.Begin);
            writer.Write(agregar.ToString("D11"));
            writer.Flush();
        }

        public int ObtenerTamaño()
        {
            int posicion = 26;
            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //stream.Seek(posicion, SeekOrigin.Begin);
            //StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(posicion, SeekOrigin.Begin);
            int tamanio = int.Parse(reader.ReadLine());
            reader.DiscardBufferedData();

            return tamanio;
        }

        public int ObtenerGrado()
        {
            int posicion = 39;
            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //stream.Seek(posicion, SeekOrigin.Begin);
            //StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(posicion, SeekOrigin.Begin);
            int gradoArbol = int.Parse(reader.ReadLine());
            reader.DiscardBufferedData();

            return gradoArbol;
        }
    }
}