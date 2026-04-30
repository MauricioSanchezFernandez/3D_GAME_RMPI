using UnityEngine;
using System.Collections;

public class IntroUI : MonoBehaviour
{
    public GameObject panelIntro;
    public float tiempo = 3f; // segundos que dura el texto

    IEnumerator Start()
    {
        Time.timeScale = 0f;
        panelIntro.SetActive(true);

        yield return new WaitForSecondsRealtime(tiempo);

        OnContinue();
    }

    public void OnContinue()
    {
        panelIntro.SetActive(false);
        Time.timeScale = 1f;
    }
}