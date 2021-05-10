using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjects : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject roundManager;
    public GameObject cardGeneration;
    public GameObject universalCanvas;
    public GameObject roundUI;
    public GameObject UIManager;
    public GameObject VoteTeamLeader;
    public GameObject KillAPlayerUI;
    public GameObject YouAreDeadUI;
    public GameObject GetPlayerNameUI;

    [Header("UI Components")]
    public GameObject btnGood;
    public GameObject btnBad;
    public GameObject btnPickAnAssistant;
    public GameObject btnPickAPlayerToKill;


    [Header("Rocket Components")]
    public GameObject victoryProgress;
    public GameObject goodRocketComponent;
    public GameObject badRocketComponent;

}
