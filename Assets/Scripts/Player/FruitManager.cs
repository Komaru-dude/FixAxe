using UnityEngine;
using TMPro;

public class FManager : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    void Start()
    {
        LoadAndUpdateText();
    }

    public void LoadAndUpdateText()
    {
        string value = PlayerPrefs.GetInt("fruits", 0).ToString();
        textUI.text = value;
    }
}