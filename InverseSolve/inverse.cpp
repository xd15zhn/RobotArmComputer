#include "inverse.h"
#include <math.h>
#define LIMIT(x,min,max)         (((x)<=(min) ? (min) : ((x)>=(max) ? (max) : (x))))
constexpr double PI = 3.14159265358979323846;
constexpr double L1 = 93;
constexpr double L2 = 80;
constexpr double L3 = 81;
constexpr double L4 = 172;
bool Inverse_Solve(const double position[], double theta[])
{
	double x = position[0];
	double y = position[1];
	double z = position[2] - L1;
	double r;
	double pitch = 0.78;  //末端执行器的仰角
	double targetPitch;  //末端执行器的关节坐标相对于世界坐标系原点的仰角
	double costheta;
	theta[0] = atan2(y, x);
	while (1) {
		z = position[2] - L1 + L4 * sin(pitch);
		r = L4 * cos(pitch);
		x = position[0] - r * cos(theta[0]);
		y = position[1] - r * sin(theta[0]);
		r = sqrt(x * x + y * y + z * z);
		costheta = (L2 * L2 + L3 * L3 - r * r) / L2 / L3 / 2.0;
		if ((costheta >= -1) && (costheta <= 1))
			break;
		pitch += (pitch >= 0.78) ? 0.01 : -0.01;
		if (pitch > 1.5708)
			pitch = 0.77;
		else if (pitch < -1.5708) {
			theta[1] = theta[2] = theta[3] = 0;
			return false;
		}
	}
	theta[2] = -acos(-costheta);
	targetPitch = atan2(z, sqrt(x * x + y * y));
	costheta = (L2 * L2 + r * r - L3 * L3) / L2 / r / 2.0;
	theta[1] = targetPitch + acos(costheta);
	theta[3] = -pitch - theta[1] - theta[2];
	return true;
}