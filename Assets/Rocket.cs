using System;
using System.Collections;
using System.Collections.Generic;
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
        Trust();
        Rotate();
    }

    private void Trust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
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
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime*100);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * Time.deltaTime*100);
        }
        rigidBody.freezeRotation = false;//resume physics control of rotation
    }
}
