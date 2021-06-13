using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FailedElectionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text txtRemainFailures;

    public void SetRemainingFailures(int failures)
    {
        txtRemainFailures.text = (3 - failures).ToString();
    }
}
