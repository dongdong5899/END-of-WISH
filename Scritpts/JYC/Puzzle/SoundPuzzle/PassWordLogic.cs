using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassWordLogic : MonoBehaviour
{
    private int intPassword;
    [SerializeField] private MetryNumberSO[] _metryNum;

    private string _password;
    private int _passwordCnt = 4;

    [HideInInspector] public bool isclear = false;

    [HideInInspector] public List<MetryNumberSO> selecteMetryNumbers;

    public string GetPassWord()
    {
        return _password;
    }
}
