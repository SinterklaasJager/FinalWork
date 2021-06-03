using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Mirror.Discovery
{
    public class ServerButtonScript : MonoBehaviour
    {
        //  private NetworkDiscoveryUI networkDiscoveryUI;
        private NewNetworkDiscoveryHUD newNetworkDiscoveryHUD;
        private ServerResponse info;
        [SerializeField] private TMP_Text txtButton;
        public void SetServerResponseInfo(ServerResponse info, NewNetworkDiscoveryHUD newNetworkDiscoveryHUD)
        {
            this.info = info;
            this.newNetworkDiscoveryHUD = newNetworkDiscoveryHUD;

            txtButton.text = info.EndPoint.Address.ToString();
        }

        public void OnServerButtonClick()
        {
            newNetworkDiscoveryHUD.OnClientJoinServerClicked(info);
        }
    }
}