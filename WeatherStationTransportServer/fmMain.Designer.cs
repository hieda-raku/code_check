namespace WeatherStationTransportServer
{
    partial class fmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmMain));
            this.spWind = new System.IO.Ports.SerialPort(this.components);
            this.btnTest = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.LocalIPTxt = new System.Windows.Forms.TextBox();
            this.LocalPortTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PortTxt = new System.Windows.Forms.TextBox();
            this.IPTxt = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnClrS = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.STxt = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnClrR = new System.Windows.Forms.Button();
            this.RTxt = new System.Windows.Forms.TextBox();
            this.timReadData = new System.Windows.Forms.Timer(this.components);
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // spWind
            // 
            this.spWind.PortName = "COM9";
            this.spWind.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.spWind_DataReceived);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(944, 419);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(94, 48);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1126, 419);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 51);
            this.button1.TabIndex = 1;
            this.button1.Text = "测试1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.LocalIPTxt);
            this.groupBox6.Controls.Add(this.LocalPortTxt);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.btnClose);
            this.groupBox6.Controls.Add(this.btnStart);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.PortTxt);
            this.groupBox6.Controls.Add(this.IPTxt);
            this.groupBox6.Location = new System.Drawing.Point(32, 60);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox6.Size = new System.Drawing.Size(1104, 119);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "设    置";
            // 
            // LocalIPTxt
            // 
            this.LocalIPTxt.Location = new System.Drawing.Point(129, 22);
            this.LocalIPTxt.Margin = new System.Windows.Forms.Padding(4);
            this.LocalIPTxt.Name = "LocalIPTxt";
            this.LocalIPTxt.Size = new System.Drawing.Size(135, 25);
            this.LocalIPTxt.TabIndex = 9;
            this.LocalIPTxt.Text = "192.168.1.16";
            // 
            // LocalPortTxt
            // 
            this.LocalPortTxt.Location = new System.Drawing.Point(129, 72);
            this.LocalPortTxt.Margin = new System.Windows.Forms.Padding(4);
            this.LocalPortTxt.Name = "LocalPortTxt";
            this.LocalPortTxt.Size = new System.Drawing.Size(135, 25);
            this.LocalPortTxt.TabIndex = 8;
            this.LocalPortTxt.Text = "51008";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(51, 76);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "端口号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "本机IP：";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(625, 70);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 29);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关  闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(625, 20);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 29);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "开  启";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(335, 76);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "端口号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(335, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "对方IP：";
            // 
            // PortTxt
            // 
            this.PortTxt.Location = new System.Drawing.Point(429, 72);
            this.PortTxt.Margin = new System.Windows.Forms.Padding(4);
            this.PortTxt.Name = "PortTxt";
            this.PortTxt.Size = new System.Drawing.Size(135, 25);
            this.PortTxt.TabIndex = 1;
            this.PortTxt.Text = "31246";
            // 
            // IPTxt
            // 
            this.IPTxt.Location = new System.Drawing.Point(429, 22);
            this.IPTxt.Margin = new System.Windows.Forms.Padding(4);
            this.IPTxt.Name = "IPTxt";
            this.IPTxt.Size = new System.Drawing.Size(135, 25);
            this.IPTxt.TabIndex = 0;
            this.IPTxt.Text = "180.168.27.10";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnClrS);
            this.groupBox8.Controls.Add(this.btnSend);
            this.groupBox8.Controls.Add(this.STxt);
            this.groupBox8.Location = new System.Drawing.Point(32, 394);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox8.Size = new System.Drawing.Size(775, 118);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "发送窗口";
            // 
            // btnClrS
            // 
            this.btnClrS.Location = new System.Drawing.Point(628, 69);
            this.btnClrS.Margin = new System.Windows.Forms.Padding(4);
            this.btnClrS.Name = "btnClrS";
            this.btnClrS.Size = new System.Drawing.Size(100, 29);
            this.btnClrS.TabIndex = 3;
            this.btnClrS.Text = "清  空";
            this.btnClrS.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(628, 25);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 29);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "发  送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // STxt
            // 
            this.STxt.Location = new System.Drawing.Point(56, 18);
            this.STxt.Margin = new System.Windows.Forms.Padding(4);
            this.STxt.Multiline = true;
            this.STxt.Name = "STxt";
            this.STxt.Size = new System.Drawing.Size(511, 92);
            this.STxt.TabIndex = 1;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnClrR);
            this.groupBox7.Controls.Add(this.RTxt);
            this.groupBox7.Location = new System.Drawing.Point(32, 207);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox7.Size = new System.Drawing.Size(1233, 179);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "接收窗口";
            // 
            // btnClrR
            // 
            this.btnClrR.Location = new System.Drawing.Point(1114, 26);
            this.btnClrR.Margin = new System.Windows.Forms.Padding(4);
            this.btnClrR.Name = "btnClrR";
            this.btnClrR.Size = new System.Drawing.Size(100, 29);
            this.btnClrR.TabIndex = 1;
            this.btnClrR.Text = "清  空";
            this.btnClrR.UseVisualStyleBackColor = true;
            this.btnClrR.Click += new System.EventHandler(this.btnClrR_Click);
            // 
            // RTxt
            // 
            this.RTxt.Location = new System.Drawing.Point(53, 16);
            this.RTxt.Margin = new System.Windows.Forms.Padding(4);
            this.RTxt.Multiline = true;
            this.RTxt.Name = "RTxt";
            this.RTxt.Size = new System.Drawing.Size(1011, 154);
            this.RTxt.TabIndex = 0;
            // 
            // timReadData
            // 
            this.timReadData.Interval = 1000;
            this.timReadData.Tick += new System.EventHandler(this.timReadData_Tick);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1475, 621);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnTest);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fmMain";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmMain_FormClosed);
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort spWind;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox LocalIPTxt;
        private System.Windows.Forms.TextBox LocalPortTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PortTxt;
        private System.Windows.Forms.TextBox IPTxt;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnClrS;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox STxt;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnClrR;
        private System.Windows.Forms.TextBox RTxt;
        private System.Windows.Forms.Timer timReadData;
    }
}

