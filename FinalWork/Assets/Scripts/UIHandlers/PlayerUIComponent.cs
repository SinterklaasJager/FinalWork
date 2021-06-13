using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIComponent : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private Image imgRoleImage;
    [SerializeField] private List<Image> imgAllies = new List<Image>();
    [SerializeField] private GameObject alliesListGameObject;
    [SerializeField] private Button btnAlliesList;
    [SerializeField] private TMP_Text txtPlayer;
    [SerializeField] private TMP_Text txtRole;
    [SerializeField] private TMP_Text txtMoreRoleInfo;
    [SerializeField] List<Sprite> images = new List<Sprite>();
    private int roleNum;
    private bool moreInfoActive, alliesListBool;
    private GameManager gameManager;
    private int i = 0;
    public void SetUI(string playerName, int roleNum, GameManager gm)
    {
        gameManager = gm;
        txtPlayer.text = playerName;
        this.roleNum = roleNum;
        Debug.Log("role num: " + roleNum);
        imgRoleImage.sprite = images[roleNum];

        ConvertRole(roleNum);
    }

    public void SetAllies(int roleNum, string playerName)
    {
        imgAllies[i].sprite = images[roleNum];
        imgAllies[i].gameObject.GetComponentInChildren<TMP_Text>().text = playerName;
        i++;
    }

    public void ShowInfo()
    {
        if (moreInfoActive)
        {
            ShowLessInfo();
            moreInfoActive = false;
        }
        else
        {
            ShowMoreInfo();
            moreInfoActive = true;
        }
    }
    public void ShowAlliesList()
    {
        if (alliesListBool)
        {
            alliesListGameObject.SetActive(false);
            alliesListBool = false;
        }
        else
        {
            alliesListGameObject.SetActive(true);
            alliesListBool = true;
        }
    }
    private void ShowMoreInfo()
    {
        txtMoreRoleInfo.gameObject.SetActive(true);
        background.offsetMin = new Vector2(-108, -100);
    }

    private void ShowLessInfo()
    {
        txtMoreRoleInfo.gameObject.SetActive(false);
        background.offsetMin = new Vector2(-108, 8);
    }

    private void ConvertRole(int roleNum)
    {
        if (roleNum == 0)
        {
            btnAlliesList.gameObject.SetActive(false);

            txtRole.text = "Good Guy";
            txtMoreRoleInfo.text = "You are a Good Guy. Try to build the Rocket, find who is secretly a rebel, and kill the Saboteur!";
        }
        if (roleNum == 1)
        {
            txtRole.text = "Rebel";
            txtMoreRoleInfo.text = "You are a rebel, the same people who poisened our world are now leaving us behind. Don't let them escape and destroy their rocket, their actions will have consequences!";
        }
        if (roleNum == 2)
        {
            txtRole.text = "The Saboteur";
            txtMoreRoleInfo.text = "You are the fabled 'Saboteur', you have sabotaged dozens of rockets before, and this one won't be your last. Salute comrade!";
        }
    }

}
