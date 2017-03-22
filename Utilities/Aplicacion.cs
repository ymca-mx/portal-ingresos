using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utilities
{
    public class Aplicacion
    {

        #region Servicios Interop - Mover

        const int WM_SYSCOMMAND = 0x112;
        const int MOUSE_MOVE = 0xF012;
        //
        // Declaraciones del API
        [System.Runtime.InteropServices.DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        //
        [System.Runtime.InteropServices.DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        #endregion

        #region Servicios Interop - No Cerrar
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);

        [DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
        private static extern int GetMenuItemCount(IntPtr hmenu);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        private static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);

        [DllImport("user32.dll", EntryPoint = "DrawMenuBar")]
        private static extern int DrawMenuBar(IntPtr hwnd);

        private const int MF_BYPOSITION = 0x0400;
        private const int MF_DISABLED = 0x0002;
        #endregion

        public static bool UnicaInstancia(string Aplicacion)
        {
            bool Valor;
            Mutex Instancia = new Mutex(true, Aplicacion, out Valor);

            return Valor;
        }

        public static string DireccionFisica()
        {
            return (from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                  ).FirstOrDefault().ToString();
        }

        public static void Mover(Form Ventana)
        {
            ReleaseCapture();
            SendMessage(Ventana.Handle, WM_SYSCOMMAND, MOUSE_MOVE, 0);    
        }

        public static void NoCerrar(Form Ventana)
        {
            IntPtr hmenu = GetSystemMenu(Ventana.Handle, 0);
            int cnt = GetMenuItemCount(hmenu);
            RemoveMenu(hmenu, cnt - 1, MF_DISABLED | MF_BYPOSITION);
            RemoveMenu(hmenu, cnt - 2, MF_DISABLED | MF_BYPOSITION);
            DrawMenuBar(Ventana.Handle);
        }
    }
}
