using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private String gameplayScene = "";
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
                MenuRequester.SetIsConsoleOpen(false);
            }
            else
            {
                Console.SetActive(true);
                MenuRequester.SetIsConsoleOpen(true);
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
    public void OnClientButtonClicked()
    {
        MenuRequester.AddMessageToConsole("Client Button was Clicked Changing Scene", MessageType.Info);
        NetworkManager.Singleton.StartClient();
    }
    public void OnHostButtonClicked()
    {
        MenuRequester.AddMessageToConsole("Host Button was Clicked Changing Scene", MessageType.Info);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(gameplayScene, LoadSceneMode.Single);
    }
    public void OnServerButtonClicked()
    {
        MenuRequester.AddMessageToConsole("Server Button was Clicked Changing Scene", MessageType.Info);
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene(gameplayScene, LoadSceneMode.Single);
    }
    private void OnExitButtonClicked()
    {
        if (Debug.isDebugBuild)
        {
            MenuRequester.AddMessageToConsole("Exit Pressed");
        }
        Application.Quit();
    }
}
