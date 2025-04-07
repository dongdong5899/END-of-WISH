using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomEnemySpawnEvent : MonoBehaviour, IRoomEvent
{
    [SerializeField] private List<EnemySpawner> enemySpawners;
    private bool isEnd = false;

    private void Awake()
    {
        if (enemySpawners.Count > 0)
        {
            enemySpawners.ForEach(x => x.Initialize());
        }
    }

    public void StartRoomEvent()
    {
        enemySpawners.ForEach(x => x.SpawnEnemy());
    }

    public bool EndRoomEvent()
    {
        if (isEnd == true) return true;
        isEnd = enemySpawners.All(x => x.IsAllDead());
        return isEnd;
    }
}
