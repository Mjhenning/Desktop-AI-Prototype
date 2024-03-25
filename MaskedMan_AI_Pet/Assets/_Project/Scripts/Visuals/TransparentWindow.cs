using System;
using System.Runtime.InteropServices;
using UnityEngine;
 
public class TransparentWindow : MonoBehaviour //script I joinked from unity forums used to allow user to click through window and make it transparent when needed
{
    private struct Margins
    {
        public int CxLeftWidth;
        public int CxRightWidth;
        public int CyTopHeight;
        public int CyBottomHeight;
    }
 
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
 
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
 
    [DllImport("user32.dll")]
    static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
 
    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);
 
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern int SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
 
    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins margins);
 
    const int GwlStyle = -16;
    const uint WsPopup = 0x80000000;
    const uint WsVisible = 0x10000000;
    const int HwndTopmost = -1;
 
    void Start()
    {
#if !UNITY_EDITOR // You really don't want to enable this in the editor..
 
        int fWidth = Screen.width;
        int fHeight = Screen.height;
        var margins = new MARGINS() { cxLeftWidth = -1 };
        var hwnd = GetActiveWindow();
 
        SetWindowLong(hwnd, -20, 524288 | 32);
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, 32 | 64);
        DwmExtendFrameIntoClientArea(hwnd, ref margins);
 
#endif
    }
}