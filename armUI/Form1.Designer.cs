namespace armUI
{
    partial class Form1
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
            this.btnCenter = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnInverse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnOpen1 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.cbxBaudRate1 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbxPort1 = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tmr250ms = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cbxSend = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.tmr50ms = new System.Windows.Forms.Timer(this.components);
            this.btnCatch = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.tmr500ms = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnCenter
            // 
            this.btnCenter.Location = new System.Drawing.Point(836, 169);
            this.btnCenter.Name = "btnCenter";
            this.btnCenter.Size = new System.Drawing.Size(100, 31);
            this.btnCenter.TabIndex = 0;
            this.btnCenter.Text = "归中";
            this.btnCenter.UseVisualStyleBackColor = true;
            this.btnCenter.Click += new System.EventHandler(this.btnCenter_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "肩关节";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "Joint1";
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point(836, 243);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(100, 31);
            this.btnForward.TabIndex = 0;
            this.btnForward.Text = "正解";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // btnInverse
            // 
            this.btnInverse.Location = new System.Drawing.Point(836, 280);
            this.btnInverse.Name = "btnInverse";
            this.btnInverse.Size = new System.Drawing.Size(100, 31);
            this.btnInverse.TabIndex = 0;
            this.btnInverse.Text = "逆解";
            this.btnInverse.UseVisualStyleBackColor = true;
            this.btnInverse.Click += new System.EventHandler(this.btnInverse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(14, 439);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "腕关节";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 21);
            this.label4.TabIndex = 5;
            this.label4.Text = "Joint2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 21);
            this.label5.TabIndex = 5;
            this.label5.Text = "Joint3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(12, 194);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 21);
            this.label6.TabIndex = 5;
            this.label6.Text = "Joint4";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(12, 245);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 21);
            this.label7.TabIndex = 5;
            this.label7.Text = "Joint5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(12, 296);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 21);
            this.label8.TabIndex = 5;
            this.label8.Text = "Joint6";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(12, 347);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 21);
            this.label9.TabIndex = 5;
            this.label9.Text = "Joint7";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(12, 398);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 21);
            this.label10.TabIndex = 5;
            this.label10.Text = "Joint8";
            // 
            // btnOpen1
            // 
            this.btnOpen1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpen1.Location = new System.Drawing.Point(529, 486);
            this.btnOpen1.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpen1.Name = "btnOpen1";
            this.btnOpen1.Size = new System.Drawing.Size(105, 32);
            this.btnOpen1.TabIndex = 10;
            this.btnOpen1.Text = "打开连接";
            this.btnOpen1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpen1.UseVisualStyleBackColor = true;
            this.btnOpen1.Click += new System.EventHandler(this.btnOpen1_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(638, 495);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 14);
            this.label11.TabIndex = 11;
            this.label11.Text = "波特率";
            // 
            // cbxBaudRate1
            // 
            this.cbxBaudRate1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBaudRate1.FormattingEnabled = true;
            this.cbxBaudRate1.Items.AddRange(new object[] {
            "9600",
            "38400",
            "57600",
            "115200"});
            this.cbxBaudRate1.Location = new System.Drawing.Point(695, 492);
            this.cbxBaudRate1.Margin = new System.Windows.Forms.Padding(2);
            this.cbxBaudRate1.Name = "cbxBaudRate1";
            this.cbxBaudRate1.Size = new System.Drawing.Size(92, 22);
            this.cbxBaudRate1.TabIndex = 9;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(791, 495);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 14);
            this.label12.TabIndex = 12;
            this.label12.Text = "端口";
            // 
            // cbxPort1
            // 
            this.cbxPort1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPort1.FormattingEnabled = true;
            this.cbxPort1.Location = new System.Drawing.Point(830, 492);
            this.cbxPort1.Margin = new System.Windows.Forms.Padding(2);
            this.cbxPort1.Name = "cbxPort1";
            this.cbxPort1.Size = new System.Drawing.Size(92, 22);
            this.cbxPort1.TabIndex = 8;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(836, 206);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 31);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(13, 505);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(56, 14);
            this.lblVersion.TabIndex = 13;
            this.lblVersion.Text = "label13";
            // 
            // tmr250ms
            // 
            this.tmr250ms.Enabled = true;
            this.tmr250ms.Interval = 250;
            this.tmr250ms.Tick += new System.EventHandler(this.tmr200ms_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(836, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "0.0000";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(836, 73);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 23);
            this.textBox2.TabIndex = 14;
            this.textBox2.Text = "0.0000";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(836, 102);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 23);
            this.textBox3.TabIndex = 14;
            this.textBox3.Text = "0.0000";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(813, 47);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 14);
            this.label13.TabIndex = 15;
            this.label13.Text = "x";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(813, 76);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(14, 14);
            this.label14.TabIndex = 15;
            this.label14.Text = "y";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(813, 105);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(14, 14);
            this.label15.TabIndex = 15;
            this.label15.Text = "z";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(832, 10);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(104, 19);
            this.label16.TabIndex = 5;
            this.label16.Text = "目标点坐标";
            // 
            // cbxSend
            // 
            this.cbxSend.AutoSize = true;
            this.cbxSend.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbxSend.Location = new System.Drawing.Point(442, 494);
            this.cbxSend.Name = "cbxSend";
            this.cbxSend.Size = new System.Drawing.Size(82, 18);
            this.cbxSend.TabIndex = 16;
            this.cbxSend.Text = "实时发送";
            this.cbxSend.UseVisualStyleBackColor = true;
            this.cbxSend.CheckedChanged += new System.EventHandler(this.cbxSend_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(434, 12);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(28, 14);
            this.label17.TabIndex = 17;
            this.label17.Text = "PWM";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(496, 12);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(28, 14);
            this.label18.TabIndex = 17;
            this.label18.Text = "rad";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(575, 12);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(21, 14);
            this.label19.TabIndex = 17;
            this.label19.Text = "DH";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(752, 12);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(35, 14);
            this.label20.TabIndex = 17;
            this.label20.Text = "反向";
            // 
            // tmr50ms
            // 
            this.tmr50ms.Interval = 50;
            this.tmr50ms.Tick += new System.EventHandler(this.tmr50ms_Tick);
            // 
            // btnCatch
            // 
            this.btnCatch.Location = new System.Drawing.Point(836, 317);
            this.btnCatch.Name = "btnCatch";
            this.btnCatch.Size = new System.Drawing.Size(100, 31);
            this.btnCatch.TabIndex = 0;
            this.btnCatch.Text = "抓取";
            this.btnCatch.UseVisualStyleBackColor = true;
            this.btnCatch.Click += new System.EventHandler(this.btnCatch_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(650, 12);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(63, 14);
            this.label21.TabIndex = 17;
            this.label21.Text = "设置参数";
            // 
            // tmr500ms
            // 
            this.tmr500ms.Interval = 2000;
            this.tmr500ms.Tick += new System.EventHandler(this.tmr500ms_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 525);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.cbxSend);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnOpen1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cbxBaudRate1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbxPort1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCatch);
            this.Controls.Add(this.btnInverse);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCenter);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCenter;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnInverse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnOpen1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbxBaudRate1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbxPort1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Timer tmr250ms;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox cbxSend;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Timer tmr50ms;
        private System.Windows.Forms.Button btnCatch;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Timer tmr500ms;
    }
}

