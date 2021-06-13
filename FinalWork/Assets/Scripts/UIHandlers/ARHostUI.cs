using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Mirror;

public class ARHostUI : NetworkBehaviour
{
    [SerializeField] private GameObject hostPickGameLocation, confirmationButton, failureMessage;
    [SerializeField] private Button btnConfirmation;
    private GameManager gameManager;
    public GameObject spawnedObject;
    public NetworkManagerPlus networkManagerPlus;
    public ARAnchor anchor;


    private void OnAwake()
    {
        Debug.Log("ARHOST UI AWOKE");
        failureMessage.SetActive(false);
        confirmationButton.SetActive(false);
    }
    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void EnableHostPickLocationUI()
    {
        hostPickGameLocation.SetActive(true);
    }

    public void DisableHostPickLocationUI()
    {
        hostPickGameLocation.SetActive(false);
    }

    public void EnableConfirmationButton()
    {
        confirmationButton.SetActive(true);
        btnConfirmation.interactable = true;
    }

    public void FailedToHostAnchor()
    {
        failureMessage.SetActive(true);
        EnableConfirmationButton();
    }
    private void SetAnchor(GameObject AnchorController)
    {
        AnchorController.GetComponent<AnchorController>().HostAnchor(anchor);
        Destroy(spawnedObject);
        //Destroy(gameObject);

    }

    public void ConfirmClick()
    {
        if (gameManager == null)
        {
            Debug.Log("gameManager = null");
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        btnConfirmation.interactable = false;
        gameManager.AREvents.OnReadyToSetAnchor = (AnchorController) => SetAnchor(AnchorController);
        Debug.Log("confirm Click");
        Debug.Log("spawnedObject: " + spawnedObject);

        Debug.Log("Trigger GameLocation Event");
        // SpawnGameLocation(spawnedObject);
        gameManager.SetGameLocationPosition(spawnedObject.transform.position, spawnedObject.transform.rotation);

        //gameManager.AREvents.OnHostGameLocation.Invoke(spawnedObject.transform.position, spawnedObject.transform.rotation);


    }
}
