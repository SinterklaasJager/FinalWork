using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YouDiedUI : MonoBehaviour
{

    public void OnSpectateClick()
    {
        Destroy(gameObject);
    }
}
