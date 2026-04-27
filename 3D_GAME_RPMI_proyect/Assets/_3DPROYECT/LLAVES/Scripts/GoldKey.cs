using UnityEngine;

public class GoldKey : MonoBehaviour
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
