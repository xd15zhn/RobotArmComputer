﻿using System;
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
        private const string version = "Robot Arm V1.00";
        private string[] LastPorts = { };
        private bool sp1Open;
        private TrackBar[] trbServo = new TrackBar[8];
        private TextBox[] tbxServo = new TextBox[8];
        private Label[] lblServo = new Label[8];
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cbxBaudRate1.Text = "38400";
            lblVersion.Text = version;
            Init_UI();
            SerialPort1_Close();
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
        /*打开主串口后需要完成的一系列操作*/
        private void SerialPort1_Open()
        {
            btnOpen1.Image = Properties.Resources.ledon;
            btnOpen1.Text = "断开连接";
            cbxPort1.Enabled = false;
            cbxBaudRate1.Enabled = false;
            lblVersion.Text = version;
            sp1Open = true;
        }
        /*关闭主串口后需要完成的一系列操作*/
        private void SerialPort1_Close()
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();
            cbxPort1.Enabled = true;
            cbxBaudRate1.Enabled = true;
            btnOpen1.Image = Properties.Resources.ledoff;
            btnOpen1.Text = "打开连接";
            sp1Open = false;
        }
        private void Timing_PortCheck()
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
                cbxPort1.Text = ports[0];  //默认选择第一个可用端口
            }
        }
        /*200ms定时任务:定时检测端口状况,定时发送舵机角度*/
        private void tmr200ms_Tick(object sender, EventArgs e)
        {
            Timing_PortCheck();
            Timing_Send();
        }
    }
}
