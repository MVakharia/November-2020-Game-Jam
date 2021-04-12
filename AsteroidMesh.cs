using UnityEngine;

public class AsteroidMesh : MonoBehaviour
{
    private Asteroid thisAsteroid;

    private Element thisAsteroidElement;

    private void Start()
    {
        thisAsteroid = transform.GetComponentInParent<Asteroid>();

        thisAsteroidElement = thisAsteroid.AsteroidElement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ObjectEraser")
        {
            DestroyAsteroid();
        }

        if (other.gameObject.tag == "Player Shield")
        {
            PlayerSpaceship.Singleton.ShieldHit();

            if (thisAsteroidElement == Element.Cryo)
            {
                PlayerSpaceship.Singleton.StartFreezingShield(thisAsteroid.FreezeLength);
            }
            else if(thisAsteroidElement == Element.Shock)
            {                
                PlayerSpaceship.Singleton.ShockShield(thisAsteroid.ShockLength);                
            }
            
            DestroyAsteroid();
        }

        if(other.gameObject.tag == "Player Spaceship")
        {
            PlayerSpaceship.Singleton.HullHit();
            DestroyAsteroid();
        }

        if(other.gameObject.tag == "Laser Projectile")
        {
            Destroy(other.gameObject);
            thisAsteroid.SpawnMetal();
            DestroyAsteroid();
        }
    }

    public void DestroyAsteroid ()
    {
        thisAsteroid.DestroyAsteroid();
    }
}