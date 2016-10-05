using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


namespace CANMon
{
    class cargaArchivo
    {
        string archivo = "";
        OpenFileDialog cargador = new OpenFileDialog();
        static CParse CParse = new CParse();

        public struct Mensaje
        {
            public string tipo;
            public string id;
            public string datos;
        };

        public void captaMensajes(int _tipo)
        {

            List<ECMensaje> list = new List<ECMensaje>();
            cargador.Filter = "xml files (*.xml)|*.xml";
            
            if(cargador.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    FileStream fs = new FileStream(cargador.FileName, FileMode.Open);

                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(list.GetType());

                    list = (List<ECMensaje>) x.Deserialize(fs);
                    
                    fs.Close();

                    if (_tipo == 1)
                    {
                        for (int i = 0; i < list.Count; i++)
                            Program.gFrmPpal.listBox2.Items.Add(list[i].getString());
                    }
                    else if (_tipo == 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                            Program.gFrmPpal.listBox1.Items.Add(list[i].getString());
                    }

                    MessageBox.Show("El proceso ha terminado", "INFORMACION");
                }
                catch(Exception)
                {
                }
            
            }

        }
    }
}
