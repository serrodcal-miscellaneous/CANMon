using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CANMon
{
    class CConfig
    {
        public static dsconfig configuracion = new dsconfig();

        public static void cargar()
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "config.xml");

            CConfig.configuracion.configuracion.ReadXml(ruta);
            
        }
        public static void grabar()
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "config.xml");

            CConfig.configuracion.configuracion.WriteXml(ruta);

        }

    }
}
