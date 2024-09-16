using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    private float length, startPos;
    public float parallax;

    public GameObject cam;
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = cam.transform.position.x * (1 - parallax);
        float distance = cam.transform.position.x * parallax;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
