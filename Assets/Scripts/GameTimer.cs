using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timerDuration = 90f; // Timer duration in seconds
    [SerializeField] private TMP_Text timerText; // Reference to the UI text for the timer
    [SerializeField] private GameObject endScreenUI; // Reference to the end screen UI
    [SerializeField] private TMP_Text recordText; // Reference to the UI text for the record
    [SerializeField] private GameObject startScreenUI;

    private float timeRemaining;
    private bool timerRunning = true;
    private int collectedCrystals = 0; // Record of crystals collected
    private bool gameStarted = false;

    private void Start()
    {
        timeRemaining = timerDuration;

        // Ensure the end screen UI is hidden initially
        if (endScreenUI != null)
            endScreenUI.SetActive(false);
    }

    private void Update()
    {
        if (!gameStarted)
    {
        // Wait for the player to click to start the game
        if (Input.GetMouseButtonDown(0)) // Detect mouse click or screen tap
        {
            StartGame();
        }
        return; // Prevent the timer logic from running before the game starts
    }
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                // Reduce time and update UI
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                // Timer finished
                timeRemaining = 0;
                timerRunning = false;
                EndGame();
            }
        }
    }

    private void UpdateTimerUI()
    {
        // Format the time into minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EndGame()
    {
        Debug.Log("EndGame called! Crystals Collected: " + collectedCrystals);

        // Show the end screen UI and record
        if (endScreenUI != null)
            endScreenUI.SetActive(true);

        if (recordText != null)
            recordText.text = $"Time Ended\nCrystals Collected: {collectedCrystals}";
    }

    public void AddCrystal()
    {
        // Increase the crystal count when collected
        collectedCrystals++;
        Debug.Log("Crystal collected! Total: " + collectedCrystals);
    }
    private void StartGame()
    {
        // Hide the start screen UI
        if (startScreenUI != null)
            startScreenUI.SetActive(false);

        // Start the timer
        gameStarted = true;
        timerRunning = true;
    }

}
