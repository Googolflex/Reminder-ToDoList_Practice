using Microsoft.Win32;
using System.Reflection;

namespace reminder
{
    internal class AutoRunManager
    {
        private string executablePath;
        private string appRegistryKey;

        public AutoRunManager(string registryKey)
        {
            executablePath = Assembly.GetExecutingAssembly().Location;
            appRegistryKey = registryKey;
        }

        public bool IsAutoStartEnabled()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                return key?.GetValue(appRegistryKey) as string == executablePath;
            }
        }

        public void AddToAutoStart()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                key?.SetValue(appRegistryKey, executablePath);
            }
        }

        public void RemoveFromAutoStart()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                key?.DeleteValue(appRegistryKey, false);
            }
        }
    }
}
