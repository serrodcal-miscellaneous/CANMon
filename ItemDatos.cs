using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CANMon
{
    public partial class ItemDatos : UserControl
    {
        public ItemDatos()
        {
            InitializeComponent();
        }

        public void InsertarDatos(byte[] _datos)
        {
            this.txtB1.Text = _datos[0].ToString();
            this.txtB2.Text = _datos[1].ToString();
            this.txtB3.Text = _datos[2].ToString();
            this.txtB4.Text = _datos[3].ToString();
            this.txtB5.Text = _datos[4].ToString();
            this.txtB6.Text = _datos[5].ToString();
            this.txtB7.Text = _datos[6].ToString();
            //this.txtB8.Text = _datos[7].ToString();
        }


        public void InsertarDatos2(string[] p)
        {
            this.txtB1.Text = p[0].ToString();
            this.txtB2.Text = p[1].ToString();
            this.txtB3.Text = p[2].ToString();
            this.txtB4.Text = p[3].ToString();
            this.txtB5.Text = p[4].ToString();
            this.txtB6.Text = p[5].ToString();
            this.txtB7.Text = p[6].ToString();
            //this.txtB8.Text = p[7].ToString();
        }
    }
}
