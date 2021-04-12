using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    #region Fields
    private float expiry = 2F, count = 0;
    [SerializeField]
    private Vector3 reticlePosition;
    [SerializeField]
    private float movementSpeed;
    #endregion

    #region Methods
    private void LookAtReticle() => transform.LookAt(GameObject.FindGameObjectWithTag("Reticle").transform);
    private void GetMovementSpeed() => movementSpeed = PlayerSpaceship.Singleton.LaserProjectileSpeed;
    private void MoveProjectile() => transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    private void DestroyProjectileAfterDelay()
    {
        count += Time.deltaTime;

        if (count > expiry)
        {
            Destroy(gameObject);
        }
    }
    #endregion

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