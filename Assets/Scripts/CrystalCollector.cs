using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrystalCollector : MonoBehaviour
{
    public int score = 0; // Player score
    public TMP_Text scoreText; // Drag your UI Text element here in the Inspector
    public AudioClip collectSound; // Drag your audio clip here in the Inspector
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        UpdateScoreUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crystal"))
        {
            score += 1;
            Destroy(other.gameObject);
            PlaySoundEffect(); // Play the sound effect
            UpdateScoreUI();
        }
    }

    private void PlaySoundEffect()
    {
        audioSource.PlayOneShot(collectSound); // Play the assigned audio clip
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Crystals: " + score;
        // scoreText.text = $"💎: {score}";
    }
}
