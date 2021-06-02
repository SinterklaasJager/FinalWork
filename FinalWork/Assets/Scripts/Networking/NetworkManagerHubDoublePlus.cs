using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class NetworkManagerHubDoublePlus : MonoBehaviour
{
    [SerializeField] private Button buttonHost, buttonServer, buttonClient, buttonStop, buttonConfirm;
    // [SerializeField] private GameObject PanelStart, PanelStop;
    [SerializeField] private TMP_InputField inputFieldAddress;
    [SerializeField] private TMP_Text serverText, clientText, attemptingToConnectText, connectionErrorText;
    [SerializeField] private NetworkManagerPlus networkManagerPlus;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject ipInputPanel, mainMenu;
    [SerializeField] private AudioManager globalAudio;
    [SerializeField] private MainMenuManager mainMenuManager;

    private bool clientConnected, clientConnectionFailed;

    [Header("AR Components")]

    public static string userName;

    private void OnEnable()
    {
        NetworkManagerPlus.onClientConnected += HandleClientConnected;
        NetworkManagerPlus.onClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerPlus.onClientConnected -= HandleClientConnected;
        NetworkManagerPlus.onClientDisconnected -= HandleClientDisconnected;
    }
    public void Start()
    {

        //Update the canvas text if you have manually changed network managers address from the game object before starting the game scene
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }

        //Adds a listener to the main input field and invokes a method when the value changes.
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        //Make sure to attach these Buttons in the Inspector
        buttonHost.onClick.AddListener(ButtonHost);
        buttonServer.onClick.AddListener(ButtonServer);
        buttonClient.onClick.AddListener(ButtonClient);
        //  buttonStop.onClick.AddListener(ButtonStop);

        //This updates the Unity canvas, we have to manually call it every change, unlike legacy OnGUI.
        //  SetupCanvas();
    }

    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    public void onPlayerNameValueChange(string name)
    {
        buttonConfirm.interactable = !string.IsNullOrEmpty(name);
    }

    public void ButtonHost()
    {
        //ARObject.SetActive(true);
        //  NetworkManagerPlus.AREvents.OnHostGameLocationPicked = (gameLocationObj) => OnHostLocationPicked(gameLocationObj);
        NetworkManager.singleton.StartHost();
        gameObject.SetActive(false);
    }
    public void OnHostLocationPicked(GameObject gameLocationObj)
    {
        NetworkManager.singleton.StartHost();
    }
    public void ButtonServer()
    {
        NetworkManager.singleton.StartServer();
    }

    public void ButtonClient()
    {
        clientConnectionFailed = false;
        buttonClient.interactable = false;
        connectionErrorText.gameObject.SetActive(false);
        // ARObject.SetActive(true);
        NetworkManager.singleton.StartClient();
        StartCoroutine("ConnectionMessage");
    }

    private IEnumerator ConnectionMessage()
    {
        while (!clientConnected && !clientConnectionFailed)
        {
            attemptingToConnectText.gameObject.SetActive(true);
            attemptingToConnectText.text = "Attempting to connect";
            yield return new WaitForSeconds(.5f);
            attemptingToConnectText.text = "Attempting to connect.";
            yield return new WaitForSeconds(.5f);
            attemptingToConnectText.text = "Attempting to connect..";
            yield return new WaitForSeconds(.5f);
            attemptingToConnectText.text = "Attempting to connect...";
            yield return new WaitForSeconds(.5f);
        }
        // if (clientConnectionFailed)
        // {
        //     attemptingToConnectText.gameObject.SetActive(false);
        //     connectionErrorText.gameObject.SetActive(true);
        //     yield return null;
        // }

    }

    private void HandleClientConnected()
    {
        mainMenuManager.PlayMainMenuSong(false);
        mainMenu.SetActive(false);
        clientConnected = true;
        globalAudio.PlayIntroTune(true);
    }
    private void HandleClientDisconnected()
    {
        mainMenu.SetActive(true);
        buttonClient.interactable = true;
        clientConnected = false;
        clientConnectionFailed = true;
        attemptingToConnectText.gameObject.SetActive(false);
        connectionErrorText.gameObject.SetActive(true);
        StopCoroutine("ConnectionMessage");
    }

    public void LeaveGame()
    {
        mainMenu.SetActive(true);
        buttonClient.interactable = true;
        clientConnected = false;
        ipInputPanel.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }

    public void ButtonStop()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }

        SetupCanvas();
    }

    public void HideIPPanel()
    {
        Debug.Log("HideIpPanel");
        ipInputPanel.SetActive(false);
        StopCoroutine("ConnectionMessage");
        NetworkManager.singleton.StopClient();
        attemptingToConnectText.gameObject.SetActive(false);
        connectionErrorText.gameObject.SetActive(false);
    }

    public void ShowIPPanel()
    {
        ipInputPanel.SetActive(true);
    }

    public void SetupCanvas()
    {
        mainMenu.SetActive(false);
        /*
        // Here we will dump majority of the canvas UI that may be changed.

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (NetworkClient.active)
            {
                // PanelStart.SetActive(false);
                // PanelStop.SetActive(true);
                clientText.text = "Connecting to " + NetworkManager.singleton.networkAddress + "..";
            }
            else
            {
                // PanelStart.SetActive(true);
                // PanelStop.SetActive(false);
            }
        }
        else
        {
            // PanelStart.SetActive(false);
            // PanelStop.SetActive(true);

            // server / client status message
            if (NetworkServer.active)
            {
                serverText.text = "Server: active. Transport: " + Transport.activeTransport;
            }
            if (NetworkClient.isConnected)
            {
                clientText.text = "Client: address=" + NetworkManager.singleton.networkAddress;
            }
        }*/
    }
}

