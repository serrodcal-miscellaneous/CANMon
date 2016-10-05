using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CANMon
{
    static class Program
    {
        public static Form1 gFrmPpal;
        public static cargaArchivo cargaArchivo = new cargaArchivo();
        public static string nombrePuerto = "";
        public static int velocidadPuerto = 0;
                /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
            
            //string hex = String.Format("{0:X}", 10).PadLeft(2, '0');
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CConfig.cargar();
            
            Program.gFrmPpal = new Form1();
            Application.Run(Program.gFrmPpal);
        }
    }
}
