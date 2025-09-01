using UnityEngine;
using UnityEngine.InputSystem;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private DiceRoller2D dicePrefab;
    [SerializeField] private float projectileRange = 12f;
    [SerializeField] private Transform player;
    [SerializeField] private float orbitRadius = 1.5f;
    [SerializeField] private float orbitSpeed = 180f;
    [SerializeField] private float launchSpeed = 5f;

    private DiceRoller2D activeDice;
    private float orbitAngle;

    private enum DiceState { None, Rolling, Stopped }
    private DiceState state = DiceState.None;

    // One action, multiple bindings
    InputAction cast;

    void Awake()
    {
        // Create a button action and add bindings
        cast = new InputAction("Cast", InputActionType.Button);
        cast.AddBinding("<Keyboard>/1");
        cast.AddBinding("<Keyboard>/numpad1");
        cast.AddBinding("<Gamepad>/buttonSouth"); 
    }

    void OnEnable()
    {
        cast.performed += OnCast; // fires once per press
        cast.Enable();
    }

    void OnDisable()
    {
        cast.performed -= OnCast;
        cast.Disable();
    }

    void OnCast(InputAction.CallbackContext _)
    {
        if (player == null) return;

        if (state == DiceState.None)        SpawnDice();
        else if (state == DiceState.Rolling) StopDice();
        else                                 LaunchDice();
    }

    void Update()
    {
        if (state == DiceState.Rolling && activeDice != null && player != null)
        {
            if (!activeDice.IsRolling) activeDice.Roll();

            orbitAngle += orbitSpeed * Time.deltaTime;
            Vector3 offset = new Vector3(
                Mathf.Cos(orbitAngle * Mathf.Deg2Rad),
                Mathf.Sin(orbitAngle * Mathf.Deg2Rad),
                0f
            ) * orbitRadius;

            activeDice.transform.position = player.position + offset;
        }
    }

    void SpawnDice()
    {
        if (player == null) return;

        activeDice = Instantiate(dicePrefab, player.position, Quaternion.identity);
        var rb = activeDice.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        state = DiceState.Rolling;
        orbitAngle = 0f;
        activeDice.Roll();
    }

    void StopDice()
    {
        state = DiceState.Stopped;
        // current Roll() finishes and face stays
    }

    void LaunchDice()
    {
        if (player == null || activeDice == null) { state = DiceState.None; activeDice = null; return; }

        Vector2 dir = (activeDice.transform.position - player.position).normalized;
        Vector2 vel = dir * launchSpeed;

        var proj = activeDice.GetComponent<DiceProjectile>();
        if (proj == null) proj = activeDice.gameObject.AddComponent<DiceProjectile>();
        proj.maxRange = projectileRange;
        proj.Launch(activeDice.transform.position, vel);

        state = DiceState.None;
        activeDice = null;
    }

    public void SetPlayer(Transform t) { player = t; }
}
