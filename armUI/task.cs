using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace armUI
{
    public partial class Form1 : Form
    {
        private TrackBar[] trbServo = new TrackBar[8];  //8个拖动条
        private Label[] lblServo = new Label[8];  //显示8个PWM值
        private TextBox[] tbxServo = new TextBox[8];  //可编辑的8个文本框
        private Label[] lblDH = new Label[8];  //显示8个参数
        private Button[] btnSetValue = new Button[8];  //8个设置参数按钮
        private CheckBox[] cbxReverse = new CheckBox[8];  //反向勾选框
        private double[] ResetJoint = new double[4];  //舵机归中时的关节DH参数角度
        private double[] DHangle = new double[4];  //关节实时DH参数角度
        private int[] CurrentServo = new int[8];  //用于发送至下位机的当前关节舵机值
        private int[] ResetServo = new int[4];  //舵机复位时的关节舵机值
        private int CatchValue, ReleaseValue;  //抓取和释放时的舵机PWM值
        //private int MoveTime;  //抓取计时
        private enum State { Normal, Moving, Catching, Releasing };  //快速移动、慢速抓取、慢速释放
        State state;
        //-------------------------------------
        //界面初始化要进行的任务
        //-------------------------------------
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
                trbServo[i].Scroll += new EventHandler(trbServo_Change);
                Controls.Add(trbServo[i]);
                lblServo[i] = new Label
                {
                    Name = $"lblServo{i}",
                    Location = new Point(434, 47 + 51 * i),
                    Size = new Size(35, 14),
                };
                Controls.Add(lblServo[i]);
                tbxServo[i] = new TextBox
                {
                    Name = $"tbxServo{i}",
                    Location = new Point(475, 44 + 51 * i),
                    Size = new Size(80, 23),
                    TabIndex = i + 1,
                };
                Controls.Add(tbxServo[i]);
                lblDH[i] = new Label
                {
                    Name = $"lblDH{i}",
                    Location = new Point(575, 47 + 51 * i),
                    Size = new Size(60, 14),
                };
                Controls.Add(lblDH[i]);
                btnSetValue[i] = new Button
                {
                    Text = "设置参数",
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
            ResetJoint[0] = 0;
            ResetJoint[1] = 0.3597;
            ResetJoint[2] = 0.2191;
            ResetJoint[3] = 0.5661;
            cbxReverse[0].Checked = false;
            cbxReverse[1].Checked = false;
            cbxReverse[2].Checked = true;
            cbxReverse[3].Checked = false;
            ResetServo[0] = 50;
            ResetServo[1] = 100;
            ResetServo[2] = 0;
            ResetServo[3] = 85;
            CatchValue = 20;
            lblDH[7].Text = (CatchValue * 10 + 1000).ToString();
            for (int i = 0; i < 8; i++)
            {
                CurrentServo[i] = i < 4 ? ResetServo[i] : 50;
                TraceBar_Update(i);
            }
        }
        //-------------------------------------
        //根据拖动条的舵机值计算并更新其它数值
        //n:舵机编号
        //-------------------------------------
        private void TraceBar_Update(int n)
        {
            if (n < 4)
            {
                double angle = (trbServo[n].Value - 50) * 0.015707963267948967;
                tbxServo[n].Text = angle.ToString("0.0000");
                DHangle[n] = (cbxReverse[n].Checked) ? (ResetJoint[n] - angle) : (ResetJoint[n] + angle);
                lblServo[n].Text = (trbServo[n].Value * 10 + 1000).ToString();
                lblDH[n].Text = DHangle[n].ToString("0.0000");
            }
            else if (n == 7)
            {
                CurrentServo[n] = trbServo[n].Value;
                tbxServo[n].Text = lblServo[n].Text = (CurrentServo[n] * 10 + 1000).ToString();
            }
            else
            {
                CurrentServo[n] = trbServo[n].Value = 50;
                lblServo[n].Text = "1500";
                lblDH[n].Text = tbxServo[n].Text = "0.0000";
            }
        }
        //-------------------------------------
        //根据文本框内的舵机rad角度计算并更新其它数值
        //n:舵机编号
        //-------------------------------------
        private void TextBox_Update(int n)
        {
            int val;  //拖动条的值
            if (n < 4)
            {
                double angle;  //关节舵机实际转动rad角度
                try  //提取rad角度
                {
                    angle = Limit(Convert.ToDouble(tbxServo[n].Text), -0.7854, 0.7854);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; };
                val = (int)(angle / 0.015707963267948967 + 50.5);
                tbxServo[n].Text = angle.ToString("0.0000");
                trbServo[n].Value = val;
                lblServo[n].Text = (val * 10 + 1000).ToString("0.0000");
                DHangle[n] = (cbxReverse[n].Checked) ? (ResetJoint[n] - angle) : (ResetJoint[n] + angle);
                lblDH[n].Text = DHangle[n].ToString("0.0000");
            }
            else if (n == 7)
            {
                try  //提取抓取舵机PWM值
                {
                    val = Limit(Convert.ToInt32(tbxServo[7].Text), 1000, 2000);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; };
                lblServo[n].Text = tbxServo[n].Text = val.ToString();
                trbServo[n].Value = (val - 1000) / 10;
            }
        }
        //-------------------------------------
        //复位按钮按下事件,机械臂回到起始位置
        //-------------------------------------
        private void btnReset_Click(object sender, EventArgs e)
        {
            state = rbnFast.Checked ? State.Normal : State.Moving;
            //MoveTime = 0;
            for (int i = 0; i < 4; i++)
            {
                trbServo[i].Value = ResetServo[i];
                if (rbnFast.Checked)
                    CurrentServo[i] = trbServo[i].Value;
                TraceBar_Update(i);
            }
        }
        //-------------------------------------
        //更新按钮按下事件,根据舵机rad角度计算舵机pwm位置
        //-------------------------------------
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
                TextBox_Update(i);
            TextBox_Update(7);
            CurrentServo[7] = trbServo[7].Value;
        }
        //-------------------------------------
        //正解按钮按下事件,计算机械臂正运动学
        //-------------------------------------
        [DllImport("armSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void Forward_Solve(double[] theta, double[] point);
        private void btnForward_Click(object sender, EventArgs e)
        {
            double[] EndPos = new double[3];  //目标点坐标
            try
            {
                Forward_Solve(DHangle, EndPos);
                tbxPosX.Text = EndPos[0].ToString("0.0000");
                tbxPosY.Text = EndPos[1].ToString("0.0000");
                tbxPosZ.Text = EndPos[2].ToString("0.0000");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); };
        }
        //-------------------------------------
        //逆解按钮按下事件,计算机械臂逆运动学
        //-------------------------------------
        [DllImport("armSolve.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int Inverse_Solve(double[] position, double[] theta);
        private void btnInverse_Click(object sender, EventArgs e)
        {
            double[] EndPos = new double[3];  //末端目标位置
            double angle;  //关节舵机实际转动角度
            string errMessage;
            int errCode;
            for (int i = 0; i < 4; i++)
                DHangle[i] = ResetJoint[i];
            try
            {
                EndPos[0] = Convert.ToDouble(tbxPosX.Text);
                EndPos[1] = Convert.ToDouble(tbxPosY.Text);
                EndPos[2] = Convert.ToDouble(tbxPosZ.Text);
                errCode = Inverse_Solve(EndPos, DHangle);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; };
            if (errCode != 0)
            {
                switch (errCode)
                {
                    case 1: errMessage = "目标点超出工作距离"; break;
                    case 2: errMessage = "目标点方位角超出工作范围"; break;
                    case 3: errMessage = "无可行解"; break;
                    default: errMessage = "未知原因"; break;
                }
                MessageBox.Show("求逆解失败!原因:" + errMessage);
                return;
            }
            state = rbnFast.Checked ? State.Normal : (rbnCatch.Checked ? State.Catching : State.Releasing);
            ReleaseValue = (cbxReverse[7].Checked) ? 0 : 100;
            for (int i = 0; i < 4; i++)
            {
                angle = (cbxReverse[i].Checked) ? (ResetJoint[i] - DHangle[i]) : (DHangle[i] - ResetJoint[i]);
                tbxServo[i].Text = angle.ToString("0.0000");
                TextBox_Update(i);  //给出舵机转动角度,更新其它数值
            }
            CurrentServo[7] = trbServo[7].Value = (rbnCatch.Checked) ? ReleaseValue : CatchValue;
            TraceBar_Update(7);
        }
        //-------------------------------------
        //归中按钮按下事件,所有舵机归中
        //-------------------------------------
        private void btnCenter_Click(object sender, EventArgs e)
        {
            state = rbnFast.Checked ? State.Normal : State.Moving;
            //MoveTime = 0;
            for (int i = 0; i < 4; i++)
            {
                trbServo[i].Value = 50;
                if (rbnFast.Checked)
                    CurrentServo[i] = trbServo[i].Value;
                TraceBar_Update(i);
            }
            TraceBar_Update(7);
        }
        //-------------------------------------
        //拖动条拖动事件,重新计算舵机角度
        //-------------------------------------
        private void trbServo_Change(object sender, EventArgs e)
        {
            int i = (int)((TrackBar)sender).Tag;
            state = rbnFast.Checked ? State.Normal : State.Moving;
            if (rbnFast.Checked)
                CurrentServo[i] = trbServo[i].Value;
            TraceBar_Update(i);
        }
        //-------------------------------------
        //设置参数按钮按下事件,将当前角度设置为复位位置,舵机归中
        //最后一个舵机的当前角度设置为抓取角度
        //-------------------------------------
        private void btnSet_Click(object sender, EventArgs e)
        {
            int i = (int)((Button)sender).Tag;
            if (i < 4)
            {
                double angle;
                try  //提取rad角度
                {
                    angle = Limit(Convert.ToDouble(tbxServo[i].Text), -3.1416, 3.1416);
                    tbxServo[i].Text = angle.ToString("0.0000");
                    ResetJoint[i] = angle;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); };
                trbServo[i].Value = 50;
                lblServo[i].Text = "1500";
                lblDH[i].Text = tbxServo[i].Text = "0.0000";
            }
            else if (i == 7)
            {
                int val;
                try  //提取抓取舵机PWM值
                {
                    val = Limit(Convert.ToInt32(tbxServo[7].Text), 1000, 2000);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); return; };
                lblDH[i].Text = lblServo[i].Text = tbxServo[i].Text = val.ToString();
                val = (val - 1000) / 10;
                CatchValue = trbServo[7].Value = CurrentServo[7] = val;
            }
        }
        //-------------------------------------
        //移动速度切换事件
        //-------------------------------------
        private void rbnFast_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = rbnSlow.Checked;
        }
        //-------------------------------------
        //缓慢移动(50ms定时任务)
        //-------------------------------------
        private void Slow_Moving()
        {
            int diff, cnt = 0;
            for (int i = 0; i < 4; i++)
            {
                diff = trbServo[i].Value - CurrentServo[i];
                if (diff == 0) cnt++;
                CurrentServo[i] += Limit(diff, -3, 3);
            }
            if (cnt >= 4)
            {
                //MoveTime = 0;
                if (state == State.Catching)
                    CurrentServo[7] = trbServo[7].Value = CatchValue;
                else if (state == State.Releasing)
                    CurrentServo[7] = trbServo[7].Value = ReleaseValue;
                state = State.Normal;
                TraceBar_Update(7);
            }
        }
        //-------------------------------------
        //定时发送舵机数据(拖动条的值)给下位机(50ms定时任务)
        //-------------------------------------
        private void SerialPort1_Send()
        {
            byte[] DataToSend = new byte[10];
            byte sum = 0x3C;
            DataToSend[0] = 0x3C;
            for (int i = 0; i < 8; i++)
            {
                DataToSend[i + 1] = (byte)CurrentServo[i];
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
        //-------------------------------------
        //50ms定时任务
        //-------------------------------------
        private void tmr50ms_Tick(object sender, EventArgs e)
        {
            if (state != State.Normal)
            {
                Slow_Moving();
                //MoveTime++;
            }
            SerialPort1_Send();
        }
        //-------------------------------------
        //定时发送点击事件,是否允许定时发送舵机数据给下位机
        //-------------------------------------
        private void cbxSend_CheckedChanged(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen) return;
            tmr50ms.Enabled = cbxSend.Checked ? true : false;
            foreach (TextBox i in tbxServo)
                i.Enabled = cbxSend.Checked ? false : true;
        }
        //-------------------------------------
        //限幅
        //-------------------------------------
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