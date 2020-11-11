#pragma once
extern "C" _declspec(dllexport) int Inverse_Solve(const double position[], double theta[]);
extern "C" _declspec(dllexport) void Forward_Solve(const double theta[], double position[]);
extern "C" _declspec(dllexport) int Trajectory_Init(const double* ResetAngle, const double* StartAngle, const double* EndPosition);
extern "C" _declspec(dllexport) int Trajectory_Output(int t, double* angle);
extern "C" _declspec(dllexport) void Trajectory_Finish(void);