using UnityEngine;
using System.Collections;

public class MetalCube : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Transform _destination;
    [SerializeField]
    private bool delayComplete;
    [SerializeField]
    private int cubeValue;
    #endregion

    #region Properties
    private bool AtDestination => transform.position == Destination.position;

    private Transform Destination
    {
        get
        {
            if(_destination == null)
            {
                _destination = GameObject.FindGameObjectWithTag("Player Spaceship").GetComponent<Transform>();
            }
            return _destination;
        }
    }
    #endregion

    #region Methods
    private void MarkDelayAsComplete () => delayComplete = true;
    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(2);

        MarkDelayAsComplete();
    }
    private void Move() => transform.position = Vector3.MoveTowards(transform.position, Destination.position, 400F * Time.deltaTime);
    private void AddToCurrency(int amount) => GameManager.Singleton.AddToCurrency(amount);
    #endregion

    void Update()
    {
        StartCoroutine(Pause());

        if(delayComplete)
        {
            Move();
        }

        if(AtDestination)
        {
            AddToCurrency(cubeValue);
            Destroy(gameObject);
        }
    }
}
