using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaxScoreUI : MonoBehaviour
{
    [SerializeField] private ScoreSO scoreSO;

    private TextMeshProUGUI maxScoreText;

    private void Awake()
    {
        maxScoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        maxScoreText.text = $"MAX SCORE: {scoreSO.maxScore}";
    }
}
