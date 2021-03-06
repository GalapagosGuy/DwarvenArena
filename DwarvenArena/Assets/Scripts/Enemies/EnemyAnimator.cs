using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    protected Animator animator;

    public string runAnimTrigger = "Run";
    public string attackAnimTrigger = "Attack";

    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnAttackAnimation()
    {
        animator.SetTrigger(attackAnimTrigger);
    }

    public void OnRunAnimation()
    {
        animator.SetBool(runAnimTrigger, true);
    }

    public void OnStopAnimation()
    {
        animator.SetBool(runAnimTrigger, false);
    }
}
