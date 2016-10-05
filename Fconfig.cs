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
    public partial class Fconfig : Form
    {

        Boolean salir = false;

        public Fconfig()
        {
            this.InitializeComponent();
        }

        public int velocidad(string text)
        {
            if (text == "250")
                return 2;
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Si hay un item seleccionado y la caja de texto no esta vacia
            if (this.comboBox2.SelectedItem != null & this.textBox1.Text != "")
            {
                
                //Se obtiene el nombre del puerto del combobox 'puerto serie'
                Program.nombrePuerto = this.comboBox2.SelectedItem.ToString();
                //Se obtiene la velocidad del puerto
                Program.velocidadPuerto = int.Parse(this.textBox1.Text);


                //Se establece el nombre del puerto
                this.serialPort1.PortName = this.comboBox2.SelectedItem.ToString();
                //Se establece la velocidad del puerto serie
                this.serialPort1.BaudRate = int.Parse(textBox1.Text);
                this.serialPort1.ReadBufferSize = 40000;

                //Se abre el puerto serie
                this.serialPort1.Open();

                while (!this.serialPort1.IsOpen)
                    this.serialPort1.Open();

                this.serialPort1.ReadExisting();
                this.serialPort1.WriteLine("c");
                //se envia una c para confirmar que se está en modo configuración
                //Mientras bytes a leer sea 0
                while (this.serialPort1.ReadByte() != (int)'c')
                {
                    this.serialPort1.WriteLine("c");
                }

                Thread.Sleep(1000);

                //Si el combobox 'velocidad CAMBUS' es distinto cadena vacia y comprueba es distinto de caracter 0
                if (comboBox1.Text != "" & this.comprueba() != (char)0)
                {
                    byte[] auxiliar = new byte[2];
                    byte var = 50;//Hay que revisar
                    this.serialPort1.ReadExisting();
                    this.serialPort1.WriteLine("2");

                    int b1 = this.serialPort1.ReadByte();
                    while (b1 != var)
                    {
                        this.serialPort1.Write("2");

                        b1 = this.serialPort1.ReadByte();
                    }

                    this.serialPort1.ReadExisting();
                    this.serialPort1.WriteLine(comprueba().ToString());

                    int b = this.serialPort1.ReadByte();

                    while (b != (int) comprueba())
                    {
                        this.serialPort1.WriteLine(comprueba().ToString());

                        b = this.serialPort1.ReadByte();
                    }
                    this.serialPort1.ReadExisting();
                    this.Close();

                    CConfig.configuracion.configuracion[0].velocidad = this.comboBox1.Text;
                    CConfig.configuracion.configuracion[0].tipo = grabaTipoMensaje();
                    CConfig.configuracion.configuracion[0].velocidadPuerto = this.textBox1.Text;
                    CConfig.configuracion.configuracion[0].nombrePuerto = this.comboBox2.Text;
                    CConfig.grabar();
                }
            }
            else
                MessageBox.Show("Introduzca los datos necesarios para la configuración", "INFORMACIÓN");
        }


        private string pasaByteToString(byte _letra)
        {
            string s = "";
            s += string.Format("{0:X}", _letra).PadLeft(2, '0');
            return s;
        }

        private char grabaTipoMensaje()
        {
            char res=(char)0;

            if (radioButton1.Checked == true) res = 'S';
            if (radioButton2.Checked == true) res = 'E';
            if (radioButton3.Checked == true) res = 'A';

            return res;
        }

        //COMPRUENBA EL TIPO DE MENSAJE
        //0 sino es correcto
        //S si es estandar
        //E si es extendido
        //A si es extendido y estandar
        private char comprueba()
        {
            char res = (char)0;
            if (radioButton1.Checked == true) 
                res = 'S';
            if (radioButton2.Checked == true) 
                res = 'E';
            if (radioButton3.Checked == true) 
                res = 'A';
            return res;
        }

        private void Fconfig_Load(object sender, EventArgs e)
        {

            //Array de puertos
            String[] Puertos = System.IO.Ports.SerialPort.GetPortNames();

            //Si existen puertos
            if (Puertos.Length != 0)
            {
                //Limpiamos el combobox
                this.comboBox2.Items.Clear();
                //Añadimos los puertos
                this.comboBox2.Items.AddRange(Puertos);
                //Seleccionamos el primero ()
                this.comboBox2.SelectedIndex = 0;
                setForm();
            }   

        }

        private void setForm()
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.ControlBox = false;
            CConfig.cargar();
            this.comboBox1.Text = CConfig.configuracion.configuracion[0].velocidad;
            ponerTipoMensaje(CConfig.configuracion.configuracion[0].tipo);
            this.textBox1.Text = CConfig.configuracion.configuracion[0].velocidadPuerto;
            this.comboBox2.Text = CConfig.configuracion.configuracion[0].nombrePuerto;
        }

        private void ponerTipoMensaje(char p)
        {
            switch(p)
            {
                case 'S':
                    radioButton1.Checked = true;
                    break;

                case 'E':
                    radioButton2.Checked = true;
                    break;

                case 'A':
                    radioButton3.Checked = true;
                    break;
            }
        }

        private void Fconfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!salir)
            {
                Program.gFrmPpal.serialPort1.PortName = Program.nombrePuerto;
                Program.gFrmPpal.serialPort1.BaudRate = Program.velocidadPuerto;
                Program.gFrmPpal.serialPort1.ReadBufferSize = 40000;
            }
            this.serialPort1.Close();
        }

        private void serialPort1_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("Fallo al recibir");
        }

        private void serialPort1_PinChanged(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
            Console.WriteLine("Cambio en la paridad");
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            salir = true;
            this.Close();
        }


    }
}
