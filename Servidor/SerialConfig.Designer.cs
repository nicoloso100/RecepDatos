namespace Servidor
{
    partial class SerialConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label59 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "1) Seleccione el puerto serial:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(16, 30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "2) Pruebe la conexión:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Conectar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(20, 348);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Guardar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(101, 348);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(129, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Borrar configuracion";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(236, 348);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Salir";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tableLayoutPanel3);
            this.groupBox7.Location = new System.Drawing.Point(12, 139);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(268, 177);
            this.groupBox7.TabIndex = 8;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "3) Establezca la configuración puerto serial";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.label59, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label61, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label62, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label63, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label64, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.label65, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.comboBox2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.comboBox3, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.comboBox4, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.comboBox5, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.comboBox6, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.comboBox7, 1, 3);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(8, 13);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(253, 151);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // label59
            // 
            this.label59.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label59.Location = new System.Drawing.Point(3, 25);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(120, 23);
            this.label59.TabIndex = 2;
            this.label59.Text = "Parity";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label61
            // 
            this.label61.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label61.Location = new System.Drawing.Point(3, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(120, 23);
            this.label61.TabIndex = 0;
            this.label61.Text = "BaudRate";
            this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label62
            // 
            this.label62.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label62.Location = new System.Drawing.Point(3, 50);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(120, 23);
            this.label62.TabIndex = 3;
            this.label62.Text = "StopBits";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label63
            // 
            this.label63.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label63.Location = new System.Drawing.Point(3, 75);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(120, 23);
            this.label63.TabIndex = 4;
            this.label63.Text = "DataBits";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label64
            // 
            this.label64.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label64.Location = new System.Drawing.Point(3, 100);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(120, 23);
            this.label64.TabIndex = 5;
            this.label64.Text = "Handshake";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label65
            // 
            this.label65.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label65.Location = new System.Drawing.Point(3, 125);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(120, 23);
            this.label65.TabIndex = 6;
            this.label65.Text = "RtsEnable";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "EVEN",
            "MARK",
            "ODD",
            "NONE",
            "SPACE"});
            this.comboBox2.Location = new System.Drawing.Point(129, 28);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 8;
            this.comboBox2.Text = "NONE";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "NONE",
            "ONE",
            "ONE POINT FIVE",
            "TWO"});
            this.comboBox3.Location = new System.Drawing.Point(129, 53);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 9;
            this.comboBox3.Text = "NONE";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "NONE",
            "REQUEST TO SEND",
            "XONXOFF",
            "REQUESTTOSENDXONXOFF"});
            this.comboBox4.Location = new System.Drawing.Point(129, 103);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 21);
            this.comboBox4.TabIndex = 11;
            this.comboBox4.Text = "NONE";
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "TRUE",
            "FALSE"});
            this.comboBox5.Location = new System.Drawing.Point(129, 128);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(121, 21);
            this.comboBox5.TabIndex = 12;
            this.comboBox5.Text = "FALSE";
            // 
            // comboBox6
            // 
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "57600",
            "115200"});
            this.comboBox6.Location = new System.Drawing.Point(129, 3);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(121, 21);
            this.comboBox6.TabIndex = 13;
            this.comboBox6.Text = "1200";
            // 
            // comboBox7
            // 
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Items.AddRange(new object[] {
            "7",
            "8"});
            this.comboBox7.Location = new System.Drawing.Point(129, 78);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(121, 21);
            this.comboBox7.TabIndex = 14;
            this.comboBox7.Text = "8";
            // 
            // SerialConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 383);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SerialConfig";
            this.Text = "SerialConfig";
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.ComboBox comboBox7;
    }
}