using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rcsThrust = 100f;


    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                print("Ok"); //todo remove
                break;
            case "Fuel":
                print("Fuel");
                break;
            default:
                print("Dead");
                // kill player
                break;
        }
    }
    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating.
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!audioSource.isPlaying) // so it doesn't layer
            {
                audioSource.Play();
            }

            print("Thrusting");
        }
        else
        {
            audioSource.Stop();
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