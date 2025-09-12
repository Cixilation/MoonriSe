using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour, IInteractable
{
    private string _noPlantPrompt = "";
    private string _destroyPrompt = "Destroy Plant";
    private string _collectPrompt = "Collect Plant";

    [SerializeField] private PlantController _plantController;

    private void OnEnable()
    {
        if (_plantController != null)
            _plantController.OnPhaseChange += UpdateInteractionPrompt;
    }

    private void OnDisable()
    {
        if (_plantController != null)
            _plantController.OnPhaseChange -= UpdateInteractionPrompt;
    }

    public string InteractionPrompt
    {
        set => _noPlantPrompt = value;
        get
        {
            if (_plantController == null)
                return _noPlantPrompt;

            if (_plantController.IsReadyToCollect)
                return _collectPrompt;

            return _destroyPrompt;
        }
    }

    private void UpdateInteractionPrompt()
    {
        InteractionPromptUI.instance.SetUp(_collectPrompt);
    }

    public void Interact(Interactor interactor)
    {
        if (_plantController == null)
        {
            Debug.Log("No plant to interact with.");
            return;
        }

        if (_plantController.IsReadyToCollect)
        {
            _plantController.CollectPlant();
            RemovePlant();
        }
        else
        {
            RemovePlant();
        }

        GameObject.Find("InteractableUI").SetActive(false);
    }

    private void RemovePlant()
    {
        Debug.Log("Plant collected!");
        _plantController.RemovePlant();
    }

    public void AnimationEventInteract(Interactor interactor, GameObject interactableGameObject)
    {
        // Placeholder for animation event handling
    }
}
