using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JSY_LoadingSystem : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private float currentTime;
    private void Start()
    {
        StartCoroutine(LoadSceneAsyncCoroutine());
    }

    private IEnumerator LoadSceneAsyncCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            currentTime += Time.deltaTime;
            if(currentTime > 2) operation.allowSceneActivation = true;
            yield return null;
        }
    }
}
