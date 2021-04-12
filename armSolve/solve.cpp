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
constexpr double L1 = 93;  //连杆1长度
constexpr double L2 = 80;  //连杆2长度
constexpr double L3 = 81;  //连杆3长度
constexpr double L4 = 195;  //连杆4长度

/**********************
正运动学求解各关节角度
* theta:输入,各关节DH参数
* position:输出,目标点坐标
**********************/
void Forward_Solve(const double theta[], double position[])
{
	double x = L3 + L4 * sin(theta[3]);
	double y = -L4 * cos(theta[3]);
	double z = 0;
	double temp[3];
	temp[0] = x * cos(theta[2]) - y * sin(theta[2]) + L2;
	temp[1] = x * sin(theta[2]) + y * cos(theta[2]);
	temp[2] = z;
	x = temp[0]; y = temp[1]; z = temp[2];
	temp[0] = x * cos(theta[1]) - y * sin(theta[1]);
	temp[1] = -z;
	temp[2] = x * sin(theta[1]) + y * cos(theta[1]);
	x = temp[0]; y = temp[1]; z = temp[2];
	temp[0] = x * cos(theta[0]) - y * sin(theta[0]);
	temp[1] = x * sin(theta[0]) + y * cos(theta[0]);
	temp[2] = z + L1;
	position[0] = temp[0];
	position[1] = temp[1];
	position[2] = temp[2];
}

/**********************
逆运动学求解各关节角度
除了特别说明为世界坐标之外,该函数中的坐标均为固定坐标
各关节角度方向定义:抬头为正,俯视逆时针为正
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
		return 1;
	theta[0] = atan2(y, x);  //方位角
	if (ABS(theta[0] - ResetJoint[0]) >= 0.7854)  //目标点方位角超出工作范围
		return 2;
	while (1) {
		z = position[2] - L1 + L4 * sin(pitch);  //腕关节z坐标
		r = L4 * cos(pitch);  //腕关节与末端执行器在xoy平面的投影的距离
		x = position[0] - r * cos(theta[0]);  //腕关节x坐标
		y = position[1] - r * sin(theta[0]);  //腕关节y坐标
		r = sqrt(x * x + y * y + z * z);  //腕关节与原点的距离
		costheta = (L2 * L2 + L3 * L3 - r * r) / L2 / L3 / 2.0;  //求L2和L3的夹角
		if ((costheta < -1) || (costheta > 1)) goto PITCHADD;  //夹角无效
		theta[2] = -acos(-costheta);  //取补角作为关节角度,在多解时取肘部朝上解
		if (ABS(theta[2] - ResetJoint[2]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
		targetPitch = atan2(z, sqrt(x * x + y * y));  //求腕关节相对于原点的仰角
		costheta = (L2 * L2 + r * r - L3 * L3) / L2 / r / 2.0;  //求L2仰角(肘关节)和腕关节仰角之差
		theta[1] = targetPitch + acos(costheta);  //即L2仰角
		if (ABS(theta[1] - ResetJoint[1]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
		theta[3] = PI / 2.0 - pitch - theta[1] - theta[2];  //L4相对于L3的仰角
		if (ABS(theta[3] - ResetJoint[3]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
		return 0;
	PITCHADD:  //调整末端关节仰角重新计算
		pitch += (pitch >= 0.78) ? 0.01 : -0.01;
		if (pitch > 1.5708)
			pitch = 0.77;
		else if (pitch < -1.5708) {  //无可行解
			theta[1] = ResetJoint[1];
			theta[2] = ResetJoint[2];
			theta[3] = ResetJoint[3];
			return 3;
		}
	}
}
