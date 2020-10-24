#include "pch.h"
#include "forward.h"
#include <math.h>

double* Matrix_Product(double theta1, double theta2, double theta3, double theta4)
{
	static double ans[3];
	double x = 81 + 172 * sin(theta4);
	double y = -172 * cos(theta4);
	double z = 0;
	ans[0] = x * cos(theta3) - y * sin(theta3) + 80;
	ans[1] = x * sin(theta3) + y * cos(theta3);
	ans[2] = z;
	x = ans[0]; y = ans[1]; z = ans[2];
	ans[0] = x * cos(theta2) - y * sin(theta2);
	ans[1] = -z;
	ans[2] = x * sin(theta2) + y * cos(theta2);
	x = ans[0]; y = ans[1]; z = ans[2];
	ans[0] = x * cos(theta1) - y * sin(theta1);
	ans[1] = x * sin(theta1) + y * cos(theta1);
	ans[2] = z;
	return ans;
}

double EndPos_X(double theta1, double theta2, double theta3, double theta4)
{
	double *ans = Matrix_Product(theta1,  theta2,  theta3,  theta4);
	return ans[0];
}

double EndPos_Y(double theta1, double theta2, double theta3, double theta4)
{
	double* ans = Matrix_Product(theta1, theta2, theta3, theta4);
	return ans[1];
}

double EndPos_Z(double theta1, double theta2, double theta3, double theta4)
{
	double* ans = Matrix_Product(theta1, theta2, theta3, theta4);
	return ans[2] + 93;
}
