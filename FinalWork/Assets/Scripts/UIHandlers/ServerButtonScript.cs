using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Mirror.Discovery
{
    public class ServerButtonScript : MonoBehaviour
    {
        private NetworkDiscoveryUI networkDiscoveryUI;
        private ServerResponse info;
        [SerializeField] private TMP_Text txtButton;

        public void SetServerResponseInfo(ServerResponse info, NetworkDiscoveryUI networkDiscoveryUI)
        {
            this.info = info;
            this.networkDiscoveryUI = networkDiscoveryUI;

            txtButton.text = info.EndPoint.Address.ToString();
        }

        public void OnServerButtonClick()
        {
            networkDiscoveryUI.OnServerButtonClick(info);
        }
    }
}