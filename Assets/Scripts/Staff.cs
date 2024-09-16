using UnityEngine;

public class Staff : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponentInParent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
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
