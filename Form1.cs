using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Data.OleDb;
using System.IO;
using System.IO.Ports;



namespace CANMon
{
    public partial class Form1 : Form
    {
        List<CMensaje> listaEB = new List<CMensaje>();
        List<CMensaje> listaDescartes = new List<CMensaje>();//Lista donde se almacenan los multipaquetes
        TransIns frmTransIns = null;
        DateTime dateTime;
        public bool hiloActivo = false;
        public Thread tRecepcion = null; //hilo donde se ejecuta la lectura
        List<CMensaje> listaTotal = new List<CMensaje>();
        List<CMensaje> listaAuxiliar = new List<CMensaje>();
        CIdenti identi = new CIdenti();
        public dsMensajesPeriodicos dsMensajesPeriodicos = new dsMensajesPeriodicos();
        public SerialPort serialPort1 = new SerialPort("COM11"); //Objeto serialport
        DateTime ahora; //Para controlar el tiempo en el que llega el mensaje
        int numMensajesMulti = 0;
        List<byte[]> historico = new List<byte[]>();
        List<CMensaje> historicoMensajes = new List<CMensaje>();
        int contadorHistorico = 0;

        public Form1()
        {
            InitializeComponent();
        }

        //Cierra el hilo cuando cerramos el formulario
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.hiloActivo)
            {
                this.hiloActivo = false;
            }
        }

        private delegate void imprimeTotalDelegate(CMensaje _mensaje);

        //Imprime por pantalla los mensajes
        private void imprimir(CMensaje _mensaje)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new imprimeTotalDelegate(imprimir), new CMensaje[] { _mensaje });
            }
            else
            {
                string datosSep = separaDatos(_mensaje.getDatos()); //Formatea cadena, separando por byte
                string hex = separaId(_mensaje);
                int aux;
                if (Int32.TryParse(_mensaje.Tipo, out aux))
                {
                    int num_bandera = aux;
                    if (num_bandera == 0)//Standar
                    {
                        listBox1.Items.Add(_mensaje.Tiempo.PadLeft(7, '0') + "   " + hex + "  " + datosSep);
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                    else if (num_bandera == 1) //Extendido
                    {
                        string insert = "";
                        insert = _mensaje.Tiempo.PadLeft(7, '0') + "   " + hex + "  " + datosSep;
                        listBox2.Items.Add(insert);
                        //listBox2.Items.Add(_mensaje.Tiempo.PadLeft(7, '0') + "  " + hex + "  " + datosSep);
                        listBox2.SelectedIndex = listBox2.Items.Count - 1;
                    }
                }
                _mensaje = null;

            }
        }

        //Funcion que formatea el ID
        private static string separaId(CMensaje _mensaje)
        {
            string hex = _mensaje.Id.Substring(0, 2) + " " +
            _mensaje.Id.Substring(2, 2) + " " +
            _mensaje.Id.Substring(4, 2) + " " +
            _mensaje.Id.Substring(6, 2);
            return hex;
        }

        //Funcion que formatea los datos
        private string separaDatos(byte[] _cad)
        {
            string aux = " ";
            aux = pasaBytesToString(_cad);
            string cadena = aux.Substring(0, 2) + " " +
            aux.Substring(2, 2) + " " +
            aux.Substring(4, 2) + " " +
            aux.Substring(6, 2) + " " +
            aux.Substring(8, 2) + " " +
            aux.Substring(10, 2) + " " +
            aux.Substring(12, 2) + " " +
            aux.Substring(14, 2);
            return cadena;
        }

        //Devuelve Id del mensaje recibido
        private dsMensajes.mensajesRow devuelveId(CMensaje _mensaje)
        {
            //Revisar: dsMensajes.mensajesRow row = this.dsMensajes.mensajes.FindByid(men.Tipo + men.Id);
            dsMensajes.mensajesRow filaMensaje = this.dsMensajes.mensajes.FindByid(_mensaje.Id);
            if (filaMensaje != null)
            {
                return filaMensaje;
            }
            else
            {
                return null;
            }

        }

        //Devuelve true si existe el Id del mensaje recibido
        private bool existeId(CMensaje _mensaje)
        {
            dsMensajes.mensajesRow filaMensaje = this.dsMensajes.mensajes.FindByid(_mensaje.Id);
            if (filaMensaje != null)
            {
                filaMensaje = null;
                return true;
            }
            else
            {
                filaMensaje = null;
                return false;
            }
        }

        //Cuando se inicia el Formulario...
        private void Form1_Load(object sender, EventArgs e)
        {
            this.serialPort1.ReadBufferSize = 4096;
            this.serialPort1.NewLine = "\n\r";
        }

        //Boton que inicia el proceso de lectura [PLAY]
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                while (!this.serialPort1.IsOpen)
                    this.serialPort1.Open();
                dateTime = DateTime.Now;
                this.serialPort1.ReadExisting();
                this.serialPort1.WriteLine("P");
                while (this.serialPort1.ReadByte() != (int)'P')
                {
                    this.serialPort1.WriteLine("P");
                }
                btn_Play.Enabled = false;
                btn_Pause.Enabled = true;
                toolStripButton3.Enabled = false;
                toolStripDropDownButton1.Enabled = false;
                toolStripButton5.Enabled = true;
                toolStripButton6.Enabled = false;

                //this.serialPort1.Open();
                this.tRecepcion = new Thread(new ThreadStart(InicioRecepcion));
                this.hiloActivo = true;
                this.tRecepcion.Start();
            }
            catch (Exception exe)
            {
                MessageBox.Show("Conecte dispositivo al puerto serie o configure el puerto correctamente");
            }
        }

        //Proceso que inicia el puerto
        public void InicioRecepcion()
        {
            if (this.serialPort1.IsOpen)
                this.serialPort1.Close();

            this.serialPort1 = new System.IO.Ports.SerialPort(this.serialPort1.PortName, Program.velocidadPuerto);
            this.serialPort1.Open();

            //this.serialPort1.ReadTimeout = 3000;
            this.serialPort1.ReadExisting(); //Se llamaba a este metodo antes de ProcesarDatos()

            while (this.hiloActivo) //Mientras el hilo este activo
            {
                try
                {
                    LeerDatos();//Procedimiento que realiza el proceso de leer datos y los filtra
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Tiempo de espera en lectura superado");
                }
                catch (Exception ex)
                { }
            }
        }

        //Comprueba que el salto de line ha sido recibida sea correcto
        private bool compruebaCabeceroEsSalto()
        {

            bool cabeceraBien = false;// indica si la cabecera es [[  \n\r  ]]
            byte[] cadena = new byte[1];
            string aux;
            while (!cabeceraBien)
            {
                do
                {
                    this.serialPort1.Read(cadena, 0, 1);
                    aux = pasaBytesToString(cadena);
                } while (aux != "0A");

                do
                {
                    this.serialPort1.Read(cadena, 0, 1);
                    aux = pasaBytesToString(cadena);

                } while (aux == "0A");

                if (aux == "0D")
                {
                    //cabeceraBien = true;
                    cadena = null;
                    return true;
                }
            }
            cadena = null;
            return false;
        }

        //Convierte un byte a string
        private string pasaByteToString(byte _letra)
        {
            string s = "";
            s += string.Format("{0:X}", _letra).PadLeft(2, '0');
            return s;
        }

        //Pasa un array de byte a string
        private string pasaBytesToString(byte[] _datos)
        {
            string auxiliar = "";
            foreach (byte letra in _datos)
            {
                auxiliar += string.Format("{0:X}", letra).PadLeft(2, '0');
            }
            return auxiliar;
        }

        //Devuelve true cuando los mensajes EC(1) y EB(?) estan leido y guardados
        private bool compruebaLista()
        {
            bool correcto = true;
            if (listaDescartes.Count > 0)
            {
                if (listaDescartes[0].Id_1.Equals("EC"))
                {
                    int numeroMensajesEB = (int)listaDescartes[0].Byte_3; //Se extraen el numero de mensajes EB (Multipaquetes) que tienen que llegar
                    if (listaDescartes.Count - 1 == numeroMensajesEB) //Si existen todos los mensajes que se esperan
                    {
                        for (int i = 1; i < listaDescartes.Count && correcto; i++)
                        {
                            if (i != (int)listaDescartes[i].Byte_0) //Si el mensaje no tiene la posicion correcta
                            {
                                correcto = false;
                                listaDescartes.Clear();//Si falla la enumeracion EB, limpio la lista
                            }
                        }
                        return correcto;
                    }
                }
            }
            return false;
        }

        //Devuelve true si el mensaje recibido es de tipo EC o EB
        private bool noEsMensajeMultipaquete(CMensaje _mensaje)
        {
            //Los multipaquetes se forman por una cabercera (contiene en el Id - EC) y uno o varios cuerpos(contiene en el Id - EB)
            if (!_mensaje.Id_1.Equals("EC") && !_mensaje.Id_1.Equals("EB"))
            {
                return true;//No es multipaquete
            }
            else
            {
                return false;//Es multipaquete
            }
        }

        //Ejecuta el proceso normal
        private void procesoNormal(CMensaje _mensaje, dsMensajes.mensajesRow _fila)
        {

            this.listaTotal.Add(_mensaje); //introduce en la lista total
            if (existeId(_mensaje))
            //si el codigo leido existe en la tabla resumen
            {
                //devuelve la fila anterior con el mismo id que exista en la lista resumen
                dsMensajes.mensajesRow filaExistente = devuelveId(_mensaje);

                if (filaExistente != null)//si la fila  no es nula
                {
                    //se cambian los datos de la fila anterior por los de la fila nueva
                    filaExistente.datos = _fila.datos;
                    filaExistente.recuento = filaExistente.recuento + 1;
                }
            }
            else
            {
                //se introduce el registro nuevo
                _fila.recuento = 1;//inicializo recuento a 1 antes de introducirlo en la tabla
                this.dsMensajes.mensajes.AddmensajesRow(_fila);//Añade la fila leida
            }
            imprimir(_mensaje);
            //Recoleccion de basura
            _mensaje = null;
            _fila = null;
            //Debug.WriteLine(fila.datos);
        }

        //Para mensajes multipaquete
        private void procesoNormal2(CMensaje _mensaje, dsMensajes.mensajesRow _fila)
        {

            //this.listaTotal.Add(_mensaje); //introduce en la lista total
            if (existeId(_mensaje))
            //si el codigo leido existe en la tabla resumen
            {
                //devuelve la fila anterior con el mismo id que exista en la lista resumen
                dsMensajes.mensajesRow filaExistente = devuelveId(_mensaje);

                if (filaExistente != null)//si la fila  no es nula
                {
                    //se cambian los datos de la fila anterior por los de la fila nueva
                    filaExistente.datos = _fila.datos;
                    filaExistente.recuento = filaExistente.recuento + 1;
                }
            }
            else
            {
                //se introduce el registro nuevo
                _fila.recuento = 1;//inicializo recuento a 1 antes de introducirlo en la tabla
                this.dsMensajes.mensajes.AddmensajesRow(_fila);//Añade la fila leida
            }
            //imprimir(_mensaje);
            //Recoleccion de basura
            _mensaje = null;
            _fila = null;
            //Debug.WriteLine(fila.datos);
        }

        //Solo EC
        private void procesoNormal3(CMensaje _mensaje, dsMensajes.mensajesRow _fila)
        {

            this.listaTotal.Add(_mensaje); //introduce en la lista total
            imprimir(_mensaje);
            //Recoleccion de basura
            _mensaje = null;
            _fila = null;
        }

        private bool mensajeCorrecto(CMensaje _in)
        {
            string id = _in.Id;
            if ((id[id.Length - 2].ToString() + id[id.Length-1].ToString() == "0A" && _in.Byte_0.ToString() == "13"))
                return false;
            if ( _in.Byte_0.ToString() == "10" && _in.Byte_1.ToString() == "13")
                return false;
            for (int i = 0; i < id.Length - 3; i+=2)
            {
                string idAux = id[i].ToString() + id[i+1].ToString();
                string idAuxSig = id[i+2].ToString() + id[i+3].ToString();
                if (idAux == "0A" && idAuxSig == "0D")
                {
                    return false;
                }

            }
            return true;
        }

        //Procedimiento que procesa los datos
        private void LeerDatos()
        {
            byte[] datos = new byte[13];

            string datos_cadena = "";
            CMensaje mensaje = null;
            dsMensajes.mensajesRow fila = this.dsMensajes.mensajes.NewmensajesRow();
            if (compruebaCabeceroEsSalto())
            {
                try
                {
                    for (int i = 0; i < 13; i++)
                    {
                        datos[i] = (byte)this.serialPort1.ReadByte();
                    }

                    //historico.Add(datos);//Historico

                    string aux = pasaBytesToString(datos);

                    mensaje = new CMensaje(datos);

                    //historicoMensajes.Add(mensaje);//Historico
                    //contadorHistorico++;

                    //Este filtro no es valido, aunque quita algo de basura
                    if ((datos[0] == 0 || datos[0] == 1) && (mensajeCorrecto(mensaje)))
                    {

                        ahora = DateTime.Now;
                        mensaje.Tiempo = Convert.ToInt32((ahora - dateTime).TotalMilliseconds).ToString();
                        datos_cadena = pasaBytesToString(datos);
                        //Debug.WriteLine(datos_cadena);
                        fila.id = mensaje.Id;
                        fila.tipo = datos[0];
                        fila.datos = this.pasaBytesToString(mensaje.getDatos());
                        fila.id_mostrar = mensaje.Id;
                        if (noEsMensajeMultipaquete(mensaje))
                        {
                            listaDescartes.Clear();//Limpia la lista, por si a fallado alguna lectura de miltupaquetes

                            datos = null;

                            if (compruebaLista())//Si la lista multipaquetes es correcta
                            {
                                dsMensajes.mensajesRow filaAux = this.dsMensajes.mensajes.NewmensajesRow();
                                CMensaje mensajeAuxiliar = montarMensaje();
                                filaAux.id = mensajeAuxiliar.Id;
                                filaAux.tipo = 1;
                                filaAux.datos = this.pasaBytesToString(mensajeAuxiliar.getDatos());
                                filaAux.id_mostrar = mensajeAuxiliar.Id;
                                procesoNormal2(mensajeAuxiliar, filaAux);
                                listaDescartes.Clear();
                            }
                            procesoNormal(mensaje, fila);
                            mensaje = null;
                            fila = null;
                        }
                        else//Para tratar los multipaquetes
                        {
                            if (aux.Substring(4, 2).Equals("EC")) //Si es cabecera
                            {
                                listaDescartes.Clear(); //Se limpia la lista puesto que a llegado una nueva cabecera
                                numMensajesMulti = sacaNumDatos(aux); //Se obtiene el numero de multipaquetes que tienen que llegar
                            }
                            if (listaDescartes.Count < numMensajesMulti) //Mientras falten multipaquetes
                            {
                                listaDescartes.Add(mensaje);
                                procesoNormal3(mensaje, fila);
                            }
                            if (listaDescartes.Count == numMensajesMulti && listaDescartes.Count != 0 && numMensajesMulti != 0) //Cuando esten todos
                            {
                                if (compruebaLista())//Si la lista es correcta
                                {
                                    dsMensajes.mensajesRow filaAux = this.dsMensajes.mensajes.NewmensajesRow();
                                    CMensaje mensajeAuxiliar = montarMensaje();//Se monta el mensaje a partir de los multipaquetes //Exception
                                    filaAux.id = mensajeAuxiliar.Id;
                                    filaAux.tipo = 1;
                                    filaAux.datos = this.pasaBytesToString(mensajeAuxiliar.getDatos());
                                    filaAux.id_mostrar = mensajeAuxiliar.Id;
                                    procesoNormal2(mensajeAuxiliar, filaAux);
                                }
                                listaDescartes.Add(mensaje);
                                procesoNormal3(mensaje, fila);//Se añade el mensaje montado
                            }
                        }
                    }//If para mensajes correctos
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Tiempo de espera en lectura superado");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception");
                }

            }
        }

        //Proceso que saca el numero de datos por llegar
        public int sacaNumDatos(string _cadena)
        {
            int retorno = 0;
            string aux = _cadena.Substring(16, 2);
            retorno = Int32.Parse(aux);
            retorno += 1;
            return retorno;
        }

        //Monta los mensajes multipaquetes
        public CMensaje montarMensaje()
        {
            int tam = (listaDescartes.Count - 1) * 7;
            byte[] datos = new byte[tam];
            int cont = 0;
            byte[] aux = new byte[8];
            int numPaquetes = (int)listaDescartes[0].Byte_3;
            int numDatos = (int)listaDescartes[0].Byte_1;
            for (int i = 1; i < listaDescartes.Count && i <= numPaquetes; i++)
            {
                //&& cont < numDatos
                for (int j = 1; j < 8 && cont < tam ; j++)
                {
                        aux = listaDescartes[i].getDatos();
                        datos[cont] = aux[j];
                        cont++;
                }
            }
            byte[] auxiliarBytes = new byte[tam];
            for (int i = 0; i < tam; i++)
            {
                auxiliarBytes[i] = datos[i];
            }
            CMensaje mensajeRetorno = new CMensaje();
            mensajeRetorno.setDatos(auxiliarBytes);
            string id1 = pasaByteToString(listaDescartes[0].Byte_6);
            string id2 = pasaByteToString(listaDescartes[0].Byte_5);
            mensajeRetorno.setId(listaDescartes[0].Id_0 + id1 + id2 + listaDescartes[0].Id_3);
            mensajeRetorno.Tipo = listaDescartes[0].Tipo;
            return mensajeRetorno;
        }

        //Comprueba de que tipo es el mensaje, 0 Standar, 1 Extendido
        private byte compruebaTipo(string _tipo)
        {
            byte res;
            int num_bandera = Int32.Parse(_tipo);
            if (num_bandera == 0)//Standar
            {
                res = 0;
            }
            else//Extendido
            {
                res = 1;
            }
            return res;
        }

        //Pasa a hexadecimal la cadena
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

        //Proceso que pausa la lectura [PAUSE]
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.hiloActivo = false;
            Thread.Sleep(500);
            tRecepcion = null;
            this.serialPort1.WriteLine("p"); ;
            this.serialPort1.ReadExisting();
            Thread.Sleep(1000);
            this.serialPort1.Close();
            btn_Pause.Enabled = false;
            btn_Play.Enabled = true;
            toolStripButton3.Enabled = true;
            toolStripDropDownButton1.Enabled = true;
            toolStripButton6.Enabled = true;
        }

        //toma la prioridad del primer byte y la pasa a entero
        private void prioridad(string _id)
        {
            BitArray ba = new BitArray(new byte[] { byte.Parse(_id.Substring(1, 1), System.Globalization.NumberStyles.HexNumber) });
            BitArray prioridad = new BitArray(new bool[] { ba[3], ba[4], ba[5] });
            byte[] num = new byte[1];
            prioridad.CopyTo(num, 0);
            identi.prio = int.Parse(num[0].ToString());
        }

        private void listaESTANDARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void listaEXTENDIDOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Fconfig p_config = new Fconfig();
            p_config.ShowDialog();
        }

        private void eSTANDARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = true;
        }

        private void eXTENDIDOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1Collapsed = true;
        }

        private void todoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1Collapsed = false;
            splitContainer2.Panel2Collapsed = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //La columna del DataGrid es visible
            this.tipo.Visible = true;
            //Pasar mensajes al data source
            this.dataGridView1.DataSource = this.dsMensajes.mensajes;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //La columna del DataGrid deja de ser visible
            this.tipo.Visible = false;
            //Pasar mensajes al data source, solo los de tipo standar
            this.dataGridView1.DataSource = this.dsMensajes.mensajes.Select("tipo = 0");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            //La columna del DataGrid deja de ser visible
            this.tipo.Visible = false;
            //Pasar mensajes al data source, solo los de tipo extendido
            this.dataGridView1.DataSource = this.dsMensajes.mensajes.Select("tipo = 1");
        }

        //Cuando se hace click en una fila del datagrid
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (hiloActivo == false)
            {
                try
                {
                    string linea = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    string datos = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

                    identi.tiempo = linea.Substring(0, 6);
                    identi.identificador = linea;
                    identi.SAhex = linea.Substring(6, 2);
                    identi.PF = linea.Substring(2, 2);
                    identi.PS = linea.Substring(4, 2);
                    identi.PGNhex = linea.Substring(2, 4);
                    identi.datos = datos;
                    prioridad(identi.identificador);
                    identi.PFint = int.Parse(identi.PF, System.Globalization.NumberStyles.HexNumber);

                    Form2 form = new Form2(this.identi);
                    form.ShowDialog();
                }
                catch (Exception excepcion1) { }
            }
            else
            {
                //this.toolStripStatusLabel1.Text = "Debe parar el puerto serie antes de consultar algún dato.";
            }
        }

        private void transmisiónPeriodicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransPer nuevo = new TransPer();
            nuevo.Show();
        }

        private void transmisiónInstantaneaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frmTransIns == null)
                this.frmTransIns = new TransIns();

            this.frmTransIns.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.hiloActivo = false;

            Thread.Sleep(3000);

            tRecepcion = null;

            tRecepcion = new Thread(new ThreadStart(InicioRecepcion));
            tRecepcion.Start();
        }

        private void resumenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML File| *.xml";
            saveFileDialog1.Title = "Guardar en XML";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                List<CMensaje> list = new List<CMensaje>();

                foreach (dsMensajes.mensajesRow row in dsMensajes.mensajes)
                    list.Add(MensajesRowToCMensaje(row));

                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(list.GetType());
                x.Serialize(fs, list);
                fs.Close();

                //dsMensajes.mensajes.WriteXml(saveFileDialog1.FileName);
            }

        }

        //Convierte un Objeto CMensaje en una estructura ECMensajes
        public List<ECMensaje> convierteLista(List<CMensaje> _in)
        {
            List<ECMensaje> salida = new List<ECMensaje>();

            for (int i = 0; i < _in.Count; i++)
            {
                salida.Add(_in[i].convierteAStruct());
            }

            return salida;
        }

        private void listadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML File| *.xml";
            saveFileDialog1.Title = "Guardar en XML";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {

                List<ECMensaje> list = convierteLista(this.listaTotal);

                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(list.GetType());
                x.Serialize(fs, list);
                fs.Close();
            }
        }

        //Convierte una fila en un objeto CMensaje
        public CMensaje MensajesRowToCMensaje(dsMensajes.mensajesRow _mensaje)
        {
            CMensaje mensaje = new CMensaje();

            mensaje.Tipo = _mensaje.tipo.ToString();
            mensaje.Id = _mensaje.id_mostrar;

            mensaje.Recuento = _mensaje.recuento;

            return mensaje;
        }

        private void tipoExtendidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.cargaArchivo.captaMensajes(1);
        }

        private void tipoEstandarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.cargaArchivo.captaMensajes(0);
        }

        private void btn_Pause_Click(object sender, EventArgs e)
        {
            this.hiloActivo = false;
            Thread.Sleep(500);
            tRecepcion = null;
            this.serialPort1.WriteLine("p"); ;
            this.serialPort1.ReadExisting();
            Thread.Sleep(1000);
            this.serialPort1.Close();
            btn_Pause.Enabled = false;
            btn_Play.Enabled = true;
            toolStripButton3.Enabled = true;
            toolStripDropDownButton1.Enabled = true;
            toolStripButton6.Enabled = true;
        }

        //Pasa string a cadena de byte
        public byte[] pasaStringToBytes(string datos)
        {
            datos = datos.Replace(" ", "");
            byte[] retorno;
            retorno = new byte[datos.Length / 2];
            int cont = 0;
            for (int i = 0; i < datos.Length; i += 2)
            {
                byte b = byte.Parse(datos.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                retorno[cont] = b;
                cont++;
            }
            return retorno;
        }

        //Cuando se haga click en la lista Extendido
        private void listBox2_Click(object sender, EventArgs e)
        {
            try
            {
                if (hiloActivo == false)
                {
                    string fila = (string)listBox2.SelectedItem;
                    string tiempo = fila.Substring(0, 10);
                    tiempo = tiempo.Trim();
                    string id = fila.Substring(10, 11);
                    string datos = fila.Substring(23, 23);
                    string mensajeString = id + " " + datos;
                    if (id.Substring(3, 2).Equals("EC"))
                    {
                        //Montar multipaquete

                        CMensaje mensajeEC = new CMensaje();
                        mensajeEC.Tipo = "01";
                        id = id.Replace(" ", "");
                        mensajeEC.Id = id;
                        mensajeEC.setDatos(pasaStringToBytes(datos));
                        listaEB.Add(mensajeEC);
                        bool correcto = true;
                        mensajeEC.Tiempo = tiempo;

                        //buscar X EB siempre que no encuentre 1 EC
                        for (int i = listBox2.SelectedIndex + 1; i < this.listBox2.Items.Count && correcto; i++)
                        {
                            fila = listBox2.Items[i].ToString();
                            id = fila.Substring(10, 11);
                            datos = fila.Substring(23, 23);
                            mensajeString = id + " " + datos;
                            if (id.Substring(3, 2).Equals("EB"))
                            {
                                CMensaje mensajeEB = new CMensaje();
                                mensajeEB.Tipo = "01";
                                id = id.Replace(" ", "");
                                mensajeEB.Id = id;
                                mensajeEB.setDatos(pasaStringToBytes(datos));
                                if(!listaEB.Contains(mensajeEB))
                                    listaEB.Add(mensajeEB);
                            }
                            else if (id.Substring(3, 2).Equals("EC"))
                            {
                                correcto = false;
                            }

                        }

                        //var lista = from string s in this.listBox2.Items where s.Contains("EC") select s;

                        //MontarMultiPaquete
                        if (compruebaLista2())
                        {
                            //dsMensajes.mensajesRow filaAux = this.dsMensajes.mensajes.NewmensajesRow();
                            CMensaje mensajeAuxiliar = montarMensaje2();
                            identi.identificador = mensajeAuxiliar.Id;
                            identi.SAhex = string.Format(mensajeAuxiliar.Id_3);
                            identi.PF = string.Format(mensajeAuxiliar.Id_1);
                            identi.PS = string.Format(mensajeAuxiliar.Id_2);
                            identi.PGNhex = identi.PF + identi.PS;
                            string datosAuxiliar2 = pasaBytesToString(mensajeAuxiliar.getDatos());
                            identi.datos = datosAuxiliar2.Substring(0, 2) + datosAuxiliar2.Substring(2, 2)
                                + datosAuxiliar2.Substring(4, 2) + datosAuxiliar2.Substring(6, 2)
                                + datosAuxiliar2.Substring(8, 2) + datosAuxiliar2.Substring(10, 2)
                                + datosAuxiliar2.Substring(12, 2) + datosAuxiliar2.Substring(14, 2)
                                + datosAuxiliar2.Substring(16, 2) + datosAuxiliar2.Substring(18, 2);
                            prioridad(identi.identificador);
                            identi.PFint = int.Parse(identi.PF, System.Globalization.NumberStyles.HexNumber);

                            Form2 form = new Form2(this.identi);
                            form.ShowDialog();
                            listaEB.Clear();
                        }


                        //Form2 form = new Form2(this.identi);
                        //form.ShowDialog();
                    }
                    else if (id.Substring(3, 2).Equals("EB"))
                    {
                        //Ignorar
                    }
                    else
                    {
                        //Mensaje Normal
                        identi.identificador = mensajeString;
                        identi.SAhex = id.Substring(9, 2);
                        identi.PF = id.Substring(3, 2);
                        identi.PS = id.Substring(6, 2);
                        identi.PGNhex = identi.PF + identi.PS;
                        identi.datos = datos.Substring(0, 2) + datos.Substring(3, 2)
                            + datos.Substring(6, 2) + datos.Substring(9, 2)
                            + datos.Substring(12, 2) + datos.Substring(15, 2)
                            + datos.Substring(18, 2) + datos.Substring(21, 2);
                        prioridad(identi.identificador);
                        identi.PFint = int.Parse(identi.PF, System.Globalization.NumberStyles.HexNumber);
                        identi.tiempo = tiempo;
                        Form2 form = new Form2(this.identi);
                        form.ShowDialog();
                    }
                }
            }
            catch (Exception exce)
            {
                MessageBox.Show("No hay mensajes");
            }
        }

        private bool compruebaLista2()
        {
            bool correcto = true;
            if (listaEB.Count > 0)
            {
                if (listaEB[0].Id_1.Equals("EC"))
                {
                    int numeroMensajesEB = (int)listaEB[0].Byte_3;
                    if (listaEB.Count - 1 == numeroMensajesEB)
                    {
                        for (int i = 1; i < listaEB.Count && correcto; i++)
                        {
                            if (i != (int)listaEB[i].Byte_0)
                            {
                                correcto = false;
                                listaEB.Clear();//Si falla la enumeracion EB, limpio la lista
                            }
                        }
                        return correcto;
                    }
                }
            }
            return false;
        }

        public CMensaje montarMensaje2()
        {
            int tam = (listaEB.Count - 1) * 7;
            byte[] datos = new byte[tam];
            int cont = 0;
            byte[] aux = new byte[8];
            int numPaquetes = (int)listaEB[0].Byte_3;
            int numDatos = (int)listaEB[0].Byte_1;
            for (int i = 1; i < listaEB.Count && i <= numPaquetes; i++)
            {
                for (int j = 1; j < 8 && cont < tam && cont < numDatos; j++)
                {
                    aux = listaEB[i].getDatos();
                    datos[cont] = aux[j];
                    cont++;
                }
            }
            byte[] auxiliarBytes = new byte[numDatos];
            for (int i = 0; i < numDatos; i++)
            {
                auxiliarBytes[i] = datos[i];
            }
            CMensaje mensajeRetorno = new CMensaje();
            mensajeRetorno.setDatos(auxiliarBytes);
            string id1 = pasaByteToString(listaEB[0].Byte_6);
            string id2 = pasaByteToString(listaEB[0].Byte_5);
            mensajeRetorno.setId(listaEB[0].Id_0 + id1 + id2 + listaEB[0].Id_3);
            mensajeRetorno.Tipo = listaEB[0].Tipo;
            return mensajeRetorno;
        }

        private void listaResumenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dsMensajes.mensajes.Clear();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.serialPort1.IsOpen)
                this.serialPort1.Close();

            Application.Exit();


        }

        private void todoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DsGuardar = new dsGuardar();
            //Recorrer las listas

            //Standar 
            if(listBox1.Items.Count > 0)
            {
                for(int i = 0; i<listBox1.Items.Count; i++)
                {
                    string fila = "";
                    fila = (string) listBox1.Items[i];
                    insertar(fila, 0);
                }
            }

            //Extendido
            if(listBox2.Items.Count > 0)
            {
                for(int i = 0; i<listBox2.Items.Count; i++)
                {
                    string fila = "";
                    fila = (string) listBox2.Items[i];
                    insertar(fila, 1);
                }
            }

            //Resumen
            DsGuardar.Resumen.Merge(dsMensajes.mensajes);//Une dos tablas, pero al estar Resumen vacia, la rellena desde 0 siempre que tenga los mismos campos

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML File| *.xml";
            saveFileDialog1.Title = "Guardar en XML";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                DsGuardar.WriteXml(saveFileDialog1.FileName);//Guarda un XML para la lista Resumen
            }
        }

        //Inserta en la tabla correspondiente los datos extraidos de la fila
        //0 para Standar, 1 para Extendidos
        public void insertar(string _fila, int _opc)
        {
            string datos_fila = quitarEspacios(_fila);

            string tiempo = datos_fila.Substring(0, 6);
            string id = datos_fila.Substring(7, 8);
            string datos = datos_fila.Substring(15, 16);

            //Insertar en data table
            if(_opc == 0){
                //DsGuardar.ListaStandar.TiempoColumn.ColumnName = tiempo;
                //DsGuardar.ListaStandar.IdColumn.ColumnName = id;
                //DsGuardar.ListaStandar.DatosColumn.ColumnName = datos;
                //DsGuardar.ListaStandar.TipoColumn.ColumnName = "0";

                DsGuardar.ListaStandar.AddListaStandarRow(tiempo, id, datos, "0");
            }else if (_opc == 1){
                //DsGuardar.ListaExtendido.TiempoColumn.ColumnName = tiempo;
                //DsGuardar.ListaExtendido.IdColumn.ColumnName = id;
                //DsGuardar.ListaExtendido.DatosColumn.ColumnName = datos;
                //DsGuardar.ListaExtendido.TipoColumn.ColumnName = "1";

                DsGuardar.ListaExtendido.AddListaExtendidoRow(tiempo, id, datos, "1");
            }
        }
        
        //Quita los espacios de la fila
        public string quitarEspacios(string _in)
        {
            string retorno = "";

            for(int i=0; i<_in.Length; i++)
            {
                if(_in[i] != ' ')
                {
                    retorno += _in[i];
                }
            }

            return retorno;
        }

        private void todoToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DsGuardar = new dsGuardar();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML File| *.xml";
            openFileDialog1.Title = "Abrir XML";
            openFileDialog1.ShowDialog();
            try{
                if (openFileDialog1.FileName != "")
                {
                    DsGuardar.ReadXml(openFileDialog1.FileName);//abre un XML para la lista Resumen
                }

                //Recargar las Listas

                for (int i = 0; i < DsGuardar.ListaStandar.Count; i++)
                {
                    string fila = "0" + DsGuardar.ListaStandar[i].Tiempo + "   " + separa(DsGuardar.ListaStandar[i].Id) + "  " + separa(DsGuardar.ListaStandar[i].Datos);
                    listBox1.Items.Add(fila);
                }

                for (int i = 0; i < DsGuardar.ListaExtendido.Count; i++)
                {
                    string fila = "0" + DsGuardar.ListaExtendido[i].Tiempo + "   " + separa(DsGuardar.ListaExtendido[i].Id) + "  " + separa(DsGuardar.ListaExtendido[i].Datos);
                    listBox2.Items.Add(fila);
                }

                dsMensajes.mensajes.Clear();
                dsMensajes.mensajes.Merge(DsGuardar.Resumen);
            }catch(Exception abrir){}
        }

        //Introduce espacios
        public string separa(string _in)
        {
            string retorno = "";

            for (int i = 0; i < _in.Length; i+=2 )
            {
                retorno += _in[i].ToString() + _in[i+1].ToString() + " ";
            }

            retorno = retorno.Trim();//Borra el ultimo espacio

            return retorno;
        }

    }

}

