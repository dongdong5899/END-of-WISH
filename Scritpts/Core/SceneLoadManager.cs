using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    LobbyScene,
    LoadingScene,
    MergeScene,
    ClearScene
}

public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    public void SceneChange(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
    public void SceneChange(int sceneName)
    {
        SceneManager.LoadScene(((SceneName)sceneName).ToString());
    }
    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
