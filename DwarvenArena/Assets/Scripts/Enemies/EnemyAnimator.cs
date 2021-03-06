using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    protected Animator animator;

    public string runAnimTrigger = "Run";
    public string attackAnimTrigger = "Attack";
    public string staggerAnimTrigger = "Stagger";

    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnAttackAnimation()
    {
        //animator.SetTrigger(attackAnimTrigger);
        animator.Play("Attack");
    }

    public void OnGetHitAnimation()
    {
        //animator.SetTrigger(staggerAnimTrigger);
        animator.Play("Hit");
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
