using UnityEngine;

public enum Element { Normal, Shock, Cryo }

public class Asteroid : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject metalCube;
    [SerializeField]
    private Element asteroidElement;
    private AsteroidMesh thisMesh;
    private Renderer thisRenderer;
    #endregion

    #region Properties
    public Element AsteroidElement => asteroidElement;
    public float MovementSpeed => 100;
    public Color ShockColor => new Color(0, .25F, 1);
    public Color CryoColor => new Color(0, 1, 1);
    public int ShockLength => 6;
    public int FreezeLength => 6;

    public AsteroidMesh ThisMesh
    {
        get
        {
            if (thisMesh == null)
            {
                thisMesh = transform.GetChild(0).GetComponent<AsteroidMesh>();
            }
            return thisMesh;
        }
    }

    public Renderer ThisRenderer
    {
        get
        {
            if (thisRenderer == null)
            {
                thisRenderer = transform.GetChild(0).GetComponent<Renderer>();
            }
            return thisRenderer;
        }
    }
    #endregion

    #region Methods
    private void Move() => transform.Translate(0, 0, -MovementSpeed * Time.deltaTime);
    public void DestroyAsteroid()
    {
        AsteroidSetup.Singleton.IncrementAsteroidsDestroyed();
        Destroy(gameObject);
    }
    public void SpawnMetal() => Instantiate(metalCube, transform.position, transform.rotation);
    private Element AssignedElement ()
    {
        switch (Random.Range(0, 7))
        {
            case 5: return Element.Shock;
            case 6: return Element.Cryo;
            default: return Element.Normal;
        }
        
    }

    private Color NewAsteroidColor()
    {
        if (asteroidElement == Element.Cryo)
        {
            return CryoColor;
        }
        else if (asteroidElement == Element.Shock)
        {
            return ShockColor;
        }
        return new Color(1, 1, 1);
    }
    #endregion

    public void Start()
    {
        asteroidElement = AssignedElement();

        ThisRenderer.material.color = NewAsteroidColor();
    }

    private void Update() => Move();

}