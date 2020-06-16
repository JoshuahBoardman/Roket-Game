using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Rocket : MonoBehaviour
{
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
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thrust while rotating.
        {
            rigidBody.AddRelativeForce(Vector3.up);
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
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
            print("Rotating left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
            print("Rotating right");
        }

    }
}