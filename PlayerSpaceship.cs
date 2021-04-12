using UnityEngine;
using TMPro;

public class PlayerSpaceship : Spaceship
{
    [SerializeField]
    private int shieldLevel;
    public int ShieldLevel { get => shieldLevel; private set => shieldLevel = value; }
    private void SetNewMaximumHullHealth() { HullMaximumHealth = NewMaximumHullHealth(); }
    private int NewMaximumHullHealth() { return (HullLevel * 2) + 1; }
    public void UpgradeHull() { HullLevel++; SetNewMaximumHullHealth(); FullyRepairHull(); }
    public void DowngradeHull() { HullLevel--; SetNewMaximumHullHealth(); FullyRepairHull(); }
    public int HullLevel { get => hullLevel; private set => hullLevel = value; }
    public void RepairHull() { HullHealth++; }
    public void FullyRepairHull() { HullHealth = HullMaximumHealth; }
    public void UpgradeShield() { ShieldLevel++; SetNewMaximumShieldHealth(); FullyRepairShield(); }
    public void DowngradeShield() { ShieldLevel--; SetNewMaximumShieldHealth(); FullyRepairShield(); }
    private int NewMaximumShieldHealth() { return (ShieldLevel * 2) + 1; }
    private void SetNewMaximumShieldHealth() { ShieldMaximumHealth = NewMaximumShieldHealth(); }
    public void RepairShield() { ShieldHealth++; }
    private void FullyRepairShield() { ShieldHealth = ShieldMaximumHealth; }

    private static PlayerSpaceship singleton;


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

    public static PlayerSpaceship Singleton
    {
        get
        {
            if(singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("PSHolder").GetComponent<PlayerSpaceship>();
            }
            return singleton;
        }
    }

    public LayerMask Mask { get => mask; private set => mask = value; }
    public GameObject Reticle { get => reticle; private set => reticle = value; }
    public float ReticleSpeed { get => reticleSpeed; private set => reticleSpeed = value; }
    
    public int LaserLevel { get => laserLevel; private set => laserLevel = value; }
    public GameObject LaserProjectile { get => laserProjectile; private set => laserProjectile = value; }
    public float LaserProjectileSpeed { get => laserProjectileSpeed; private set => laserProjectileSpeed = value; }
    public GameObject ClusterBombProjectile { get => clusterBombProjectile; private set => clusterBombProjectile = value; }
    public GameObject DeathRay { get => deathRay; private set => deathRay = value; }
    public GameObject Cannon { get => cannon; private set => cannon = value; }
    public float LaserCooldown { get => laserCooldown; private set => laserCooldown = value; }
    public float LaserCooldownCount { get { return laserCooldownCount; } private set { if (laserCooldownCount < 0) { laserCooldownCount = 0; } else { laserCooldownCount = value; } } }
    public TMP_Text HullHealthText { get => hullHealthText; private set => hullHealthText = value; }
    public TMP_Text ShieldHealthText { get => shieldHealthText; private set => shieldHealthText = value; }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray;

            RaycastHit hit;

            if(GameManager.Singleton.CurrentPhase == GamePhase.Combat || GameManager.Singleton.CurrentPhase == GamePhase.Boss)
            {
                if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask))
                    {
                        Reticle.transform.position = Vector3.MoveTowards(Reticle.transform.position, hit.point, ReticleSpeed * Time.deltaTime);
                    }
                }

                if(touch.phase == TouchPhase.Ended)
                {
                    FireLaserProjectile();
                }
            }
        }

        CountLaserCooldown();

        InheritedUpdateFunctionality();

        HullHealthText.text = HullHealthTextToDisplay();

        shieldHealthText.text = ShieldHealthTextToDisplay();
    }

    private void FireLaserProjectile ()
    {
        if(laserCooldownCount <= 0 && Reticle.transform.position.z > 125)
        {
            ResetLaserCooldown();
            Instantiate(laserProjectile, cannon.transform.position, cannon.transform.rotation);
            oneShotAudioSource.PlayOneShot(laser, 0.1F);
        }
    }

    private void CountLaserCooldown ()
    {
        if (laserCooldownCount > 0)
        {
            laserCooldownCount -= Time.deltaTime;
        }
    }

    private void ResetLaserCooldown ()
    {
        laserCooldownCount = laserCooldown;
    }
    
    
    
    public void UpgradeLaser ()
    {
        LaserLevel++;
        SetNewLaserFireRate();
    }

    public void DowngradeLaser ()
    {
        LaserLevel--;
        SetNewLaserFireRate();
    }
    private float NewLaserFireRate(float multiplier)
    {
        float a = LaserLevel - 1F;

        float b = a * multiplier;

        return 0.5F - b;
    }
    private void SetNewLaserFireRate ()
    {
        LaserCooldown = NewLaserFireRate(0.04F);
    }

    private string HullHealthTextToDisplay ()
    {
        return "Hull health:\n\n" + HullHealth;
    }

    private string ShieldHealthTextToDisplay ()
    {
        if(ShieldIsFrozen)
        {
            return "Danger. Further damage will destroy shield.";
        }
        else if (ShieldIsRebooting)
        {
            if(ShieldHealth > 1)
            {
                return "Danger. Shield rebooting in transit. Please stand by.";
            }            
        }

        return "Shield health:\n\n" + ShieldHealth;
    }
}