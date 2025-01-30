using Microsoft.Win32;
using System.Reflection;

namespace reminder
{
    public class AutoRunManager
    {
        private string executablePath;
        private string appRegistryKey;

        Path path = new Path();

        public AutoRunManager(string registryKey)
        {
            executablePath = Assembly.GetExecutingAssembly().Location;
            appRegistryKey = registryKey;
        }

        public bool ManageAutorun(bool option)
        {
            bool res;
            if (option == true)
            {
                //If autorun is already enabled drops message
                if (!IsAutoRunEnabled())
                {
                    AddToAutoRun();
                    res = true;
                }
                else
                    res = false;
            }
            else
            {
                //If autorun is already disabled drops message
                if (IsAutoRunEnabled())
                {
                    RemoveFromAutoRun();
                    res = true;
                }
                else
                    res = false;
            }
            return res;
        }

        private bool IsAutoRunEnabled()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path.AutoRunKey, true))
            {
                return key?.GetValue(appRegistryKey) as string == executablePath;
            }
        }

        private void AddToAutoRun()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path.AutoRunKey, true))
            {
                key?.SetValue(appRegistryKey, executablePath);
            }
        }

        private void RemoveFromAutoRun()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path.AutoRunKey, true))
            {
                key?.DeleteValue(appRegistryKey, false);
            }
        }
    }
}
