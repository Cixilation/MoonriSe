using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Fish : MonoBehaviour, IInteractable
{
    private static readonly int Fish1 = Animator.StringToHash("Fish");
    
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject InteractionPanel;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private GameObject fishingPanel;
    [SerializeField] private AimingCameraLookAt aimingCameraScript;
    public string InteractionPrompt => _prompt;
    
    private Player _player;
    public void Interact(Interactor interactor)
    {
        if (InteractionPanel == null)
        {
            InteractionPanel = GameObject.Find("InteractableUI");
        }
        if (fishingPanel == null)
        {
            fishingPanel = GameObject.Find("UI").transform.Find("FishingPanel").gameObject;
        }
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject && PlayerData.Instance.currentEquipmentName == "Fishing Rod")
        {
            _player = playerObject.GetComponent<Player>();
            _player.PlayerAnimator.SetBool(Fish1, true);
            PlayerData.Instance.canWalk = false;
            StartCoroutine(DelayedActions());
        }
    }

    private void Update()
    {
        if ((FishingMiniGame.instance.GotTheFish || Input.GetKeyDown(KeyCode.Escape)) && FishingMiniGame.instance.isFishing)
        {
            _player.PlayerAnimator.SetBool(Fish1, false);
            fishingPanel.SetActive(false);
            InteractionPanel.SetActive(true);
            camera.gameObject.SetActive(false);
            camera.Priority = 10;
            aimingCameraScript.enabled = true;
        }
    }

    private IEnumerator DelayedActions()
    {
        yield return new WaitForSeconds(2f);
        fishingPanel.SetActive(true);
        InteractionPanel.SetActive(false);
        camera.gameObject.SetActive(true);
        camera.Priority = 100;
        aimingCameraScript.enabled = false;
    }

    public void AnimationEventInteract(Interactor interactor, GameObject interactableGameObject)
    {
        throw new System.NotImplementedException();
    }
}
