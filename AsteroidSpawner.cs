using UnityEngine;
using System.Collections;
using System.Linq;

public class AsteroidSpawner : MonoBehaviour
{
    private static AsteroidSpawner singleton;

    [SerializeField]
    private GameObject[] spawners;

    [SerializeField]
    private GameObject[] asteroids;

    [SerializeField]
    private float[] asteroidSpawnDelayMinimum = new float[] { 0, 0.45F, 0.45F, 0.45F, 0.42F, 0.39F, 0.36F, .3F, .27F, .24F, .21F };

    [SerializeField]
    private float[] asteroidSpawnDelayMaximum = new float[] { 0, 0.5F, 0.47F, 0.44F, 0.41F, 0.38F, 0.35F, 0.32F, .29F, .26F, .23F };

    [SerializeField]
    private bool[] markedSpawners;

    /// <summary> Empty gameobjects that serve as positions for asteroids to spawn in. </summary>
    public GameObject[] Spawners { get => spawners; private set => spawners = value; }

    /// <summary> The asteroid prefabs. </summary>
    public GameObject[] Asteroids { get => asteroids; private set => asteroids = value; }
    
    /// <summary> The delay between spawning waves of asteroids. </summary>
    public float[] AsteroidSpawnDelayMinimum { get => asteroidSpawnDelayMinimum; private set => asteroidSpawnDelayMinimum = value; }

    public float[] AsteroidSpawnDelayMaximum { get => asteroidSpawnDelayMaximum; private set => asteroidSpawnDelayMaximum = value; }

    public static AsteroidSpawner Singleton
    {
        get
        {
            if(singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Asteroid Spawner").GetComponent<AsteroidSpawner>();
            }
            return singleton;
        }
    }

    public bool[] MarkedSpawners { get { if (markedSpawners.Length < spawners.Length) { markedSpawners = new bool[spawners.Length]; } return markedSpawners; } }

    private void Awake()
    {
        for (int i = 0; i < Spawners.Length; i++)
        {
            if (Spawners[i] == null)
            {
                Spawners[i] = transform.GetChild(i).gameObject;
            }
        }
    }

    public IEnumerator SpawnAsteroids ()
    {
        while(AsteroidManager.Singleton.AsteroidsLeftToSpawn > 0)
        {
            int spawnerToUse = SpawnerToUse();

            Instantiate(Asteroids[AsteroidToSpawn()], spawners[spawnerToUse].transform.position, spawners[spawnerToUse].transform.rotation);

            AsteroidManager.Singleton.DecrementAsteroidsLeftToSpawn();

            MarkSpawner(spawnerToUse);

            if(MarkedSpawners.All(x => x))
            {
                UnmarkAllSpawners();
            }

            yield return new WaitForSeconds(SpawnDelay());
        }
    }

    private void UnmarkAllSpawners ()
    {
        for (int i = 0; i < MarkedSpawners.Length; i++)
        {
            MarkedSpawners[i] = false;
        }
    }

    private void MarkSpawner (int spawner)
    {
        MarkedSpawners[spawner] = true;
    }

    private float SpawnDelay ()
    {
        return Random.Range(AsteroidSpawnDelayMinimum[GameManager.Singleton.CurrentLevel], AsteroidSpawnDelayMaximum[GameManager.Singleton.CurrentLevel]);
    }

    private int AsteroidToSpawn ()
    {
        return Random.Range(0, Asteroids.Length);
    }

    private int SpawnerToUse ()
    {
        int spawner = Random.Range(0, Spawners.Length);

        if(!MarkedSpawners[spawner])
        {
            return spawner;
        }
        else
        {
            return SpawnerToUse();
        }
    }
}