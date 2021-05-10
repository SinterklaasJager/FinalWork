// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Mirror;

// public class BeforeGameStart : NetworkBehaviour
// {
//     [SyncVar(hook = nameof(OnUserNameChange))] public string UserName;
//     public string localUsername;
//     private BeforeGameStart beforeGameStart;
//     private GameManager gameManager;

//     private void Start()
//     {
//         beforeGameStart = gameObject.GetComponent<BeforeGameStart>();
//     }

//     private void OnUserNameChange(string oldUsername, string newUsername)
//     {
//         Debug.Log("On UserName Changed: " + newUsername);
//         // gameManager.SetPlayerName(beforeGameStart);
//     }

//     public override void OnStartAuthority()
//     {
//         Debug.Log("onstartauthority");
//         cmdSetUserName(NetworkManagerHubDoublePlus.userName);
//     }

//     // public void SetName(string name)
//     // {
//     //     Debug.Log("Before Game Start; Set USER NAME: " + name);
//     //     newUsername = name;
//     //     Debug.Log("Before Game Start; Set USER NAME: " + newUsername);
//     //     //  cmdSetUserName(name);
//     // }
//     // [TargetRpc]
//     // public void targetSetUserName(NetworkConnection target, GameManager gm)
//     // {
//     //     gameManager = gm;
//     //     Debug.Log("TARGET RPC NEW USERNAME: " + newUsername + " +++ " + NetworkManagerHubDoublePlus.userName);
//     //     cmdSetUserName(newUsername);
//     // }

//     [Command]
//     public void cmdSetUserName(string name)
//     {
//         UserName = name;
//         Debug.Log("syncvarring name: " + UserName);
//     }
// }
