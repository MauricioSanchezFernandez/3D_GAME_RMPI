using UnityEngine;

public class KeyFloat : MonoBehaviour
{
    public float amplitude = 0.3f;
    public float speed = 2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, offset, 0);
    }
}
