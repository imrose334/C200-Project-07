using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundController : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip attackSound;

    private AudioSource audioSource;
    private Animator animator;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // SPACE BAR (Jump)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySound(jumpSound);
        }

        // ATTACK (Animator bool trigger)
        if (animator.GetBool("attack"))
        {
            PlaySound(attackSound);
            animator.SetBool("attack", false); // prevent looping sound
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
