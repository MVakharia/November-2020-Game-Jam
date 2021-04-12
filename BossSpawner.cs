using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject spawner;
    [SerializeField]
    private GameObject[] bosses = new GameObject[5];
    [SerializeField]
    private bool[] bossHasBeenSpawned = new bool[5];
    #endregion

    #region Properties
    private bool IsInBossPhase => GameManager.Singleton.CurrentPhase == GamePhase.Boss;
    private int CurrentLevel() => GameManager.Singleton.CurrentLevel;
    private int BossToSpawn() => (CurrentLevel() / 2) - 1;
    #endregion

    #region Methods
    private void CheckBossAsSpawned() => bossHasBeenSpawned[BossToSpawn()] = true;
    private void SpawnBoss()
    {
        Instantiate(bosses[BossToSpawn()], spawner.transform.position, spawner.transform.rotation);
        CheckBossAsSpawned();
    }
    #endregion

    private void Update()
    {
        if(IsInBossPhase && !bossHasBeenSpawned[BossToSpawn()])
        {
            SpawnBoss();
        }
    }
}