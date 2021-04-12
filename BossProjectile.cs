using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    #region Fields
    private Renderer _this_Renderer;
    private const float moveSpeed = 200;
    private const float projectileChargeSpeed = 1F;
    [SerializeField]
    private float redAmount = 0;
    [SerializeField]
    private float greenAmount = 1;
    #endregion

    #region Properties
    private GameObject PlayerShip => PlayerSpaceship.Singleton.gameObject;
    private bool ProjectileIsFullyCharged => redAmount == 1 && greenAmount == 0;
    private Renderer ThisRenderer
    {
        get
        {
            if (_this_Renderer == null)
            {
                _this_Renderer = GetComponent<Renderer>();
            }
            return _this_Renderer;
        }
    }
    #endregion

    #region Methods
    private void Move() => transform.position = Vector3.MoveTowards(transform.position, PlayerShip.transform.position, moveSpeed * Time.deltaTime);
    private void ChargeProjectile()
    {
        ThisRenderer.material.color = new Color(redAmount, greenAmount, 0, 0);

        if (redAmount != 1)
        {
            redAmount += projectileChargeSpeed * Time.deltaTime;
        }

        if (redAmount > 1)
        {
            redAmount = 1;
        }

        if (redAmount == 1 && greenAmount != 0)
        {
            greenAmount -= projectileChargeSpeed * Time.deltaTime;
        }

        if (greenAmount < 0)
        {
            greenAmount = 0;
        }
    }

    private void DestroyProjectile() => Destroy(gameObject);
    #endregion


    private void Update()
    {
        if (ProjectileIsFullyCharged)
        {
            Move();
        }
        else
        {
            ChargeProjectile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Shield"))
        {
            PlayerSpaceship.Singleton.ShieldHit();
            DestroyProjectile();
        }

        if(other.gameObject.CompareTag("Player Spaceship"))
        {
            PlayerSpaceship.Singleton.HullHit();
            DestroyProjectile();
        }
        if (other.gameObject.CompareTag("Laser Projectile"))
        {
            Destroy(other.gameObject);
            DestroyProjectile();
        }
    }    
}