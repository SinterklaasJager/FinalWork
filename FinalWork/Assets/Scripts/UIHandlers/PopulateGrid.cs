using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateGrid : MonoBehaviour
{
    [SerializeField] private GameObject LobbyPlayerPrefab;
    public int objectsToCreate = 0;
    void Start()
    {
        // Populate();
    }

    public void AddPlayerToGrid(string name)
    {
        var newObj = Instantiate(LobbyPlayerPrefab, transform);
        newObj.GetComponent<Image>().color = Random.ColorHSV();
        newObj.GetComponentInChildren<TMP_Text>().text = name;
    }

    public void Populate()
    {
        GameObject newObj;
        for (int i = 0; i < objectsToCreate; i++)
        {
            newObj = Instantiate(LobbyPlayerPrefab, transform);
            newObj.GetComponent<Image>().color = Random.ColorHSV();
        }
    }
}
