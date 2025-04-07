using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Practice_LSM : MonoBehaviour
{

    public TextMeshProUGUI text;
    public string input;

    private void Start()
    {

        //Debug.Log();

        Debug.Log(text.rectTransform.sizeDelta);
        Debug.Log(text.preferredWidth);
        Debug.Log(text.preferredHeight);
    }

}
