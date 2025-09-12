using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    private static readonly int Chop = Animator.StringToHash("Chop");
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public TreeData tree;
    private Player _player;
    public void Interact(Interactor interactor) 
    {
        if (PlayerData.Instance.currentEquipmentName == "Axe")
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject)
            {
                _player = playerObject.GetComponent<Player>();
                _player.PlayerAnimator.SetBool(Chop, true);
            }
            else
            {
                Debug.Log("Tree Player object not found!");
            }
        }
    }

    public void AnimationEventInteract(Interactor interactor, GameObject interactableGameObject)
    {
        if (PlayerData.Instance.currentEquipmentName == "Axe")
        {
            tree.ApplyDamage();
        }
    }
}
