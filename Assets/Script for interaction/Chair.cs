using UnityEngine;

public class Chair : MonoBehaviour
{
    public GameObject playerStanding;  // Reference to the XR Rig
    public Transform sitPoint;         // The sitting position on the chair
    public GameObject intText;         // Canvas text (child of the chair)
    public bool sitting = false;       // Is the player currently sitting
    public float standUpThreshold = 0.1f; // Threshold for detecting movement to stand up

    private Vector3 lastPlayerPosition; // Tracks the player's last position

    private void Start()
    {
        // Ensure the text starts disabled
        if (intText != null)
            intText.SetActive(false);

        if (playerStanding != null)
            lastPlayerPosition = playerStanding.transform.position; // Initialize player position
    }

    private void Update()
    {
        if (sitting)
        {
            // Check if the player is moving
            if (PlayerIsMoving())
            {
                Stand(); // Stand up if the player moves
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Automatically sit when the player enters the chair's trigger
        if (other.CompareTag("MainCamera")) // Assuming MainCamera is part of the XR Rig
        {
            if (!sitting)
            {
                Sit();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stand up when the player leaves the chair's trigger
        if (other.CompareTag("MainCamera"))
        {
            if (sitting)
            {
                Stand();
            }
        }
    }

    private void Sit()
    {
        // Move playerStanding (XR Rig) to the SitPoint
        if (playerStanding != null)
        {
            playerStanding.transform.SetPositionAndRotation(sitPoint.position, sitPoint.rotation);
        }

        sitting = true;

        // Show interaction text if applicable
        if (intText != null)
        {
            intText.GetComponentInChildren<UnityEngine.UI.Text>().text = "You are sitting";
            intText.SetActive(true);
        }

        // Update player's last position to prevent false movement detection
        if (playerStanding != null)
        {
            lastPlayerPosition = playerStanding.transform.position;
        }
    }

    private void Stand()
    {
        sitting = false;

        // Hide interaction text when standing
        if (intText != null)
        {
            intText.SetActive(false);
        }
    }

    private bool PlayerIsMoving()
    {
        // Detect if the player's XR Rig is moving
        if (playerStanding != null)
        {
            Vector3 currentPosition = playerStanding.transform.position;
            float distanceMoved = Vector3.Distance(currentPosition, lastPlayerPosition);

            if (distanceMoved > standUpThreshold)
            {
                // Update the last position for future checks
                lastPlayerPosition = currentPosition;
                return true; // Player is moving
            }
        }

        return false; // Player is not moving
    }
}
