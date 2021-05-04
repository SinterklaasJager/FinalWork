using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIComponent : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private Image imgRoleImage;
    [SerializeField] private TMP_Text txtPlayer;
    [SerializeField] private TMP_Text txtRole;
    [SerializeField] private TMP_Text txtMoreRoleInfo;
    [SerializeField] List<Sprite> images = new List<Sprite>();
    private int roleNum;
    private bool moreInfoActive = false;
    public void SetUI(string playerName, int roleNum)
    {
        txtPlayer.text = playerName;
        this.roleNum = roleNum;
        Debug.Log("role num: " + roleNum);
        imgRoleImage.sprite = images[roleNum];

        ConvertRole(roleNum);
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

    private void ShowMoreInfo()
    {
        txtMoreRoleInfo.gameObject.SetActive(true);
        background.offsetMin = new Vector2(-108, -100);
    }

    private void ShowLessInfo()
    {
        txtMoreRoleInfo.gameObject.SetActive(false);
        background.offsetMin = new Vector2(-108, -20);
    }

    private void ConvertRole(int roleNum)
    {
        if (roleNum == 0)
        {
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
