using UnityEngine;

[DisallowMultipleComponent]
public class Ocsillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector=new Vector3(10f,10f,10f);
    //TODO remove from inspector
    Vector3 startingPosition;
    [Range(0,1)][SerializeField] float movementFactor;
    [SerializeField]private float period=2f;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period<=Mathf.Epsilon) { throw new System.DivideByZeroException();}
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = (rawSinWave / 2f) + 0.5f;
        Vector3 newPosition = startingPosition + movementVector * movementFactor;
        transform.position = newPosition;
    }
}
