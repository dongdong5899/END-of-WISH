using DG.Tweening;
using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private int playerFloor = 0;

    [HideInInspector] public PlayerBrain player;
    [HideInInspector] public MapMaker map;
    [HideInInspector] public int currentChipIndex;
    [HideInInspector] public ChipSO currentChip;
    [HideInInspector] public bool isDie;

    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private CanvasGroup dieUICanvasGroup;
    [SerializeField] private GameObject pauseMenu;

    public Sprite nullSprite;
    public ChipSO defaultChipSO;

    public bool CanPlayerMouseControl = true;
    public bool isPaused = false;

    //임시 점수
    [Space(20)]
    public ScoreSO scoreSO;
    public void AddScore()
    {
        scoreSO.Score++;
        UIManager.Instance.ScoreUpdate();
    }

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerBrain>();
        map = FindAnyObjectByType<MapMaker>();

    }

    private void Start()
    {
        currentChip = defaultChipSO;
        scoreSO.Score = 0;
        UIManager.Instance.ScoreUpdate();
    }

    private void Update()
    {
        if (isDie) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.IsOnInventory())
            {
                UIManager.Instance.ActiveInventory(false);
            }
            else
            {
                Pause(!isPaused);
            }
        }
    }


    public void Pause(bool active)
    {
        isPaused = active;
        SetCursorActive(active);
        pauseMenu.SetActive(active);
        SetTimeScale(active ? 0 : 1);
    }

    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    public void SetFloor(int newFloor)
    {
        playerFloor = newFloor;

        for (int i = 0; i < map.transform.childCount; i++)
        {
            map.transform.GetChild(i).gameObject.SetActive(false);
        }
        map.transform.GetChild(newFloor - 1).gameObject.SetActive(true);
    }

    public Coroutine DelayCallback<T>(T delay, Action callback)
    {
        return StartCoroutine(DelayCoroutine(delay, callback));
    }
    public Coroutine DelayFrameCallback(int delayCount, Action callback)
    {
        return StartCoroutine(DelayFrameCoroutine(delayCount, callback));
    }

    public void StopDelayCallback(Coroutine coroutine)
    {
        if (coroutine == null) return;
        StopCoroutine(coroutine);
    }

    private IEnumerator DelayCoroutine<T>(T delay, Action action)
    {
        yield return delay;
        action?.Invoke();
    }
    private IEnumerator DelayFrameCoroutine(int delayCount, Action action)
    {
        for (int i = 0; i < delayCount; ++i)
        {
            yield return null;
        }
        action?.Invoke();
    }

    public void SetCursorActive(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;

        CanPlayerMouseControl = !value;
    }

    public void NavBake()
    {
        navMeshSurface.BuildNavMesh();
    }

    public void PlayerDie()
    {
        isDie = true;
        float diePanelOpenTime = 1;

        dieUICanvasGroup.DOFade(1, diePanelOpenTime);
        SetCursorActive(true);
    }
}
