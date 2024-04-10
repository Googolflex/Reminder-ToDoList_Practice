using Microsoft.Win32;
using System.Reflection;

namespace reminder
{
    internal class AutoRunManager
    {
        private string executablePath;
        private string appRegistryKey;

        Path path = new Path();

        public AutoRunManager(string registryKey)
        {
            executablePath = Assembly.GetExecutingAssembly().Location;
            appRegistryKey = registryKey;
        }

        public bool IsAutoRunEnabled()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path.AutoRunKey, true))
            {
                return key?.GetValue(appRegistryKey) as string == executablePath;
            }
        }

        public void AddToAutoRun()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path.AutoRunKey, true))
            {
                key?.SetValue(appRegistryKey, executablePath);
            }
        }

        public void RemoveFromAutoRun()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path.AutoRunKey, true))
            {
                key?.DeleteValue(appRegistryKey, false);
            }
        }
    }
}
