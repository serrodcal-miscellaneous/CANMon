using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace CANMon
{
    public partial class Form2 : Form
    {

        List<ListViewItem> general = new List<ListViewItem>();

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(CIdenti mensaje)
        {
            InitializeComponent();
            txPF.Text = mensaje.PF;
            txPS.Text = mensaje.PS;
            txSA.Text = mensaje.SAhex;
            txPrio.Text = mensaje.prio.ToString();
            label16.Text = mensaje.PF + mensaje.PS;
            string nombre = pgnTableAdapter1.GetNombrePGN(label16.Text);
            label19.Text = nombre;
            int dec = Int32.Parse(mensaje.PF + mensaje.PS, System.Globalization.NumberStyles.HexNumber);
            label17.Text = dec.ToString();
            ponerDatos(mensaje.datos);
            parametros(mensaje.PF+mensaje.PS, mensaje);            
        }

        public string[] cargaDatos(string _datos, int _num)
        {
            string[] retorno = new string[_num];
            int cont=0;

            for (int i = 0; i < _datos.Length && cont<_num; i+=14)
            {
                retorno[cont] = _datos.Substring(i, 14);
                cont++;
            }

            return retorno;
        }

        private void parametros(string pgn, CIdenti men)
        {
            BitArray ba = new BitArray(stringDatosTobyte(men.datos));
            string cadena = men.datos;
            int subdivision = cadena.Length / 14;
            string[] datos = new string[subdivision];
            datos = cargaDatos(cadena, subdivision);
            for (int v = 0; v < datos.Length; v++)
            {
                decodificaion_mensajes_j1939DataSet.PARAMETERSDataTable dt = parametersTableAdapter1.GetDataParametersBy(pgn);
                for (int i = 0; i < dt.Count; i++)
                {
                    string tipo = dt[i].TIPO_DATO_LENGTH;
                    if (tipo.Equals("BIT"))
                    {
                        int tam = dt[i].LENGTH;
                        String s = dt[i].START_POSITION;
                        double x = Double.Parse(s);
                        string spn = dt[i].SPN;
                        int bytes = Int32.Parse(s.Substring(0, 1));
                        int bit = Int32.Parse(s.Substring(2, 1));

                        decodificaion_mensajes_j1939DataSet.SPNDataTable dtSPN = spnTableAdapter1.GetSPNBy(spn);
                        byte[] datosInvertidos = invierteDatos(datos[v]);
                        string nombre = dtSPN[0].NOMBRE;
                        string offset = dtSPN[0].OFFSET.ToString();
                        string opprangelow = dtSPN[0].OPERATIONAL_RANGE_LOW.ToString();
                        string opprangehigh = dtSPN[0].OPERATIONAL_RANGE_HIGH.ToString();
                        string unidad = dtSPN[0].UNIDAD;
                        string resolution = dtSPN[0].RESOLUTION;
                        string tiempo = men.tiempo;
                        string addLista = "              " + nombre + "  " + offset.ToString() + "  " + spn + "  " + offset.ToString() + "  " + opprangelow.ToString() + "  " + opprangehigh.ToString() + "  " + unidad;
                        int cont = 0;

                        int tam2;
                        byte[] aux = new byte[0];

                        if (tam <= 8)
                        {
                            double rest1 = ((double)(tam + x)) / (double)8;//Operacion
                            //rest += 1;
                            int rest = (int)rest1;
                            //rest = Math.Round(rest);
                            tam2 = (int)rest;
                            if(tam2 == 0)
                                aux = new byte[1];
                            else
                                aux = new byte[tam2];

                            for (int j = bytes - 1; j < datosInvertidos.Length && cont < rest; j++)
                            {
                                aux[cont] += datosInvertidos[j];
                                cont++;
                            }
                        }
                        else if (tam > 8)
                        {
                            double rest = ((double)(tam + x)) / (double)8;//Operacion
                            //rest += 1;
                            rest = Math.Round(rest);
                            tam2 = (int)rest;
                            aux = new byte[tam2];

                            for (int j = bytes - 1; j < datosInvertidos.Length && cont < rest; j++)
                            {
                                aux[cont] += datosInvertidos[j];
                                cont++;
                            }
                        }


                        string bits = String.Empty;

                        int numero;
                        for (int j = aux.Length - 1; j >= 0; j--)
                        {
                            BitArray baux = new BitArray(new byte[] { aux[j] });

                            if (j == aux.Length - 1)
                            {
                                string bitsAux = Reverse(BitArrayToString(baux));

                                if (tam <= 8)
                                {
                                    numero = tam;
                                    bits += bitsAux.Substring(bitsAux.Length - numero, numero);
                                }
                                else
                                {
                                    numero = (aux.Length * 8) - tam;
                                    bits += bitsAux.Substring(0, 8 - numero);
                                }

                            }
                            else
                                bits += Reverse(BitArrayToString(baux));
                        }

                        bool[] arrayBoolean = new bool[bits.Length];

                        for (int j = 0; j < bits.Length; j++)
                        {
                            if (bits.Substring(j, 1) == "0")
                            {
                                arrayBoolean[j] = false;
                            }
                            else if (bits.Substring(j, 1) == "1")
                            {
                                arrayBoolean[j] = true;
                            }
                        }

                        bool[] array2 = new bool[tam];

                        Array.Copy(arrayBoolean, (int)0, array2, (int)0, tam);

                        Array.Reverse(array2);//Da la vuelta

                        long result = 0;
                        for (int j = 0; j < array2.Length; j++)
                        {
                            result += (long)(Math.Pow(2, j) * Convert.ToInt16(array2[j]));
                        }

                        resolution = resolution.Replace(",", ".");
                        double res = double.Parse(resolution);
                        double off = double.Parse(offset);
                        double valor = result + off;
                        this.listView1.Items.Add(new ListViewItem(new string[] { nombre, spn, valor.ToString(), opprangelow + " / " + opprangehigh, unidad, tiempo }));
                    }
                    else if (tipo.Equals("BYTE"))
                    {
                        int numBytes = dt[i].LENGTH;
                        int posIni = Int32.Parse(dt[i].START_POSITION);
                        string spn = dt[i].SPN;
                        decodificaion_mensajes_j1939DataSet.SPNDataTable dtSPN = spnTableAdapter1.GetSPNBy(spn);
                        byte[] datosInvertidos = invierteDatos(men.datos);
                        string nombre = dtSPN[0].NOMBRE;
                        string offset = dtSPN[0].OFFSET.ToString();
                        string opprangelow = dtSPN[0].OPERATIONAL_RANGE_LOW.ToString();
                        string opprangehigh = dtSPN[0].OPERATIONAL_RANGE_HIGH.ToString();
                        string unidad = dtSPN[0].UNIDAD;
                        string resolution = dtSPN[0].RESOLUTION;
                        string tiempo = men.tiempo;
                        string addLista = "              " + nombre + "  " + offset.ToString() + "  " + spn + "  " + offset.ToString() + "  " + opprangelow.ToString() + "  " + opprangehigh.ToString() + "  " + unidad;
                        int cont = 0;
                        byte[] aux = new byte[numBytes];
                        for (int j = posIni - 1; j < datosInvertidos.Length && cont < numBytes; j++)
                        {
                            aux[cont] += datosInvertidos[j];
                            cont++;
                        }
                        int val = 0;
                        val = byteToInt(aux);
                        resolution = resolution.Replace(".", ",");
                        double res = double.Parse(resolution);
                        double off = double.Parse(offset);
                        double valor = val * res + off;
                        this.listView1.Items.Add(new ListViewItem(new string[] { nombre, spn, valor.ToString(), opprangelow + " / " + opprangehigh, unidad, tiempo }));
                    }
                }
            }
        }

        public string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }

        private string BitArrayToString(BitArray _ba)
        { 
            string resultado = String.Empty;

            for (int i = 0; i < _ba.Length; i++)
                resultado += Convert.ToInt16(_ba[i]).ToString();

            return resultado;
        }

        private void parametrosAuxiliar(string pgn, CIdenti men)
        {
            BitArray ba = new BitArray(stringDatosTobyte(men.datos));
            decodificaion_mensajes_j1939DataSet.PARAMETERSDataTable dt = parametersTableAdapter1.GetDataParametersBy(pgn);
            for (int i = 0; i < dt.Count; i++)
            {
                string tipo = dt[i].TIPO_DATO_LENGTH;
                if (tipo.Equals("BIT"))
                {
                    int tam = dt[i].LENGTH;
                    String s = dt[i].START_POSITION;
                    double x = Double.Parse(s);
                    int bytes = Int32.Parse(s.Substring(0, 1));
                    int bit = Int32.Parse(s.Substring(2, 1));
                    string spn = dt[i].SPN;
                    decodificaion_mensajes_j1939DataSet.SPNDataTable dtSPN = spnTableAdapter1.GetSPNBy(spn);
                    byte[] datosInvertidos = invierteDatos(men.datos);
                    string nombre = dtSPN[0].NOMBRE;
                    string offset = dtSPN[0].OFFSET.ToString();
                    string opprangelow = dtSPN[0].OPERATIONAL_RANGE_LOW.ToString();
                    string opprangehigh = dtSPN[0].OPERATIONAL_RANGE_HIGH.ToString();
                    string unidad = dtSPN[0].UNIDAD;
                    string resolution = dtSPN[0].RESOLUTION;
                    string tiempo = men.tiempo;
                    string addLista = "              " + nombre + "  " + offset.ToString() + "  " + spn + "  " + offset.ToString() + "  " + opprangelow.ToString() + "  " + opprangehigh.ToString() + "  " + unidad;
                    int cont = 0;
                    byte[] aux = new byte[tam];
                    double rest = ((double)(tam + x)) / (double)8;//Operacion
                    rest += 1;
                    rest = Math.Round(rest);

                    for (int j = bytes - 1; j < datosInvertidos.Length && cont < rest; j++)
                    {
                        aux[cont] += datosInvertidos[j];
                        cont++;
                    }

                    //Pasar los bytes a bit y contar apartir del bit indicado, el tamaño que especifica el length de la base de datos
                    BitArray ba2 = new BitArray(aux);
                    bool[] array = new bool[ba2.Count];//ba.length
                    ba2.CopyTo(array, 0);
                    bool[] auxiliar = new bool[tam];
                    Array.Copy(array, (long)5, auxiliar, (long)0, (long)auxiliar.Length);
                    BitArray util = new BitArray(auxiliar);

                    long result = 0;
                    for (int j = 0; j < util.Count; j++)
                    {
                        result += (long)(Math.Pow(2, j) * Convert.ToInt16(util[j]));
                    }

                    resolution = resolution.Replace(",", ".");
                    double res = double.Parse(resolution);
                    double off = double.Parse(offset);
                    double valor = result * res + off;
                    this.general.Add(new ListViewItem(new string[] { nombre, spn, valor.ToString(), opprangelow + " / " + opprangehigh, unidad, tiempo }));
                }
                else if (tipo.Equals("BYTE"))
                {
                    int numBytes = dt[i].LENGTH;
                    int posIni = Int32.Parse(dt[i].START_POSITION);
                    string spn = dt[i].SPN;
                    decodificaion_mensajes_j1939DataSet.SPNDataTable dtSPN = spnTableAdapter1.GetSPNBy(spn);
                    byte[] datosInvertidos = invierteDatos(men.datos);
                    string nombre = dtSPN[0].NOMBRE;
                    string offset = dtSPN[0].OFFSET.ToString();
                    string opprangelow = dtSPN[0].OPERATIONAL_RANGE_LOW.ToString();
                    string opprangehigh = dtSPN[0].OPERATIONAL_RANGE_HIGH.ToString();
                    string unidad = dtSPN[0].UNIDAD;
                    string resolution = dtSPN[0].RESOLUTION;
                    string tiempo = men.tiempo;
                    string addLista = "              " + nombre + "  " + offset.ToString() + "  " + spn + "  " + offset.ToString() + "  " + opprangelow.ToString() + "  " + opprangehigh.ToString() + "  " + unidad;
                    int cont = 0;
                    byte[] aux = new byte[numBytes];
                    for (int j = posIni - 1; j < datosInvertidos.Length && cont < numBytes; j++)
                    {
                        aux[cont] += datosInvertidos[j];
                        cont++;
                    }
                    int val = 0;
                    val = byteToInt(aux);
                    resolution = resolution.Replace(".", ",");
                    double res = double.Parse(resolution);
                    double off = double.Parse(offset);
                    double valor = val * res + off;
                    this.general.Add(new ListViewItem(new string[] { nombre, spn, valor.ToString(), opprangelow + " / " + opprangehigh, unidad, tiempo }));
                }
            }
        }

        private int byteToInt(byte[] _datos)
        {
            int retorno = 0;
            for (int i = 0; i < _datos.Length; i++)
            {
                retorno = retorno * 256;
                retorno += _datos[i];
            }

            return retorno;
        }

        private string pasaBytesToString(byte[] _datos)
        {
            string auxiliar = "";
            foreach (byte letra in _datos)
            {
                auxiliar += string.Format("{0:X}", letra).PadLeft(2, '0');
            }
            return auxiliar;
        }

        public byte[] invierteDatos(string _datos)
        {
            byte[] retorno = new byte[13];
            retorno = stringDatosTobyte(_datos);
            return retorno;
        }

        public byte[] stringDatosTobyte(string datos)
        {
            datos = datos.Replace(" ", "");
            byte[] retorno;
            retorno = new byte[datos.Length/2];
            int cont = 0;
            for (int i = 0; i < datos.Length; i += 2)
            {
                byte b = byte.Parse(datos.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                retorno[cont] = b;
                cont++;
            }
            return retorno;
        }

        private void ponerDatos(string p)
        {
            int cont = 0;
            string[] aux = new string[8];
            ItemDatos id = new ItemDatos();
            string auxiliar = "";
            for (int i = 0; i <= p.Length-2; i+=2 )
            {
                auxiliar = p.Substring(i, 2);
                if (cont == 7 || i == p.Length-2)
                {
                    aux[cont] = auxiliar;
                    id.InsertarDatos2(aux);
                    flowLayoutPanel2.Controls.Add(id);
                    cont = 0;
                    id = new ItemDatos();
                }
                aux[cont] = auxiliar;
                cont++;
            }
        }

        private string pasa_hex(string _cadena)
        {
            string datos = "";
            int num = 0;
            char[] valores = _cadena.ToCharArray();

            foreach (char letra in valores)
            {
                num = Convert.ToInt32(letra);
                datos += String.Format("{0:X}", num).PadLeft(2, '0');
            }

            return datos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'decodificaion_mensajes_j1939DataSet.VistaMuestraDatos' table. You can move, or remove it, as needed.
            this.vistaMuestraDatosTableAdapter.Fill(this.decodificaion_mensajes_j1939DataSet.VistaMuestraDatos);
            // TODO: This line of code loads data into the 'decodificaion_mensajes_j1939DataSet.VistaMuestraDatos' table. You can move, or remove it, as needed.
            this.vistaMuestraDatosTableAdapter.Fill(this.decodificaion_mensajes_j1939DataSet.VistaMuestraDatos);
            // TODO: This line of code loads data into the 'decodificaion_mensajes_j1939DataSet.VistaMuestraDatos' table. You can move, or remove it, as needed.
            this.vistaMuestraDatosTableAdapter.FillBy(this.decodificaion_mensajes_j1939DataSet.VistaMuestraDatos, txPF.Text+txPS.Text);
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            // this.tipo.Visible = false;
            panel3.Visible = false;
            panel5.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = !this.panel3.Visible;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel5.Visible = !this.panel5.Visible;
            ListView.SelectedListViewItemCollection it = listView1.SelectedItems;
            for (int i = 0; i < it.Count; i++)
            {
                ListViewItem.ListViewSubItemCollection itc = listView1.Items[listView1.SelectedIndices[i]].SubItems;
                label12.Text = itc[0].Text;
                label11.Text = itc[1].Text;
                decodificaion_mensajes_j1939DataSet.SPNDataTable dtSPN = spnTableAdapter1.GetSPNBy(itc[1].Text);
                textBox5.Text = dtSPN[0].OFFSET.ToString();
                textBox4.Text = dtSPN[0].RESOLUTION;
                decodificaion_mensajes_j1939DataSet.PARAMETERSDataTable dt = parametersTableAdapter1.GetDataParametersBy(label16.Text);
                //textBox2.Text = dt[listView1.SelectedIndices[0]].START_POSITION;
                //textBox3.Text = dt[listView1.SelectedIndices[0]].LENGTH.ToString();
                DataRow[] dr = dt.Select("SPN = '" + itc[1].Text + "'");
                textBox2.Text = dr[0].ItemArray[2].ToString();
                textBox3.Text = dr[0].ItemArray[3].ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            exportar();
        }

        public void exportar()
        {
            List<string> mensajesExportar = new List<string>();
            String id = txPF.Text + " " + txPS.Text;
            if (Program.gFrmPpal.listBox2.Items.Count > 0)
            {
                for (int i = 0; i < Program.gFrmPpal.listBox2.Items.Count; i++)
                {
                    string aux = (string)Program.gFrmPpal.listBox2.Items[i];
                    if (aux.Contains(id))
                        mensajesExportar.Add(aux);
                }
            }
            else
            {
                for (int i = 0; i < Program.gFrmPpal.listBox1.Items.Count; i++)
                {
                    string aux = (string)Program.gFrmPpal.listBox1.Items[i];
                    if (aux.Contains(id))
                        mensajesExportar.Add(aux);
                }
            }


            List<CMensaje> listaCMensajes = new List<CMensaje>();
            for (int i = 0; i < mensajesExportar.Count; i++)
            {
                string idMensajes_0 = mensajesExportar[i].Substring(10, 2);
                string idMensajes_1 = mensajesExportar[i].Substring(13, 2);
                string idMensajes_2 = mensajesExportar[i].Substring(16, 2);
                string idMensajes_3 = mensajesExportar[i].Substring(19, 2);
                string datos = mensajesExportar[i].Substring(23, 23);
                string tiempo = mensajesExportar[i].Substring(0, 7);
                CMensaje m = new CMensaje();
                m.Id = idMensajes_0 + idMensajes_1 + idMensajes_2 + idMensajes_3;
                byte[] b = stringDatosTobyte(datos);
                m.setDatos(b);
                m.Tiempo = tiempo;
                listaCMensajes.Add(m);
            }

            //Desglosar Mensajes para sacar SPN

            foreach (CMensaje m in listaCMensajes)
            {
                CIdenti identi = new CIdenti();
                identi.datos = pasaBytesToString(m.getDatos());
                identi.PF = txPF.Text;
                identi.PS = txPS.Text;
                identi.SAhex = txSA.Text;
                identi.tiempo = m.Tiempo;
                parametrosAuxiliar(identi.PF + identi.PS, identi);
            }

            //--------------------------//

            this.saveFileDialog1.ShowDialog();
            string name = this.saveFileDialog1.FileName;
            if (!name.Trim().Equals("") && this.saveFileDialog1.CheckPathExists)
            {
                ListViewItem it;
                List<ListViewItem> lista = new List<ListViewItem>();
                ListViewItem.ListViewSubItemCollection itc;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].Checked)
                    {
                        it = listView1.Items[i];
                        lista.Add(it);
                    }
                }

                //Cargar en lista resto de mensajes

                if (lista.Count <= 0)
                {
                    //No hay ningun elemento seleccionado
                }
                else
                {
                    List<string> listaAuxiliar = new List<string>();
                    foreach (ListViewItem item in lista)
                    {
                        itc = item.SubItems;
                        listaAuxiliar.Add(itc[1].Text);
                        //dsMensajes1.MensajesExport2.Columns.Add(itc[1].Text + " - " + itc[0].Text);
                        //dsMensajes1.MensajesExport2.SPNColumn.ColumnName = itc[1].Text + " - " + itc[0].Text;
                        //dsMensajes1.MensajesExport.AddMensajesExportRow(itc[0].Text, itc[1].Text, itc[2].Text, itc[3].Text, itc[4].Text, "");
                        dsMensajes1.MensajesExport2.AddMensajesExport2Row(itc[2].Text + " - " + itc[5].Text);
                    }
                    foreach (ListViewItem item2 in general)
                    {
                        itc = item2.SubItems;
                        if (listaAuxiliar.Contains(itc[1].Text))
                        {
                            dsMensajes1.MensajesExport2.Columns.Add(itc[1].Text + " - " + itc[0].Text);
                            dsMensajes1.MensajesExport2.AddMensajesExport2Row(itc[2].Text + " - " + itc[5].Text);
                            //dsMensajes1.MensajesExport2.SPNColumn.ColumnName = itc[1].Text + " - " + itc[0].Text;
                        }
    
                        //dsMensajes1.MensajesExport.AddMensajesExportRow(itc[0].Text, itc[1].Text, itc[2].Text, itc[3].Text, itc[4].Text, "");
                    }
                    dsMensajes1.MensajesExport2.WriteXml(name);
                }
            }
        }
    }
}
