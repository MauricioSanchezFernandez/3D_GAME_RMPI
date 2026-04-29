using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Keys")]
    public int silverKeys = 0;
    public int goldKeys = 0;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI silverText;
    [SerializeField] TextMeshProUGUI goldText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    // LLAVE PLATA
    public void AddSilverKey()
    {
        silverKeys++;
        UpdateUI();
    }

    // LLAVE DORADA
    public void AddGoldKey()
    {
        goldKeys++;
        UpdateUI();
    }

    // CHECKS (MUY ÚTILES)
    public bool HasSilverKeys(int amount)
    {
        return silverKeys >= amount;
    }

    public bool HasGoldKey()
    {
        return goldKeys > 0;
    }

    // UI
    void UpdateUI()
    {
        if (silverText != null)
            silverText.text = silverKeys.ToString();

        if (goldText != null)
            goldText.text = goldKeys.ToString();
    }
}
