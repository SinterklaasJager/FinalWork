using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Mirror;

public class ARHostUI : NetworkBehaviour
{
    [SerializeField] private GameObject hostPickGameLocation, btnConfirmation;
    private GameManager gameManager;
    public GameObject spawnedObject;
    public NetworkManagerPlus networkManagerPlus;
    public ARAnchor anchor;


    private void OnAwake()
    {
        Debug.Log("ARHOST UI AWOKE");
    }
    public void SetGameManager(GameManager gameManager)
    {
        Debug.Log("GameManager");
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
        btnConfirmation.SetActive(true);
    }
    private void SetAnchor(GameObject AnchorController)
    {
        AnchorController.GetComponent<AnchorController>().HostAnchor(anchor);
        Destroy(gameObject);

    }
    [TargetRpc]
    private void TargetSetAnchor()
    {

    }
    public void ConfirmClick()
    {
        gameManager.AREvents.OnReadyToSetAnchor = (AnchorController) => SetAnchor(AnchorController);
        Debug.Log("confirm Click");
        Debug.Log("spawnedObject: " + spawnedObject);

        if (gameManager == null)
        {
            Debug.Log("gameManager = null");
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        Debug.Log("Trigger GameLocation Event");
        // SpawnGameLocation(spawnedObject);
        gameManager.SetGameLocationPosition(spawnedObject.transform.position, spawnedObject.transform.rotation);
        Destroy(spawnedObject);
        //gameManager.AREvents.OnHostGameLocation.Invoke(spawnedObject.transform.position, spawnedObject.transform.rotation);


    }
}
