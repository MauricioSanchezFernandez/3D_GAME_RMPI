using UnityEngine;
using UnityEngine.SceneManagement;

public class WinDoor : MonoBehaviour
{
    [SerializeField] string winSceneName = "WinScene";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.instance == null) return;

        if (GameManager.instance.goldKeys > 0)
        {
            SceneManager.LoadScene(4);
        }
    }
}
