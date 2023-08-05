using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.InputSystem;
using static UnityEditor.Recorder.OutputPath;
using Unity.VisualScripting;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public TextMeshProUGUI Version;
    public TextMeshProUGUI BuildType;
    public GameObject Console;
    public TextMeshProUGUI ConsoleInput;
    public TextMeshProUGUI ConsoleOutput;
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
    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (Console.activeSelf == true)
            {
                Console.SetActive(false);
            }
            else
            {
                Console.SetActive(true);
            }
            
        }
        ConsoleOutput.text = "";
        foreach (var ConsoleMessage in MenuRequester.ConsoleMessages)
        {
            ConsoleOutput.text += ConsoleMessage.ToString();
        }
    }
    private void OnConsoleMessage()
    {
        string[] Parameters = ConsoleInput.text.Split(' ');
        int lastParameter = Parameters.Length - 1;
        Parameters[lastParameter] = Parameters[lastParameter].Remove(Parameters[lastParameter].Length-1);
        string ToPrint = "";

        void CheckForCommand()
        {
            switch(Parameters[0])
            {
                case "help":
                    ToPrint = "Usage:\n" +
                        "print - Will print anything after the word print";
                    break;

                case "print":
                    ToPrint = ConsoleInput.text.Remove(0, 5);
                    break;

                default:
                    ToPrint = "Error No Matching Command";
                    break;
            }
        }
        CheckForCommand();
        MenuRequester.AddMessageToConsole(ToPrint, MessageType.Info);
    }
    private void OnPlayButtonClicked()
    {
        MenuRequester.AddMessageToConsole("Play Button was Clicked Changing Scene", MessageType.Info);
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
