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
        string[] hijos;
        string[] llaves;
        string[] datos;

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

        public string[] Hijos
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

        public string[] Llaves
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

        public string[] Datos
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
            this.Grado = Grado;
            //Posicion del nodo
            Posicion = int.Parse(informacion[0]);
            //Padre del nodo
            Padre = int.Parse(informacion[1]);

            Hijos = new string[grado];
            Llaves = new string[grado - 1];
            Datos = new string[grado - 1];

            //Almacenar hijos del nodo
            int recorrido = 4;
            for(int i = 0; i < grado; i++)
            {
                Hijos[i] = informacion[recorrido];
                recorrido++;
            }

            //Almacenar llaves y datos del nodo
            recorrido = grado + 6;
            for(int x = 0; x < grado -1; x++)
            {
                Llaves[x] = informacion[recorrido];
                Datos[x] = informacion[recorrido + grado + 1];
                recorrido++;
            }

        }

        public string[] Informacion()
        {
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
            for (int i = 0; i < hijos.Length; i++)
            {
                nodo.Add(hijos[i]);
            }

            //Son necesarios para identificar los separadores
            nodo.Add("");
            nodo.Add("");

            //Almacenar las llaves del nodo
            for (int x = 0; x < llaves.Length; x++)
            {
                nodo.Add(llaves[x]);
            }

            //Son necesarios para identificar los separadores
            nodo.Add("");
            nodo.Add("");

            //Almacenar la data del nodo
            for (int y = 0; y < llaves.Length; y++)
            {
                nodo.Add(datos[y]);
            }

            return nodo.ToArray();
        }

    }
}
