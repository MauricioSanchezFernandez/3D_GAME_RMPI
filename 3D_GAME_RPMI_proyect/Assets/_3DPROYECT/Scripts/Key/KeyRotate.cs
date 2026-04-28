using UnityEngine;

public class KeyRotate : MonoBehaviour
{
    public float speed = 100f;

    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}