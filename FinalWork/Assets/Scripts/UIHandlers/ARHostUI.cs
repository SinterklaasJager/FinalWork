using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARHostUI : MonoBehaviour
{
    [SerializeField] private GameObject hostPickGameLocation, btnConfirmation;
    private GameManager gameManager;
    public GameObject spawnedObject;
    public NetworkManagerPlus networkManagerPlus;


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

    public void ConfirmClick()
    {
        Debug.Log("confirm Click");
        Debug.Log("spawnedObject: " + spawnedObject);

        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager(clone)").GetComponent<GameManager>();
        }

        if (gameManager.AREvents.OnHostGameLocation != null)
        {
            gameManager.AREvents.OnHostGameLocation.Invoke(spawnedObject.transform.position, spawnedObject.transform.rotation);
            Destroy(gameObject);
        }

    }
}
