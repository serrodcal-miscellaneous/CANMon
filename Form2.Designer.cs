namespace CANMon
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txSA = new System.Windows.Forms.TextBox();
            this.txPrio = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txPS = new System.Windows.Forms.TextBox();
            this.txPF = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label23 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.button3 = new System.Windows.Forms.Button();
            this.vistaMuestraDatosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.decodificaion_mensajes_j1939DataSet = new CANMon.decodificaion_mensajes_j1939DataSet();
            this.vistaMuestraDatosTableAdapter = new CANMon.decodificaion_mensajes_j1939DataSetTableAdapters.VistaMuestraDatosTableAdapter();
            this.pgnTableAdapter1 = new CANMon.decodificaion_mensajes_j1939DataSetTableAdapters.PGNTableAdapter();
            this.parametersTableAdapter1 = new CANMon.decodificaion_mensajes_j1939DataSetTableAdapters.PARAMETERSTableAdapter();
            this.spnTableAdapter1 = new CANMon.decodificaion_mensajes_j1939DataSetTableAdapters.SPNTableAdapter();
            this.dsMensajes1 = new CANMon.dsMensajes();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vistaMuestraDatosBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decodificaion_mensajes_j1939DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsMensajes1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 456);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Salir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txSA);
            this.panel1.Controls.Add(this.txPrio);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txPS);
            this.panel1.Controls.Add(this.txPF);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 96);
            this.panel1.TabIndex = 30;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(208, 9);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(61, 17);
            this.label19.TabIndex = 44;
            this.label19.Text = "label19";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 33;
            this.label2.Text = "PGN";
            // 
            // txSA
            // 
            this.txSA.Location = new System.Drawing.Point(128, 54);
            this.txSA.Name = "txSA";
            this.txSA.ReadOnly = true;
            this.txSA.Size = new System.Drawing.Size(30, 20);
            this.txSA.TabIndex = 43;
            this.txSA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txPrio
            // 
            this.txPrio.Location = new System.Drawing.Point(21, 54);
            this.txPrio.Name = "txPrio";
            this.txPrio.ReadOnly = true;
            this.txPrio.Size = new System.Drawing.Size(31, 20);
            this.txPrio.TabIndex = 31;
            this.txPrio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(133, 38);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(21, 13);
            this.label18.TabIndex = 42;
            this.label18.Text = "SA";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Prio.";
            // 
            // txPS
            // 
            this.txPS.Location = new System.Drawing.Point(94, 54);
            this.txPS.Name = "txPS";
            this.txPS.ReadOnly = true;
            this.txPS.Size = new System.Drawing.Size(30, 20);
            this.txPS.TabIndex = 41;
            this.txPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txPF
            // 
            this.txPF.Location = new System.Drawing.Point(58, 54);
            this.txPF.Name = "txPF";
            this.txPF.ReadOnly = true;
            this.txPF.Size = new System.Drawing.Size(30, 20);
            this.txPF.TabIndex = 40;
            this.txPF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(99, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "PS";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(55, 12);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(48, 13);
            this.label16.TabIndex = 36;
            this.label16.Text = "label16";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "PF";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(125, 12);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 13);
            this.label17.TabIndex = 37;
            this.label17.Text = "label17";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel2.Controls.Add(this.button4);
            this.panel2.Location = new System.Drawing.Point(12, 114);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(661, 19);
            this.panel2.TabIndex = 31;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.Location = new System.Drawing.Point(642, 0);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(19, 19);
            this.button4.TabIndex = 0;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.textBox5);
            this.panel5.Controls.Add(this.textBox4);
            this.panel5.Controls.Add(this.textBox3);
            this.panel5.Controls.Add(this.textBox2);
            this.panel5.Controls.Add(this.textBox1);
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.label21);
            this.panel5.Controls.Add(this.label15);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Location = new System.Drawing.Point(679, 12);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(495, 438);
            this.panel5.TabIndex = 34;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox5.Location = new System.Drawing.Point(108, 184);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 13;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox4.Location = new System.Drawing.Point(108, 158);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 12;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox3.Location = new System.Drawing.Point(108, 132);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 11;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox2.Location = new System.Drawing.Point(108, 106);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox1.Location = new System.Drawing.Point(108, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 9;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(20, 187);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(40, 13);
            this.label22.TabIndex = 8;
            this.label22.Text = "Off Set";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(20, 161);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(57, 13);
            this.label21.TabIndex = 7;
            this.label21.Text = "Resolution";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(20, 135);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 13);
            this.label15.TabIndex = 6;
            this.label15.Text = "Length";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 109);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Start Position";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 83);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Info";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(219, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Nombre";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(105, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Nª (dec)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(20, 54);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "SPN";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(20, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "Detalles:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 139);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(661, 311);
            this.flowLayoutPanel1.TabIndex = 35;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.flowLayoutPanel2);
            this.panel3.Controls.Add(this.label23);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(661, 91);
            this.panel3.TabIndex = 33;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(58, 12);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(593, 65);
            this.flowLayoutPanel2.TabIndex = 17;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(12, 12);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(40, 13);
            this.label23.TabIndex = 16;
            this.label23.Text = "Datos";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.listView1);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Location = new System.Drawing.Point(3, 94);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(658, 206);
            this.panel4.TabIndex = 34;
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(30, 24);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(596, 97);
            this.listView1.TabIndex = 38;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Param. Name";
            this.columnHeader1.Width = 103;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "SPN";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Valor";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Rango";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Unidad";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(551, 127);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 37;
            this.button3.Text = "Exportar >>";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // vistaMuestraDatosBindingSource
            // 
            this.vistaMuestraDatosBindingSource.DataMember = "VistaMuestraDatos";
            this.vistaMuestraDatosBindingSource.DataSource = this.decodificaion_mensajes_j1939DataSet;
            // 
            // decodificaion_mensajes_j1939DataSet
            // 
            this.decodificaion_mensajes_j1939DataSet.DataSetName = "decodificaion_mensajes_j1939DataSet";
            this.decodificaion_mensajes_j1939DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // vistaMuestraDatosTableAdapter
            // 
            this.vistaMuestraDatosTableAdapter.ClearBeforeFill = true;
            // 
            // pgnTableAdapter1
            // 
            this.pgnTableAdapter1.ClearBeforeFill = true;
            // 
            // parametersTableAdapter1
            // 
            this.parametersTableAdapter1.ClearBeforeFill = true;
            // 
            // spnTableAdapter1
            // 
            this.spnTableAdapter1.ClearBeforeFill = true;
            // 
            // dsMensajes1
            // 
            this.dsMensajes1.DataSetName = "dsMensajes";
            this.dsMensajes1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Tiempo";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 491);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Datos";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vistaMuestraDatosBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decodificaion_mensajes_j1939DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsMensajes1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        //private decodificaion_mensajes_j1939DataSet2 decodificaion_mensajes_j1939DataSet1;
        //private CANMon.decodificaion_mensajes_j1939DataSet2TableAdapters.VistaMuestraDatosTableAdapter vistaMuestraDatosTableAdapter1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTARTPOSITIONDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lENGTHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nOMBREDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uNIDADDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rESOLUTIONDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPERATIONALRANGELOWDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPERATIONALRANGEHIGHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sPNDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pGNDataGridViewTextBoxColumn;
        private decodificaion_mensajes_j1939DataSet decodificaion_mensajes_j1939DataSet;
        private System.Windows.Forms.BindingSource vistaMuestraDatosBindingSource;
        private CANMon.decodificaion_mensajes_j1939DataSetTableAdapters.VistaMuestraDatosTableAdapter vistaMuestraDatosTableAdapter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label19;
        public System.Windows.Forms.TextBox txSA;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.TextBox txPS;
        public System.Windows.Forms.TextBox txPF;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txPrio;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button3;
        private CANMon.decodificaion_mensajes_j1939DataSetTableAdapters.PGNTableAdapter pgnTableAdapter1;
        private CANMon.decodificaion_mensajes_j1939DataSetTableAdapters.PARAMETERSTableAdapter parametersTableAdapter1;
        private System.Windows.Forms.Label label23;
        private CANMon.decodificaion_mensajes_j1939DataSetTableAdapters.SPNTableAdapter spnTableAdapter1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private dsMensajes dsMensajes1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ColumnHeader columnHeader6;

    }
}