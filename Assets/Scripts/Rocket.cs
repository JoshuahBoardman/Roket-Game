﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathSoundParticles;
    [SerializeField] ParticleSystem finishSoundParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = Rocket.State.Alive;

    bool CollisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            Rotate();
        }
        if (Debug.isDebugBuild)
        {
            RespondingToDevInput();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || CollisionsDisabled) { return; } 
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                startSuccessSequence();
                break;
            default:
                startDeathSequence();
                break;
        }
    }

    private void startSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishSoundParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay); 
    }

    private void startDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathSoundParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay); 
                                      // kill player
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int NextSceneIndex = currentSceneIndex + 1;
        int LastScene = SceneManager.sceneCountInBuildSettings;
        print(LastScene);
        if (NextSceneIndex == LastScene)
        {
            SceneManager.LoadScene(0); // loop back to start
        }
        else
        {
            SceneManager.LoadScene(NextSceneIndex);
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondingToDevInput()
    {
        if (Input.GetKeyDown(KeyCode.F)) //  Loads next level for easier testing
        {
            print("Pushing F");
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CollisionsDisabled = !CollisionsDisabled; // toggle
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating.
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying) // so it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
     
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {            transform.Rotate(Vector3.forward * rotationThisFrame);
            print("Rotating left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
            print("Rotating right");
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }
}