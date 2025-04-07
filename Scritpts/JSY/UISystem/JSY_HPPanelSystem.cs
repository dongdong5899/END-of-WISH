using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class JSY_HPPanelSystem : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private Transform hpBar_Empty;
    
    private TextMeshProUGUI hpPercentText;

    [Header("Hp Value Sprite Settings")]
    [SerializeField] private Image whiteHpImage;
    [SerializeField] private Image blueHpImage;

    [Header("Hp Value Settings")]
    [SerializeField] private int maxHp = 5000;
    [SerializeField] private int hpValueLevel = 10;
    [SerializeField] private int hpPercent = 100;
    private bool isDie = false;
    private int hp;

    public event Action OnDieEvent;
    public int Hp
    {
        get => Mathf.Clamp(hp, 0, maxHp);
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
            if (hp <= 0)
            {
                PlayerDie();
            }
        }
    }

    private void Awake()
    {
        Initialize();
        hp = maxHp;
    }

    private void Initialize()
    {
        hpPercentText = hpBar_Empty.Find("HpText").GetComponent<TextMeshProUGUI>();
    }

    private void PlayerDie()
    {
        Debug.Log("½Ã¿Â±ú²¿´Ú");
        isDie = true;
        OnDieEvent?.Invoke();
    }

    public void HpSetting(int value)
    {
        if (isDie) return;

        Hp = value;
        
        HpImageSetting((float)Hp / maxHp * 100);
        StartCoroutine(CountDownHpText((int)((float)Hp / maxHp * 100)));
    }

    private IEnumerator CountDownHpText(int newValue)
    {
        WaitForSeconds ws = new WaitForSeconds(0.1f);
        while(hpPercent != newValue)
        {
            hpPercent += hpPercent > newValue ? -1 : 1;
            hpPercentText.text = hpPercent + "%";
            yield return ws;
        }
        yield return null;
    }

    private void HpImageSetting(float hp)
    {
        float ming = -54.7f * (10 - (hp * 0.1f));
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            blueHpImage.rectTransform.anchoredPosition =
            new Vector2(ming, 0);
        });
        seq.AppendInterval(0.7321f);
        seq.OnComplete(() => { whiteHpImage.rectTransform.DOAnchorPosX(ming, 0.5f); });
        
        
    }
}
