using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Renderer thisRenderer;

    private float redAmount = 0;

    private float greenAmount = 1;

    private GameObject playerShip;

    private float moveSpeed = 200;

    private float projectileChargeSpeed = 1F;

    private void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        playerShip = PlayerSpaceship.Singleton.gameObject;
    }

    private void Update()
    {
        if(!ProjectileIsFullyCharged())
        {
            ChargeProjectile();
        }
        else
        {
            Move();
        }
    }

    private void ChargeProjectile ()
    {
        thisRenderer.material.color = new Color(redAmount, greenAmount, 0, 0);

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

    private bool ProjectileIsFullyCharged ()
    {
        return redAmount == 1 && greenAmount == 0;
    }

    private void Move ()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerShip.transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Shield")
        {
            PlayerSpaceship.Singleton.ShieldHit();
            DestroyProjectile();
        }

        if(other.gameObject.tag == "Player Spaceship")
        {
            PlayerSpaceship.Singleton.HullHit();
            DestroyProjectile();
        }
        if (other.gameObject.tag == "Laser Projectile")
        {
            Destroy(other.gameObject);
            DestroyProjectile();
        }
    }

    private void DestroyProjectile ()
    {
        Destroy(gameObject);
    }
}