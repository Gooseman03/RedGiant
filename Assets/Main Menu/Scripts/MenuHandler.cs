using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.FullSerializer;

public class MenuHandler : MonoBehaviour
{
    public TextMeshProUGUI Version;
    public TextMeshProUGUI BuildType;
    public TextMeshProUGUI ConsoleInput;
    public TextMeshProUGUI ConsoleOutput;
    public prefab
    private void Start()
    {
        if (Debug.isDebugBuild)
        {
            BuildType.text = "Dev Build";
            Version.text = Application.version;

        }
        else
        {
            Destroy(BuildType);
            Destroy(Version);
        }
    }
    public void PrintToConsole(string Message = "")
    {
        OnConsoleMessage(Message);
    }
    private void OnConsoleMessage(string ForcedMessage = "")
    {
        string[] Parameters = ConsoleInput.text.Split(' ');
        string ToPrint = "";

        void CheckForCommand()
        {
            if (Parameters[0] == "print")
            {
                ToPrint = Parameters[1];
                return;
            }
            ToPrint = "Error No Matching Command";
        }

        if (ForcedMessage != "")
        {
            ToPrint = ForcedMessage;
        }
        else
        {
            CheckForCommand();
        }
        ConsoleOutput.text = ConsoleOutput.text + "[" + System.DateTime.Now.ToString("hh:mm:ss") + "] " +ToPrint+ '\n';
    }
    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void OnExitButtonClicked()
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log("Exit Pressed");
        }
        Application.Quit();
    }
}
