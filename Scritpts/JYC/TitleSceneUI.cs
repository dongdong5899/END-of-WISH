using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneUI : MonoBehaviour
{
    public void GameStart()
    {
        //SceneManager.LoadScene("���?");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
