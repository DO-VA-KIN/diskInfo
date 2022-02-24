using System;
using System.Diagnostics;
using System.IO;


namespace diskInfo
{
    class staticFunc
    {

        public static string parseWay(string way)
        {
            int end = way.LastIndexOf(".");
            if (end == -1)
                end = way.Length;
            int beg = way.LastIndexOf("\\");

            way = way.Remove(end, way.Length - end);
            way = way.Remove(0, beg + 1);
            return way;
        }


        public static void checkCreate_Exist_SettingsFile(string way)
        {
            FileInfo fileInfo = new FileInfo(way);
            if (fileInfo.Exists == false)
            { File.Create(way); }
        }


        public static void killProcessByName()
        {
            Process[] proc = Process.GetProcessesByName("Notepad");
            foreach (Process process in proc)
            {
                if (parseWay(process.MainWindowTitle) == "settingsFile")
                    process.Kill();
            }
        }
    }
}
