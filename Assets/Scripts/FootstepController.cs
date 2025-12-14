using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepControllerStandalone : MonoBehaviour
{
    [Header("References")]
    [Tooltip("If left empty this will try to GetComponent<Controller2D>() on Start.")]
    public Controller2D controller;

    [Header("Input")]
    [Tooltip("Name of the horizontal axis (defaults to Unity's 'Horizontal').")]
    public string horizontalAxis = "Horizontal";
    [Tooltip("Minimum absolute axis value to count as moving.")]
    public float movementThreshold = 0.1f;

    [Header("Footstep Clips")]
    [Tooltip("Drag multiple footstep clips here. The script will iterate through them in order. Enable shuffle to randomize.")]
    public List<AudioClip> footstepClips = new List<AudioClip>();
    public bool shuffleClips = false;

    [Header("Timing & Sound")]
    [Tooltip("Seconds between footsteps while moving.")]
    public float stepInterval = 0.35f;
    [Tooltip("Volume used for PlayOneShot (0-1).")]
    [Range(0f, 1f)]
    public float volume = 1f;
    public float pitchMin = 0.95f;
    public float pitchMax = 1.05f;

    // internal
    private AudioSource audioSource;
    private float stepTimer = 0f;
    private int clipIndex = 0;
    private System.Random rng;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        rng = new System.Random();
    }

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<Controller2D>();
            if (controller == null)
            {
                Debug.LogWarning("[FootstepControllerStandalone] No Controller2D found on the GameObject. Please assign a Controller2D in inspector. Footsteps will not play until one is assigned.");
            }
        }

        if (footstepClips.Count == 0)
            Debug.LogWarning("[FootstepControllerStandalone] No footstep clips assigned.");
    }

    void Update()
    {
        // Safety: if no clips or no controller, do nothing (but avoid throwing errors)
        if (footstepClips == null || footstepClips.Count == 0) return;
        if (controller == null) return;

        // Must be grounded to play footsteps
        if (!controller.collisions.below)
        {
            // reset timer so steps won't immediately fire when you re-land and hold move
            stepTimer = 0f;
            return;
        }

        // Read horizontal input axis (works with Visual Scripting if it uses the same axis)
        float h = Input.GetAxisRaw(horizontalAxis);
        bool isMoving = Mathf.Abs(h) > movementThreshold;

        if (!isMoving)
        {
            stepTimer = 0f; // reset spacing when stopped
            return;
        }

        // countdown and play
        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f)
        {
            PlayNextStep();
            stepTimer = stepInterval;
        }
    }

    void PlayNextStep()
    {
        if (footstepClips == null || footstepClips.Count == 0) return;

        // pick clip (sequential or shuffle)
        AudioClip clipToPlay;
        if (shuffleClips)
        {
            int ri = rng.Next(0, footstepClips.Count);
            clipToPlay = footstepClips[ri];
        }
        else
        {
            clipToPlay = footstepClips[clipIndex];
            clipIndex = (clipIndex + 1) % footstepClips.Count;
        }

        if (clipToPlay == null) return;

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(clipToPlay, volume);
    }

    // Optional: helper to force-play (usable from other scripts or Visual Scripting)
    public void ForcePlayStepNow()
    {
        PlayNextStep();
        stepTimer = stepInterval;
    }
}
