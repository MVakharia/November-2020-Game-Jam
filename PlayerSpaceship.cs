using UnityEngine;
using TMPro;

public class PlayerSpaceship : Spaceship
{
    private static PlayerSpaceship singleton;

    public static PlayerSpaceship Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("PSHolder").GetComponent<PlayerSpaceship>();
            }
            return singleton;
        }
    }

    #region Fields
    [SerializeField]
    private int shieldLevel;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private GameObject reticle;
    [SerializeField]
    private float reticleSpeed;
    [SerializeField]
    private int laserLevel;
    [SerializeField]
    private GameObject laserProjectile;
    [SerializeField]
    private float laserProjectileSpeed;
    [SerializeField]
    private GameObject clusterBombProjectile;
    [SerializeField]
    private GameObject deathRay;
    [SerializeField]
    private GameObject cannon;
    [SerializeField]
    private float laserCooldown;
    [SerializeField]
    private float laserCooldownCount;
    [SerializeField]
    private TMP_Text hullHealthText;
    [SerializeField]
    private TMP_Text shieldHealthText;
    [SerializeField]
    private AudioSource oneShotAudioSource;
    [SerializeField]
    private AudioClip laser;
    #endregion

    #region Properties
    private bool GameIsInBattlePhase => GameManager.Singleton.CurrentPhase == GamePhase.Combat || GameManager.Singleton.CurrentPhase == GamePhase.Boss;
    private bool IsTouchingScreen => Input.touchCount > 0;
    public int ShieldLevel => shieldLevel;
    public int HullLevel => hullLevel;
    public int LaserLevel => laserLevel;
    public float LaserProjectileSpeed => laserProjectileSpeed;
    private int NewMaximumHullHealth => (hullLevel * 2) + 1;
    private int NewMaximumShieldHealth() => (shieldLevel * 2) + 1;
    private string HullHealthTextToDisplay() => "Hull health:\n\n" + HullHealth;
    private string ShieldHealthTextToDisplay
    {
        get
        {
            if (shieldIsFrozen)
            {
                return "Danger. Further damage will destroy shield.";
            }
            else if (shieldIsRebooting)
            {
                if (ShieldHealth > 1)
                {
                    return "Danger. Shield rebooting in transit. Please stand by.";
                }
            }
            return "Shield health:\n\n" + shieldHealth;
        }
    }
    private float NewLaserFireRate()
    {
        float a = laserLevel - 1F;

        float b = a * 0.04F;

        return 0.5F - b;
    }
    #endregion

    #region Methods
    private void SetNewMaximumHullHealth() => hullMaximumHealth = NewMaximumHullHealth;
    public void UpgradeHull() { hullLevel++; SetNewMaximumHullHealth(); FullyRepairHull(); }
    public void DowngradeHull() { hullLevel--; SetNewMaximumHullHealth(); FullyRepairHull(); }
    public void RepairHull() => hullHealth++;
    public void FullyRepairHull() => hullHealth = hullMaximumHealth;
    public void UpgradeShield() { shieldLevel++; SetNewMaximumShieldHealth(); FullyRepairShield(); }
    public void DowngradeShield() { shieldLevel--; SetNewMaximumShieldHealth(); FullyRepairShield(); }
    private void SetNewMaximumShieldHealth() => shieldMaximumHealth = NewMaximumShieldHealth();
    public void RepairShield() => shieldHealth++;
    private void FullyRepairShield() => shieldHealth = shieldMaximumHealth;
    private void CountDownLaserCooldown() => laserCooldownCount -= Time.deltaTime;
    private void ResetLaserCooldown() => laserCooldownCount = laserCooldown;
    private void SetNewLaserFireRate() => laserCooldown = NewLaserFireRate();
    public void UpgradeLaser()
    {
        laserLevel++;
        SetNewLaserFireRate();
    }

    public void DowngradeLaser()
    {
        laserLevel--;
        SetNewLaserFireRate();
    }
    private void FireLaserProjectile()
    {
        if (laserCooldownCount <= 0 && reticle.transform.position.z > 125)
        {
            ResetLaserCooldown();
            Instantiate(laserProjectile, cannon.transform.position, cannon.transform.rotation);
            oneShotAudioSource.PlayOneShot(laser, 0.1F);
        }
    }

    private void SetHullHealthText () => hullHealthText.text = HullHealthTextToDisplay();
    private void SetShieldHealthText() => shieldHealthText.text = ShieldHealthTextToDisplay;
    #endregion

    private void Update()
    {
        if (IsTouchingScreen && GameIsInBattlePhase)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit, Mathf.Infinity, mask))
                {
                    reticle.transform.position = Vector3.MoveTowards(reticle.transform.position, hit.point, reticleSpeed * Time.deltaTime);
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                FireLaserProjectile();
            }

        }

        if (laserCooldownCount > 0)
        {
            CountDownLaserCooldown();
        }

        InheritedUpdateFunctionality();

        SetHullHealthText();

        SetShieldHealthText();        
    }
}