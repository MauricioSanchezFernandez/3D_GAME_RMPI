using UnityEngine;

public class GoldenKey : MonoBehaviour
{
    public GameObject door2;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddGoldKey();
            Destroy(gameObject);
            door2.SetActive(true);

        }
    }
}
