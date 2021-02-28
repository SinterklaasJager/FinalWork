using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Player playerClass = new Player();

    void Start()
    {

    }
    void Update()
    {

    }
    public Player GetPlayerClass()
    {
        return playerClass;
    }
}
