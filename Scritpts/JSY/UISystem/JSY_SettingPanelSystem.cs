using UnityEngine;
using UnityEngine.UI;

public class JSY_SettingPanelSystem : MonoBehaviour
{
    //[SerializeField] private Transform settingPanel;
    [SerializeField] private CanvasGroup settingPanel;
    private GameObject swordSettingPanel;
    private GameObject passiveSettingPanel;
    private Button swordBtn;
    private Button passiveBtn;

    [HideInInspector] public bool isSettingPanel = false;
    void Awake()
    {
        Initialize();

        swordBtn.onClick.AddListener(() => OpenSettingPanel(swordSettingPanel));
        passiveBtn.onClick.AddListener(() => OpenSettingPanel(passiveSettingPanel));
    }

    private void Initialize()
    {
        swordSettingPanel = settingPanel.transform.Find("SwordSettingPanel").gameObject;
        passiveSettingPanel = settingPanel.transform.Find("PassiveSettingPanel").gameObject;
        swordBtn = settingPanel.transform.Find("SwordSettingBtn").GetComponent<Button>();
        passiveBtn = settingPanel.transform.Find("PassiveSettingBtn").GetComponent<Button>();
    }

    private void Update()
    {
        if (GameManager.Instance.isDie) return;
        if (GameManager.Instance.isPaused) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ActiveInventory(!isSettingPanel);
        }
    }

    public void ActiveInventory(bool value)
    {
        isSettingPanel = value;
        GameManager.Instance.SetCursorActive(value);
        //settingPanel.gameObject.SetActive(value);
        settingPanel.alpha = value ? 1:0;
        settingPanel.interactable = value;
        settingPanel.blocksRaycasts = value;
    }

    private void OpenSettingPanel(GameObject panel)
    {
        passiveSettingPanel.SetActive(false);
        swordSettingPanel.SetActive(false);
        panel.SetActive(true);
    }
}
