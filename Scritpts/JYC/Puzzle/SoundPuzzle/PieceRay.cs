using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceRay : MonoBehaviour
{
    [SerializeField] private float _layDistance = 1.2f;
    [SerializeField] private LayerMask _checkLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Piece"))
                {
                    PuzzlePiece puzzlePiece = hit.collider.GetComponent<PuzzlePiece>();
                    AudioSource audio = Instantiate(puzzlePiece.metryNumberSO.metryAudio);
                    audio.Play();
                    StartCoroutine(AudioDelete());

                    IEnumerator AudioDelete()
                    {   
                        yield return new WaitForSeconds(audio.clip.length);
                        Destroy(audio.gameObject);
                    }
                }
            }
        }
    }
}
