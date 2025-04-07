using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JSY_SkillUI : MonoBehaviour
{
    [SerializeField] private Transform uiCanvas;

    private Transform Q_SkillSet;
    private Transform E_SkillSet;
    private Transform ChipSet;

    private Image Q_SkillImage;
    private Image E_SkillImage;

    private Image Q_CooltimeImage;
    private Image E_CooltimeImage;

    private Image[] Chip_Icon = new Image[4];

    private TextMeshProUGUI Q_CooltimeText;
    private TextMeshProUGUI E_CooltimeText;

    private ChipSO[] datas = new ChipSO[4];
    private ChipSO currentChipSO;
    private bool isSkill;

    private JSY_SettingPanelSystem panelSystem;

    private int currentIndex = 0;

    private void Awake()
    {
        CanvasSetting();
    }

    private void CanvasSetting()
    {
        panelSystem = GetComponent<JSY_SettingPanelSystem>();

        Q_SkillSet = uiCanvas.Find("QSkillSet").transform;
        E_SkillSet = uiCanvas.Find("ESkillSet").transform;
        ChipSet = uiCanvas.Find("Chips").transform;

        Q_SkillImage = Q_SkillSet.Find("QSkillImage").GetComponent<Image>();
        E_SkillImage = E_SkillSet.Find("ESkillImage").GetComponent<Image>();

        Q_CooltimeImage = Q_SkillSet.Find("QCooltimeImage").GetComponent<Image>();
        E_CooltimeImage = E_SkillSet.Find("ECooltimeImage").GetComponent<Image>();

        for (int i = 0; i < 4; ++i)
        {
            Transform chipSlot = ChipSet.transform.GetChild(i);
            Chip_Icon[i] = chipSlot.Find("Image").GetComponent<Image>();
        }

        Q_CooltimeText = Q_SkillSet.Find("QCooltimeText").GetComponent<TextMeshProUGUI>();
        E_CooltimeText = E_SkillSet.Find("ECooltimeText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (panelSystem.isSettingPanel) return;

        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown((KeyCode)(49 + i)))
            {
                currentIndex = i;
                currentChipSO = datas[currentIndex];
                ChipChange();
            }
        }

        /*if (datas[currentIndex] != null)
        {
            ChipSO skillData = datas[currentIndex];
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log(skillData.qSkill.coolTime);
                Debug.Log(skillData.qSkill.lastUseTime);
                Debug.Log(Time.time);
                Debug.Log(skillData.qSkill.CanUseSkill);
                if (skillData.qSkill.CanUseSkill)
                {
                }
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                if(skillData.eSkill.CanUseSkill)
                {
                    skillData.eSkill.lastUseTime = Time.time;
                    StartCoroutine(SkillCooldown(E_CooltimeText, false));
                    
                }
            }
        }*/
    }

    public void UseQSKill(ChipSO skillData)
    {
        skillData.qSkill.lastUseTime = Time.time;
        StartCoroutine(SkillCooldown(Q_CooltimeText, true));
    }
    public void UseESKill(ChipSO skillData)
    {
        skillData.eSkill.lastUseTime = Time.time;
        StartCoroutine(SkillCooldown(E_CooltimeText, false));
    }

    private void ChipChange()
    {
        SetSkillImage();
        SettingChipUI();
        if (currentChipSO != null)
        {
            if (currentChipSO.qSkill != null && currentChipSO.qSkill.lastUseTime != 0)
                StartCoroutine(SkillCooldown(Q_CooltimeText, true));
            if (currentChipSO.eSkill != null && currentChipSO.eSkill.lastUseTime != 0)
                StartCoroutine(SkillCooldown(E_CooltimeText, false));
        }
    }


    private void SettingChipUI()
    {
        Q_CooltimeImage.fillAmount = 0;
        E_CooltimeImage.fillAmount = 0;
        Q_CooltimeText.text = "";
        E_CooltimeText.text = "";
    }

    public void AddChipData(ChipSO chipData, int index)
    {
        datas[index] = chipData;
        ChipSet.GetChild(index).Find("Image").GetComponent<Image>().sprite =
            chipData == null ? GameManager.Instance.nullSprite : chipData.sprite;
        ChipChange();
    }
    private void SetSkillImage()
    {
        Debug.Log(currentChipSO);
        StopAllCoroutines();
        if (currentChipSO == null)
        {
            GameManager.Instance.currentChip = GameManager.Instance.defaultChipSO;
            Q_SkillImage.sprite = GameManager.Instance.nullSprite;
            E_SkillImage.sprite = GameManager.Instance.nullSprite;
        }
        else
        {
            GameManager.Instance.currentChip = currentChipSO;
            Q_SkillImage.sprite = currentChipSO.qSkill == null ? 
                GameManager.Instance.nullSprite : currentChipSO.qSkill.skillSprite;
            E_SkillImage.sprite = currentChipSO.eSkill == null ? 
                GameManager.Instance.nullSprite : currentChipSO.eSkill.skillSprite;
        }
    }
    private IEnumerator SkillCooldown(TextMeshProUGUI skillCooltimeText, bool isQSkill)
    {
        SkillSO skill = null;
        Image cooltimeImage;
        if (isQSkill)
        {
            skill = currentChipSO.qSkill;
            cooltimeImage = Q_CooltimeImage;
        }
        else
        {
            skill = currentChipSO.eSkill;
            cooltimeImage = E_CooltimeImage;
        }

        while (!skill.CanUseSkill)
        {
            if (skill.CurrentCoolTime > 1f) skillCooltimeText.text = Mathf.Ceil(skill.CurrentCoolTime).ToString();
            else skillCooltimeText.text = skill.CurrentCoolTime.ToString("F1");
            cooltimeImage.fillAmount = skill.CurrentCoolTime / skill.coolTime;
            yield return null;
        }
        yield return null;
        skillCooltimeText.text = "";
    }
}
