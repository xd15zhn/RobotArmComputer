using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace armUI
{
    public partial class Form1 : Form
    {
        private TrackBar[] trbServo = new TrackBar[8];  //8个拖动条
        private TextBox[] tbxServo = new TextBox[8];  //可编辑的8个舵机的rad角度
        private Label[] lblServo = new Label[8];  //显示8个舵机的PWM值
        private Label[] lblDH = new Label[8];  //显示DH参数中的theta角度
        private CheckBox[] cbxReverse = new CheckBox[8];  //显示DH参数中的theta角度
        private double[] ResetJoint = new double[8];  //舵机归中时的关节DH参数角度
        double[] DHangle = new double[4];  //关节实时DH参数角度
        private void Init_UI()
        {
            ResetJoint[0] = Properties.Settings.Default.setResetJoint0;
            ResetJoint[1] = Properties.Settings.Default.setResetJoint1;
            ResetJoint[2] = Properties.Settings.Default.setResetJoint2;
            ResetJoint[3] = Properties.Settings.Default.setResetJoint3;
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
                };
                Controls.Add(lblServo[i]);
                tbxServo[i] = new TextBox
                {
                    Name = $"tbxServo{i}",
                    Text = "0.0000",
                    Location = new Point(475, 44 + 51 * i),
                    Size = new Size(80, 23),
                };
                Controls.Add(tbxServo[i]);
                lblDH[i] = new Label
                {
                    Name = $"lblDH{i}",
                    Text = ResetJoint[i].ToString("0.0000"),
                    Location = new Point(575, 47 + 51 * i),
                    Size = new Size(70, 14),
                };
                Controls.Add(lblDH[i]);
                cbxReverse[i] = new CheckBox
                {
                    Text = "",
                    Location = new Point(660, 44 + 51 * i),
                    Size = new Size(22, 22),
                };
                Controls.Add(cbxReverse[i]);
            }
            cbxReverse[0].Checked = Properties.Settings.Default.setReverse0;
            cbxReverse[1].Checked = Properties.Settings.Default.setReverse1;
            cbxReverse[2].Checked = Properties.Settings.Default.setReverse2;
            cbxReverse[3].Checked = Properties.Settings.Default.setReverse3;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setResetJoint0 = ResetJoint[0];
            Properties.Settings.Default.setResetJoint1 = ResetJoint[1];
            Properties.Settings.Default.setResetJoint2 = ResetJoint[2];
            Properties.Settings.Default.setResetJoint3 = ResetJoint[3];
            Properties.Settings.Default.setReverse0 = cbxReverse[0].Checked;
            Properties.Settings.Default.setReverse1 = cbxReverse[1].Checked;
            Properties.Settings.Default.setReverse2 = cbxReverse[2].Checked;
            Properties.Settings.Default.setReverse3 = cbxReverse[3].Checked;
            Properties.Settings.Default.Save();
        }
        /*界面更新*/
        private void Servo_Update()
        {
            int val;
            double angle;  //关节舵机实际转动角度
            for (int i = 0; i < 8; i++)
            {
                try  //提取rad角度
                {
                    angle = Limit(Convert.ToDouble(tbxServo[i].Text), -0.7854, 0.7854);
                    tbxServo[i].Text = angle.ToString("0.0000");
                    val = (int)(angle / 0.015707963267948967 + 50.5);
                }
                catch (Exception)
                {
                    angle = 0;
                    val = 50;
                    tbxServo[i].Text = "0";
                };
                trbServo[i].Value = val;  //根据rad角度更新PWM跟踪条
                lblServo[i].Text = (trbServo[i].Value * 10 + 1000).ToString("0.0000");
                if (i >= 4) continue;
                DHangle[i] = (cbxReverse[i].Checked) ? (ResetJoint[i] - angle) : (ResetJoint[i] + angle);
                lblDH[i].Text = DHangle[i].ToString("0.0000");
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
                if (i >= 4) continue;
                lblDH[i].Text = ResetJoint[i].ToString("0.0000");
            }
        }
        /*设置复位按钮按下事件，将当前角度设置为复位位置，舵机归中*/
        private void btnSet_Click(object sender, EventArgs e)
        {
            double angle;
            for (int i = 0; i < 8; i++)
            {
                try  //提取rad角度
                {
                    angle = Limit(Convert.ToDouble(tbxServo[i].Text), -3.1416, 3.1416);
                    tbxServo[i].Text = angle.ToString("0.0000");
                }
                catch (Exception)
                {
                    angle = 0;
                    tbxServo[i].Text = "0.0000";
                };
                trbServo[i].Value = 50;
                lblServo[i].Text = "1500";
                ResetJoint[i] = angle;
                lblDH[i].Text = tbxServo[i].Text;
                tbxServo[i].Text = "0.0000";
            }
        }
        /*跟踪条拖动事件，重新计算舵机角度*/
        private void ServoVal_Change(object sender, EventArgs e)
        {
            int i = (int)((TrackBar)sender).Tag;
            lblServo[i].Text = (trbServo[i].Value * 10 + 1000).ToString("0.0000");
            double angle = (trbServo[i].Value - 50) * 0.015707963267948967;
            tbxServo[i].Text = angle.ToString("0.0000");
            if (i >= 4) return;
            DHangle[i] = (cbxReverse[i].Checked) ? (ResetJoint[i] - angle) : (ResetJoint[i] + angle);
            lblDH[i].Text = DHangle[i].ToString("0.0000");
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
            double[] EndPos = new double[3];  //目标点坐标
            Servo_Update();
            try
            {
                Forward_Solve(DHangle, EndPos);
            }
            catch (DllNotFoundException) { MessageBox.Show("Error: ForwardSolve.dll not found!"); }
            catch (Exception) { EndPos[0] = EndPos[1] = EndPos[2] = 0; };
            textBox1.Text = EndPos[0].ToString("0.0000");
            textBox2.Text = EndPos[1].ToString("0.0000");
            textBox3.Text = EndPos[2].ToString("0.0000");
        }
        /*逆解按钮按下事件，计算机械臂逆运动学*/
        [DllImport("InverseSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Inverse_Solve(double[] position, double[] theta);
        private void btnInverse_Click(object sender, EventArgs e)
        {
            double[] EndPos = new double[3];
            double angle;  //关节舵机实际转动角度
            try
            {
                EndPos[0] = Convert.ToDouble(textBox1.Text);
                EndPos[1] = Convert.ToDouble(textBox2.Text);
                EndPos[2] = Convert.ToDouble(textBox3.Text);
                for (int i = 0; i < 4; i++)
                    DHangle[i] = ResetJoint[i];
                if (Inverse_Solve(EndPos, DHangle) != 0)
                    MessageBox.Show("求逆解失败!");
            }
            catch (DllNotFoundException) { MessageBox.Show("Error: InverseSolve.dll not found!"); }
            catch (Exception) { DHangle[0] = DHangle[1] = DHangle[2] = DHangle[3] = 0; };
            for (int i = 0; i < 4; i++)
            {
                angle = (cbxReverse[i].Checked) ? (ResetJoint[i] - DHangle[i]) : (DHangle[i] - ResetJoint[i]);
                tbxServo[i].Text = angle.ToString("0.0000");
            }
            Servo_Update();
        }
        /*50ms定时发送*/
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
                lblVersion.Text = version;
            }
            catch (Exception ex)
            {
                SerialPort1_Close();
                lblVersion.Text = "串口发送失败!" + ex.Message;
            }
        }
        private double Limit(double num, double min, double max)
        {
            return num <= min ? (min) : (num >= max ? max : num);
        }
    }
}