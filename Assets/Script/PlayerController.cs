using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float hungerDecreaseRate = 10f;
    [SerializeField] private float hungerRecoverAmount = 20f;
    private float currentHunger;
    private Rigidbody2D rb;
    [SerializeField] private UnityEngine.UI.Slider staminaBar;
    private Vector2 movementInput; // Store input from Input System

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHunger = maxHunger;
        UpdateHungerUI();
    }

    void Update()
    {
        // player movement
        Vector2 movement = movementInput.normalized * moveSpeed;
        rb.linearVelocity = movement;

        // hunger reduce speed
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerUI();

        if (currentHunger <= 0)
        {
           // Degeneration();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // restore stamina
        if (other.CompareTag("SmallFish"))
        {
            currentHunger += hungerRecoverAmount;
            currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
            Destroy(other.gameObject);
            UpdateHungerUI();
        }
    }

    private void UpdateHungerUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentHunger / maxHunger;
        }
        Debug.Log(currentHunger);

    }

    private void Degeneration()
    {
        Time.timeScale = 0;
        Debug.Log("Out of Hunger bar, Start Degeneration");
    }
}
