using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button creditButton;

    private UnityAction action;

    private void Start()
    {
        action = () => OnButtonClick(startButton.name);
        startButton.onClick.AddListener(action);

        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        creditButton.onClick.AddListener(() => OnButtonClick(creditButton.name));
    }

    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button!!!: : {msg}");
        SceneManager.LoadScene("Play");
    }
}
