using System.Collections;
using TMPro;
using UnityEngine;

public class DoorLockRayCheck : MonoBehaviour
{
    [SerializeField] private float _layDistance = 1.2f;
    [SerializeField] private TMP_InputField _inputField;
    private SoundPuzzle _soundPuzzle;

    private void Awake()
    {
        _soundPuzzle = FindAnyObjectByType<SoundPuzzle>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                DoorLock doorlock = hit.collider.GetComponent<DoorLock>();
                if (doorlock != null)
                {
                    _inputField.gameObject.SetActive(true); // UI매니저로 옳기기
                    GameManager.Instance.SetCursorActive(true);
                    Debug.Log(doorlock);
                }
                else
                    return;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)) // UI매니저로 옳기기
        {
            _inputField.gameObject.SetActive(false);
            GameManager.Instance.SetCursorActive(false);
        }
    }
}
