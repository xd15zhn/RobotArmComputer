#include "solve.h"
#include <math.h>
#define ABS(x)                   ((x)>=0?(x):-(x))
#define LIMIT(x,min,max)         (((x)<=(min) ? (min) : ((x)>=(max) ? (max) : (x))))
/*****文件说明*********
机械臂的正逆运动学和轨迹插值
固定坐标指固定于肩关节的坐标,相对于世界坐标有一个沿z轴正方向的平移
除了Inverse_Solve()函数中含有固定坐标之外,其它函数中所提到的坐标均为世界坐标
以下所提到的角度均为rad角度
**********************/

constexpr double PI = 3.14159265358979323846;
constexpr double L1 = 93;  //关节1长度
constexpr double L2 = 80;  //关节2长度
constexpr double L3 = 81;  //关节3长度
constexpr double L4 = 172;  //关节4长度
constexpr double DIST = 100;  //插值点距离

/**********************
正运动学求解各关节角度
* theta:输入,各关节DH参数
* position:输出,目标点坐标
**********************/
void Forward_Solve(const double theta[], double position[])
{
	double x = 81 + 172 * sin(theta[3]);
	double y = -172 * cos(theta[3]);
	double z = 0;
	double temp[3];
	temp[0] = x * cos(theta[2]) - y * sin(theta[2]) + 80;
	temp[1] = x * sin(theta[2]) + y * cos(theta[2]);
	temp[2] = z;
	x = temp[0]; y = temp[1]; z = temp[2];
	temp[0] = x * cos(theta[1]) - y * sin(theta[1]);
	temp[1] = -z;
	temp[2] = x * sin(theta[1]) + y * cos(theta[1]);
	x = temp[0]; y = temp[1]; z = temp[2];
	temp[0] = x * cos(theta[0]) - y * sin(theta[0]);
	temp[1] = x * sin(theta[0]) + y * cos(theta[0]);
	temp[2] = z + 93;
	position[0] = temp[0];
	position[1] = temp[1];
	position[2] = temp[2];
}

/**********************
逆运动学求解各关节角度
除了特别说明为世界坐标之外,该函数中的坐标均为固定坐标
* position:输入,目标点的世界坐标
* theta:输入复位时的DH参数,输出逆解各关节参数
* return:逆解是否存在.0,存在;其它,不存在
**********************/
int Inverse_Solve(const double position[], double theta[])
{
	double x = position[0];
	double y = position[1];
	double z = position[2] - L1;
	double r = x * x + y * y + z * z;
	double ResetJoint[4];
	double pitch = 0.78;  //末端执行器的腕关节相对于目标点的仰角
	double targetPitch;  //末端执行器的腕关节相对于原点的仰角
	double costheta;
	for (int i = 0; i < 4; i++)
		ResetJoint[i] = theta[i];
	if (r > (L2 + L3 + L4) * (L2 + L3 + L4))  //目标点超出工作距离
		return -1;
	theta[0] = atan2(y, x);
	if (ABS(theta[0] - ResetJoint[0]) >= 0.7854)  //目标点方位角超出工作范围
		return -2;
	while (1) {
		z = position[2] - L1 + L4 * sin(pitch);  //腕关节z坐标
		r = L4 * cos(pitch);  //腕关节在xoy平面的投影至原点的距离
		x = position[0] - r * cos(theta[0]);  //腕关节x坐标
		y = position[1] - r * sin(theta[0]);  //腕关节y坐标
		r = sqrt(x * x + y * y + z * z);  //腕关节与原点的距离
		costheta = (L2 * L2 + L3 * L3 - r * r) / L2 / L3 / 2.0;  //求L2和L3的夹角
		if ((costheta >= -1) && (costheta <= 1)) {  //夹角有效
			theta[2] = -acos(-costheta);  //取补角作为关节角度,在多解时取肘部朝上解
			if (ABS(theta[2] - ResetJoint[2]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
			targetPitch = atan2(z, sqrt(x * x + y * y));  //求肘部关节相对于原点的仰角
			costheta = (L2 * L2 + r * r - L3 * L3) / L2 / r / 2.0;  //求L2仰角和肘部关节仰角之差
			theta[1] = targetPitch + acos(costheta);  //即L2仰角
			if (ABS(theta[1] - ResetJoint[1]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
			theta[3] = PI / 2.0 - pitch - theta[1] - theta[2];  //L4相对于L3的仰角
			if (ABS(theta[3] - ResetJoint[3]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
			return 0;
		}
	PITCHADD:
		pitch += (pitch >= 0.78) ? 0.01 : -0.01;
		if (pitch > 1.5708)
			pitch = 0.77;
		else if (pitch < -1.5708) {  //无可行解
			theta[1] = ResetJoint[1];
			theta[2] = ResetJoint[2];
			theta[3] = ResetJoint[3];
			return -3;
		}
	}
}

/**********************
机械臂轨迹规划
pos/theta           |x        |y        |z		  | |方位角     |肩关节     |肘关节     |腕关节
起始位置			|pos[0][0]|pos[0][1]|pos[0][2]| |theta[0][0]|theta[0][1]|theta[0][2]|theta[0][3]
靠近起始位置的插值点|         |         |pos[1][2]| |           |           |           |theta[1][3]
靠近终止位置的插值点|         |         |pos[2][2]| |           |           |           |theta[2][3]
终止位置			|pos[3][0]|         |pos[3][2]| |theta[3][0]|           |           |theta[3][3]
**********************/
struct Trajectory
{
	double pos[4][3];
	double theta[4][4];
} *mcu61;

/**********************
寻找插值点.插值点和目标点的方位角相同,距离设为定值
* position:输入,目标点坐标
* AngleIns:输出,插值点坐标对应的关节角度
* return:插值点是否存在.0,存在;其它,不存在
**********************/
int Trajectory_Interpolation(const double* position, double* AngleIns)
{
	double azimuth = atan2(position[1], position[0]);  //方位角
	double PosIns[3];  //插值点坐标
	for (double pitch = 0; pitch <= 3.14; pitch += 0.01)
	{
		PosIns[2] = position[2] + DIST * sin(pitch);
		PosIns[0] = position[0] - DIST * cos(pitch) * cos(azimuth);
		PosIns[1] = position[1] - DIST * cos(pitch) * sin(azimuth);
		if (Inverse_Solve(PosIns, AngleIns) == 0)
			return 0;
	}
	return -1;
}

/**********************
轨迹规划初始化
* ResetAngle:输入,归中时的DH参数角度
* StartAngle:输入,起始位置的各关节DH参数角度
* EndPosition:输入,终止位置
* return:轨迹规划是否存在解.0,存在;其它,不存在
**********************/
int Trajectory_Init(const double* ResetAngle, const double* StartAngle, const double* EndPosition)
{
	mcu61 = new Trajectory;
	double StartPosition[3];
	mcu61->theta[0][0] = StartAngle[0];
	mcu61->theta[0][1] = StartAngle[1];
	mcu61->theta[0][2] = StartAngle[2];
	mcu61->theta[0][3] = StartAngle[3];
	mcu61->theta[1][0] = mcu61->theta[2][0] = mcu61->theta[3][0] = ResetAngle[0];
	mcu61->theta[1][1] = mcu61->theta[2][1] = mcu61->theta[3][1] = ResetAngle[1];
	mcu61->theta[1][2] = mcu61->theta[2][2] = mcu61->theta[3][2] = ResetAngle[2];
	mcu61->theta[1][3] = mcu61->theta[2][3] = mcu61->theta[3][3] = ResetAngle[3];
	Forward_Solve(StartAngle, StartPosition);
	mcu61->pos[0][0] = StartPosition[0];
	mcu61->pos[0][1] = StartPosition[1];
	mcu61->pos[0][2] = StartPosition[2];
	mcu61->pos[3][0] = EndPosition[0];
	mcu61->pos[3][1] = EndPosition[1];
	mcu61->pos[3][2] = EndPosition[2];
	if (Trajectory_Interpolation(mcu61->pos[0], mcu61->theta[1]) != 0)
		return 1;
	if (Trajectory_Interpolation(mcu61->pos[3], mcu61->theta[2]) != 0)
		return 2;
	if (Inverse_Solve(EndPosition, mcu61->theta[3]) != 0)
		return 3;
	return 0;
}

/**********************
输出插值各关节DH参数角度
* t:输入,时间
* angle:输出,各关节DH参数角度
* return:1,释放;2,抓紧;3,抓取结束;0,其它
**********************/
int Trajectory_Output(int t, double* angle)
{
	int i = t / 40;
	i = LIMIT(i, 0, 3);
	angle[0] = mcu61->theta[i][0];
	angle[1] = mcu61->theta[i][1];
	angle[2] = mcu61->theta[i][2];
	angle[3] = mcu61->theta[i][3];
	if (t < 120)
		return 1;
	else if (t < 160)
		return 2;
	else if (t >= 160)
		return 3;
	else
		return 0;
}

/**********************
轨迹规划结束
**********************/
void Trajectory_Finish(void)
{
	if (mcu61 != NULL)
		delete mcu61;
}