using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowMoreInfoManager : MonoBehaviour
{
    [SerializeField] private GameObject infoContainer, exitConfirmationScreen, moreInfoButton, lessInfoButton;
    [SerializeField] private Slider goodProgress;
    [SerializeField] private Slider badProgress;
    private IntroScreenManager introScreenManager;
    private void Start()
    {
        gameObject.name = "ExtraInfoUI";
    }
    public void ClickMoreInfo()
    {
        infoContainer.SetActive(true);
        moreInfoButton.SetActive(false);
        lessInfoButton.SetActive(true);
    }

    public void ClickLessInfo()
    {
        infoContainer.SetActive(false);
        moreInfoButton.SetActive(true);
        lessInfoButton.SetActive(false);
    }

    public void ClickMorePlayerInfo()
    {
        if (introScreenManager == null)
        {
            // var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            // gameManager.uIManager
            introScreenManager = GameObject.Find("PlayerIntro").GetComponent<IntroScreenManager>();
        }
        introScreenManager.ShowBasicInfo();
    }

    public void ClickLessPlayerInfo()
    {

    }
    public void ClickExitButton()
    {
        exitConfirmationScreen.SetActive(true);
    }

    public void HideExitScreen()
    {
        exitConfirmationScreen.SetActive(false);
    }

    public void ConfirmExitChoice(bool choice)
    {
        if (choice)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            HideExitScreen();
        }
    }

    //progress

    public void SetCurrentScore(int goodPoints, int badPoints)
    {
        if (goodPoints == 0)
        {
            goodProgress.value = 0;
        }
        else if (goodPoints == 1)
        {
            goodProgress.value = 0.27f;
        }
        else if (goodPoints == 2)
        {
            goodProgress.value = 0.47f;
        }
        else if (goodPoints == 3)
        {
            goodProgress.value = 0.65f;
        }
        else if (goodPoints == 4)
        {
            goodProgress.value = 0.815f;
        }
        else if (goodPoints == 5)
        {
            goodProgress.value = 1;
        }

        if (badPoints == 0)
        {
            badProgress.value = 0;
        }
        else if (badPoints == 1)
        {
            badProgress.value = 0.225f;
        }
        else if (badPoints == 2)
        {
            badProgress.value = 0.36f;
        }
        else if (badPoints == 3)
        {
            badProgress.value = 0.49f;
        }
        else if (badPoints == 4)
        {
            badProgress.value = 0.62f;
        }
        else if (badPoints == 5)
        {
            badProgress.value = 0.785f;
        }
        else if (badPoints == 6)
        {
            badProgress.value = 1;
        }

    }
}
