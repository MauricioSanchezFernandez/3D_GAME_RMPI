using UnityEngine;
using System.Collections;

public class CastleMessage : MonoBehaviour
{
    public GameObject mensajeTemporal;   // Este desaparece
    public GameObject mensajeFijo;       // Este se queda

    public float duracion = 10f;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            activado = true;
            StartCoroutine(MostrarMensajes());
        }
    }

    IEnumerator MostrarMensajes()
    {
        // Mensaje que desaparece
        mensajeTemporal.SetActive(true);

        // Mensaje permanente
        mensajeFijo.SetActive(true);

        yield return new WaitForSeconds(duracion);

        mensajeTemporal.SetActive(false);

        // mensajeFijo NO se apaga
    }
}

