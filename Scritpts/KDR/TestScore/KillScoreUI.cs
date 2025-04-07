using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillScoreUI : MonoBehaviour
{
    private TextMeshProUGUI killCountText;

    private void Awake()
    {
        killCountText = GetComponent<TextMeshProUGUI>();
    }
     
    public void ScoreUpdate()
    {
        killCountText.text = $"KILLCOUNT: {GameManager.Instance.scoreSO.Score}";
    }
}
