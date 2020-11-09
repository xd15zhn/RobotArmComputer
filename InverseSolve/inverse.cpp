#include "inverse.h"
#include <cmath>
//#include <iostream>
//using namespace std;
#define LIMIT(x,min,max)         (((x)<=(min) ? (min) : ((x)>=(max) ? (max) : (x))))

constexpr double PI = 3.14159265358979323846;
//各关节长度
constexpr double L1 = 93;
constexpr double L2 = 80;
constexpr double L3 = 81;
constexpr double L4 = 172;

/**********************
逆运动学求解各关节角度
*@position:目标点坐标
*@theta:输入复位时的DH参数,输出逆解各关节参数
*@return:逆解是否存在
**********************/
int Inverse_Solve(const double position[], double theta[])
{
	double x = position[0];
	double y = position[1];
	double z = position[2] - L1;
	double r = x * x + y * y + z * z;
	double ResetJoint[4];
	double pitch = 0.78;  //末端执行器的腕关节相对于目标点的仰角
	double targetPitch;  //末端执行器的腕关节相对于固定坐标系原点的仰角
	double costheta1, costheta2;
	for (int i = 0; i < 4; i++)
		ResetJoint[i] = theta[i];
	if (r > (L2 + L3 + L4) * (L2 + L3 + L4))  //目标点超出工作距离
		return -1;
	theta[0] = atan2(y, x);
	if (abs(theta[0] - ResetJoint[0]) >= 0.7854)  //目标点方位角超出工作范围
		return -2;
	while (1) {
		z = position[2] - L1 + L4 * sin(pitch);  //腕关节z坐标
		r = L4 * cos(pitch);  //腕关节在固定坐标系xoy平面的投影至原点的距离
		x = position[0] - r * cos(theta[0]);  //腕关节x坐标
		y = position[1] - r * sin(theta[0]);  //腕关节y坐标
		r = sqrt(x * x + y * y + z * z);  //腕关节与原点的距离
		costheta2 = (L2 * L2 + L3 * L3 - r * r) / L2 / L3 / 2.0;  //求L2和L3的夹角
		if ((costheta2 >= -1) && (costheta2 <= 1)) {  //夹角有效
			theta[2] = -acos(-costheta2);  //取补角作为关节角度,在多解时取肘部朝上解
			if (abs(theta[2] - ResetJoint[2]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
			targetPitch = atan2(z, sqrt(x * x + y * y));  //求肘部关节相对于固定坐标系原点的仰角
			costheta1 = (L2 * L2 + r * r - L3 * L3) / L2 / r / 2.0;  //求L2仰角和肘部关节仰角之差
			theta[1] = targetPitch + acos(costheta1);  //即L2仰角
			if (abs(theta[1] - ResetJoint[1]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
			theta[3] = PI / 2.0 - pitch - theta[1] - theta[2];  //L4相对于L3的仰角
			if (abs(theta[3] - ResetJoint[3]) >= 0.7854) goto PITCHADD;  //超出舵机范围,放弃当前结果
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