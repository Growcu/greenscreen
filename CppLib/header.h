#pragma once

extern "C" _declspec(dllexport) int DLLTestFn(int a, int b);

extern "C" _declspec(dllexport) void removeGreenScreenCPP(unsigned char* pixelArray, unsigned char* colorRgbBytes, int size);