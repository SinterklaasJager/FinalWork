using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Mirror.Discovery
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkDiscoveryHUD")]
    [HelpURL("https://mirror-networking.com/docs/Articles/Components/NetworkDiscovery.html")]
    [RequireComponent(typeof(NetworkDiscovery))]
    public class NewNetworkDiscoveryHUD : MonoBehaviour
    {
        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
        Vector2 scrollViewPos = Vector2.zero;

        public NetworkDiscovery networkDiscovery;

        [SerializeField]
        private TMP_Text txtAmountOfServersFound;
        [SerializeField]
        private GameObject serverListContent, serverButton, content;
        private List<GameObject> serverButtons = new List<GameObject>();

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

        public void OnHostClicked()
        {
            Debug.Log("advertise server");
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        public void OnFindServersClicked()
        {
            ClearButtonsObjectsFromServerList();
            networkDiscovery.StartDiscovery();

            txtAmountOfServersFound.text = $"Discovered Servers [{discoveredServers.Count}]:";

            foreach (ServerResponse info in discoveredServers.Values)
            {
                var obj = Instantiate(serverButton, serverListContent.transform);
                obj.GetComponent<ServerButtonScript>().SetServerResponseInfo(info, this);
                serverButtons.Add(obj);
            }
            // if (GUILayout.Button(info.EndPoint.Address.ToString()))
            //     Connect(info);
        }

        public void OnClientJoinServerClicked(ServerResponse info)
        {
            Connect(info);
        }

        public void OnStartServerClicked()
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartServer();

            networkDiscovery.AdvertiseServer();
        }

        void Connect(ServerResponse info)
        {
            NetworkManager.singleton.StartClient(info.uri);
        }

        public void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method

            txtAmountOfServersFound.text = $"Discovered Servers [{discoveredServers.Count}]:";
            Debug.Log("discoveredServers: " + info.serverId);

            discoveredServers[info.serverId] = info;
        }

        public void ShowServerWindow(bool show)
        {
            if (show)
            {
                content.SetActive(true);
                OnFindServersClicked();
            }
            else
            {
                content.SetActive(false);
            }
        }

        private void ClearButtonsObjectsFromServerList()
        {
            if (serverButtons.Count != 0)
            {
                foreach (var button in serverButtons)
                {
                    GameObject.Destroy(button);
                }
                serverButtons.Clear();
            }
        }

    }
}
