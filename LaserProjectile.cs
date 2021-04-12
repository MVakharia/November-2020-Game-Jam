using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField]
    private Vector3 reticlePosition;

    [SerializeField]
    private float movementSpeed;

    private float expiry = 2F, count = 0;

    public Vector3 ReticlePosition { get => reticlePosition; private set => reticlePosition = value; }
    public float MovementSpeed { get => movementSpeed; private set => movementSpeed = value; }

    private void Start()
    {
        LookAtReticle();

        GetMovementSpeed();
    }

    private void Update()
    {
        MoveProjectile();

        DestroyProjectileAfterDelay();
    }

    private void LookAtReticle ()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Reticle").transform);
    }

    private void GetMovementSpeed ()
    {
        MovementSpeed = PlayerSpaceship.Singleton.LaserProjectileSpeed;
    }

    private void MoveProjectile ()
    {
        transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
    }

    private void DestroyProjectileAfterDelay()
    {
        count += Time.deltaTime;

        if (count > expiry)
        {
            Destroy(gameObject);
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss Shield")
        {
            GameObject.FindGameObjectWithTag("Boss Spaceship").GetComponent<BossSpaceship>().ShieldHit();
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Boss Hull")
        {
            GameObject.FindGameObjectWithTag("Boss Spaceship").GetComponent<BossSpaceship>().HullHit();
            Destroy(gameObject);
        }
    }
}