using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_todo_trapo.Clases
{
    class Articulos
    {
        private float precioCosto;
        private float precioVenta;
        private string nombre;
        private int cantidad;
        private int codigo;

        public float PrecioCosto
        {
            get
            {
                return precioCosto;
            }
            set
            {
                precioCosto = value;
            }
        }

        public float PrecioVenta
        {
            get
            {
                return precioVenta;
            }
            set
            {
                precioVenta = value;
            }
        }
        public string Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                nombre = value;
            }
        }

        public int Cantidad
        {
            get
            {
                return cantidad;
            }
            set
            {
                cantidad = value;
            }
        }

        public int Codigo
        {
            get
            {
                return codigo;
            }
            set
            {
                codigo = value;
            }
        }
    }
}
