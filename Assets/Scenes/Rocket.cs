using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField]float rcsThrust =100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngineAudio;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] AudioClip successAudio;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;
    bool isTransitioning = false;
    bool collistionDisable = false;

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
        if (!isTransitioning) {
            ResponseToThrustInput();
            ResponseToRotateInput();
        }
        if (Debug.isDebugBuild) {ResponseToDebugKeys();}
    }

    private void ResponseToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.C) ) {
            collistionDisable = !collistionDisable;
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextScene();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collistionDisable) { return; }
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
        isTransitioning = true;
        if (audioSource.isPlaying) { audioSource.Stop(); }
        audioSource.PlayOneShot(deathAudio);
        deathParticles.Play();
        Invoke("LoadFristLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        if (audioSource.isPlaying) { audioSource.Stop(); }
        audioSource.PlayOneShot(successAudio);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void LoadFristLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex+ 1;
        if (SceneManager.sceneCountInBuildSettings ==nextSceneIndex) { nextSceneIndex = 0; }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ResponseToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust*Time.deltaTime);
        if (!audioSource.isPlaying) { audioSource.PlayOneShot(mainEngineAudio); }
        mainEngineParticles.Play();
    }

    private void ResponseToRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(Time.deltaTime * rcsThrust);
        }
        else if (Input.GetKey(KeyCode.D)) {
            RotateManually(-Time.deltaTime * rcsThrust);
        }
    }
    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true;//take manual control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false;//resume physics control of rotation
    }
}
