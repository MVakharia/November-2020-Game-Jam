using UnityEngine;
using System.Collections;
using System.Linq;

public class AsteroidSpawner : MonoBehaviour
{
    private static AsteroidSpawner singleton;
    public static AsteroidSpawner Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Asteroid Spawner").GetComponent<AsteroidSpawner>();
            }
            return singleton;
        }
    }

    #region Fields
    private readonly float[] asteroidSpawnDelayMinimum = new float[] { 0, 0.45F, 0.45F, 0.45F, 0.42F, 0.39F, 0.36F, .3F, .27F, .24F, .21F };
    private readonly float[] asteroidSpawnDelayMaximum = new float[] { 0, 0.5F, 0.47F, 0.44F, 0.41F, 0.38F, 0.35F, 0.32F, .29F, .26F, .23F };
    [SerializeField]
    private bool[] _marked_Spawners;
    [SerializeField]
    private GameObject[] spawners;
    [SerializeField]
    private GameObject[] asteroids;
    #endregion

    #region Properties
    private bool[] MarkedSpawners
    { 
        get 
        { 
            if (_marked_Spawners.Length < spawners.Length) 
            { 
                _marked_Spawners = new bool[spawners.Length]; 
            } 
            return _marked_Spawners; 
        } 
    }

    private int CurrentLevel => GameManager.Singleton.CurrentLevel;
    private float SpawnDelay() => Random.Range(asteroidSpawnDelayMinimum[CurrentLevel], asteroidSpawnDelayMaximum[CurrentLevel]);
    private int AsteroidToSpawn() => Random.Range(0, asteroids.Length);
    private int SpawnerToUse
    {
        get
        {
            int spawner = Random.Range(0, spawners.Length);

            if (!MarkedSpawners[spawner])
            {
                return spawner;
            }
            else
            {
                return SpawnerToUse;
            }
        }
    }
    #endregion

    #region Methods
    private void MarkSpawner(int spawner) => MarkedSpawners[spawner] = true;
    private void UnmarkAllSpawners() { for (int i = 0; i < MarkedSpawners.Length; i++) { MarkedSpawners[i] = false; } }
    public IEnumerator SpawnAsteroids()
    {
        while (AsteroidManager.Singleton.AsteroidsLeftToSpawn > 0)
        {
            int spawnerToUse = SpawnerToUse;

            Instantiate(asteroids[AsteroidToSpawn()], spawners[spawnerToUse].transform.position, spawners[spawnerToUse].transform.rotation);

            AsteroidManager.Singleton.DecrementAsteroidsLeftToSpawn();

            MarkSpawner(spawnerToUse);

            if (MarkedSpawners.All(x => x))
            {
                UnmarkAllSpawners();
            }

            yield return new WaitForSeconds(SpawnDelay());
        }
    }
    #endregion

    private void Awake()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i] == null)
            {
                spawners[i] = transform.GetChild(i).gameObject;
            }
        }
    }
}