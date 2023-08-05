using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    Info,
    Warn, 
    Error
}

public static class MenuRequester
{
    public class ConsoleMessage
    {
        public MessageType type;
        public string message;
        public string TimeStamp;
        public override string ToString()
        {
            string returnString = string.Format("[{0}] [{1}]: {2}\n",TimeStamp,type,message);
            return returnString;
        }
    }

    private static MenuHandler MenuHandler;
    public static List<ConsoleMessage> ConsoleMessages { get; private set; } = new List<ConsoleMessage>();
    /// <summary>
    /// Will Add a Message to the ingame Console 
    /// Usage: AddMessageToConsole(String)
    /// Usage: AddMessageToConsole(String, MessageType)
    /// </summary>
    public static void AddMessageToConsole(string message, MessageType type = MessageType.Info)
    {
        ConsoleMessages.Add(new ConsoleMessage { type = type, message = message, TimeStamp = System.DateTime.Now.ToString("hh:mm:ss") });
    }
}
