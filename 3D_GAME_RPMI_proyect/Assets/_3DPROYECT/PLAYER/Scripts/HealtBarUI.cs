using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Image fillImage;

    private void Start()
    {
        playerHealth.OnHealthChanged += UpdateBar;
    }

    void UpdateBar(float value)
    {
        fillImage.fillAmount = value;
    }
}
