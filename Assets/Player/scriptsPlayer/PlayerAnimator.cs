using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;
    PlayerController controller;
    SpriteRenderer sr;

    static readonly int SpeedHash = Animator.StringToHash("Speed");
    static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    static readonly int PullGadgetHash = Animator.StringToHash("PullGadget");
    static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float speed = Mathf.Abs(controller.Velocity.x);
        anim.SetFloat(SpeedHash, speed);

        if (controller.Velocity.x > 0.1f) sr.flipX = false;
        else if (controller.Velocity.x < -0.1f) sr.flipX = true;

        anim.SetBool(IsGroundedHash, controller.IsGrounded);

        if (Input.GetKeyDown(KeyCode.E))
            anim.SetTrigger(PullGadgetHash);
    }


    public void TriggerDeath()
    {
        anim.SetBool(IsDeadHash, true);
    }
}
