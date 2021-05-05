using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Mirror.Discovery
{
    public class NetworkManagerHubPlus : MonoBehaviour
    {

        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

        public NetworkDiscovery networkDiscovery;

        [SerializeField] private GameObject networkDiscoveryPreFab;
        [SerializeField] private GameObject btnServer;
        private GameObject networkDiscoveryObj;
        [SerializeField] private Canvas cvsNetworkCanvas;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (networkDiscovery == null)
            {
                networkDiscovery = GetComponent<NetworkDiscovery>();

                UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
                UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");

            }
        }

#endif

        void Start()
        {
            if (NetworkManager.singleton == null)
                return;

            if (NetworkServer.active || NetworkClient.active)
                return;

            if (!NetworkClient.isConnected && !NetworkServer.active && !NetworkClient.active)
                DrawGUI();
        }

        public void DrawGUI()
        {
            networkDiscoveryObj = Instantiate(networkDiscoveryPreFab, cvsNetworkCanvas.transform);
            networkDiscoveryObj.GetComponent<NetworkDiscoveryUI>().nmhp = this;
        }

        public void OnFindServersClick()
        {
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();

            ShowServerList();
        }

        public void OnHostServerClick()
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
            Destroy(networkDiscoveryObj);
        }

        public void ShowServerList()
        {
            // GUILayout.Label($"Discovered Servers [{discoveredServers.Count}]:");

            // servers
            //  scrollViewPos = GUILayout.BeginScrollView(scrollViewPos);
            var netdisui = networkDiscoveryObj.GetComponent<NetworkDiscoveryUI>();
            var list = netdisui.scrollableListObject;
            list.SetActive(true);
            Debug.Log("show server list");
            Debug.Log("discovered server: " + discoveredServers);
            foreach (ServerResponse info in discoveredServers.Values)
            {
                Debug.Log("add Adress");
                Instantiate(btnServer, list.transform);
                btnServer.GetComponent<ServerButtonScript>().SetServerResponseInfo(info, netdisui);
            }
            // if (GUILayout.Button(info.EndPoint.Address.ToString()))
            //     Connect(info);

            // GUILayout.EndScrollView();
        }


        public void Connect(ServerResponse info)
        {
            NetworkManager.singleton.StartClient(info.uri);
        }

        public void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            discoveredServers[info.serverId] = info;
        }
    }
}
