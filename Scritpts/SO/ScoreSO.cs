using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TestScoreSO")]
public class ScoreSO : ScriptableObject
{
    [SerializeField] private int score = 0;
    public int maxScore = 0;

    public int Score 
    { 
        get
        {
            return score;
        }
        set
        {
            score = value;
            if (score > maxScore)
                maxScore = score;
        }
    }
}
