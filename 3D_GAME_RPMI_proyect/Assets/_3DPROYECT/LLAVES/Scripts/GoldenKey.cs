using UnityEngine;

public class GoldenKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddGoldKey();
            Destroy(gameObject);
        }
    }
}
