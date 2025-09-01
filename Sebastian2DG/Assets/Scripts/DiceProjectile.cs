using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DiceProjectile : MonoBehaviour
{
    [SerializeField] public float maxRange = 1f; 
    private Vector2 startPos;
    private bool tracking;

    public void Launch(Vector2 origin, Vector2 velocity)
    {
        startPos = origin;
        tracking = true;
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }

    void Update()
    {
        if (!tracking) return;

        Vector2 d = (Vector2)transform.position - startPos;
        if (d.sqrMagnitude >= maxRange * maxRange)
            Destroy(gameObject);
    }
}