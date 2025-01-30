using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace reminder.Values
{
    public class MessageValues
    {
        public enum MessageIcon
        {
            INFO,
            ERROR,
            WARNING,
            QUESTION
        }

        public static string ErrorText = "Application failed with unknown error";
        public static string OnClosingQuestionText = "Are you sure you want to close ToDoList?";
        public static string WarningText = "";
        public static string InfoText = "";

    }
}
