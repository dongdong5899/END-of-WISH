using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="SO/ChipDataSO")]
public class ChipDataSO : ScriptableObject
{
    public string chipName;
    public Sprite chipSprite;

    public Sprite skill_1Sprite;
    public Sprite skill_2Sprite;

    public float skill_1CoolTime;
    public float skill_2CoolTime;

    public int[] ASkillDamage;
    public int[] BSkillDamage;
    public int[] QSkillDamage;

    public void skill_1()
    {

    }

    public void Skill_2()
    {

    }
}
