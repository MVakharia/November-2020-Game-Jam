using UnityEngine;

public enum Element
{
    Normal, Shock, Cryo
}

public class Asteroid : MonoBehaviour
{
    private float movementSpeed = 100;

    [SerializeField]
    private GameObject metalCube;

    [SerializeField]
    private Element asteroidElement;

    public float MovementSpeed { get => movementSpeed; private set => movementSpeed = value; }
    public GameObject MetalCube { get => metalCube; private set => metalCube = value; }
    public AsteroidMesh ThisMesh { get; private set; }
    public Renderer ThisRenderer { get; private set; }
    public Element AsteroidElement { get => asteroidElement; private set => asteroidElement = value; }
    public Color ShockColor { get; private set; }
    public Color CryoColor { get; private set; }
    public int ShockLength { get; private set; }
    public int FreezeLength { get; private set; }

    public void Start()
    {
        ThisMesh = transform.GetChild(0).GetComponent<AsteroidMesh>();
        ThisRenderer = transform.GetChild(0).GetComponent<Renderer>();

        ShockColor = new Color(0, .25F, 1);
        CryoColor = new Color(0, 1, 1);
        ShockLength = 6;
        FreezeLength = 6;

        AsteroidElement = AssignedElement();

        ThisRenderer.material.color = NewAsteroidColor();
    }

    private void Update()
    {
        transform.Translate(0, 0, -MovementSpeed * Time.deltaTime);
    }

    public void DestroyAsteroid ()
    {
        AsteroidSetup.Singleton.IncrementAsteroidsDestroyed();
        Destroy(gameObject);
    }

    public void SpawnMetal ()
    {
        Instantiate(MetalCube, transform.position, transform.rotation);
    }

    private Element AssignedElement ()
    {
        switch(Random.Range(0, 7))
        {
            case 5: return Element.Shock;
            case 6: return Element.Cryo;
            default: return Element.Normal;
        }
    }

    private Color NewAsteroidColor ()
    {
        if(AsteroidElement == Element.Cryo)
        {
            return CryoColor;
        }
        else if (AsteroidElement == Element.Shock)
        {
            return ShockColor;
        }
        return new Color(1, 1, 1);
    }
}