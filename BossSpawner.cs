using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawner;
    [SerializeField]
    private GameObject[] bosses = new GameObject[5];

    [SerializeField]
    private bool[] bossHasBeenSpawned = new bool[5];

    public GameObject Spawner { get => spawner; private set => spawner = value; }
    public GameObject[] Bosses { get => bosses; private set => bosses = value; }
    public bool[] BossHasBeenSpawned { get => bossHasBeenSpawned; private set => bossHasBeenSpawned = value; }

    private void Start()
    {

    }

    private void Update()
    {
        if(GameManager.Singleton.CurrentPhase == GamePhase.Boss && !BossHasBeenSpawned[BossToSpawn()])
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss ()
    {
        Instantiate(Bosses[BossToSpawn()], Spawner.transform.position, Spawner.transform.rotation);
        CheckBossAsSpawned();
    }

    private void CheckBossAsSpawned ()
    {
        BossHasBeenSpawned[BossToSpawn()] = true;
    }

    private int CurrentLevel ()
    {
        return GameManager.Singleton.CurrentLevel;
    }

    private int BossToSpawn ()
    {
        return (CurrentLevel() / 2) - 1;
    }
}