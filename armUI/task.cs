using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace armUI
{
    public partial class Form1 : Form
    {
        private TrackBar[] trbServo = new TrackBar[8];  //8个拖动条
        private Label[] lblServo = new Label[8];  //显示8个舵机的PWM值
        private TextBox[] tbxServo = new TextBox[8];  //可编辑的7个舵机的rad角度和抓取舵机的PWM值
        private Label[] lblDH = new Label[8];  //显示7个关节DH参数中的theta角度和抓取角度
        private Button[] btnSetValue = new Button[8];  //设置参数按钮
        private CheckBox[] cbxReverse = new CheckBox[8];  //显示DH参数中的theta角度
        private double[] ResetJoint = new double[8];  //舵机归中时的关节DH参数角度
        private double[] DHangle = new double[4];  //关节实时DH参数角度
        private int CatchValue, ReleaseValue;  //抓取和释放时的舵机PWM值
        private int catchTime;  //抓取计时
        private bool isCatching = false;
        private void Init_UI()
        {
            for (int i = 0; i < 8; i++)
            {
                trbServo[i] = new TrackBar
                {
                    Name = $"trbServo{i}",
                    Maximum = 100,
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
                    TabIndex = i + 1,
                };
                Controls.Add(tbxServo[i]);
                lblDH[i] = new Label
                {
                    Name = $"lblDH{i}",
                    Text = ResetJoint[i].ToString("0.0000"),
                    Location = new Point(575, 47 + 51 * i),
                    Size = new Size(60, 14),
                };
                Controls.Add(lblDH[i]);
                btnSetValue[i] = new Button
                {
                    Text = "设置复位",
                    Location = new Point(640, 40 + 51 * i),
                    Size = new Size(80, 31),
                    Tag = i,
                };
                btnSetValue[i].Click += new EventHandler(btnSet_Click);
                Controls.Add(btnSetValue[i]);
                cbxReverse[i] = new CheckBox
                {
                    Text = "",
                    Location = new Point(760, 44 + 51 * i),
                    Size = new Size(22, 22),
                };
                Controls.Add(cbxReverse[i]);
            }
            ResetJoint[0] = Properties.Settings.Default.setResetJoint0;
            ResetJoint[1] = Properties.Settings.Default.setResetJoint1;
            ResetJoint[2] = Properties.Settings.Default.setResetJoint2;
            ResetJoint[3] = Properties.Settings.Default.setResetJoint3;
            cbxReverse[0].Checked = Properties.Settings.Default.setReverse0;
            cbxReverse[1].Checked = Properties.Settings.Default.setReverse1;
            cbxReverse[2].Checked = Properties.Settings.Default.setReverse2;
            cbxReverse[3].Checked = Properties.Settings.Default.setReverse3;
            CatchValue = Properties.Settings.Default.setCatchValue;
            tbxServo[7].Text = "1500";
            btnSetValue[7].Text = "抓取角度";
            lblDH[7].Text = (CatchValue * 10 + 1000).ToString();
            Servo_Update();
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
            Properties.Settings.Default.setCatchValue = CatchValue;
            Properties.Settings.Default.Save();
        }
        /*界面更新*/
        private void Servo_Update()
        {
            int val;  //拖动条的值
            double angle;  //关节舵机实际转动rad角度
            for (int i = 0; i < 7; i++)
            {
                try  //提取rad角度
                {
                    angle = Limit(Convert.ToDouble(tbxServo[i].Text), -0.7854, 0.7854);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); continue; };
                val = (int)(angle / 0.015707963267948967 + 50.5);
                tbxServo[i].Text = angle.ToString("0.0000");
                trbServo[i].Value = val;
                lblServo[i].Text = (val * 10 + 1000).ToString("0.0000");
                if (i >= 4) continue;
                DHangle[i] = (cbxReverse[i].Checked) ? (ResetJoint[i] - angle) : (ResetJoint[i] + angle);
                lblDH[i].Text = DHangle[i].ToString("0.0000");
            }
            try  //提取抓取舵机PWM值
            {
                val = Limit(Convert.ToInt32(tbxServo[7].Text), 1000, 2000);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; };
            lblServo[7].Text = tbxServo[7].Text = val.ToString();
            val = (val - 1000) / 10;
            trbServo[7].Value = val;
        }
        /*根据坐标计算逆运动学并更新拖动条*/
        [DllImport("armSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Inverse_Solve(double[] position, double[] theta);
        private int Inverse_Calculate(double[] position)
        {
            double angle;  //关节舵机实际转动角度
            int errCode;
            string errMessage;
            try
            {
                for (int i = 0; i < 4; i++)
                    DHangle[i] = ResetJoint[i];
                errCode = Inverse_Solve(position, DHangle);
                switch (errCode)
                {
                    case -1: errMessage = "目标点超出工作距离"; break;
                    case -2: errMessage = "目标点方位角超出工作范围"; break;
                    case -3: errMessage = "无可行解"; break;
                    default: errMessage = "未知原因"; break;
                }
                if (errCode != 0)
                    MessageBox.Show("求逆解失败!原因:" + errMessage);
            }
            catch (Exception) { throw; };
            for (int i = 0; i < 4; i++)
            {
                angle = (cbxReverse[i].Checked) ? (ResetJoint[i] - DHangle[i]) : (DHangle[i] - ResetJoint[i]);
                tbxServo[i].Text = angle.ToString("0.0000");
            }
            Servo_Update();
            return errCode;
        }
        /*归中按钮按下事件，所有舵机归中，用于测试*/
        private void btnCenter_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                trbServo[i].Value = 50;
                lblServo[i].Text = "1500";
                if (i < 7)
                    tbxServo[i].Text = "0.0000";
                else
                    tbxServo[i].Text = "1500";
                if (i >= 4) continue;
                lblDH[i].Text = ResetJoint[i].ToString("0.0000");
            }
        }
        /*更新按钮按下事件，根据舵机rad角度计算舵机pwm位置*/
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Servo_Update();
        }
        /*正解按钮按下事件，计算机械臂正运动学*/
        [DllImport("armSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void Forward_Solve(double[] theta, double[] point);
        private void btnForward_Click(object sender, EventArgs e)
        {
            double[] EndPos = new double[3];  //目标点坐标
            Servo_Update();
            try
            {
                Forward_Solve(DHangle, EndPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                EndPos[0] = EndPos[1] = EndPos[2] = 0;
            };
            textBox1.Text = EndPos[0].ToString("0.0000");
            textBox2.Text = EndPos[1].ToString("0.0000");
            textBox3.Text = EndPos[2].ToString("0.0000");
        }
        /*逆解按钮按下事件，计算机械臂逆运动学*/
        private void btnInverse_Click(object sender, EventArgs e)
        {
            double[] EndPos = new double[3];
            try
            {
                EndPos[0] = Convert.ToDouble(textBox1.Text);
                EndPos[1] = Convert.ToDouble(textBox2.Text);
                EndPos[2] = Convert.ToDouble(textBox3.Text);
                Inverse_Calculate(EndPos);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); };
        }
        /*抓取按钮按下事件，抓取目标*/
        [DllImport("armSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Trajectory_Init(double[] ResetAngle, double[] StartAngle, double[] EndPosition);
        private void btnCatch_Click(object sender, EventArgs e)
        {
            if (!cbxSend.Checked) return;
            ReleaseValue = cbxReverse[7].Checked ? 0 : 100;
            double[] EndPos = new double[3];
            try
            {
                EndPos[0] = Convert.ToDouble(textBox1.Text);
                EndPos[1] = Convert.ToDouble(textBox2.Text);
                EndPos[2] = Convert.ToDouble(textBox3.Text);
                int errCode = Trajectory_Init(ResetJoint, DHangle, EndPos);
                if ((!isCatching) && (errCode == 0))
                {
                    isCatching = true;
                    catchTime = 0;
                }
                else
                {
                    isCatching = false;
                    MessageBox.Show("抓取失败!错误码:" + errCode.ToString());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); };
        }
        /*拖动条拖动事件，重新计算舵机角度*/
        private void ServoVal_Change(object sender, EventArgs e)
        {
            int i = (int)((TrackBar)sender).Tag;
            tbxServo[i].Text = lblServo[i].Text = (trbServo[i].Value * 10 + 1000).ToString();
            if (i == 7) return;
            double angle = (trbServo[i].Value - 50) * 0.015707963267948967;
            tbxServo[i].Text = angle.ToString("0.0000");
            if (i >= 4) return;
            DHangle[i] = (cbxReverse[i].Checked) ? (ResetJoint[i] - angle) : (ResetJoint[i] + angle);
            lblDH[i].Text = DHangle[i].ToString("0.0000");
        }
        /*设置参数按钮按下事件，将当前角度设置为复位位置，舵机归中。最后一个舵机的当前角度设置为抓取角度*/
        private void btnSet_Click(object sender, EventArgs e)
        {
            double angle;
            int i = (int)((Button)sender).Tag;
            if (i == 7)
            {
                lblDH[i].Text = lblServo[i].Text;
                CatchValue = (int)Convert.ToDouble(lblDH[i].Text);
                CatchValue = (CatchValue - 1000) / 10;
                return;
            }
            try  //提取rad角度
            {
                angle = Limit(Convert.ToDouble(tbxServo[i].Text), -3.1416, 3.1416);
                tbxServo[i].Text = angle.ToString("0.0000");
                ResetJoint[i] = angle;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); };
            trbServo[i].Value = 50;
            lblServo[i].Text = "1500";
            lblDH[i].Text = tbxServo[i].Text;
            tbxServo[i].Text = "0.0000";
        }
        /*定时发送点击事件，是否允许定时发送舵机数据给下位机*/
        private void cbxSend_CheckedChanged(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen) return;
            tmr50ms.Enabled = cbxSend.Checked ? true : false;
            foreach (TextBox i in tbxServo)
                i.Enabled = cbxSend.Checked ? false : true;
        }
        /*50ms定时任务:发送舵机数据(拖动条的值)给下位机*/
        [DllImport("armSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Trajectory_Output(int t, double[] angle);
        [DllImport("armSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void Trajectory_Finish();
        private void tmr50ms_Tick(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen) return;
            if (isCatching)
            {
                double angle;
                int catchState = Trajectory_Output(catchTime, DHangle);
                catchTime++;
                for (int i = 0; i < 4; i++)
                {
                    angle = (cbxReverse[i].Checked) ? (ResetJoint[i] - DHangle[i]) : (DHangle[i] - ResetJoint[i]);
                    tbxServo[i].Text = angle.ToString("0.0000");
                }
                switch (catchState)
                {
                    case 1: tbxServo[7].Text = (ReleaseValue * 10 + 1000).ToString(); break;
                    case 2: tbxServo[7].Text = (CatchValue * 10 + 1000).ToString(); break;
                    case 3: catchTime = 0; isCatching = false; Trajectory_Finish(); break;
                    default: break;
                }
                lblCatchTime.Text = catchTime.ToString();
            }
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
        private int Limit(int num, int min, int max)
        {
            return num <= min ? (min) : (num >= max ? max : num);
        }
    }
}