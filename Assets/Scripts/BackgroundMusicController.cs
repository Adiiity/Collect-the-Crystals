using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    [Header("Background Music Settings")]
    [SerializeField] private AudioSource audioSource; // Reference to the Audio Source
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f; // Volume slider (0-100%)

    private void Start()
    {
        // Set initial volume and start the music
        if (audioSource != null)
        {
            audioSource.volume = volume;
            audioSource.Play(); // Start playing the background music
        }
    }

    private void Update()
    {
        // Optional: Adjust volume dynamically (e.g., via UI slider)
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
