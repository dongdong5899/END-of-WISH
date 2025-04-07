using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillSO")]
public class SkillSO : ScriptableObject
{
    public Sprite skillSprite;
    private Skill _skill;
    public Skill Skill
    {
        get
        {
            if (_skill == null)
            {
                Type type = Type.GetType($"{skillName}Skill");
                Skill skill = Activator.CreateInstance(type) as Skill;
                _skill = skill;
                if (_skill == null)
                {
                    Debug.LogError("스킬 이름 잘못썼어 개 멍청이 허~접아♥");
                }
            }

            return _skill;
        }
    }
    public string skillName;

    public float coolTime;
    [HideInInspector] public float lastUseTime;

    public float CurrentCoolTime => Mathf.Clamp(lastUseTime + coolTime - Time.time, 0, coolTime);
    public bool CanUseSkill => CurrentCoolTime == 0;
}
