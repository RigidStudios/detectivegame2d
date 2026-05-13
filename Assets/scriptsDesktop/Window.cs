using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [Header("Window")]
    public string windowTitle = "App";
    public bool startOpen = false;

    [Header("Dragging")]
    public Transform titleBar;
    public Vector2 dragOffset;

    [Header("Entry Colliders")]
    public Collider2D entryLeft;
    public Collider2D entryRight;
    public Collider2D entryBottom;

    [Header("Open/Close Animation")]
    public float openSpeed = 12f;
    public Vector3 closedScale = new Vector3(1f, 0f, 1f); 
    public Vector3 openScale = Vector3.one;

    //priv 
    bool isOpen = false;
    bool isDragging = false;
    Camera mainCam;
    Vector3 targetScale;
    
    public bool IsOpen => isOpen;

    void Awake()
    {
        mainCam = Camera.main;

        // Start closed
        transform.localScale = closedScale;
        gameObject.SetActive(startOpen);
        targetScale = startOpen ? openScale : closedScale;

        if (startOpen) isOpen = true;
    }

    void Update()
    {
        AnimateScale();
        HandleTitleBarDrag();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        isOpen = true;
        targetScale = openScale;
        SetEntryCollidersActive(true);
    }

    public void Close()
    {
        isOpen = false;
        targetScale = closedScale;
        SetEntryCollidersActive(false);
    }

    void AnimateScale()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * openSpeed
        );

        if (!isOpen && Vector3.Distance(transform.localScale, closedScale) < 0.01f)
            gameObject.SetActive(false);
    }

    void HandleTitleBarDrag()
    {
        if (!isDragging) return;

        Vector3 mouse = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = transform.position.z;
        transform.position = mouse + (Vector3)dragOffset;
    }

    public void OnTitleBarMouseDown()
    {
        Vector3 mouse = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = transform.position.z;
        dragOffset = transform.position - mouse;
        isDragging = true;
    }

    public void OnTitleBarMouseUp()
    {
        isDragging = false;
    }

    void SetEntryCollidersActive(bool active)
    {
        if (entryLeft) entryLeft.enabled = active;
        if (entryRight) entryRight.enabled = active;
        if (entryBottom) entryBottom.enabled = active;
    }
}
