using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace RunApp
{
    internal class IniFile
    {
        private readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name; // Имя приложения
        private readonly string AppNameWithPath;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        // Конструктор класса
        public IniFile(string IniPath = null)
        {
            AppNameWithPath = new FileInfo(IniPath ?? AppName + ".ini").FullName;
        }

        // Запись ключа Key со значением Value в секцию Section ini-файла
        public void WriteKey(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? AppName, Key, Value, AppNameWithPath);
        }

        // Чтение ключа Key из секции Section ini-файла
        public string ReadKey(string Key, string Section = null)
        {
            int Size = 1024;
            var RetVal = new StringBuilder(Size);
            GetPrivateProfileString(Section ?? AppName, Key, "", RetVal, Size, AppNameWithPath);
            return RetVal.ToString();
        }

        // Удаление ключа Key из секции Section ini-файла
        public void DeleteKey(string Key, string Section = null)
        {
            WriteKey(Key, null, Section ?? AppName);
        }

        // Удаление секции Section из ini-файла
        public void DeleteSection(string Section = null)
        {
            WriteKey(null, null, Section ?? AppName);
        }

        // Проверка существования секции Section в ini-файле
        public bool KeyExists(string Key, string Section = null)
        {
            return ReadKey(Key, Section).Length > 0;
        }

        // Удаление ini-файла
        public bool DeleteFile()
        {
            if (File.Exists(AppNameWithPath))
            {
                File.Delete(AppNameWithPath);
            }
            return !File.Exists(AppNameWithPath);
        }

    }
}
