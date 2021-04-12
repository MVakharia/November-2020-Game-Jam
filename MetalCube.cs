using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MetalCube : MonoBehaviour
{
    [SerializeField]
    private Transform destination;
    [SerializeField]
    private bool delayComplete;
    [SerializeField]
    private int cubeValue;

    public Transform Destination { get => destination; private set => destination = value; }
    public bool DelayComplete { get => delayComplete; private set => delayComplete = value; }
    public int CubeValue { get => cubeValue; private set => cubeValue = value; }

    private void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Player Spaceship").GetComponent<Transform>();
    }

    void Update()
    {
        StartCoroutine(Pause());

        if(delayComplete)
        {
            Move();
        }

        if(transform.position == destination.position)
        {
            AddToCurrency(CubeValue);
            Destroy(gameObject);
        }
    }

    private IEnumerator Pause ()
    {
        yield return new WaitForSeconds(2);

        delayComplete = true;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, 400F * Time.deltaTime);
    }

    private void AddToCurrency (int amount)
    {
        GameManager.Singleton.AddToCurrency(amount);
    }
}
