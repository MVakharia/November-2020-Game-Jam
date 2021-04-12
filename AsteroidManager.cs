using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    private static AsteroidManager singleton;
    public static AsteroidManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<AsteroidManager>();
            }
            return singleton;
        }
    }

    #region Fields
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
    #endregion

    #region Properties
    public int AsteroidsLeftToSpawn => asteroidsLeftToSpawn;
    public int AsteroidsDestroyedThisRound => asteroidsDestroyedThisRound;
    public int AsteroidsOnFirstRound => asteroidsOnFirstRound;
    public int AsteroidsThisRound => asteroidsThisRound;
    public int ExtraAsteroidsPerRound => extraAsteroidsPerRound;
    public int NumberOfAsteroidsThisRound() => asteroidsPerLevel[GameManager.Singleton.CurrentLevel];
    public bool NoAsteroidsLeftToSpawn => AsteroidsDestroyedThisRound + asteroidsLeftToSpawn == AsteroidsThisRound;
    public bool AllAsteroidsDestroyed => AsteroidsDestroyedThisRound >= AsteroidsThisRound && NoAsteroidsLeftToSpawn;
    #endregion

    #region Methods
    public void SetAsteroidsLeftToSpawn() => asteroidsLeftToSpawn = asteroidsPerLevel[GameManager.Singleton.CurrentLevel];
    public void DecrementAsteroidsLeftToSpawn() => asteroidsLeftToSpawn--;
    public void SetNumberOfAsteroidsThisRound() => asteroidsThisRound = NumberOfAsteroidsThisRound();
    public void SetNumberOfAsteroidsLeftToSpawn() => asteroidsLeftToSpawn = NumberOfAsteroidsThisRound();
    public void IncrementAsteroidsDestroyed() => asteroidsDestroyedThisRound += 1;
    public void ResetAsteroidsDestroyed() => asteroidsDestroyedThisRound = 0;
    public void SetAsteroidsPerLevel()
    {
        asteroidsPerLevel = new int[]
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
    #endregion
}