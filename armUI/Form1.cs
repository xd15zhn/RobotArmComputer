using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO.Ports;

namespace armUI
{
    public partial class Form1 : Form
    {
        private const string version = "Robot Arm V0.01";
        private string[] LastPorts = { };
        private bool sp1Open;
        private TrackBar[] trbServo = new TrackBar[8];
        private TextBox[] tbxServo = new TextBox[8];
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                trbServo[i] = new TrackBar
                {
                    Name = $"trbServo{i}",
                    Maximum = 100,
                    Value = 50,
                    Location = new Point(94, 41 + 51 * i),
                    Size = new Size(334, 45),
                    Tag = i 
                };
                trbServo[i].Scroll += new EventHandler(ServoVal_Change);
                Controls.Add(trbServo[i]);
                tbxServo[i] = new TextBox
                {
                    Name = $"tbxServo{i}",
                    Text = "1500",
                    Location = new Point(435, 44 + 51 * i),
                    Size = new Size(100, 23),
                    Tag = i 
                };
                Controls.Add(tbxServo[i]);
            }
            cbxBaudRate1.Text = "38400";
            lblVersion.Text = version;
        }
        private void btnOpen1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                SerialPort1_Close();
                return;
            }
            try
            {
                if (cbxPort1.Text == "") return;
                if (cbxBaudRate1.Text == "") return;
                serialPort1.PortName = cbxPort1.Text;
                serialPort1.BaudRate = Convert.ToInt32(cbxBaudRate1.Text);
                serialPort1.Open();
                SerialPort1_Open();
            }
            catch (Exception ex)
            {
                lblVersion.Text = "串口打开失败!" + ex.Message;
            }
        }
        /***********************
         打开主串口后需要完成的一系列操作
         **********************/
        private void SerialPort1_Open()
        {
            btnOpen1.Image = Properties.Resources.ledon;
            btnOpen1.Text = "断开连接";
            cbxPort1.Enabled = false;
            cbxBaudRate1.Enabled = false;
            lblVersion.Text = version;
            tmrPortChk.Enabled = true;
            sp1Open = true;
        }
        /***********************
         关闭主串口后需要完成的一系列操作
         **********************/
        private void SerialPort1_Close()
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();
            cbxPort1.Enabled = true;
            cbxBaudRate1.Enabled = true;
            btnOpen1.Image = Properties.Resources.ledoff;
            btnOpen1.Text = "打开连接";
            tmrPortChk.Enabled = false;
            sp1Open = false;
        }
        private void btnCenter_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                trbServo[i].Value = 50;
                tbxServo[i].Text = "1500";
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                trbServo[i].Value = 50;
                tbxServo[i].Text = (trbServo[i].Value * 10 + 1000).ToString();
            }
        }
        private void ServoVal_Change(object sender, EventArgs e)
        {
            int n = (int)((TrackBar)sender).Tag;
            tbxServo[n].Text=(trbServo[n].Value*10+1000).ToString();
        }
        [DllImport("ForwardSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double EndPos_X(double theta1, double theta2, double theta3, double theta4);
        [DllImport("ForwardSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double EndPos_Y(double theta1, double theta2, double theta3, double theta4);
        [DllImport("ForwardSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double EndPos_Z(double theta1, double theta2, double theta3, double theta4);
        private void btnForward_Click(object sender, EventArgs e)
        {
            double t1 = (trbServo[0].Value - 50 )* 0.0314159265359;
            double t2 = (trbServo[1].Value - 50 )* 0.0314159265359;
            double t3 = (trbServo[2].Value - 50 )* 0.0314159265359;
            double t4 = (trbServo[3].Value - 50 )* 0.0314159265359;
            textBox1.Text = EndPos_X(t1, t2, t3, t4).ToString();
            textBox2.Text = EndPos_Y(t1, t2, t3, t4).ToString();
            textBox3.Text = EndPos_Z(t1, t2, t3, t4).ToString();
        }
        /*定时每秒检测端口状况*/
        private void tmrPortChk_Tick(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)  //没有可用端口
            {
                if (LastPorts.Length == 0) return;  //原因是一直没有可用端口                    
                SerialPort1_Close();  //原因是端口意外断开
                cbxPort1.Items.Clear();
                LastPorts = ports;
            }
            else  //有可用端口
            {
                if (Enumerable.SequenceEqual(ports, LastPorts))  //可用端口没有变化
                {
                    if ((serialPort1.IsOpen == false) && (sp1Open))  //端口短时间内断开重连
                        SerialPort1_Close();
                    if ((serialPort1.IsOpen == true) && (!sp1Open))  //未知原因
                        SerialPort1_Open();
                    return;
                }
                if (LastPorts.Length != 0)  //可用端口改变
                    cbxPort1.Items.Clear();
                foreach (string port in ports)  //扫描并添加可用端口
                    cbxPort1.Items.Add(port);
                LastPorts = ports;
                cbxPort1.Text =  ports[0];  //默认选择第一个可用端口
            }
        }
    }
}
