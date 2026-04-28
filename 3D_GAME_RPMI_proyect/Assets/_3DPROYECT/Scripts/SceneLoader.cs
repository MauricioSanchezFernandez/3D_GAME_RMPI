using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string NameScene; //nombre esecena a cargar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(NameScene))
            {
                SceneManager.LoadScene(NameScene);
            }
            else
            {
                Debug.LogWarning("No se ha asignado el nombre de la escena en ChangeSceneOnTrigger.");
            }
        }

    }
}
