using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CANMon
{
    public partial class TransIns : Form
    {
        dsMensajes.MensajesTransmitidosRow rowSeleccionado=null;
        public TransIns()
        {
            InitializeComponent();
        }

        private void TransIns_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.mensajesTransmitidosBindingSource.DataSource = Program.gFrmPpal.dsMensajes.MensajesTransmitidos;

            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.gFrmPpal.dsMensajes.MensajesTransmitidos.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            borraVentana();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.GetItemText(comboBox1.SelectedItem).Trim() != "" & radioButton1.Checked == true)
            {
                Program.gFrmPpal.dsMensajes.MensajesTransmitidos.RemoveMensajesTransmitidosRow(Program.gFrmPpal.dsMensajes.MensajesTransmitidos.FindByid(comboBox1.GetItemText(comboBox1.SelectedItem)));
                if (this.radioButton1.Checked == true)
                {
                    comboBox1.DataSource = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.Select("tipo = 1");
                    this.comboBox1.Text = "";
                }
            }
            if (comboBox1.GetItemText(comboBox2.SelectedItem).Trim() != "" & radioButton2.Checked == true)
            {
                Program.gFrmPpal.dsMensajes.MensajesTransmitidos.RemoveMensajesTransmitidosRow(Program.gFrmPpal.dsMensajes.MensajesTransmitidos.FindByid(comboBox1.GetItemText(comboBox2.SelectedItem)));
                if (this.radioButton2.Checked == true)
                {

                    comboBox2.DataSource = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.Select("tipo = 0");
                    this.comboBox2.Text = "";
                }
            }
            borraVentana();
        }

        private void borraVentana()
        {
            txDatos.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
                          
        }


        private void txDatos_Validated(object sender, EventArgs e)
        {
            txDatos.Text = txDatos.Text.ToUpper().PadLeft(2, '0');
        }

        public bool IsHex(char e)
        {
            //si es hexadecimal devuelve true
            bool res = false;
            switch (e.ToString().ToUpper())
            {
                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                    res = true;
                    break;
            }
            return res;
        }

        private void txDatos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RBHex.Checked == true)
            {
                char cosa = e.KeyChar;

                if (!(Char.IsDigit(e.KeyChar) | this.IsHex(e.KeyChar) | Char.IsControl(e.KeyChar)))
                    {
                        e.Handled = true;
                    }
            }
            else
            {
                if (!(Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            ///ENVIO AL PUERTO SERIE///

            try
            {
                transmitirMensajeSerialPort();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            this.comboBox1.SelectedIndexChanged -= new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox2.SelectedIndexChanged -= new EventHandler(this.comboBox2_SelectedIndexChanged);
            guardaMensajeInstantaneo();// guardo el mensaje enviado
            comboBox1.DataSource = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.Select("tipo = 1");
            comboBox2.DataSource = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.Select("tipo = 0");
            //Program.gFrmPpal.serialPort1.Write();
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);

             
            
            if (this.radioButton1.Checked == true)
            {
                this.comboBox1_SelectedIndexChanged(null, null);
            }
            if (this.radioButton2.Checked == true)
            {
                this.comboBox2_SelectedIndexChanged(null, null);
            }
            


        }

        private void transmitirMensajeSerialPort()
        {
            bool estabaAbierto = false;
            Program.gFrmPpal.serialPort1.ReadTimeout = 3000;
            if (Program.gFrmPpal.hiloActivo == true)
            {
                Program.gFrmPpal.serialPort1.ReadExisting();

                Program.gFrmPpal.hiloActivo = false;
                estabaAbierto = true;
            }
            else
                Program.gFrmPpal.serialPort1.Open();
            //Thread.Sleep(3000);

            int countLecturas = 0;
            char [] leido = new char [1];
            do 
            {
                try
                {
                    countLecturas++;
                    Program.gFrmPpal.serialPort1.Write(new Char[] { 'T' }, 0, 1);
                    Thread.Sleep(500);
                    Program.gFrmPpal.serialPort1.Read(leido, 0, 1);
                    
                }
                catch (TimeoutException ex)
                {
                    if(countLecturas == 5)
                        throw new Exception("No se reciben datos desde el cacharro");       
                }
                

            }while(leido[0] != 'T');

            Program.gFrmPpal.serialPort1.ReadTimeout = -1;
            cambiarVelocidadPuerto(9600);

            Thread.Sleep(200);
            Program.gFrmPpal.serialPort1.Write(componeMensaje(), 0, componeMensaje().Length);

            Program.gFrmPpal.serialPort1.Write(new Char[] { 'F' }, 0, 1);

            cambiarVelocidadPuerto(115200);
            Program.gFrmPpal.serialPort1.Close();
            if (estabaAbierto)
            {
                Program.gFrmPpal.tRecepcion = new Thread(new ThreadStart(Program.gFrmPpal.InicioRecepcion));
                Program.gFrmPpal.hiloActivo = true;
                Program.gFrmPpal.tRecepcion.Start();
                Thread.Sleep(1000);
                Program.gFrmPpal.serialPort1.Write("P");
            }

            //------------------------------//

            //Program.gFrmPpal.serialPort1.ReadExisting();
            //while ()
            //{
            //    Program.gFrmPpal.serialPort1.Write(new Char[] { 'T' }, 0, 1); 
            //}
            //Program.gFrmPpal.serialPort1.Write(componeMensaje(), 0, componeMensaje().Length);
            //Program.gFrmPpal.serialPort1.Write(new Char[] { 'F' }, 0, 1);
        }

        private static void cambiarVelocidadPuerto(int num) //cambia la velocidad del puerto serie.
        {
            Program.gFrmPpal.serialPort1.Close();
            Program.gFrmPpal.serialPort1.BaudRate = num;
            Program.gFrmPpal.serialPort1.Open();
        }

        private byte[] componeMensaje()
        {
            string datos = null;
            datos = recojeDatos(RBHex.Checked);// recojo los datos de los textboxs de datos
            byte tipo = getTipoForm();
            int num = 0;
            string id = completaId(tipo);
            //byte.Parse(datos);
            byte[] a_mensaje = new byte[15];
            a_mensaje[0] = tipo;
            if (tipo == 0)
            {
                num = 3;
            }
            else
            {
                num = 1;
            }
            pasa_byte(a_mensaje, id, num);
            pasa_byte(a_mensaje, datos, 5);

            return a_mensaje;
        }

        private void pasa_byte(byte[] aB, string cadena, int pos)
        {
            for (int i = 0; i < (cadena.Length / 2); i++, pos++)
            {
                aB[pos] = byte.Parse(cadena.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
        }

        private void guardaMensajeInstantaneo()
        {
            string datos = null;
            datos = recojeDatos(RBHex.Checked);// recojo los datos de los textboxs de datos
           // byte.Parse(datos);
           // string.Format("{0:X}", 255);
            byte tipo = getTipoForm();
            string id = completaId(tipo);//completa con 0 a la izquierda del id segun el tipo de mensaje
            grabaMensajeBD(datos, tipo, id);
        }

        private static void grabaMensajeBD(string datos, byte tipo, string id)
        {
            dsMensajes.MensajesTransmitidosRow rowBuscado = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.FindByid(id);
            if (rowBuscado == null)
            {
                Program.gFrmPpal.dsMensajes.MensajesTransmitidos.AddMensajesTransmitidosRow(id, datos, tipo);
            }
            else
            {
                rowBuscado.datos = datos;
            }
        }

        private byte getTipoForm()
        {
            byte res= 0;
            if (this.radioButton1.Checked == true)
            {
                res = 1;
            }
            if (this.radioButton2.Checked == true)
            {
                res = 0;
            }
            return res;
        }

        private string completaId(byte p)
        {
            string cadena;
            if (p == 1)
            {
                cadena = comboBox1.Text.PadLeft(8, '0').ToUpper();
            }
            else
            {
                cadena = comboBox2.Text.PadLeft(4, '0').ToUpper();
            }
            return cadena;
        }


        private string recojeDatos(bool hexMarcado)
        {
            string cadena = null;
            if (hexMarcado == false)
            {
                cadena = pasa_hex(txDatos.Text.PadLeft(2, '0')) +
                pasa_hex(textBox1.Text.PadLeft(2, '0')) + pasa_hex(textBox2.Text.PadLeft(2, '0')) +
                pasa_hex(textBox3.Text.PadLeft(2, '0')) + pasa_hex(textBox4.Text.PadLeft(2, '0')) +
                pasa_hex(textBox5.Text.PadLeft(2, '0')) + pasa_hex(textBox6.Text.PadLeft(2, '0')) +
                pasa_hex(textBox7.Text.PadLeft(2, '0'));
            }
            else
            {
                cadena = txDatos.Text.PadLeft(2, '0') + textBox1.Text.PadLeft(2, '0') + 
                textBox2.Text.PadLeft(2, '0') + textBox3.Text.PadLeft(2, '0') + 
                textBox4.Text.PadLeft(2, '0') + textBox5.Text.PadLeft(2, '0') + 
                textBox6.Text.PadLeft(2, '0') + textBox7.Text.PadLeft(2, '0');
            }
            return cadena;
        }

        private string pasa_hex(string cadena)
        {
            string datos = "";
                int num;
                num = int.Parse(cadena);
                datos += String.Format("{0:X}", num).PadLeft(2,'0');
            return datos;
            //

        }


        private void textBox1_Validated(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.ToUpper().PadLeft(2, '0');
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RBHex.Checked == true)
            {
                if (!(Char.IsDigit(e.KeyChar) | this.IsHex(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!(Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RBHex.Checked == true)
            {
                if (!(Char.IsDigit(e.KeyChar) | this.IsHex(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!(Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RBHex.Checked == true)
            {
                if (!(Char.IsDigit(e.KeyChar) | this.IsHex(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!(Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RBHex.Checked == true)
            {
                if (!(Char.IsDigit(e.KeyChar) | this.IsHex(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!(Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RBHex.Checked == true)
            {
                if (!(Char.IsDigit(e.KeyChar) | this.IsHex(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!(Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RBHex.Checked == true)
            {
                if (!(Char.IsDigit(e.KeyChar) | this.IsHex(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!(Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RBHex.Checked == true)
            {
                if (!(Char.IsDigit(e.KeyChar) | this.IsHex(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!(Char.IsDigit(e.KeyChar) | Char.IsControl(e.KeyChar)))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text.ToUpper().PadLeft(2, '0');
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            textBox3.Text = textBox3.Text.ToUpper().PadLeft(2, '0');
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            textBox4.Text = textBox4.Text.ToUpper().PadLeft(2, '0');
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            textBox5.Text = textBox5.Text.ToUpper().PadLeft(2, '0');
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            textBox6.Text = textBox6.Text.ToUpper().PadLeft(2, '0');
        }

        private void textBox7_Validated(object sender, EventArgs e)
        {
                    textBox7.Text = textBox7.Text.ToUpper().PadLeft(2,'0');
                   
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            rowSeleccionado = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.FindByid(comboBox1.Text);
            if (rowSeleccionado!=null) setForm(rowSeleccionado);
        }

        private void setForm(dsMensajes.MensajesTransmitidosRow rowSeleccionado)
        {
            if (Convert.ToBoolean(rowSeleccionado.tipo) == true)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }

            setDatos(rowSeleccionado.datos);
        }

        private void setDatos(string p)
        {
            txDatos.Text = p.Substring(0, 2);
            textBox1.Text = p.Substring(2, 2);
            textBox2.Text = p.Substring(4, 2);
            textBox3.Text = p.Substring(6, 2);
            textBox4.Text = p.Substring(8, 2);
            textBox5.Text = p.Substring(10, 2);
            textBox6.Text = p.Substring(12, 2);
            textBox7.Text = p.Substring(14, 2);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked == true)
            {
                comboBox1.DataSource = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.Select("tipo = 1");
                tableLayoutPanel1.Visible = true;
            }
            else
            {
                tableLayoutPanel1.Visible = false;
                borraVentana();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked == true)
            {
                comboBox2.DataSource = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.Select("tipo = 0");
                tableLayoutPanel2.Visible = true;
            }
            else
            {
                tableLayoutPanel2.Visible = false;
                borraVentana();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            rowSeleccionado = Program.gFrmPpal.dsMensajes.MensajesTransmitidos.FindByid(comboBox2.Text);
            if (rowSeleccionado != null) setForm(rowSeleccionado);
        }

        private void RBInt_CheckedChanged(object sender, EventArgs e)
        {
            if (RBInt.Checked == true)
            {
                cabiaTamañoDatos(3);//cambia el tamaño de las cajas de texto a 3
                if (rowSeleccionado != null) setDatosInAscii(rowSeleccionado.datos);
                
            }
            else
            {
                borraVentana();
            }
        }

        private void setDatosInAscii(string p)
        {
            txDatos.Text = pasa_ascii(p.Substring(0, 2));
            textBox1.Text = pasa_ascii(p.Substring(2, 2));
            textBox2.Text = pasa_ascii(p.Substring(4, 2));
            textBox3.Text = pasa_ascii(p.Substring(6, 2));
            textBox4.Text = pasa_ascii(p.Substring(8, 2));
            textBox5.Text = pasa_ascii(p.Substring(10, 2));
            textBox6.Text = pasa_ascii(p.Substring(12, 2));
            textBox7.Text = pasa_ascii(p.Substring(14, 2));
        }

        private string pasa_ascii(string cadena)
        {
            return Convert.ToChar(byte.Parse(cadena, System.Globalization.NumberStyles.HexNumber)).ToString();
        }

        private void cabiaTamañoDatos(int num)
        {
            txDatos.MaxLength = num;
            textBox1.MaxLength = num;
            textBox2.MaxLength = num;
            textBox3.MaxLength = num;
            textBox4.MaxLength = num;
            textBox5.MaxLength = num;
            textBox6.MaxLength = num;
            textBox7.MaxLength = num;
        }

        private void RBHex_CheckedChanged(object sender, EventArgs e)
        {
            if (RBHex.Checked == true)
            {
                cabiaTamañoDatos(2);//cambia el tamaño de las cajas de texto a 3
                if (rowSeleccionado != null) setDatos(rowSeleccionado.datos);
            }
            else
            {
                borraVentana();
            }
        }

        private void txDatos_TextChanged(object sender, EventArgs e)
        {
            if (txDatos.Text != "")
            {
                if(RBInt.Checked == true && int.Parse(txDatos.Text) > 255)
                        txDatos.Text = "255";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (RBInt.Checked == true && int.Parse(textBox1.Text) > 255)
                    txDatos.Text = "255";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (RBInt.Checked == true && int.Parse(textBox2.Text) > 255)
                    txDatos.Text = "255";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                if (RBInt.Checked == true && int.Parse(textBox3.Text) > 255)
                    txDatos.Text = "255";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                if (RBInt.Checked == true && int.Parse(textBox4.Text) > 255)
                    txDatos.Text = "255";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                if (RBInt.Checked == true && int.Parse(textBox5.Text) > 255)
                    txDatos.Text = "255";
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                if (RBInt.Checked == true && int.Parse(textBox6.Text) > 255)
                    txDatos.Text = "255";
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != "")
            {
                if (RBInt.Checked == true && int.Parse(textBox7.Text) > 255)
                    txDatos.Text = "255";
            }
        }

        






    }
}
