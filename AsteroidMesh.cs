using UnityEngine;

public class AsteroidMesh : MonoBehaviour
{
    #region Fields
    private Asteroid _this_Asteroid;
    #endregion

    #region Properties
    public Element ThisAsteroidElement => ThisAsteroid.AsteroidElement;

    public Asteroid ThisAsteroid
    {
        get
        {
            if (_this_Asteroid == null)
            {
                _this_Asteroid = transform.GetComponentInParent<Asteroid>();
            }
            return _this_Asteroid;
        }
    }
    #endregion

    #region Methods
    public void DestroyAsteroid()
    {
        ThisAsteroid.DestroyAsteroid();
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ObjectEraser"))
        {
            DestroyAsteroid();
        }

        if (other.gameObject.CompareTag("Player Shield"))
        {
            PlayerSpaceship.Singleton.ShieldHit();

            if (ThisAsteroidElement == Element.Cryo)
            {
                PlayerSpaceship.Singleton.StartFreezingShield(ThisAsteroid.FreezeLength);
            }
            else if(ThisAsteroidElement == Element.Shock)
            {                
                PlayerSpaceship.Singleton.ShockShield(ThisAsteroid.ShockLength);                
            }
            
            DestroyAsteroid();
        }

        if(other.gameObject.CompareTag("Player Spaceship"))
        {
            PlayerSpaceship.Singleton.HullHit();
            DestroyAsteroid();
        }

        if(other.gameObject.CompareTag("Laser Projectile"))
        {
            Destroy(other.gameObject);
            ThisAsteroid.SpawnMetal();
            DestroyAsteroid();
        }
    }
}