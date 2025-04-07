using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private JSY_HPPanelSystem hpPanelSystem;
    private JSY_SettingPanelSystem settingPanelSystem;
    private JSY_SkillUI skillUI;
    [SerializeField] private JSY_InventorySystem inventorySystem;
    //임시 점수
    private KillScoreUI killScoreUI;

    private void Awake()
    {
        hpPanelSystem = GameObject.FindObjectOfType<JSY_HPPanelSystem>();
        settingPanelSystem = GameObject.FindObjectOfType<JSY_SettingPanelSystem>();
        skillUI = GameObject.FindObjectOfType<JSY_SkillUI>();
        killScoreUI = GameObject.FindObjectOfType<KillScoreUI>();
    }

    private void Start()
    {
        ScoreUpdate();
    }

    public void AddItem(JSY_ChipItem item)
    {
        inventorySystem.AddItem(item);
    }

    public void ActiveInventory(bool value)
    {
        settingPanelSystem.ActiveInventory(value);
    }
    public bool IsOnInventory()
    {
        return settingPanelSystem.isSettingPanel;
    }

    public void UseQSkill(ChipSO chipSO)
    {
        skillUI.UseQSKill(chipSO);
    }
    public void UseESkill(ChipSO chipSO)
    {
        skillUI.UseESKill(chipSO);
    }

    public void HpUIUpDate(int hp)
    {
        hpPanelSystem.HpSetting(hp);
    }

    public void ScoreUpdate()
    {
        killScoreUI.ScoreUpdate();
    }
}
