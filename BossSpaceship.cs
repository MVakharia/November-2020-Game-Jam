using UnityEngine;
using System.Collections;

public class BossSpaceship : Spaceship
{
    #region Fields
    private GameManager gameMgr => GameManager.Singleton;
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
    #endregion

    #region Properties
    private bool IsAtTargetPosition() => transform.position == targetPosition.position;

    public int RemainingEffectiveHealth => hullHealth + shieldHealth;

    public int TotalEffectiveHealth => hullMaximumHealth + shieldMaximumHealth;
    #endregion

    #region Methods
    private void SetHullHealthValue() => hullHealth = HullMaximumHealth;
    private void SetHullShieldValue() => shieldHealth = ShieldMaximumHealth;
    private void SetHullMaxHPPerLevel() => hullMaximumHealth = 10 + (gameMgr.CurrentLevel);
    private void SetShieldMaxHPToHullMaxHP() => shieldMaximumHealth = HullMaximumHealth;
    private void GetTargetPosition() => targetPosition = GameObject.FindGameObjectWithTag("Boss Position").transform;
    private void ToggleProjectileSpawningStatus() => hasStartedSpawningProjectiles = !hasStartedSpawningProjectiles;
    private void MoveToTargetPosition() => transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
    private void SpawnProjectile(Vector3 position) => Instantiate(projectile, position, Quaternion.identity);
    private void CallGameManager() => GameManager.Singleton.LocateBossShip();
    private void StartSpawningProjectiles()
    {
        if (!hasStartedSpawningProjectiles)
        {
            ToggleProjectileSpawningStatus();
            StartCoroutine(SpawnProjectiles());
        }
    }

    private IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            foreach (Transform p in projectileSpawnPositions)
            {
                SpawnProjectile(p.position);

                yield return new WaitForSeconds(projectileSpawnDelay);
            }
        }
    }


    #endregion

    private void Start()
    {
        CallGameManager();
        GetTargetPosition();

        SetHullMaxHPPerLevel();
        SetShieldMaxHPToHullMaxHP();

        SetHullHealthValue();
        SetHullShieldValue();
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
}