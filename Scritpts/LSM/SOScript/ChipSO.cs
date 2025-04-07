using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct TransformSetting
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

[System.Serializable]
public class SkillData
{
    public bool isEventSkill;
    public UnityEvent skillEvent;

    public Sprite sprite;
    public int damage;
    public Vector3 cameraShakeValue;
    public AnimationCurve shakeAnimationCurve;
    public Vector3 actionMove;
    public string skillEffectPrefab;
    public TransformSetting prefabTrmSetting;
    public float coolTime;
    public float lastUseTime;

    public float CurrentCoolTime => Mathf.Clamp(lastUseTime + coolTime - Time.time, 0, coolTime);
    public bool CanUseSkill => CurrentCoolTime == 0;
}

[CreateAssetMenu(menuName = "SO/ChipSO")]

public class ChipSO : ScriptableObject
{
    [Header("DefaultAttackSetting")]
    public int defaultAttackAnimationIndex;
    public Sprite sprite;
    public int[] damage;
    public Vector3[] cameraShakeValue;
    public Vector3[] actionMove;
    public string[] defaultAttackFrefabName;
    public TransformSetting[] prefabTrmSetting;

    [Header("SkillSetting")]
    public int skillAnimationIndex;
    public SkillSO qSkill;
    public SkillSO eSkill;

    public void Initialize()
    {
        if (qSkill != null)
            qSkill.lastUseTime = int.MinValue;
        if (eSkill != null)
            eSkill.lastUseTime = int.MinValue;
    }
}
