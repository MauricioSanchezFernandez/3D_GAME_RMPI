using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int silverKeys = 0;
    public int goldKeys = 0;

    public GameObject goldKeyObject;

    public TextMeshProUGUI keyText;
    public GameObject messageText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
        messageText.SetActive(false);
        goldKeyObject.SetActive(false);
    }

    public void AddSilverKey()
    {
        silverKeys++;
        UpdateUI();

        if (silverKeys >= 2)
        {
            goldKeyObject.SetActive(true); // aparece la dorada
        }
    }

    public void AddGoldKey()
    {
        goldKeys++;
        UpdateUI();

        if (goldKeys >= 1)
        {
            messageText.SetActive(true);
        }
    }

    void UpdateUI()
    {
        int totalKeys = silverKeys + goldKeys;
        keyText.text = "Llaves: " + totalKeys + "/3";
    }

    public bool HasAllKeys()
    {
        return silverKeys >= 2 && goldKeys >= 1;
    }
}