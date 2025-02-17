using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpudgerInteraction : MonoBehaviour
{
    public Transform screen; // The screen to remove
    public XRSocketInteractor screenSocket; // Reference to the socket that holds the screen
    public Transform newScreen; // The new screen to insert back
    public float removalDistance = 0.1f; // How far the screen moves out when removed
    private bool isTouchingScreen = false;
    private bool isScreenRemoved = false;

    void Start()
    {
        if (screen != null)
        {
            // Set up the initial screen position (before removing it)
            screenSocket.interactionManager = screenSocket.GetComponent<XRInteractionManager>();
        }
    }

    void Update()
    {
        // If the screen has been removed, handle the insertion of the new screen
        if (isScreenRemoved)
        {
            // Check if the new screen is close enough to the socket to insert it
            if (newScreen != null)
            {
                if (Vector3.Distance(newScreen.position, screenSocket.transform.position) < 0.1f)
                {
                    InsertNewScreen(newScreen);
                }
            }
        }

        // If the screen is being touched by the spudger, move it out
        if (isTouchingScreen && !isScreenRemoved)
        {
            // Temporarily disable socket interaction (only disable interactions, don't null out the manager)
            screenSocket.enabled = false;

            // Immediately move the screen outward to simulate removal
            screen.position = screen.position + Vector3.up * removalDistance;
            Debug.Log("Screen removed.");

            // Mark that the screen has been removed
            isScreenRemoved = true;

            // Optionally, call method to add the new screen after a delay
            Invoke("EnableNewScreen", 1f); // Adjust delay as needed
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the interacting object is the spudger (use a specific tag for the spudger)
        if (other.CompareTag("Spudger"))
        {
            Debug.Log("Spudger touched the screen.");
            isTouchingScreen = true;
        }

        // Ensure that if the screwdriver interacts, it doesn't affect the screen
        if (other.CompareTag("Screwdriver"))
        {
            Debug.Log("Screwdriver detected. Not interacting with screen.");
            // Prevent any movement or changes if the screwdriver is interacting
            isTouchingScreen = false;
        }
    }

    private void InsertNewScreen(Transform newScreenTransform)
    {
        // Re-enable the socket interaction for inserting the new screen
        screenSocket.enabled = true;

        // Attach the new screen to the socket
        newScreenTransform.SetParent(screenSocket.transform);
        newScreenTransform.position = screenSocket.transform.position; // Snap to socket position
        newScreenTransform.rotation = screenSocket.transform.rotation; // Snap to socket rotation

        // Optional: reset the position if needed (or animate insertion)
        Debug.Log("New screen inserted.");
    }

    private void EnableNewScreen()
    {
        // Enable new screen interaction, like setting visibility
        newScreen.gameObject.SetActive(true);
    }
}
