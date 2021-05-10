using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Mirror.Discovery
{
    public class NetworkDiscoveryUI : MonoBehaviour
    {
        public NetworkManagerHubPlus nmhp;
        public GameObject scrollableListObject;
        [SerializeField] private GameObject buttonPreFab;

        public void HostAGame()
        {
            nmhp.OnHostServerClick();
        }

        public void JoinAGame()
        {
            nmhp.OnFindServersClick();
        }

        public void OnServerButtonClick(ServerResponse info)
        {
            nmhp.Connect(info);
        }
    }
}
