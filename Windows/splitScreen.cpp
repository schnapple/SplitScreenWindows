#include "StdAfx.h"
#include <stdio.h>
#include <string>
#include <iostream>
#include <fstream>
#define _WIN32_WINNT 0x0501
#include <windows.h>
#include <tlhelp32.h>
#include <tchar.h>


using namespace std;

const char g_szClassName[] = "myWindowClass";
HWND text;

BOOL GetProcessList(){
	HANDLE hProcessSnap;
	HANDLE hProcess;
	PROCESSENTRY32 pe32;
	DWORD dwPriotiryClass;

	// Take a snapshot of all processes in the systems
	hProcessSnap = CreateToolhelp32Snapshot( TH32CS_SNAPPROCESS, 0);
	if( hProcessSnap == INVALID_HANDLE_VALUE){
		//printError( TEXT("CreateToolhelp32Snapshot (of processes)"));
		return( FALSE );
	}

	pe32.dwSize = sizeof(PROCESSENTRY32);

	if(!Process32First(hProcessSnap, &pe32)){
		//printError( TEXT("Process32First"));
		CloseHandle(hProcessSnap);
		return( FALSE);
	}

	cout << "True" << endl;

	do{
		printf("PROCESS NAME: %s\n", pe32.szExeFile);
		//printf("Process Size: %lu\n", pe32.dwSize);
	}while( Process32Next(hProcessSnap, &pe32));

	return(TRUE);

}


BOOL CALLBACK EnumWindowsProc(HWND hWnd, LPARAM lParam) {
    char buff[255];
    char key[] = "Program Manager";
    int x_pixels = GetSystemMetrics(SM_CXSCREEN);
	int y_pixels = GetSystemMetrics(SM_CYSCREEN);

    if (IsWindowVisible(hWnd)) {
        GetWindowText(hWnd, (LPSTR) buff, 254);
        if(strcmp(buff, key) != 0)
        	SetWindowPos( hWnd, HWND_NOTOPMOST, x_pixels/3, 0, x_pixels/3, y_pixels, SWP_SHOWWINDOW );
        printf("%s\n", buff);
    }
    return TRUE;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    switch(msg)
    {
    	//printf("here");
    	case WM_CREATE:
    	CButton myButton1;
    		myButton1.Create(_T("My button"), WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON, CRect(10,10,100,30), pParentWnd, 1);
    	break;
    	
        case WM_CLOSE:
            DestroyWindow(hwnd);
        break;
        case WM_DESTROY:
            PostQuitMessage(0);
        break;
        default:
            return DefWindowProc(hwnd, msg, wParam, lParam);
    }
    return 0;
}



int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance,
    LPSTR lpCmdLine, int nCmdShow)
{
    WNDCLASSEX wc;
    HWND hwnd;
    MSG Msg;

    //printf("here");


    //Step 1: Registering the Window Class
    wc.cbSize        = sizeof(WNDCLASSEX);
    wc.style         = 0;
    wc.lpfnWndProc   = WndProc;
    wc.cbClsExtra    = 0;
    wc.cbWndExtra    = 0;
    wc.hInstance     = hInstance;
    wc.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
    wc.hCursor       = LoadCursor(NULL, IDC_ARROW);
    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW+2);
    wc.lpszMenuName  = NULL;
    wc.lpszClassName = g_szClassName;
    wc.hIconSm       = LoadIcon(NULL, IDI_APPLICATION);

    if(!RegisterClassEx(&wc))
    {
        MessageBox(NULL, "Window Registration Failed!", "Error!",
            MB_ICONEXCLAMATION | MB_OK);
        return 0;
    }

    // Step 2: Creating the Window
    //printf("here");
    hwnd = CreateWindowEx(
        WS_EX_RIGHT,
        g_szClassName,
        "SplitScreen",
        WS_MINIMIZEBOX | WS_SYSMENU,
        CW_USEDEFAULT, CW_USEDEFAULT, 540, 520,
        NULL, NULL, hInstance, NULL);

    if(hwnd == NULL)
    {
        MessageBox(NULL, "Window Creation Failed!", "Error!",
            MB_ICONEXCLAMATION | MB_OK);
        return 0;
    }
    //printf("here");


    ShowWindow(hwnd, nCmdShow);
    UpdateWindow(hwnd);

    // Step 3: The Message Loop
    while(GetMessage(&Msg, NULL, 0, 0) > 0)
    {
        TranslateMessage(&Msg);
        DispatchMessage(&Msg);
    }
    return Msg.wParam;
}

// int main(int argc, CHAR* argv[]){
	
// 	//GetProcessList();
// 	int x_pixels = GetSystemMetrics(SM_CXSCREEN);
// 	int y_pixels = GetSystemMetrics(SM_CYSCREEN);

// 	//printf("%d\n", y_pixels);
// 	EnumWindows(EnumWindowsProc, 0);
//     return 0;
// 	//cout << result << endl;
// 	//cout << procIDs[0] << endl;
// 	//cout << procIDs[1] << endl;
// }