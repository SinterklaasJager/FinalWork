using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddPlayerNameUI : MonoBehaviour
{
    [SerializeField] Button btnReady;
    [SerializeField] TMP_InputField inpUserName;

    Enums.EventHandlers events;

    private GameManager gameManager;
    private AddName addName;
    private GameObject networkObj;

    private string userName;

    public void SetNameUI(GameManager gm, GameObject netObj)
    {
        networkObj = netObj;
        gameManager = gm;
        addName = gm.addName;
    }


    public void onPlayerNameValueChange(string name)
    {
        btnReady.interactable = !string.IsNullOrEmpty(name);
    }

    public void ButtonClick()
    {
        userName = inpUserName.text;
        Debug.Log("UserName: " + userName);
        Debug.Log(gameManager);
        Debug.Log(gameManager.addName);
        gameManager.addName.AddUserName(userName, networkObj);
        Destroy(gameObject);
    }
}
