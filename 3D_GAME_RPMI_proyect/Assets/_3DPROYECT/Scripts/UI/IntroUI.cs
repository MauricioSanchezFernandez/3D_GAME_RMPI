using UnityEngine;
using UnityEngine.InputSystem;

public class IntroUI : MonoBehaviour
{
    [SerializeField] GameObject introPanel;

    void Start()
    {
        introPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        // Si pulsas Enter
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            OnContinue();
        }
    }

    public void OnContinue()
    {
        introPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
