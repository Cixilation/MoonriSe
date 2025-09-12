using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject _interactionPromptPrefab;
    [SerializeField] private GameObject _interactionTextPrefab;
    public static InteractionPromptUI instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _interactionPromptPrefab.SetActive(false);
    }
    public bool isDisplayed = false;
    public void SetUp(string promptText)
    {
        _interactionPromptPrefab.SetActive(true);
        _interactionTextPrefab.SetActive(true);
        _interactionTextPrefab.GetComponent<TextMeshProUGUI>().text = promptText;
        isDisplayed = true;
    }
    public void Close()
    {
        isDisplayed = false;
        _interactionPromptPrefab.SetActive(false);
        _interactionTextPrefab.SetActive(false);
    }
}