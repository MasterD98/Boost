using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField]float rcsThrust =100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
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
            ResponseToThrustInput();
            ResponseToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state!=State.Alive) { return; }//ignore collision while not alive state
        switch (collision.gameObject.tag) {
            case "Friendly":
                break;
            case"Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        if (audioSource.isPlaying) { audioSource.Stop(); }
        audioSource.PlayOneShot(death);
        Invoke("LoadFristLevel", 1.0f);//todo set parameter for wait time
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        if (audioSource.isPlaying) { audioSource.Stop(); }
        audioSource.PlayOneShot(success);
        Invoke("LoadNextScene", 1.0f);//todo set parameter for wait time
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

    private void ResponseToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying) { audioSource.PlayOneShot(mainEngine); }
    }

    private void ResponseToRotateInput()
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
