using UnityEngine;
using System.Collections;

public class BossSpaceship : Spaceship
{
    private static BossSpaceship singleton;
    [SerializeField]
    private Transform targetPosition;
    [SerializeField]
    private float moveSpeed = 100F;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform[] projectileSpawnPositions;
    [SerializeField]
    private bool hasStartedSpawningProjectiles;
    [SerializeField]
    private float projectileSpawnDelay = 1F;
    [SerializeField]
    private GameManager gameMgr;
    public static BossSpaceship Singleton { get => singleton; private set => singleton = value; }
    public Transform TargetPosition { get => targetPosition; private set => targetPosition = value; }
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    public GameObject Projectile { get => projectile; private set => projectile = value; }
    public Transform[] ProjectileSpawnPositions { get => projectileSpawnPositions; private set => projectileSpawnPositions = value; }
    public bool HasStartedSpawningProjectiles { get => hasStartedSpawningProjectiles; private set => hasStartedSpawningProjectiles = value; }
    public float ProjectileSpawnDelay { get => projectileSpawnDelay; private set => projectileSpawnDelay = value; }
    public GameManager GameMgr { get => gameMgr; private set => gameMgr = value; }

    public void SetHealthAndShieldValues()
    {
        HullHealth = HullMaximumHealth;
        ShieldHealth = ShieldMaximumHealth;
    }

    public void SetHealthAndShieldValuesPerLevel()
    {
        HullMaximumHealth = 10 + (GameMgr.CurrentLevel);
        ShieldMaximumHealth = HullMaximumHealth;
    }

    public void GetGameManager()
    {
        GameMgr = GameManager.Singleton.gameObject.GetComponent<GameManager>();
    }

    public void GetTargetPosition()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Boss Position").transform;
    }

    public void ToggleProjectileSpawningStatus()
    {
        HasStartedSpawningProjectiles = !HasStartedSpawningProjectiles;
    }

    private void Start()
    {
        GetGameManager();

        CallGameManager();
        GetTargetPosition();

        SetHealthAndShieldValuesPerLevel();
        SetHealthAndShieldValues();
    }

    private void Update()
    {
        if(!IsAtTargetPosition())
        {
            MoveToTargetPosition();
        }
        else if (IsAtTargetPosition())
        {
            StartSpawningProjectiles();
        }

        InheritedUpdateFunctionality();
    }

    public void MoveToTargetPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition.position, MoveSpeed * Time.deltaTime);
    }

    private bool IsAtTargetPosition ()
    {
        return transform.position == TargetPosition.position;
    }

    public void StartSpawningProjectiles ()
    {
        if(!HasStartedSpawningProjectiles)
        {
            ToggleProjectileSpawningStatus();
            StartCoroutine(SpawnProjectiles());
        }
    }

    private IEnumerator SpawnProjectiles ()
    {
        while(true)
        {
            foreach (Transform p in ProjectileSpawnPositions)
            {
                SpawnProjectile(p.position);

                yield return new WaitForSeconds(ProjectileSpawnDelay);
            }
        }
    }

    private void SpawnProjectile (Vector3 position)
    {
        Instantiate(Projectile, position, Quaternion.identity);
    }

    private void CallGameManager ()
    {
        GameManager.Singleton.LocateBossShip();
    }
}