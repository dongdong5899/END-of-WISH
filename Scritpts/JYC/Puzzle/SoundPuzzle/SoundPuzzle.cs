using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundPuzzle : MonoBehaviour
{
    private int intPassword;
    [SerializeField] private MetryNumberSO[] _metryNum;
    [SerializeField] private TMP_InputField _passWordInputField;

    private string _password;
    private int _passwordCnt = 4;

    [HideInInspector] public bool isclear = false;

    [HideInInspector] public List<MetryNumberSO> selecteMetryNumbers;

    private void Start()
    {
        PassWordSet();
    }

    private void Update()
    {
        if (_passWordInputField.text == _password)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                PuzzleClear();
                _passWordInputField.text = null;
                _passWordInputField.gameObject.SetActive(false);
                GameManager.Instance.SetCursorActive(false);
            }
        }
        else
        {
            //비번이 틀렸을때 처리
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("틀림");
                _passWordInputField.text = null;
            }
        }
    }

    public void PassWordSet()
    {
        List<MetryNumberSO> metryList = new List<MetryNumberSO>(_metryNum);

        for (int i = 0; i< metryList.Count; i++) // 번호랜덤세팅
        {
            MetryNumberSO temp = metryList[i];
            int randomIndex = Random.Range(i, metryList.Count);
            metryList[i] = metryList[randomIndex];
            metryList[randomIndex] = temp;
        }

        selecteMetryNumbers = new List<MetryNumberSO>();

        string passwordCheck = "";
        for (int i = 0; i < _passwordCnt; i++)
        {
            selecteMetryNumbers.Add(metryList[i]);
            passwordCheck += metryList[i].metryNum.ToString();
            Debug.Log(metryList[i]);
        }

        _password = passwordCheck;

        if (int.TryParse(_password, out intPassword))
        {
            Debug.Log("정수변환: " + intPassword);
        }
        else
        {
            Debug.Log("변환안됨");
        }

        Debug.Log("랜덤으로 나온번호 : " + _password);
    }

    public bool PuzzleClear()
    {
        Debug.Log("클리어 하였음");
        return true;
    }
}
