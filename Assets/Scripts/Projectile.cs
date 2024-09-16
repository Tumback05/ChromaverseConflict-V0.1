using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float launchSpeed;
    public Vector2 launchAngle;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private GameObject skeletal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Accelerate();
    }

    void Accelerate()
    {
        skeletal = GameObject.Find("Skeletal");
        if (skeletal.transform.localScale.x > 0)
        {
            rb.AddForce(launchAngle * launchSpeed);
            return;
        }
        rb.AddForce(new Vector2(-launchAngle.x, launchAngle.y) * launchSpeed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (col.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(1, gameObject.transform.position.x);
        }
    }

    private void OnEnable()
    {
        ColorManager.Instance.OnColorChanged += HandleColorChange;
    }

    private void HandleColorChange(string newColor)
    {
        switch (newColor)
        {
            case "blue":
                sr.color = PublicVariables.blue;
                break;
            case "purple":
                sr.color = PublicVariables.purple;
                break;
            case "red":
                sr.color = PublicVariables.red;
                break;
            case "orange":
                sr.color = PublicVariables.orange;
                break;
            case "yellow":
                sr.color = PublicVariables.yellow;
                break;
            case "green":
                sr.color = PublicVariables.green;
                break;
            default:
                sr.color = PublicVariables.blue;
                break;
        }
    }
}
