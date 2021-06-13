using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VictoryScreenHandler : MonoBehaviour
{
    [SerializeField] TMP_Text txtVictory, txtScore, txtReason;
    [SerializeField] Image imgBackground;
    [SerializeField] Color colonistColour, rebelColour;
    [SerializeField] private AudioClip colonistClip, rebelClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SetVictory(Enums.GameEndReason reason, int badPoints, int goodPoints)
    {
        Debug.Log(reason + goodPoints + badPoints);

        txtScore.text = goodPoints + "-" + badPoints;
        if (reason == Enums.GameEndReason.enoughGoodPoints || reason == Enums.GameEndReason.rebelsDead || reason == Enums.GameEndReason.saboteurDead)
        {
            txtVictory.text = "COLONIST VICTORY";
            imgBackground.color = colonistColour;
            audioSource.PlayOneShot(colonistClip);
        }
        else
        {
            txtVictory.text = "REBEL VICTORY";
            imgBackground.color = rebelColour;
            audioSource.PlayOneShot(rebelClip);
        }

        if (reason == Enums.GameEndReason.enoughGoodPoints)
        {
            txtReason.text = "The rocket succesfully landed on Mars!";
        }
        else if (reason == Enums.GameEndReason.saboteurDead)
        {
            txtReason.text = "The saboteur has been KILLED";
        }
        else if (reason == Enums.GameEndReason.bombPlaced)
        {
            txtReason.text = "The SABOTEUR was able to plant a bomb!";
        }
        else if (reason == Enums.GameEndReason.enoughBadPoints)
        {
            txtReason.text = "The rocket had too many sabotaged parts!";
        }
        else if (reason == Enums.GameEndReason.goodGuysDead)
        {
            txtReason.text = "All colonists have died...";
        }
    }
    public void QuitGame()
    {

        SceneManager.LoadScene("MainScene");

    }
}
