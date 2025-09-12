using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    private float _interactionPointRadius = 0.4f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;
    private IInteractable _interactable;
    private GameObject _interactableGameObject;
    private void Update()
    {
        if (_interactionPromptUI == null)
        {
            Transform parent = GameObject.Find("UI").transform;
            _interactionPromptUI = parent.Find("InteractableUI").GetComponent<InteractionPromptUI>();
        }
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
        if (_numFound > 0 && !PlayerData.Instance.builderMode)  
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();
            if (_interactable != null  )
            {
                _interactableGameObject = _colliders[0].gameObject;
                if(!_interactionPromptUI.isDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);
                if (Keyboard.current.fKey.wasPressedThisFrame)
                {
                    _interactable.Interact(this);
                }
            }
        }
        else
        {
            if(_interactable != null) _interactable = null;
            if(_interactionPromptUI.isDisplayed) _interactionPromptUI.Close();
        }
    }
    public void AnimationEventInteractor(GameObject interactingObject)
    {
        if (_interactable != null )
        {
            _interactable.AnimationEventInteract(this, _interactableGameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
