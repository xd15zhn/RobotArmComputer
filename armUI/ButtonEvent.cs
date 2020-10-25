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
        private void Init_UI()
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
                lblServo[i] = new Label
                {
                    Name = $"lblServo{i}",
                    Text = "1500",
                    Location = new Point(434, 47 + 51 * i),
                    Size = new Size(35, 14),
                    Tag = i
                };
                Controls.Add(lblServo[i]);
                tbxServo[i] = new TextBox
                {
                    Name = $"tbxServo{i}",
                    Text = "0",
                    Location = new Point(475, 44 + 51 * i),
                    Size = new Size(100, 23),
                    Tag = i
                };
                Controls.Add(tbxServo[i]);
            }
        }
        /*归中按钮按下事件，所有舵机归中，用于测试*/
        private void btnCenter_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                trbServo[i].Value = 50;
                lblServo[i].Text = "1500";
                tbxServo[i].Text = "0.0000";
            }
        }
        /*复位按钮按下事件，机械臂回到初始位置*/
        private void btnReset_Click(object sender, EventArgs e)
        {
            trbServo[0].Value = 50;
            trbServo[1].Value = 50;
            trbServo[2].Value = 50;
            trbServo[3].Value = 50;
            trbServo[4].Value = 50;
            trbServo[5].Value = 50;
            trbServo[6].Value = 50;
            trbServo[7].Value = 50;
            for (int i = 0; i < 8; i++)
            {
                lblServo[i].Text = (trbServo[i].Value * 10 + 1000).ToString("0.0000");
                tbxServo[i].Text = ((trbServo[i].Value - 50) * 0.0314159265).ToString("0.0000");
            }
        }
        /*跟踪条拖动事件，重新计算舵机角度*/
        private void ServoVal_Change(object sender, EventArgs e)
        {
            int i = (int)((TrackBar)sender).Tag;
            lblServo[i].Text = (trbServo[i].Value * 10 + 1000).ToString();
            tbxServo[i].Text = ((trbServo[i].Value - 50) * 0.0314159265).ToString("0.0000");
        }
        /*更新按钮按下事件，根据舵机rad角度计算舵机pwm位置*/
        private void button1_Click(object sender, EventArgs e)
        {
            Servo_Update();
        }
        /*正解按钮按下事件，计算机械臂正运动学*/
        [DllImport("ForwardSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void Forward_Solve(double[] theta, double[] point);
        private void btnForward_Click(object sender, EventArgs e)
        {
            double[] jointAngle = new double[4];
            double[] EndPos = new double[3];
            Servo_Update();
            try
            {
                jointAngle[0] = Limit(Convert.ToDouble(tbxServo[0].Text), -1.5708, 1.5708);
                jointAngle[1] = Limit(Convert.ToDouble(tbxServo[1].Text), -1.5708, 1.5708);
                jointAngle[2] = Limit(Convert.ToDouble(tbxServo[2].Text), -1.5708, 1.5708);
                jointAngle[3] = Limit(Convert.ToDouble(tbxServo[3].Text), -1.5708, 1.5708);
                Forward_Solve(jointAngle, EndPos);
            }
            catch (DllNotFoundException)
            {
                MessageBox.Show("Warning: ForwardSolve.dll not found!");
            }
            catch (Exception) { EndPos[0] = EndPos[1] = EndPos[2] = 0; };
            textBox1.Text = EndPos[0].ToString("0.0000");
            textBox2.Text = EndPos[1].ToString("0.0000");
            textBox3.Text = EndPos[2].ToString("0.0000");
        }
        /*逆解按钮按下事件，计算机械臂逆运动学*/
        [DllImport("InverseSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool Inverse_Solve(double[] position, double[] theta);
        private void btnInverse_Click(object sender, EventArgs e)
        {
            double[] jointAngle = new double[4];
            double[] EndPos = new double[3];
            try
            {
                EndPos[0] = Convert.ToDouble(textBox1.Text);
                EndPos[1] = Convert.ToDouble(textBox2.Text);
                EndPos[2] = Convert.ToDouble(textBox3.Text);
                if (!Inverse_Solve(EndPos, jointAngle))
                    lblVersion.Text = "逆解不存在!";
            }
            catch (DllNotFoundException)
            {
                MessageBox.Show("Warning: InverseSolve.dll not found!");
            }
            catch (Exception) { jointAngle[0] = jointAngle[1] = jointAngle[2] = jointAngle[3] = 0; };
            tbxServo[0].Text = jointAngle[0].ToString("0.0000");
            tbxServo[1].Text = jointAngle[1].ToString("0.0000");
            tbxServo[2].Text = jointAngle[2].ToString("0.0000");
            tbxServo[3].Text = jointAngle[3].ToString("0.0000");
            Servo_Update();
        }

        private void Timing_Send()
        {
            if (!cbxSend.Checked) return;
            if (!serialPort1.IsOpen) return;
            Servo_Update();
            byte[] DataToSend = new byte[10];
            byte sum = 0x3C;
            DataToSend[0] = 0x3C;
            for (int i = 0; i < 8; i++)
            {
                DataToSend[i + 1] = (byte)trbServo[i].Value;
                sum += DataToSend[i + 1];
            }
            DataToSend[9] = sum;
            try
            {
                serialPort1.Write(DataToSend, 0, 10);
            }
            catch (Exception ex)
            {
                SerialPort1_Close();
                lblVersion.Text = "串口发送失败!" + ex.Message;
            }
        }
        /*界面更新*/
        private void Servo_Update()
        {
            int val;
            for (int i = 0; i < 8; i++)
            {
                try
                {
                    double temp = Limit(Convert.ToDouble(tbxServo[i].Text), -1.5708, 1.5708);
                    tbxServo[i].Text = temp.ToString();
                    val = (int)(temp / 0.0314159265 + 50.5);
                }
                catch (Exception)
                {
                    val = 50;
                    tbxServo[i].Text = "0";
                };
                trbServo[i].Value = val;
                lblServo[i].Text = (trbServo[i].Value * 10 + 1000).ToString();
            }
        }
        private double Limit(double num, double min, double max)
        {
            return num <= min ? (min) : (num >= max ? max : num);
        }
    }
}
