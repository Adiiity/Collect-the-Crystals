using UnityEngine;

public class Crystal : MonoBehaviour
{
    [Header("Float Animation")]
    [SerializeField] private float floatHeight = 0.5f;
    [SerializeField] private float floatSpeed = 1f;
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 50f;

    private Vector3 startPosition;

    private void Start()
    {
        // Record the initial position of the crystal
        startPosition = transform.position;
    }

    private void Update()
    {
        // Float up and down around the starting Y position
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate the crystal around its own Y-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the GameTimer about the crystal collection
            GameTimer gameTimer = FindObjectOfType<GameTimer>();
            if (gameTimer != null)
                gameTimer.AddCrystal();

            // Destroy the crystal after collection
            Destroy(gameObject);
        }
    }
}
