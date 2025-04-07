using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChattingUI : MonoBehaviour
{

    public Image chatWindow;
    public TextMeshProUGUI printText_1;
    public TextMeshProUGUI printText_2;
    private bool isChatting = false;
    public string input;
    public float chatSpawnTime;
    Sequence mySeq;

    private void Awake()
    {
        //mySeq =  DOTween.Sequence();
        //Debug.Log(input.Length);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isChatting ==false)
        {
            StartCoroutine(OnText(input, chatSpawnTime));
        }
    }

    public IEnumerator OnText(string text,float textMaintain)
    {
        
        for(int i=0;i<text.Length;++i)
        {
            if (printText_1.preferredWidth<720)
                printText_1.text += text[i];
            else
                printText_2.text += text[i];
        }

        Vector2 textSize1 = new Vector3(printText_1.preferredWidth, printText_1.preferredHeight);
        Vector2 textSize2 = new Vector3(printText_2.preferredWidth, printText_1.preferredHeight);
        printText_1.text = null;
        printText_2.text = null;

        isChatting = true;

        yield return new WaitUntil(() => isChatting);
        chatWindow.rectTransform.DOAnchorPosY(0, textMaintain);

        yield return new WaitForSeconds(1f);
        printText_1.rectTransform.sizeDelta = textSize1;
        printText_2.rectTransform.sizeDelta = textSize2;
        for (int i = 0; i < text.Length; ++i)
        {
            if ( printText_1.preferredWidth < 720)
            {
                yield return new WaitForSeconds(0.05f);
                printText_1.text += text[i];
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
                printText_2.text += text[i];
            }
            
        }

        yield return new WaitForSeconds(3f);
        chatWindow.rectTransform.DOAnchorPosY(-chatWindow.rectTransform.sizeDelta.y, textMaintain);
        printText_1.text = null;
        printText_2.text = null;

        yield return new WaitForSeconds(1f);
        isChatting = false;


    }



}
