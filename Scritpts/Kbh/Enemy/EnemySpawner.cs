using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool isAlreadySpawn = false;

    public EnemyType spawnEnemyType = EnemyType.Punch;
    private string enemyTypeStr = string.Empty;
    private int enemyCnt;
    public int spawnCnt;
    public float spawnRadius = 3f;
    public Vector2 spawnDelayMinMax;

    private int deadEnemyCnt = 0;

    public void Initialize()
    {
        enemyTypeStr = spawnEnemyType.ToString();
    }

    [ContextMenu("SpawnEnemy")]
    public void SpawnEnemy()
    {
        deadEnemyCnt = 0;
        if (isAlreadySpawn) return;
        isAlreadySpawn = true;
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        for (int i = 0; i < spawnCnt; ++i)
        {
            CreateEnemy();

            yield return new WaitForSeconds
               (Random.Range(spawnDelayMinMax.x, spawnDelayMinMax.y));
        }
    }

    public void CreateEnemy()
    {
        if (enemyCnt - deadEnemyCnt > 15) return;
        enemyCnt++;
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        randomPos.y = 0;

        EnemyBrain brain = PoolManager.Instance.Pop($"{enemyTypeStr}EnemyBrain")
           as EnemyBrain;

        brain.transform.position
           = transform.position + randomPos;
        brain.Initialize();
        brain.SetSpawner(this);
    }

    public void EnemyDie()
    {
        deadEnemyCnt++;
    }

    public bool IsAllDead()
        => deadEnemyCnt == enemyCnt;


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawPosition();
    }
    private void DrawPosition()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.DrawWireCube(transform.position, new Vector3(0.1f, 0.1f, 2f));
        Gizmos.DrawWireCube(transform.position, new Vector3(0.1f, 2f, 0.1f));
        Gizmos.DrawWireCube(transform.position, new Vector3(2f, 0.1f, 0.1f));
        Gizmos.DrawWireCube(transform.position, new Vector3(2f, 2f, 2f));
        Gizmos.color = Color.white;
    }
#endif

}
