using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CANMon
{
    public class CMensaje
    {

        private List<byte> datos = new List<byte>();

        private int recuento;

        private string tiempo;

        private string id;

        private string tipo;

        public CMensaje()
        {
            this.recuento = 0;
            this.tiempo = "";
            this.id = "";
            this.tipo = "";
        }

        public void setDato(int i, byte dato)
        {
            datos[i] = dato;
        }

        public CMensaje(byte[] mensaje)
        {
            string datos = "";
            int num;
            foreach (byte letra in mensaje)
            {
                num = Convert.ToInt32(letra);
                datos += String.Format("{0:X}", num).PadLeft(2, '0');
            }
            for (int i = 5; i < mensaje.Length; i++)
            {
                this.datos.Add(mensaje[i]);
            }
            this.id = datos.Substring(2, 8);
            this.tipo = datos.Substring(0, 2);
        }

        public ECMensaje convierteAStruct()
        {
            ECMensaje e;

            e.Datos = this.pasaDatosACadena(this.getDatos());
            e.Id = this.id;
            e.Tiempo = this.tiempo;

            return e;
        }

        private string pasaDatosACadena(byte[] datos)
        {
            string auxiliar = "";
            foreach (byte letra in datos)
            {
                auxiliar += string.Format("{0:X}", letra).PadLeft(2, '0');
            }
            return auxiliar;
        }

        public override string ToString()
        {
            string retorno = "";
            for (int i = 0; i < this.datos.Count; i++ )
            {
                retorno += string.Format("{0:X}", datos[i]).PadLeft(2, '0');
            }
            return retorno;
        }

        public void setId(string s)
        {
            this.Id = s;
        }

        public string Tiempo
        {
            get { return tiempo; }
            set { tiempo = value; }
        }


        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Id_3
        {
            get { return this.id.Substring(6, 2); }
        }

        public string Id_2
        {
            get { return this.id.Substring(4, 2); }
        }
        
        public string Id_1
        {
            get { return this.id.Substring(2, 2); }
        }
        
        public string Id_0
        {
            get { return this.id.Substring(0, 2); }  
        }
        
        public byte Byte_0
        {
            get { return this.datos[0]; }
            set { this.datos[0]=value; }
        }

        public byte Byte_1
        {
            get { return this.datos[1]; }
            set { this.datos[1]=value; }
        }

        public byte Byte_2
        {
            get { return this.datos[2]; }
            set { this.datos[2]=value; }
        }

        public byte Byte_3
        {
            get { return this.datos[3]; }
            set { this.datos[3]=value; }
        }

        public byte Byte_4
        {
            get { return this.datos[4]; }
            set { this.datos[4]=value; }
        }

        public byte Byte_5
        {
            get { return this.datos[5]; }
            set { this.datos[5]=value; }
        }

        public byte Byte_6
        {
            get { return this.datos[6]; }
            set { this.datos[6]=value; }
        }

        public byte Byte_7
        {
            get { return this.datos[7]; }
            set { this.datos[7]=value; }
        }

        public byte[] getDatos()
        {
            byte[] aux = new byte[datos.Count];
            for (int i = 0; i < datos.Count; i++ )
            {
                aux[i] = datos[i];
            }
            return aux;
        }

        public void setDatos(byte[] cad)
        {
            for (int i = 0; i < cad.Length; i++ )
            {
                datos.Add(cad[i]);         
            }
        }

        public Nullable<byte> getByte(int index)
        {
            if (index <= datos.Count && index >= 0)
            {
                return datos[index];
            }
            else
            {
                return null;
            }
        }

        public bool setByte(int index, byte cad)
        {
            if (index <= datos.Count && index >= 0)
            {
                datos[index] = cad;
                return true;
            }
            return false;
        }

        public int Recuento
        {
            get { return recuento; }
            set { recuento = value; }
        }

    }
}