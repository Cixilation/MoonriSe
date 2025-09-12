    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    [SerializeField] private GameObject ShopPanel; 
    [SerializeField] private GameObject UIBackground;

    private void Start()
    {
        ShopPanel.SetActive(false);
    }

    public void Interact(Interactor interactor)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShopPanel.SetActive(true);
        UIBackground.SetActive(true);
        PauseGame.instance.Pause();
    }

    public void AnimationEventInteract(Interactor interactor, GameObject interactableGameObject)
    {
        throw new System.NotImplementedException();
    }
}
