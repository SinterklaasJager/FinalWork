using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroScreenManager : MonoBehaviour
{

    [SerializeField] GameObject colonistObject, rebelObject, allyObject, basicInfoObject, moreinfoscreen;
    [SerializeField] TMP_Text txtRole, txtName, txtAllyName, txtAllyRole;
    [SerializeField] Sprite saboteurImage, rebelImage;
    [SerializeField] List<Sprite> colonistImage = new List<Sprite>();
    [SerializeField] Image imgPlayer, imgAlly;
    [SerializeField] List<Button> buttons = new List<Button>();

    private Enums.Role role;

    private int playerNumber;
    private string userName;

    public Color colonistColour = new Color(105, 198, 221);
    public Color rebelColour = new Color(230, 58, 17);

    public void SetUp(Enums.Role role, string name, int playerNumber)
    {
        Debug.Log("intro setup: " + role + name + playerNumber);


        this.playerNumber = playerNumber;
        userName = name;
        this.role = role;

        txtName.text = name;

        if (role == Enums.Role.colonist)
        {
            moreinfoscreen = Instantiate(colonistObject, transform);
            SetButtonColor(colonistColour);
            allyObject.SetActive(false);
        }
        else
        {
            moreinfoscreen = Instantiate(rebelObject, transform);
            SetButtonColor(rebelColour);
            allyObject.SetActive(true);
        }

        moreinfoscreen.SetActive(false);

        SetPlayerValues(role);
    }
    private void SetPlayerValues(Enums.Role role)
    {
        if (role == Enums.Role.colonist)
        {
            txtRole.text = "COLONIST";
            imgPlayer.sprite = colonistImage[playerNumber];

        }
        else if (role == Enums.Role.rebel)
        {
            txtRole.text = "REBEL";
            imgPlayer.sprite = rebelImage;
        }
        else if (role == Enums.Role.saboteur)
        {
            txtRole.text = "SABOTEUR";
            imgPlayer.sprite = saboteurImage;
        }
    }

    public void SetAlly(string allyName)
    {
        txtAllyName.text = allyName;
        if (role == Enums.Role.rebel)
        {
            txtAllyRole.text = "SABOTEUR";
            imgAlly.sprite = saboteurImage;
        }
        else if (role == Enums.Role.saboteur)
        {
            txtAllyRole.text = "REBEL";
            imgAlly.sprite = rebelImage;
        }
    }

    private void SetButtonColor(Color color)
    {
        foreach (var button in buttons)
        {
            // var colors = button.colors;
            // colors.normalColor = color;
            // button.colors = colors;

            button.gameObject.GetComponent<Image>().color = color;
        }

    }

    public void HideBasicInfo()
    {
        basicInfoObject.SetActive(false);
    }

    public void ShowBasicInfo()
    {
        basicInfoObject.SetActive(true);
    }
    public void HideExtraInfo()
    {
        moreinfoscreen.SetActive(false);
    }

    public void ShowExtraInfo()
    {
        moreinfoscreen.SetActive(true);
    }
}
