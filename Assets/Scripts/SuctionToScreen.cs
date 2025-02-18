using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SuctionToScreen : MonoBehaviour
{
    public Transform attachPoint; // Empty GameObject on the suction tool where the screen will attach
    private XRGrabInteractable screenInteractable; // Store the screen when attached
    private XRGrabInteractable suctionGrabInteractable; // Suction tool grab interactable

    private bool isScreenAttached = false; // To check if screen is already attached
    private float attachmentTime = 3f; // Time for screen to stay attached (in seconds)
    private float timer = 0f; // Timer to count down to detachment

    private void Start()
    {
        // Get the XRGrabInteractable component of the suction tool
        suctionGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if we collided with a screen (XRGrabInteractable)
        if (other.TryGetComponent<XRGrabInteractable>(out XRGrabInteractable screen))
        {
            // Attach the screen to the suction tool if not already attached
            if (!isScreenAttached)
            {
                // Detach the screen from the chassis if it's currently attached
                DetachFromChassis(screen);

                // Attach the screen to the suction tool (temporary)
                AttachToSuction(screen);
            }
        }
    }

    // Detach the screen from the chassis (XR Socket)
    void DetachFromChassis(XRGrabInteractable screen)
    {
        // Check if the screen is selected (held by an XRSocket)
        if (screen.isSelected)
        {
            // Find the interactor currently selecting the screen (the chassis socket)
            IXRSelectInteractor interactor = screen.interactorsSelecting[0];

            // Force release the screen from the chassis socket (detach from socket)
            screen.interactionManager.SelectExit(interactor, screen);
        }
    }

    // Attach the screen to the suction tool temporarily
    void AttachToSuction(XRGrabInteractable screen)
    {
        screenInteractable = screen;

        // Disable screen interaction while attached to suction tool
        screen.interactionLayerMask = LayerMask.GetMask("Nothing");

        isScreenAttached = true; // Mark the screen as attached

        // Start the timer to detach the screen after 3 seconds
        timer = attachmentTime;

        Debug.Log("Screen attached to suction tool!");
    }

    private void Update()
    {
        // If the screen is attached to the suction tool, countdown the timer
        if (isScreenAttached)
        {
            timer -= Time.deltaTime;

            // Once the timer runs out, detach the screen
            if (timer <= 0f)
            {
                DetachFromSuction();
            }
        }

        // If the suction tool is grabbed, detach the screen
        if (suctionGrabInteractable.isSelected && isScreenAttached)
        {
            DetachFromSuction();
        }
    }

    // Detach the screen from the suction tool
    void DetachFromSuction()
    {
        // Re-enable screen interaction and allow it to be grabbed again
        if (screenInteractable != null)
        {
            screenInteractable.interactionLayerMask = LayerMask.GetMask("Default");
        }

        isScreenAttached = false; // Mark the screen as detached

        screenInteractable = null;
        Debug.Log("Screen detached from suction tool!");
    }
}
