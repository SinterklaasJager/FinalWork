using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private GameObject msgObject, msgLocation;
    public void Message(string message)
    {
        Instantiate(msgObject, msgLocation.transform);
        msgObject.GetComponent<Text>().text = message;
    }

}
