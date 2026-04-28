using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int silverKeys = 0;
    public int goldKeys = 0;

    public TextMeshProUGUI silverText;
    public TextMeshProUGUI goldText;

    public GameObject goldenKey;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddSilverKey()
    {
        silverKeys++;
        UpdateUI();

        if (silverKeys >= 2)
        {
            goldenKey.SetActive(true);
        }
    }

    public void AddGoldKey()
    {
        goldKeys++;
        UpdateUI();
    }

    void UpdateUI()
    {
        silverText.text = silverKeys.ToString();
        goldText.text = goldKeys.ToString();
    }
}
