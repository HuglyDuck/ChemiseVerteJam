using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] Animator _animator;

    private void OnEnable()
    {
        DetectPlayer.OnPlayerDied += DetectPlayer_OnPlayerDied;
        FocusLight._atchoum += FocusLight__atchoum;
    }

    private void OnDisable()
    {
        DetectPlayer.OnPlayerDied -= DetectPlayer_OnPlayerDied;
        FocusLight._atchoum -= FocusLight__atchoum;
    }

    private void FocusLight__atchoum()
    {
        _animator.SetTrigger("Atchoum");
    }

    private void DetectPlayer_OnPlayerDied()
    {
        _animator.SetTrigger("Dead");
    }

    private void Update()
    {
        _animator.SetFloat("Speed", PlayerMovement.speedPourcentage);
    }
}
