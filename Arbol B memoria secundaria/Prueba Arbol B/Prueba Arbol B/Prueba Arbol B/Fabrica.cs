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

        public void crearFolder()
        {
            // direccion = @"C:\Users\sebas\Desktop\BTree tests\";
            direccion = @"Archivo\";
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
            path = direccion + nombreArchivo;
            GenerarArbol();
        }

        public Fabrica(string NombreArchivo)
        {
            nombreArchivo = NombreArchivo;
            path = direccion + nombreArchivo;
            dataNull = "####################################"; 
            CargarEncabezado();
        }

        public void CargarEncabezado()
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            stream.Seek(13, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            posicionLibre = int.Parse(reader.ReadLine());

            stream.Seek(26, SeekOrigin.Begin);
            reader = new StreamReader(stream);
            tamaño = int.Parse(reader.ReadLine());

            stream.Seek(39, SeekOrigin.Begin);
            reader = new StreamReader(stream);
            grado = int.Parse(reader.ReadLine());

            stream.Seek(52, SeekOrigin.Begin);
            reader = new StreamReader(stream);
            altura = int.Parse(reader.ReadLine());

            stream.Close();
            reader.Close();
        }

        //Genera el archivo que contendra al árbol
        public void GenerarArbol()
        {
            if (!File.Exists(Path.GetFullPath(path)))
            {
                StreamWriter writer = File.CreateText(Path.GetFullPath(path));
                writer.WriteLine(int.MinValue.ToString()); // Raiz
                writer.WriteLine(posicionLibre.ToString("D11")); // Posición libre
                writer.WriteLine(tamaño.ToString("D11")); // Tamaño
                writer.WriteLine(grado.ToString("D11")); // Orden 
                writer.WriteLine(altura.ToString("D11")); // Altura
                writer.Close();
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
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(nuevoNodo);
            writer.Close();

            //Se cambia la posicion libre
            posicionLibre++;

            int j = nuevoNodo.Length;

            stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            stream.Seek(13, SeekOrigin.Begin);
            writer = new StreamWriter(stream);
            writer.Write(posicionLibre.ToString("D11"));
            writer.Close();
            stream.Close();
        }

        public BNode<TLlave, T> TraerNodo(int NodoActual)
        {

            if(NodoActual == int.MinValue)
            {
                return null;
            }
            else
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                stream.Seek(PosicionEnArchivo(NodoActual), SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream);
                string linea = reader.ReadLine();
                linea = linea.Remove(linea.Length - 1, 1);
                string[] componentes = linea.Split(new char[] { '|', '|', '|' });
                reader.Close();
                stream.Close();
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

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            stream.Seek(posicion, SeekOrigin.Begin);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(guardar);
            writer.Close();
            stream.Close();
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
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            string raiz = reader.ReadLine();
            reader.Close();
            stream.Close();

            return raiz == int.MinValue.ToString();
        }

        public int ObtenerRaiz()
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            string raiz = reader.ReadLine();
            reader.Close();
            stream.Close();

            return int.Parse(raiz);
        }

        public int ObtenerPosicionLibre()
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream.Seek(13, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            string posicion = reader.ReadLine();
            reader.Close();
            stream.Close();

            return int.Parse(posicion);
        }

        public void CambiarRaiz(int nuevaRaiz)
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(nuevaRaiz.ToString("D11"));
            writer.Close();
            stream.Close();
        }

        public void CambiarAltura(int agregar)
        {
            int posicion = 52;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            stream.Seek(posicion, SeekOrigin.Begin);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(agregar.ToString("D11"));
            writer.Close();
            stream.Close();
        }

        public int ObtenerAltura()
        {
            int posicion = 52;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream.Seek(posicion, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            int altura = int.Parse(reader.ReadLine());
            reader.Close();
            stream.Close();

            return altura;
        }

        public void CambiarTamaño(int agregar)
        {
            int posicion = 26;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Write);
            stream.Seek(posicion, SeekOrigin.Begin);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(agregar.ToString("D11"));
            writer.Close();
            stream.Close();
        }

        public int ObtenerTamaño()
        {
            int posicion = 26;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream.Seek(posicion, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            int tamanio = int.Parse(reader.ReadLine());
            reader.Close();
            stream.Close();

            return tamanio;
        }

        public int ObtenerGrado()
        {
            int posicion = 39;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream.Seek(posicion, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            int gradoArbol = int.Parse(reader.ReadLine());
            reader.Close();
            stream.Close();

            return gradoArbol;
        }

    }
}
