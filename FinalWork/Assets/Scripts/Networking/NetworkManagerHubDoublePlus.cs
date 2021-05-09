using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class NetworkManagerHubDoublePlus : MonoBehaviour
{
    [SerializeField] private Button buttonHost, buttonServer, buttonClient, buttonStop, buttonConfirm;
    // [SerializeField] private GameObject PanelStart, PanelStop;
    [SerializeField] private TMP_InputField inputFieldAddress, inputFieldUserName;
    [SerializeField] private TMP_Text serverText, clientText;
    [SerializeField] private GameObject playerNameInputObject, beforeGameStartObject;
    [SerializeField] private NetworkManagerPlus networkManagerPlus;
    // [SerializeField] private BeforeGameStart beforeGameStart;
    private Enums.MenuButtonType clickedButton;

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

    public void ButtonClick()
    {
        userName = inputFieldUserName.text;
        Debug.Log("username: " + userName);
        //  beforeGameStart.SetName(userName);
        playerNameInputObject.SetActive(false);
        if (clickedButton == Enums.MenuButtonType.server)
        {
            NetworkManager.singleton.StartServer();
        }
        else if (clickedButton == Enums.MenuButtonType.client)
        {
            buttonClient.interactable = false;
            NetworkManager.singleton.StartClient();
        }
        else if (clickedButton == Enums.MenuButtonType.host)
        {
            NetworkManager.singleton.StartHost();
            gameObject.SetActive(false);
        }
        // SetupCanvas();
    }

    public void ButtonHost()
    {
        clickedButton = Enums.MenuButtonType.host;
        playerNameInputObject.SetActive(true);
    }

    public void ButtonServer()
    {
        clickedButton = Enums.MenuButtonType.server;
        playerNameInputObject.SetActive(true);
    }

    public void ButtonClient()
    {
        clickedButton = Enums.MenuButtonType.client;
        playerNameInputObject.SetActive(true);
    }

    private void HandleClientConnected()
    {
        //beforeGameStart.cmdSetUserName(userName);
        //beforeGameStart.SetName(userName);
        gameObject.SetActive(false);
    }
    private void HandleClientDisconnected()
    {
        buttonClient.interactable = true;
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

    public void SetupCanvas()
    {
        gameObject.SetActive(false);
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

