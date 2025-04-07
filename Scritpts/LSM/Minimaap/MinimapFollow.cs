using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{

    PlayerBrain player;

    private void Start()
    {
        player = GameManager.Instance.player;
    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 
            transform.position.y, player.transform.position.z);
        transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0);

        
    }

}
