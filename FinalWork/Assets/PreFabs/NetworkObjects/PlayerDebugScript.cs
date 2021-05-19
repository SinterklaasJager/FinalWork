using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerDebugScript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI txtPlayerID;

    public void Start()
    {

    }

    public void SetPlayerID(int playerID)
    {
        // Debug.Log(playerID);
        txtPlayerID.text = playerID.ToString();
    }
}
