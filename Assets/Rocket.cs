using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField]float rcsThrust =100f;
    [SerializeField] float mainThrust = 100f;
    enum State {Alive,Dying,Transcending}
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // todo some where stop sound while dead(bug)
        if (state == State.Alive) {
            Trust();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state!=State.Alive) { return; }
        switch (collision.gameObject.tag) {
            case "Friendly":
                break;
            case"Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1.0f);//todo set parameter for wait time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFristLevel",1.0f);//todo set parameter for wait time
                break;
        }
    }

    private void LoadFristLevel()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);//todo allow to more than 2 levels
    }

    private void Trust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up*mainThrust);
            if (!audioSource.isPlaying) { audioSource.Play(); }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;//take manual control of rotation
        float rotationThisFrame = Time.deltaTime * rcsThrust;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;//resume physics control of rotation
    }
}
