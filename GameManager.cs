using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> The phases of the game. </summary>
public enum GamePhase
{
    StartScreen, Combat, Boss, Upgrade, Pause, Loss, End
}

public class GameManager : MonoBehaviour
{
    private static GameManager singleton;

    public static GameManager Singleton 
    {
        get 
        {
            if (singleton == null) 
            { 
                singleton = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>(); 
            }            
            return singleton; 
        } 
    }

    #region Fields
    [SerializeField]
    private GamePhase currentPhase;
    [SerializeField]
    private int currentLevel;
    [SerializeField]
    private TMP_Text startingText;
    [SerializeField]
    private TMP_Text levelProgressText;
    [SerializeField]
    private TMP_Text currentLevelText;
    [SerializeField]
    private TMP_Text currentPhaseText;
    [SerializeField]
    private GameObject startUI;
    [SerializeField]
    private GameObject combatUI;
    [SerializeField]
    private GameObject upgradeUI;
    [SerializeField]
    private GameObject lossUI;
    [SerializeField]
    private GameObject upgradeShopBackground;
    [SerializeField]
    private bool playerHasLostGame;
    [SerializeField]
    private bool playerMovedToUpgradePhase;
    [SerializeField]
    private GameObject shipDestroyedText;
    [SerializeField]
    private BossSpaceship bossShip;
    [SerializeField]
    private bool bossShipFound;
    [SerializeField]
    private int metal;
    [SerializeField]
    private AudioSource combatBackgroundAudioSource;
    [SerializeField]
    private AudioSource upgradeBackgroundAudioSource;
    [SerializeField]
    private AudioSource bossBackgroundAudioSource;
    [SerializeField]
    private TMP_Text startAndEndText;
    #endregion

    #region Properties
    public GamePhase CurrentPhase  => currentPhase;
    public int CurrentLevel => currentLevel;
    public int Metal => metal;
    #endregion

    private void Awake()
    {
        SetUpGame();
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                if(currentPhase == GamePhase.StartScreen)
                {
                    BeginGame();
                }

                if(currentPhase == GamePhase.Loss || currentPhase == GamePhase.End)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }

        if(currentPhase == GamePhase.Combat || currentPhase == GamePhase.Boss)
        {
            if(PlayerSpaceship.Singleton.HullHealth <= 0 && !playerHasLostGame)
            {
                playerHasLostGame = true;
                LoseGame();
            }
        }

        if(currentPhase == GamePhase.Combat)
        {
            if(AsteroidManager.Singleton.AllAsteroidsDestroyed)
            {
                EndCombatRound();
            }
        }

        if(currentPhase == GamePhase.Boss)
        {
            if(bossShip && !bossShipFound)
            {
                bossShipFound = true;
            }

            if(bossShipFound && bossShip.HullHealth <= 0)
            {
                Destroy(bossShip.gameObject);

                GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Boss Projectile");

                foreach(GameObject p in projectiles)
                {
                    Destroy(p);
                }
                EndBossRound();
            }
        }

        levelProgressText.text = SetLevelProgressText();

        currentPhaseText.text = SetCurrentPhaseText();

        currentLevelText.text = SetCurrentLevelText();

        if(currentPhase == GamePhase.Upgrade && !playerMovedToUpgradePhase )
        {
            playerMovedToUpgradePhase = true;
        }

        upgradeShopBackground.SetActive(currentPhase == GamePhase.Upgrade);

        SetAmbientAudio();
    }

    private void SetCurrentPhase (GamePhase phase)
    {
        currentPhase = phase;
        DisableEnableUI();
    }

    private void SetLevel(int level)
    {
        currentLevel = level;
    }

    private void SetUpGame ()
    {
        SetCurrentPhase(GamePhase.StartScreen);
        SetLevel(0);
        AsteroidManager.Singleton.SetAsteroidsPerLevel();
    }

    private void BeginGame ()
    {
        BeginALevel(1);
    }

    private void BeginALevel (int level)
    {
        SetLevel(level);

        BeginLevel();
    }

    public void BeginNextLevel ()
    {
        IncrementLevel();

        BeginLevel();
    }

    private void BeginLevel ()
    {
        SetCurrentPhase(GamePhase.Combat);
        if(PlayerSpaceship.Singleton.ShieldHealth > 0)
        {
            ReEnableShieldForEOR();
        }

        AsteroidManager.Singleton.ResetAsteroidsDestroyed();

        AsteroidManager.Singleton.SetNumberOfAsteroidsThisRound();

        AsteroidManager.Singleton.SetNumberOfAsteroidsLeftToSpawn();

        StartCoroutine(AsteroidSpawner.Singleton.SpawnAsteroids());
    }

    private void IncrementLevel ()
    {
        currentLevel++;
    }

    public void EndCombatRound ()
    {        
        if(currentLevel % 2 != 0)
        {
            SetCurrentPhase(GamePhase.Upgrade);
        }
        else
        {
            UnLocateBossShip();
            SetCurrentPhase(GamePhase.Boss);
        }
    }

    public void ReEnableShieldForEOR ()
    {
        if (PlayerSpaceship.Singleton.ShieldHealth > 0)
        {
            PlayerSpaceship.Singleton.ThawShield();
            PlayerSpaceship.Singleton.FinishRebootingShield();
            PlayerSpaceship.Singleton.ActivateShield();
        }
    }

    public void UnLocateBossShip ()
    {
        bossShipFound = false;
    }

    public void EndBossRound ()
    {
        if (currentLevel == 10)
        {
            EndGame();
        }
        else
        {
            SetCurrentPhase(GamePhase.Upgrade);
        }
    }

    public float LevelProgress ()
    {
        return (AsteroidManager.Singleton.AsteroidsDestroyedThisRound / (float)AsteroidManager.Singleton.AsteroidsThisRound) * 100;
    }

    public string SetLevelProgressText()
    {
        if (currentPhase == GamePhase.Combat)
        {
            return LevelProgress().ToString("0") + "%";
        }
        return levelProgressText.text = "";
    }

    public string SetCurrentPhaseText ()
    {
        switch(currentPhase)
        {
            case GamePhase.Combat: return "Survive the asteroid belt.";
            case GamePhase.Boss: return BossFightCompletionPercentage().ToString("0") + "% health remaining.";
            case GamePhase.Upgrade: return "";
            case GamePhase.Loss: return "You lose. Tap anywhere to restart.";
            default: return "";
        }
    }

    public float BossFightCompletionPercentage ()
    {
        if(bossShip)
        {
            int remainingUnits = bossShip.HullHealth + bossShip.ShieldHealth;

            int totalUnits = bossShip.HullMaximumHealth + bossShip.ShieldMaximumHealth;

            float percentage = (float)remainingUnits / totalUnits;

            return percentage * 100F;
        }
        return 0;
    }

    public string SetCurrentLevelText ()
    {
        if (currentPhase == GamePhase.Boss)
        {
            return "Boss";
        }

        return "Level " + currentLevel;
    }

    public void DisableEnableUI ()
    {
        startUI.SetActive(false);
        combatUI.SetActive(false);
        upgradeUI.SetActive(false);
        lossUI.SetActive(false);

        startUI.SetActive(currentPhase == GamePhase.StartScreen || currentPhase == GamePhase.End);
        combatUI.SetActive(currentPhase == GamePhase.Combat || currentPhase == GamePhase.Boss);
        upgradeUI.SetActive(currentPhase == GamePhase.Upgrade);
        lossUI.SetActive(currentPhase == GamePhase.Loss);
    }

    public void LoseGame ()
    {
        SetCurrentPhase(GamePhase.Loss);

        DisableEnableUI();
    }

    public void EndGame()
    {
        SetCurrentPhase(GamePhase.End);

        DisableEnableUI();

        SetEndGameText();
    }

    public void SetEndGameText ()
    {
        if(currentPhase == GamePhase.End)
        {
            startAndEndText.text = "You've beaten the final boss.\n\nThanks for playing.\n\nTap anywhere to restart.";
        }
    }

    public void LocateBossShip ()
    {
        bossShip = GameObject.FindGameObjectWithTag("Boss Spaceship").GetComponent<BossSpaceship>();
    }

    public void AddToCurrency(int amount)
    {
        metal += amount;
    }

    public void RemoveFromCurrency (int amount)
    {
        metal -= amount;
    }

    public void SetAmbientAudio ()
    {
        if (currentPhase == GamePhase.Combat)
        {
            if (!combatBackgroundAudioSource.isPlaying)
            {
                StopAllAmbientAudio();
                combatBackgroundAudioSource.Play();
            }
        }
        else if (currentPhase == GamePhase.Upgrade)
        {
            if (!upgradeBackgroundAudioSource.isPlaying)
            {
                StopAllAmbientAudio();
                upgradeBackgroundAudioSource.Play();
            }
        }
        else if (currentPhase == GamePhase.Boss)
        {
            if(!bossBackgroundAudioSource.isPlaying)
            {
                StopAllAmbientAudio();
                bossBackgroundAudioSource.Play();
            }
        }
    }

    public void StopAllAmbientAudio ()
    {
        combatBackgroundAudioSource.Stop();
        upgradeBackgroundAudioSource.Stop();
        bossBackgroundAudioSource.Stop();
    }
}