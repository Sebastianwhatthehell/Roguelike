using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 2f; // Not sure if I should state 2f or leave it blank cause you can set it up in inspector anyhow. So 2 is default now I guess

    private InputAction moveAction;   // reference to the Move action
    private Vector2 moveInput;

    void Awake()
    {
        if (Instance !=null && Instance != this){
            Destroy(this);
            } else {
            Instance = this;
            }
            
            
        // Get the current default input actions (from the PlayerInput component instead of old system)
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions.FindAction("Move");
        }
     
    }

    void OnEnable()
    {
        moveAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
    }

    void Update()
    {
        // Read Vector2 value from the Move action?
        moveInput = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;

        // normalize for consistent speed in diagonal movement (instead of square it becomes round)
        if (moveInput.sqrMagnitude > 1f)
            moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        // Apply movement using Rigidbody2D physics (always in FixedUpdate and not in Update)
        rb.linearVelocity = moveInput * moveSpeed;
    }

}
