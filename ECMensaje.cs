using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CANMon
{
    public struct ECMensaje
    {

        public string Tiempo;

        public string Id;

        public string Datos;

        public string getString()
        {
            string retorno = "";

            Tiempo = Tiempo.PadLeft(7, '0');

            Id = separa(Id);

            Datos = separa(Datos);

            retorno = Tiempo + "   " + Id + " " + Datos;

            return retorno;
        }

        public string separa(string _in)
        {
            string retorno = "";

            //_in = pasa_hex(_in);

            for (int i = 0; i < _in.Length; i += 2)
                retorno += _in.Substring(i, 2) + " ";

            //retorno += _in[i] + _in[i+1] + " ";

            retorno.Trim();

           return retorno;
        }

        //private string pasa_hex(string _cadena)
        //{
        //    string datos = "";
        //    int num = 0;
        //    char[] valores = _cadena.ToCharArray();

        //    foreach (char letra in valores)
        //    {
        //        num = Convert.ToInt32(letra);
        //        datos += String.Format("{0:X}", num).PadLeft(2, '0');
        //    }

        //    return datos;
        //}

    }
}
