using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtilsForAnimationEvent : MonoBehaviour
{
    [SerializeField] private Player player;

    public void enableShoot()
    {
        PlayerData.Instance.isAiming = true;
    }

    public void disableShoot()
    {
        PlayerData.Instance.isAiming = false;
    }

    public void enableArrow()
    {
        player.initArrow.SetActive(true);
    }
    
    public void disableArrow()
    {
        player.initArrow.SetActive(false);
        disableShoot();
    }

    public void isAttacking()
    {
        PlayerData.Instance.isAttacking = true;
    }

    public void isNotAttacking()
    {
        PlayerData.Instance.isAttacking = false;
    }
    public void disableMovement()
    {
        PlayerData.Instance.canWalk = false;
    }
    public void enableMovement()
    {
        PlayerData.Instance.canWalk = true;
    }
}

