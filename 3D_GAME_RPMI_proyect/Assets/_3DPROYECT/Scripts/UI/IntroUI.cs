using UnityEngine;
using UnityEngine.InputSystem; // IMPORTANTE (nuevo input system)

public class IntroUI : MonoBehaviour
{
    public GameObject panelIntro;

    void Start()
    {
        Time.timeScale = 0f; // pausa el juego
        panelIntro.SetActive(true);
    }

    void Update()
    {
        if (Time.timeScale == 0f && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            OnContinue();
        }
    }

    public void OnContinue()
    {
        panelIntro.SetActive(false);
        Time.timeScale = 1f;
    }
}
