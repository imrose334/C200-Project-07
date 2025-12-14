using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepController : MonoBehaviour
{
    [Header("References")]
    public Controller2D controller;      // drag in your controller
    public Player player;                // drag in your Player script (for directionalInput)

    [Header("Footstep Settings")]
    public AudioClip footstepClip;
    public float stepInterval = 0.35f;   // time between steps
    public float pitchMin = 0.9f;
    public float pitchMax = 1.1f;

    private AudioSource audioSource;
    private float stepTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (player == null || controller == null) return;

        // 1. Must be grounded
        if (!controller.collisions.below)
        {
            stepTimer = 0f;
            return;
        }

        // 2. Must be moving horizontally
        float moveX = Mathf.Abs(player.GetDirectionalInput().x);
        bool isMoving = moveX > 0.1f;

        if (!isMoving)
        {
            stepTimer = 0f;
            return;
        }

        // 3. Timer for footstep spacing
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayStep();
            stepTimer = stepInterval;
        }
    }

    void PlayStep()
    {
        if (footstepClip == null) return;

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(footstepClip);
    }
}
