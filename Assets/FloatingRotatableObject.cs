using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FloatingRotatableObject : MonoBehaviour
{
    public float hoverAmplitude = 0.1f; // How much it floats up and down
    public float hoverFrequency = 1f; // Speed of the hover effect
    public float rotationSpeed = 100f; // Speed of user rotation

    private Vector3 startPos;
    private XRBaseInteractor interactor;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Floating effect
        float yOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }

    public void RotateObject(Vector2 rotationInput)
    {
        // Rotate the object based on user input
        transform.Rotate(Vector3.up, rotationInput.x * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, -rotationInput.y * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
