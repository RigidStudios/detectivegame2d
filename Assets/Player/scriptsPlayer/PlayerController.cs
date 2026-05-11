using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    [Header("movement")]
    public float maxSpeed;
    public float acceleration;
    public float friction; 
    public float airControl; 

    [Header("gravity")]
    public float gravityScale;  
    public float maxFallSpeed;

    public VolumeNotch attachedNotch;
    public bool isRidingNotch = false;
    public float notchRidingOffsetY = 0.5f;
    public Vector2 Velocity => rb.velocity; public bool IsGrounded => isGrounded;


    //priv
    Rigidbody2D rb;


    bool isGrounded;
    bool isClimbing;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate()
    {
        ApplyGravity();
        HandleHorizontal();

        rb.velocity = new Vector2(
            Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
            Mathf.Clamp(rb.velocity.y, -maxFallSpeed, maxFallSpeed)
        );
        if (isRidingNotch && attachedNotch.isDragging)
        {
            transform.position = new Vector3 (transform.position.x, 
                attachedNotch.transform.position.y + notchRidingOffsetY, 
                transform.position.z);
        }

    }

    void HandleHorizontal()
    {
        float input = Input.GetAxisRaw("Horizontal");
        float control = isGrounded ? 1f : airControl;

        //accel
        if (Mathf.Abs(input) > 0.01f)
        {
            float accel = acceleration * control * input * Time.fixedDeltaTime;
            rb.velocity = new Vector2(rb.velocity.x + accel, rb.velocity.y);
        }
        else
        //friction
        {
            float frict = friction * control * Time.fixedDeltaTime;
            rb.velocity = new Vector2(
                Mathf.MoveTowards(rb.velocity.x, 0f, frict),
                rb.velocity.y
            );
        }
    }

    void ApplyGravity()
    {
        if (isClimbing) return;

        float g = Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + g);
    }

    //grounded
    void OnCollisionEnter2D(Collision2D col)
    {
        CheckGrounded(col, true);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        CheckGrounded(col, true);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        CheckGrounded(col, false);
    }

    void CheckGrounded(Collision2D col, bool entering)
    {
        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = entering;
                return;
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        print("riding notch");
        if ((other.GetComponentInChildren<VolumeNotch>() != null))
        isRidingNotch = true;
    }
    private void OnTriggerExit2D (Collider2D other)
    {
        if ((other.GetComponentInChildren<VolumeNotch>() != null))
        isRidingNotch = false;
    }
}

