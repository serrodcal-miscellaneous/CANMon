using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CANMon
{
    public partial class TransPer : Form
    {
        public Thread t;
        
        public TransPer()
        {
            InitializeComponent();
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

            if (getCadenaDatos() != "")
            {
                if (radioButton1.Checked == true)
                {
                    SetEmptyToZero(1);
                    borraDatosMal();
                    if (compruebaSiIdHex() == false)
                        dataGridView1.CurrentRow.Cells[1].ErrorText = "El periódo debe de ser un numero positivo.";
                    else
                        dataGridView1.CurrentRow.Cells[1].ErrorText = "";
                    if (compruebaPeriodo() == false)
                        dataGridView1.CurrentRow.Cells[10].ErrorText = "El ID escrito no coincide con el rango hexadecimal.";
                    else
                        dataGridView1.CurrentRow.Cells[10].ErrorText = "";
                }
                else
                {
                    SetRowToUpperAndSetZero();
                    if (compruebaSiTipoHex() == false)
                    {
                        dataGridView1.CurrentRow.Cells[0].ErrorText = "El el tipo de dato escrito no coincide. Debe ser 0 ó 1.";
                        MessageBox.Show("El el tipo de dato escrito no coincide. Debe ser 0 ó 1.", "ADVERTENCIA");
                    }
                    else
                        dataGridView1.CurrentRow.Cells[0].ErrorText = "";

                    if (compruebaSiIdHex() == false)
                        borraIdMal();
                    else
                        quitaErrorID();
                    if (compruebaPeriodo() == false)
                        dataGridView1.CurrentRow.Cells[10].ErrorText = "El periódo debe de ser un numero positivo.";
                    else
                        dataGridView1.CurrentRow.Cells[10].ErrorText = "";
                    
                    if (compruebaSiDatosHex() == false)
                        borraDatosHexMal();
                    else
                        quitaErrorDatos();
                }
            }

         }

        private bool compruebaPeriodo()
        {
            bool bandera = true;

            char [] arChar = dataGridView1.CurrentRow.Cells[10].Value.ToString().ToCharArray();
            foreach (char letra in arChar)
            {
                if (Convert.ToInt32(letra) < 48 | Convert.ToInt32(letra) > 57) bandera = false;
            }
            if (dataGridView1.CurrentRow.Cells[10].Value.ToString() == "") bandera = false;

            return bandera;
        }

private void quitaErrorID()
{
      dataGridView1.CurrentRow.Cells[1].ErrorText = "";     
}

        private void borraIdMal()
        {
                
                    dataGridView1.CurrentRow.Cells[1].ErrorText = "El ID escrito no coincide con el rango hexadecimal.";
            
        }

        private bool compruebaSiTipoHex()
        {
            bool banderaFallado = false;
            if (dataGridView1.CurrentRow.Cells[0].Value.ToString() == "0" | dataGridView1.CurrentRow.Cells[0].Value.ToString() == "1") banderaFallado = true ;
            return banderaFallado;
        }

        private void quitaErrorDatos()
        {
            for (int i = 2; i < dataGridView1.CurrentRow.Cells.Count-1; i++)
            {
                dataGridView1.CurrentRow.Cells[i].ErrorText = "";
            }
        }

        /// <summary>
        /// Pone a cadena vacia los datos que no esten en el rango Hexadecimal
        /// </summary>
        private void borraDatosHexMal()
        {
            bool bandera = false;
            for (int i = 2; i <= 9; i++)
            {
                if (IsHex(dataGridView1.CurrentRow.Cells[i].Value.ToString()) == false)
                {
                    dataGridView1.CurrentRow.Cells[i].ErrorText = "Los datos escritos no coinciden con el rango hexadecimal.";
                  //  dataGridView1.CurrentRow.Cells[i].Value = "";
                    bandera = true;
                }
            }
            if (bandera == true) MessageBox.Show("Existen datos erroneos, compruebelos.", "INFORMACION");
        }
        //SIRVE
        private void borraDatosMal()
        {
            bool bandera = false;
            for (int i = 2; i <= 9; i++)
            {
                foreach (char letra in dataGridView1.CurrentRow.Cells[i].Value.ToString())
                {
                    if (IsNum(letra)==false)
                    {
                        dataGridView1.CurrentRow.Cells[i].ErrorText = "Los datos escritos no coinciden con el rango decimal.";
                        bandera = true;
                    }
                }
            }
            if (bandera == true) MessageBox.Show("Existen datos erroneos, compruebelos.", "INFORMACION");
        }

        private bool IsNum(char letra)  
        {
          bool res;
            switch (letra.ToString())
                {
                       case "0":
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                            res = true;
                            break;
                        default:
                            res = false;
                            break;
                }
            return res;
        }




        // pone tantos ceros como se especifique en cada celda de datos vacia
        private void SetEmptyToZero(int num)
        {
            dataGridView1.CurrentRow.Cells[1].Value = dataGridView1.CurrentRow.Cells[1].Value.ToString().PadLeft(8, '0');
            for (int i = 2; i <= 9; i++)
            {
                dataGridView1.CurrentRow.Cells[i].Value = dataGridView1.CurrentRow.Cells[i].Value.ToString().PadLeft(num, '0');
            }
        }
        //private void cosa(int[] arr)
        //{
        //    for (int i = 0; i < arr.Length; i++)
        //    {
        //        dataGridView1.CurrentRow.Cells[arr[i]].Value = "";
        //    }
        //}
        private string getCadenaDatos()
        {
            string cadena = null;
            for (int i = 0; i < dataGridView1.CurrentRow.Cells.Count; i++)
            {
                cadena += dataGridView1.CurrentRow.Cells[i].Value.ToString();
            }
            return cadena;
        }

        private void SetRowToUpperAndSetZero()
        {
            dataGridView1.CurrentRow.Cells[1].Value = dataGridView1.CurrentRow.Cells[1].Value.ToString().ToUpper().PadLeft(8, '0');
            for (int i = 2; i <= 9; i++)
            {
                dataGridView1.CurrentRow.Cells[i].Value = dataGridView1.CurrentRow.Cells[i].Value.ToString().ToUpper().PadLeft(2, '0'); 
            }
        }

        private bool compruebaSiDatosHex()
        {
            bool banderaFallado = true;
            for (int i = 2; i <= 9 & banderaFallado == true; i++)
            {
                banderaFallado = IsHex(dataGridView1.CurrentRow.Cells[i].Value.ToString());
            }
            return banderaFallado;
        }
        private bool compruebaSiDatosHex(int numRow)
        {
            bool banderaFallado = true;
            for (int i = 2; i <= 9 & banderaFallado == true; i++)
            {
                banderaFallado = IsHex(dataGridView1.Rows[numRow].Cells[i].Value.ToString());
            }
            return banderaFallado;
        }

        private bool compruebaSiIdHex()
        {
            bool banderaFallado = true;
                banderaFallado = IsHex(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                if (dataGridView1.CurrentRow.Cells[1].Value.ToString() == "" | dataGridView1.CurrentRow.Cells[1].Value.ToString() == "00000000") banderaFallado = false;
            return banderaFallado;
        }
        private bool compruebaSiIdHex(int numRow)
        {
            bool banderaFallado = true;
                banderaFallado = IsHex(dataGridView1.Rows[numRow].Cells[1].Value.ToString());
                if (dataGridView1.Rows[numRow].Cells[1].Value.ToString() == "") banderaFallado = false;
            return banderaFallado;
        }

                                 //SIRVE\\
        /*****************************************************************/
        /*                                                               */
        /*   BitConverter.GetBytes(ushort.Parse( ---poner cosas--- ));   */
        /*                                                               */
        /*****************************************************************/


        private void transmitirMensajesSerialPort()
        {
            throw new NotImplementedException();
        }

        private void TransPer_Load(object sender, EventArgs e)
        {
            

           // this.dsMensajesPeriodicos1 = Program.gFrmPpal.dsMensajesPeriodicos;
            this.mensajesPeriodicosBindingSource.DataSource = Program.gFrmPpal.dsMensajesPeriodicos.MensajesPeriodicos;
            for (int i = Program.gFrmPpal.dsMensajesPeriodicos.MensajesPeriodicos.Count; i < 10; i++)
            {
                Program.gFrmPpal.dsMensajesPeriodicos.MensajesPeriodicos.AddMensajesPeriodicosRow(1, "", "","","","","","","","","");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.gFrmPpal.dsMensajesPeriodicos.MensajesPeriodicos.Clear();
            for (int i = 0; i < 10; i++)
            {
                Program.gFrmPpal.dsMensajesPeriodicos.MensajesPeriodicos.AddMensajesPeriodicosRow(1, "", "","","","","","","","","");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool res = false;
            if (t == null || !t.IsAlive)
            {
                List<byte[]> ArMensaje = new List<byte[]>();

                pasa_byte(ArMensaje);

                ThreadStart ts = delegate
                {

                    for (int k = 0; k < ArMensaje.Count - 1; k++)
                    {
                        if (ArByteIsNotNull(ArMensaje[k]))
                        {
                            res = true;
                        }
                    }
                    if (res == true)
                        transmitirMensajeSerialPort(ArMensaje);

                    t = null;
                    res = false;
                };

                t = new Thread(ts);

                t.Start();
            }
            else
                MessageBox.Show("Espere unos segundos el mensaje anterior se está enviando.","INFORMACIÓN");
        }

        //private void inicializarArray(List<byte[]> ArMensaje)
        //{
        //    for (int i = 0; i < ArMensaje.Length; i++)
        //        ArMensaje[i] = new object[15];
        //}

       
        private bool compruebaSiErrores( int i)
        {
            bool res = false;
            for (int j = 0; j < dataGridView1.Rows[0].Cells.Count - 1; j++ )
            {
                if (dataGridView1.Rows[i].Cells[j].ErrorText != "")
                    res = true;
            }
            return res;
        }

        //private string componeCadena(int numRow)
        //{
        //    string cadena = "";
        //    cadena = dataGridView1.Rows[numRow].Cells[i].Value.ToString();
        //}


        //private void pasa_byte(byte[] aB, int numRow)
        //{
        //    aB[0] = byte.Parse(dataGridView1.Rows[numRow].Cells[0].Value.ToString());
        //    aB[1] = byte.Parse(dataGridView1.Rows[numRow].Cells[1].Value.ToString().Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        //    aB[2] = byte.Parse(dataGridView1.Rows[numRow].Cells[1].Value.ToString().Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        //    aB[3] = byte.Parse(dataGridView1.Rows[numRow].Cells[1].Value.ToString().Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //    aB[4] = byte.Parse(dataGridView1.Rows[numRow].Cells[1].Value.ToString().Substring(6, 2), System.Globalization.NumberStyles.HexNumber);


        //    for (int i = 2, j = 5; i < dataGridView1.Rows[numRow].Cells.Count - 1; i++, j++)
        //    {
        //        aB[j] = byte.Parse(dataGridView1.Rows[numRow].Cells[i].Value.ToString(), System.Globalization.NumberStyles.HexNumber);
        //    }
        //    byte[] aB2 = BitConverter.GetBytes(ushort.Parse(dataGridView1.Rows[numRow].Cells[10].Value.ToString(), System.Globalization.NumberStyles.Number));
        //    aB[13] = aB2[0];
        //    aB[14] = aB2[1];

        //}




        private void pasa_byte(List<byte []> Ar)
        {
            
            for (int numRow = 0; numRow < dataGridView1.Rows.Count - 1; numRow++)
            {
                byte[] aB = new byte[15];
                if (estanTodosCampos(numRow) == true & compruebaSiErrores(numRow) == false)
                {
                    aB[0] = byte.Parse(dataGridView1.Rows[numRow].Cells[0].Value.ToString());
                    aB[1] = byte.Parse(dataGridView1.Rows[numRow].Cells[1].Value.ToString().Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    aB[2] = byte.Parse(dataGridView1.Rows[numRow].Cells[1].Value.ToString().Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    aB[3] = byte.Parse(dataGridView1.Rows[numRow].Cells[1].Value.ToString().Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    aB[4] = byte.Parse(dataGridView1.Rows[numRow].Cells[1].Value.ToString().Substring(6, 2), System.Globalization.NumberStyles.HexNumber);


                    for (int i = 2, j = 5; i < dataGridView1.Rows[numRow].Cells.Count - 1; i++, j++)
                    {
                        aB[j] = byte.Parse(dataGridView1.Rows[numRow].Cells[i].Value.ToString(), System.Globalization.NumberStyles.HexNumber);
                    }
                    byte[] aB2 = BitConverter.GetBytes(ushort.Parse(dataGridView1.Rows[numRow].Cells[10].Value.ToString(), System.Globalization.NumberStyles.Number));
                    aB[13] = aB2[1];
                    aB[14] = aB2[0];
                }

                Ar.Add(aB);
            }

        }



private bool estanTodosCampos(int numRow)
{
    bool res = false;
 	string cadena="";
    for(int i=1; i<dataGridView1.Rows[numRow].Cells.Count - 1; i++)
    {
        cadena+=dataGridView1.Rows[numRow].Cells[i].Value.ToString();
    }
    if (cadena == "")
        res =false;
    else
        res=true;

    return res;
}


private void transmitirMensajeSerialPort(List <byte []> ArMensaje)
{
    Program.gFrmPpal.serialPort1.ReadExisting();
    Program.gFrmPpal.serialPort1.ReadTimeout = 3000;
    Thread tRecep = Program.gFrmPpal.tRecepcion;

    Program.gFrmPpal.hiloActivo = false;
    //tRecep.Abort();
    
    while (Program.gFrmPpal.tRecepcion.IsAlive) ;
    
    enviaT();

    Program.gFrmPpal.serialPort1.ReadTimeout = -1;
    cambiarVelocidadPuerto(9600);

    Thread.Sleep(1500);
    for (int k = 0; k < ArMensaje.Count - 1; k++ )
    {
        if (ArByteIsNotNull(ArMensaje[k]))
        {
            
            Program.gFrmPpal.serialPort1.Write(ArMensaje[k], 0, ArMensaje[k].Length);
        }
        if (k < ArMensaje.Count - 1 && ArByteIsNotNull(ArMensaje[k + 1]))
            enviaT();
    }
    
    Program.gFrmPpal.serialPort1.Write(new Char[] { 'F' }, 0, 1);

    cambiarVelocidadPuerto(Program.velocidadPuerto);

    Program.gFrmPpal.tRecepcion = new Thread(new ThreadStart(Program.gFrmPpal.InicioRecepcion));
    Program.gFrmPpal.hiloActivo = true;
    Program.gFrmPpal.tRecepcion.Start();
    Program.gFrmPpal.serialPort1.Write(new Char[] { 'P' }, 0, 1);
}

private void enviaT()
{
    string leido = "";
    //leido[0] = 'a';

    do
    {
        try
        {
            Program.gFrmPpal.serialPort1.Write(new Char[] { 'T' }, 0, 1);
            // int i = Program.gFrmPpal.serialPort1.BytesToRead;
            Application.DoEvents();
            leido = Program.gFrmPpal.serialPort1.ReadTo("T");//leido, 0, 1);
        }
        catch (Exception ex)
        {

        }


    } while (leido == string.Empty);
    Program.gFrmPpal.serialPort1.ReadExisting();
}

private bool ArByteIsNotNull(byte[] p)
{
    bool res = false;
    string cadena = p[1].ToString() + p[2].ToString() + p[3].ToString() + p[4].ToString();
    if (cadena != "0000")
        res = true;
    return res;
}


private static void cambiarVelocidadPuerto(int num) //cambia la velocidad del puerto serie.
{
    Program.gFrmPpal.serialPort1.Close();
    Program.gFrmPpal.serialPort1.BaudRate = num;
    Program.gFrmPpal.serialPort1.Open();
}

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            idDataGridViewTextBoxColumn.MaxInputLength = 8;
            ChangeLengthTextBoxColum(2);
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ChangeLengthTextBoxColum(3);
        }

        // cambia el numero maximo de caracteres que puede ser introducido en el textbox(en el DataGridView)
        private void ChangeLengthTextBoxColum(int num)
        {
            dataGridViewTextBoxColumn1.MaxInputLength = num;
            dataGridViewTextBoxColumn2.MaxInputLength = num;
            dataGridViewTextBoxColumn3.MaxInputLength = num;
            dataGridViewTextBoxColumn4.MaxInputLength = num;
            dataGridViewTextBoxColumn5.MaxInputLength = num;
            dataGridViewTextBoxColumn6.MaxInputLength = num;
            dataGridViewTextBoxColumn7.MaxInputLength = num;
            dataGridViewTextBoxColumn8.MaxInputLength = num;
            
        }

        
        /// <summary>
        /// Comprueba que la cadena de aparametro tenga todos sus caracteres dentro del rango de numero Hexadecimal
        /// </summary>
        /// <param name="e">Cadena de datos.</param>
        /// <returns> bool </returns>
        public bool IsHex(string e)
        {
            //si es hexadecimal devuelve true
            bool res = true;
            char[] cadena = e.ToCharArray();
            foreach (char letra in cadena)
            {
                if (res == true)
                {
                    switch (letra.ToString().ToUpper())
                    {
                        case "A":
                        case "B":
                        case "C":
                        case "D":
                        case "E":
                        case "F":
                        case "0":
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                            res = true;
                            break;
                        default:
                            res = false;
                            break;
                    }
                }
            }
            return res;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentRow.Cells[0].Value = 1;
            dataGridView1.CurrentRow.Cells[0].ErrorText = "";
            for (int i = 1; i < dataGridView1.CurrentRow.Cells.Count; i++)
            {
                dataGridView1.CurrentRow.Cells[i].Value = "";
                dataGridView1.CurrentRow.Cells[i].ErrorText = "";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Program.gFrmPpal.serialPort1.Write(new Char[] { 'D' }, 0, 1);
        }

   }
}
