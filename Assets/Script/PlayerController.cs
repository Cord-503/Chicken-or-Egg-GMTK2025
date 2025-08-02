using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float hungerDecreaseRate = 10f;
    [SerializeField] private float hungerRecoverAmount = 20f;

    private float currentHunger;
    private Rigidbody2D rb;
    [SerializeField] private Slider staminaBar;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHunger = maxHunger;
        UpdateHungerUI(); // 保证UI初始化为满格
    }

    void Update()
    {
        // 处理输入
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // 饥饿值下降
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerUI();

        if (currentHunger <= 0)
        {
            Degeneration();
        }
    }

    void FixedUpdate()
    {
        // 应用移动（放在FixedUpdate中控制刚体移动）
        Vector2 movement = movementInput.normalized * moveSpeed;
        rb.linearVelocity = movement;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
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
    }

    private void Degeneration()
    {
        Time.timeScale = 0f;
        Debug.Log("Out of Hunger. Degeneration begins.");
    }
}
