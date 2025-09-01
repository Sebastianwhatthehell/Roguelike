using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;

    private Vector3 direction;
    [SerializeField] private float moveSpeed;
    void FixedUpdate()
    {
        //face the player
        if (PlayerController.Instance.transform.position.x >
        transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        //move towards the player
        direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

