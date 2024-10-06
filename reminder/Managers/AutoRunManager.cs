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

        public string ManageAutorun(string option)
        {
            string message;
            if (option == "Add to autorun")
            {
                //If autorun is already enabled drops message
                if (!IsAutoRunEnabled())
                {
                    AddToAutoRun();
                    message = "The application has been added to autorun";
                }
                else
                    message = "The application has already been added to autorun";
            }
            else
            {
                //If autorun is already disabled drops message
                if (IsAutoRunEnabled())
                {
                    RemoveFromAutoRun();
                    message = "The application has been removed from autorun";
                }
                else
                    message = "The application is not in autorun";
            }
            return message;
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
