using UnityEngine;
using System.Collections;

public class GoldenKey : MonoBehaviour
{
    public GameObject door2;
    public GameObject mensajeLlave;
    public float duracionMensaje = 5f;

    private bool recogida = false;

    private void OnTriggerEnter(Collider other)
    {
        if (recogida) return;

        if (other.CompareTag("Player"))
        {
            recogida = true;

            GameManager.instance.AddGoldKey();

            door2.SetActive(true);

            StartCoroutine(MostrarMensaje());

            Destroy(gameObject);
        }
    }

    IEnumerator MostrarMensaje()
    {
        mensajeLlave.SetActive(true);

        yield return new WaitForSeconds(duracionMensaje);

        mensajeLlave.SetActive(false);
    }
}
