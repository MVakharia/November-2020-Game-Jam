using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float LevelProgress => (AsteroidManager.Singleton.AsteroidsDestroyedThisRound / (float)AsteroidManager.Singleton.AsteroidsThisRound) * 100;
    private bool PlayerIsTouchingScreen => Input.touchCount > 0;
    private bool BeganTouchOnIndex0 => Input.GetTouch(0).phase == TouchPhase.Began;
    private bool PlayerShieldHasHealth => PlayerSpaceship.Singleton.ShieldHealth > 0;
    public string LevelProgressTextToDisplay => currentPhase == GamePhase.Combat ? LevelProgress.ToString("0") + "%" : (levelProgressText.text = "");
    public float BossFightCompletionPercentage => bossShip ? (float)bossShip.RemainingEffectiveHealth / bossShip.TotalEffectiveHealth * 100F : 0;
    public string LevelTextToDisplay => currentPhase == GamePhase.Boss ? "Boss" : "Level " + currentLevel;
    private bool GameEnded => currentPhase == GamePhase.Loss || currentPhase == GamePhase.End;
    private string StartAndEndTextToDisplay => currentPhase == GamePhase.End ? "You've beaten the final boss.\n\nThanks for playing.\n\nTap anywhere to restart." : "";
    private bool HullHealthIsZero => PlayerSpaceship.Singleton.HullHealth <= 0;
    private bool GameIsInBattlePhase => currentPhase == GamePhase.Combat || currentPhase == GamePhase.Boss;
    public string PhaseTextToDisplay
    {
        get
        {
            switch (currentPhase)
            {
                case GamePhase.Combat: return "Survive the asteroid belt.";
                case GamePhase.Boss: return BossFightCompletionPercentage.ToString("0") + "% health remaining.";
                case GamePhase.Upgrade: return "";
                case GamePhase.Loss: return "You lose. Tap anywhere to restart.";
                default: return "";
            }
        }
    }
    #endregion

    #region Methods
    private void SetLevel(int level) => currentLevel = level;
    public void LocateBossShip() => bossShip = GameObject.FindGameObjectWithTag("Boss Spaceship").GetComponent<BossSpaceship>();
    public void AddToCurrency(int amount) => metal += amount;
    public void RemoveFromCurrency(int amount) => metal -= amount;
    private void BeginGame() => BeginALevel(1);
    private void UnLocateBossShip() => bossShipFound = false;
    private void IncrementLevel() => currentLevel++;
    private void SetLevelProgressText() => levelProgressText.text = LevelProgressTextToDisplay;
    private void SetCurrentPhaseText () => currentPhaseText.text = PhaseTextToDisplay;
    private void SetCurrentLevelText () => currentLevelText.text = LevelTextToDisplay;
    private void MarkPlayerAsMovedToUpgradePhase () => playerMovedToUpgradePhase = true;
    private void SetEndGameText() => startAndEndText.text = StartAndEndTextToDisplay;
    private void MarkBossShipAsFound() => bossShipFound = true;
    private void MarkPlayerGameLoss() => playerHasLostGame = true;
    private void BeginLevel()
    {
        SetCurrentPhase(GamePhase.Combat);

        if (PlayerShieldHasHealth)
        {
            PlayerSpaceship.Singleton.ReEnableShieldForEOR();
        }

        AsteroidManager.Singleton.SetUpForLevelBeginning();

        StartCoroutine(AsteroidSpawner.Singleton.SpawnAsteroids());
    }

    public void EndCombatRound()
    {
        if (currentLevel % 2 != 0)
        {
            SetCurrentPhase(GamePhase.Upgrade);
        }
        else
        {
            UnLocateBossShip();
            SetCurrentPhase(GamePhase.Boss);
        }
    }
    
    public void EndBossRound()
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

    public void DisableEnableUI()
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

    public void LoseGame()
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

    public void SetAmbientAudio()
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
            if (!bossBackgroundAudioSource.isPlaying)
            {
                StopAllAmbientAudio();
                bossBackgroundAudioSource.Play();
            }
        }
    }
    public void StopAllAmbientAudio()
    {
        combatBackgroundAudioSource.Stop();
        upgradeBackgroundAudioSource.Stop();
        bossBackgroundAudioSource.Stop();
    }
    private void SetCurrentPhase(GamePhase phase)
    {
        currentPhase = phase;
        DisableEnableUI();
    }

    private void SetUpGame()
    {
        SetCurrentPhase(GamePhase.StartScreen);
        SetLevel(0);
        AsteroidManager.Singleton.SetAsteroidsPerLevel();
    }
    private void BeginALevel(int level)
    {
        SetLevel(level);

        BeginLevel();
    }
    public void BeginNextLevel()
    {
        IncrementLevel();

        BeginLevel();
    }
    #endregion

    private void Awake()
    {
        SetUpGame();
    }

    private void Update()
    {
        if (PlayerIsTouchingScreen && BeganTouchOnIndex0)
        {
            if (currentPhase == GamePhase.StartScreen)
            {
                BeginGame();
            }

            if (GameEnded)
            {
                SceneManager.LoadScene(0);
            }
        }

        if(GameIsInBattlePhase)
        {
            if(HullHealthIsZero && !playerHasLostGame)
            {
                MarkPlayerGameLoss();
                LoseGame();
            }
        }

        if (currentPhase == GamePhase.Combat && AsteroidManager.Singleton.AllAsteroidsDestroyed)
        {
            EndCombatRound();
        }

        if(currentPhase == GamePhase.Boss)
        {
            if(bossShip && !bossShipFound)
            {
                MarkBossShipAsFound();
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

        SetLevelProgressText();

        SetCurrentPhaseText();

        SetCurrentLevelText();

        if(currentPhase == GamePhase.Upgrade && !playerMovedToUpgradePhase)
        {
            MarkPlayerAsMovedToUpgradePhase();
        }

        upgradeShopBackground.SetActive(currentPhase == GamePhase.Upgrade);

        SetAmbientAudio();
    }
}