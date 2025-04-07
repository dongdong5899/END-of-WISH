using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleUI : MonoBehaviour
{
    private VisualElement root;

    private Button gameStartButton;
    private Button optionButton;
    private Button exitButton;
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        gameStartButton = root.Q<Button>("GameStartBtn");
        exitButton = root.Q<Button>("ExitBtn");

        gameStartButton.clicked += HandleStartButton;
        exitButton.clicked += HandleExitButton;
    }

    private void HandleStartButton()
    {
        SceneLoadManager.Instance.SceneChange(SceneName.LoadingScene);
    }

    private void HandleOptionButton()
    {
        // do nothing
    }

    private void HandleExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
