using UnityEngine;

public class SilverKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddSilverKey();
            Destroy(gameObject);
        }
    }
}
