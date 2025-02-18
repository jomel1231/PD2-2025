using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrAudioManager : MonoBehaviour
{
    [Header("Progress Control")]
    [SerializeField] private ProgressControl progressControl;
    [SerializeField] private AudioSource progressSound;
    [SerializeField] private AudioClip challengeCompleteClip;

    [Header("Suction Tool Challenge")]
    [SerializeField] private XRGrabInteractable suctionTool;
    [SerializeField] private GameObject phoneScreen;
    [SerializeField] private AudioSource suctionToolAudio;
    [SerializeField] private AudioClip suctionStartClip;
    [SerializeField] private AudioClip suctionDetachClip;

    private bool isAttached = false;
    private bool challengeCompleted = false;
    private bool gameStarted = false;

    [Header("Background Music")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioClip backgroundMusicClip;
    private bool startAudioBool;

    private void OnEnable()
    {
        if (progressControl != null)
        {
            progressControl.OnStartGame.AddListener(StartGame);
        }
        if (suctionTool != null)
        {
            suctionTool.selectEntered.AddListener(OnSuctionToolUsed);
        }
    }

    private void OnDisable()
    {
        if (progressControl != null)
        {
            progressControl.OnStartGame.RemoveListener(StartGame);
        }
        if (suctionTool != null)
        {
            suctionTool.selectEntered.RemoveListener(OnSuctionToolUsed);
        }
    }

    private void StartGame(string arg0)
    {
        gameStarted = true;
        if (!startAudioBool)
        {
            startAudioBool = true;
            if (backgroundMusic != null && backgroundMusicClip != null)
            {
                backgroundMusic.clip = backgroundMusicClip;
                backgroundMusic.loop = true;
                backgroundMusic.Play();
            }
        }
    }

    private void OnSuctionToolUsed(SelectEnterEventArgs args)
    {
        if (!gameStarted || isAttached || challengeCompleted)
        {
            return;
        }
        StartCoroutine(PlaySuctionAudio());
    }

    private IEnumerator PlaySuctionAudio()
    {
        // Play suction start sound
        suctionToolAudio.clip = suctionStartClip;
        suctionToolAudio.Play();

        yield return new WaitForSeconds(suctionStartClip.length);

        // Play detach sound
        suctionToolAudio.clip = suctionDetachClip;
        suctionToolAudio.Play();

        ChallengeComplete();
    }

    private void ChallengeComplete()
    {
        challengeCompleted = true;
        if (progressSound != null && challengeCompleteClip != null)
        {
            progressSound.clip = challengeCompleteClip;
            progressSound.Play();
        }
        progressControl.OnChallengeComplete.Invoke("Suction Tool Challenge Complete");
        Debug.Log("Challenge Completed: Move to the next step!");
    }
}
