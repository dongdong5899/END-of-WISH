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
            //����� Ʋ������ ó��
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Ʋ��");
                _passWordInputField.text = null;
            }
        }
    }

    public void PassWordSet()
    {
        List<MetryNumberSO> metryList = new List<MetryNumberSO>(_metryNum);

        for (int i = 0; i< metryList.Count; i++) // ��ȣ��������
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
            Debug.Log("������ȯ: " + intPassword);
        }
        else
        {
            Debug.Log("��ȯ�ȵ�");
        }

        Debug.Log("�������� ���¹�ȣ : " + _password);
    }

    public bool PuzzleClear()
    {
        Debug.Log("Ŭ���� �Ͽ���");
        return true;
    }
}
