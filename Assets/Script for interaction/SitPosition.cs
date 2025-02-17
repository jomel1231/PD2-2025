using UnityEngine;

public class ChairInteraction : MonoBehaviour
{
    public Transform sitPosition; // Assign a target position for sitting
    public GameObject playerCamera; // Assign the XR Camera or player object
    private bool canSit = false; // Check if the player is within the trigger zone

    void Update()
    {
        if (canSit && Input.GetKeyDown(KeyCode.E))
        {
            // Move the player to the sitting position
            playerCamera.transform.position = sitPosition.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the interacting object is the player
        {
            canSit = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSit = false;
        }
    }
}
