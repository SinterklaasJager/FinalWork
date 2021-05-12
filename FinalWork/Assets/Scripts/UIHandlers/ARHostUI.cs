using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARHostUI : MonoBehaviour
{
    [SerializeField] private GameObject hostPickGameLocation, btnConfirmation;
    public GameObject spawnedObject;

    private void Start()
    {
        EnableHostPickLocationUI();
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
        NetworkManagerPlus.AREvents.OnHostGameLocationPicked.Invoke(spawnedObject);
    }
}
