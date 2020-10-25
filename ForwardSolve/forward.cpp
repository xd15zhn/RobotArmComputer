#include "forward.h"
#include <math.h>

void Forward_Solve(const double theta[], double position[])
{
	double x = 81 + 172 * cos(theta[3]);
	double y = 172 * sin(theta[3]);
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