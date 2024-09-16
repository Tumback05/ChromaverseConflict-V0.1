using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MoveSpeed = 5;
    public float maxHealth = 2;
    private float health;

    private Rigidbody2D rb;
    private Vector2 direction = Vector2.left;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void Update()
    {
        transform.Translate(MoveSpeed * Time.deltaTime * direction);
    }

    void Turnaround()
    {
        if (direction == Vector2.left)
        {
            direction = Vector2.right;
        }
        else if (direction == Vector2.right)
        {
            direction = Vector2.left;
        }
    }

    public void TakeDamage(int damage, float playerX)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        float enemyX = transform.position.x;
        if (enemyX > playerX)
        {
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(300, 500));
        }
        else
        {
            rb.AddForce(new Vector2(-300, 500));
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(1, transform.position.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            Turnaround();
        }
    }
}
