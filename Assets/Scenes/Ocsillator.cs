using UnityEngine;

[DisallowMultipleComponent]
public class Ocsillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    //TODO remove from inspector
    Vector3 startingPosition;
    [Range(0,1)]
    [SerializeField] float movementFactor;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = startingPosition + movementVector * movementFactor;
        transform.position = newPosition;
    }
}
