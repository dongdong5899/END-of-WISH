using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomBasicEvent : MonoBehaviour, IRoomEvent
{
    [SerializeField] private List<EnemySpawner> enemySpawners;
    [SerializeField] private JSY_PortalHpSystem portalHp;
    private bool isEnd = false;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (enemySpawners.Count > 0)
        {
            enemySpawners.ForEach(x => x.Initialize());
        }

        portalHp.OnDieEvent += () =>
            {
                if (spawnCoroutine != null)
                    StopCoroutine(spawnCoroutine);
            };
    }
    public void StartRoomEvent()
    {
        enemySpawners.ForEach(x => x.SpawnEnemy());

        spawnCoroutine = StartCoroutine(EnemySpawnCoolTime());
    }

    private IEnumerator EnemySpawnCoolTime()
    {
        while (portalHp.Hp > 0 && enemySpawners.Count != 0)
        {
            int randomCoolTime = Random.Range(4, 7);
            int randomEnemySpawner = Random.Range(0, enemySpawners.Count);
            yield return new WaitForSeconds(randomCoolTime);
            enemySpawners[randomEnemySpawner].CreateEnemy();
        }
    }

    public bool EndRoomEvent()
    {
        if (isEnd == true) return true;
        isEnd = enemySpawners.All(x => x.IsAllDead()) && portalHp.Hp <= 0;
        return isEnd;
    }
}
