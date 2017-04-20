using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_Arbol_B
{
    public class BNode<TLlave, T> where TLlave : IComparable<TLlave> where T : IComparable<T>
    { 
        int posicion;
        int padre;
        int grado;
        List<string> hijos;
        List<string> llaves;
        List<string> datos;

        public int Grado
        {
            get
            {
                return grado;
            }

            set
            {
                grado = value;
            }
        }

        public int Posicion
        {
            get
            {
                return posicion;
            }

            set
            {
                posicion = value;
            }
        }

        public int Padre
        {
            get
            {
                return padre;
            }

            set
            {
                padre = value;
            }
        }

        public List<string> Hijos
        {
            get
            {
                return hijos;
            }

            set
            {
                hijos = value;
            }
        }

        public List<string> Llaves
        {
            get
            {
                return llaves;
            }

            set
            {
                llaves = value;
            }
        }

        public List<string> Datos
        {
            get
            {
                return datos;
            }

            set
            {
                datos = value;
            }
        }

        public BNode(int Grado, string[] informacion)
        {
            int i = 0;
            this.grado = Grado;
            //Posicion del nodo
            Posicion = int.Parse(informacion[0]);
            //Padre del nodo
            Padre = int.Parse(informacion[1]);

            this.hijos = new List<string>();
            this.llaves = new List<string>();
            this.datos = new List<string>();

            //Almacenar hijos del nodo
            int recorrido = 4;
            for(i = 0; i < grado; i++)
            {
                hijos.Add(informacion[recorrido]);
                recorrido++;
            }

            //Almacenar llaves y datos del nodo
            recorrido = grado + 6;
            for(i = 0; i < grado -1; i++)
            {
                llaves.Add(informacion[recorrido]);
                datos.Add(informacion[recorrido + grado + 1]);
                recorrido++;
            }
        }

        public string[] Informacion()
        {
            int i = 0;
            List<string> nodo = new List<string>();

            //Agregamos posicion 
            nodo.Add(posicion.ToString("D11"));

            //Agregamos padre            
            if(padre == int.MinValue)
            {
                nodo.Add(padre.ToString());
            }
            else
            {
                nodo.Add(padre.ToString("D11"));
            }

            //Son necesarios para identificar los separadores
            nodo.Add("");
            nodo.Add("");

            //Almacenar hijos del nodo
            for (i = 0; i < Hijos.Count; i++)
            {
                nodo.Add(Hijos[i]);
            }

            //Son necesarios para identificar los separadores
            nodo.Add("");
            nodo.Add("");

            //Almacenar las llaves del nodo
            for (i = 0; i < Llaves.Count; i++)
            {
                nodo.Add(Llaves[i]);
            }

            //Son necesarios para identificar los separadores
            nodo.Add("");
            nodo.Add("");

            //Almacenar la data del nodo
            for (i = 0; i < Datos.Count; i++)
            {
                nodo.Add(Datos[i]);
            }

            return nodo.ToArray();
        }

    }
}
