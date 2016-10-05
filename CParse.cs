using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CANMon
{
    class CParse
    {

        dsMensajes dsMensajes = new dsMensajes();
        dsMensajes.mensajesDataTable dtMensajes = new dsMensajes.mensajesDataTable();

        
        public void Parse(string _cadena, int _tipo)
        {
            Regex regex = new Regex("(?<TIEMPO>.{11})(?<ID>.{10})(?<DATOS>.{23})");

            Match match = regex.Match(_cadena);

            if (match.Success)
            {
                this.introduceMensajes(match,_tipo);
            }
        
        }

        private void introduceMensajes(Match match, int _tipo)
        {

            string tiempo;
            string id;

            tiempo = getTiempoMatch(match);
            id = getIdMatch(match);
           
            if (_tipo == 0)
            {
                Program.gFrmPpal.listBox1.Items.Add(tiempo + "   " + id + "  " + match.Groups["DATOS"].Value);
                Program.gFrmPpal.listBox1.SelectedIndex = Program.gFrmPpal.listBox1.Items.Count - 1;
            }
            else
            {
                Program.gFrmPpal.listBox2.Items.Add(tiempo + "   " + id + "  " + match.Groups["DATOS"].Value);
                Program.gFrmPpal.listBox2.SelectedIndex = Program.gFrmPpal.listBox2.Items.Count - 1;
            }

            
            //Program.gFrmPpal.dsMensajes.mensajesRow rowMensaje = Program.gFrmPpal.dsMensajes.mensajes.NewmensajesRow();
            dsMensajes.mensajesRow rowMensaje = Program.gFrmPpal.dsMensajes.mensajes.NewmensajesRow();
            rowMensaje.tipo = byte.Parse(_tipo.ToString());
            rowMensaje.id = rowMensaje.tipo.ToString().PadLeft(2,'0') + id.Replace(" ","");
            rowMensaje.id_mostrar = id.Replace(" ", ""); ;
            rowMensaje.datos = match.Groups["DATOS"].Value;
            rowMensaje.recuento = 1;

            if (this.existeIdResumen(rowMensaje.id) == true)
            //si el codigo leido existe en la tabla resumen
            {

                //devuelve la fila anterior con el mismo id que exista en la lista resumen
                dsMensajes.mensajesRow filaAnt = devuelveIdDato(rowMensaje.id);

                if (filaAnt != null)//si la fila es nula
                {
                    //se cambian los datos de la fila anterior por los de la fila nueva
                    filaAnt.datos = rowMensaje.datos;
                    filaAnt.recuento = filaAnt.recuento + 1;
                }
            }
            else
            {
                //se introducde el registro nuevo
                Program.gFrmPpal.dsMensajes.mensajes.AddmensajesRow(rowMensaje);



            }
        }

        private dsMensajes.mensajesRow devuelveIdDato(string id)// devuelve el id del dato que coincida con el id del parametro
        {
            dsMensajes.mensajesRow row = Program.gFrmPpal.dsMensajes.mensajes.FindByid(id);
            if (row != null)
            {
                return row;
            }
            else
            {
                return row;
            }

        }


        private bool existeIdResumen(string id)
        {
            dsMensajes.mensajesRow row = Program.gFrmPpal.dsMensajes.mensajes.FindByid(id);
            if (row != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string getDatosMatch(Match match)
        {
            string datos = "";
            string datosAux = "";
            datos = match.Groups["DATOS"].Value.Replace(" ", "");
            for (int i = 0; i < datos.Length; i++)
            {
                if (i!= 0 && i % 2 == 0)
                    datosAux += "  ";

                datosAux += datos[i];

            }
            return datosAux;
        }

        private string getIdMatch(Match match)
        {
            string id="";
            string idAux="";

            id = match.Groups["ID"].Value.Replace(" ", "");
            id = id.PadLeft(8, '0');
            for (int i = 0; i < id.Length; i++)
            {
                if (i != 0 && i % 2 == 0)
                    idAux += " ";

                idAux += id[i];

            }
            return idAux;
            
        }

        private string getTiempoMatch(Match match)
        {
            string tiempo;
            tiempo = match.Groups["TIEMPO"].Value.Replace(" ", "");
            tiempo = tiempo.PadLeft(7, '0');
            return tiempo;
        }

        
  
    }
}
