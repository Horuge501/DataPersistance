using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameManager : MonoBehaviour
{
    public TMP_InputField nameInputField;

    private void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "");
        nameInputField.text = playerName;
    }

    public void SavePlayerName()
    {
        string playerName = nameInputField.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
    }
}
