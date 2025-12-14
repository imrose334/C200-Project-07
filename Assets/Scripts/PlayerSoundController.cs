using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundController : MonoBehaviour
{
    public AudioClip jumpSound;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySound(jumpSound);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
    }
}
