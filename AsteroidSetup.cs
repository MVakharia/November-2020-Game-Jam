using UnityEngine;

public class AsteroidSetup : MonoBehaviour
{
    private static AsteroidSetup singleton;

    [SerializeField]
    private int[] asteroidsPerLevel = new int[] { };

    [SerializeField]
    private int asteroidsLeftToSpawn;

    [SerializeField]
    private int asteroidsDestroyedThisRound;

    [SerializeField]
    private int asteroidsOnFirstRound;

    [SerializeField]
    private int extraAsteroidsPerRound;

    [SerializeField]
    private int asteroidsThisRound;

    public static AsteroidSetup Singleton
    {
        get
        {
            if(singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<AsteroidSetup>();
            }
            return singleton;
        }
    }

    /// <summary> The number of asteroids that will appear in each level. </summary>
    public int[] AsteroidsPerLevel { get { return asteroidsPerLevel; } private set { asteroidsPerLevel = value; } }

    ///<summary> The number of asteroids left to spawn this round. </summary>
    public int AsteroidsLeftToSpawn { get { return asteroidsLeftToSpawn; } private set { asteroidsLeftToSpawn = value; } }

    /// <summary> The number of asteroids that have been destroyed this round. </summary>
    public int AsteroidsDestroyedThisRound { get { return asteroidsDestroyedThisRound; } private set { asteroidsDestroyedThisRound = value; } }

    /// <summary> The number of asteroids that will spawn int he first round. </summary>
    public int AsteroidsOnFirstRound { get { return asteroidsOnFirstRound; } private set { asteroidsOnFirstRound = value; } }

    /// <summary> The numebr of extra asteroids added per round. </summary>
    public int ExtraAsteroidsPerRound { get { return extraAsteroidsPerRound; } private set { extraAsteroidsPerRound = value; } }

    /// <summary> The total number of asteroids that will spawn this round. </summary>
    public int AsteroidsThisRound { get => asteroidsThisRound; private set => asteroidsThisRound = value; }

    /// <summary> Sets the initial number of 'remaining asteroids': the number of asteroids this level. </summary>
    public void SetAsteroidsLeftToSpawn()
    {
        asteroidsLeftToSpawn = asteroidsPerLevel[GameManager.Singleton.CurrentLevel];
    }

    public int NumberOfAsteroidsThisRound()
    {
        return AsteroidsPerLevel[GameManager.Singleton.CurrentLevel];
    }

    /// <summary> Sets the number of asteroids that will appear on each level. </summary>
    public void SetAsteroidsPerLevel()
    {
        AsteroidsPerLevel = new int[]
        {
            0,
            AsteroidsOnFirstRound,
            AsteroidsOnFirstRound + ExtraAsteroidsPerRound,
            AsteroidsOnFirstRound + ( 2 * ExtraAsteroidsPerRound),
            AsteroidsOnFirstRound + ( 3 * ExtraAsteroidsPerRound),
            AsteroidsOnFirstRound + ( 4 * ExtraAsteroidsPerRound),
            AsteroidsOnFirstRound + ( 5 * ExtraAsteroidsPerRound),
            AsteroidsOnFirstRound + ( 6 * ExtraAsteroidsPerRound),
            AsteroidsOnFirstRound + ( 7 * ExtraAsteroidsPerRound),
            AsteroidsOnFirstRound + ( 8 * ExtraAsteroidsPerRound),
            AsteroidsOnFirstRound + ( 9 * ExtraAsteroidsPerRound),
        };
    }

    public void DecrementAsteroidsLeftToSpawn()
    {
        AsteroidsLeftToSpawn--;
    }

    public bool AllAsteroidsDestroyed()
    {
        return AsteroidsDestroyedThisRound >= AsteroidsThisRound && (AsteroidsDestroyedThisRound + AsteroidsLeftToSpawn == AsteroidsThisRound);
    }

    public void SetNumberOfAsteroidsThisRound ()
    {
        AsteroidsThisRound = NumberOfAsteroidsThisRound();
    }

    public void SetNumberOfAsteroidsLeftToSpawn ()
    {
        AsteroidsLeftToSpawn = NumberOfAsteroidsThisRound();
    }

    public void IncrementAsteroidsDestroyed()
    {
        AsteroidsDestroyedThisRound += 1;
    }

    public void ResetAsteroidsDestroyed()
    {
        AsteroidsDestroyedThisRound = 0;
    }
}