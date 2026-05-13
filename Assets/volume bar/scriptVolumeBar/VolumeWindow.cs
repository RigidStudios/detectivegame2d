using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VolumeWindow : MonoBehaviour
{
    [Header("Dragging")]
    public Transform titleBar;
    Vector2 dragOffset;
    bool isDragging;
    Camera mainCam;

    public VolumeNotch notch;
    public VolumeIndicator volumeIndicator;
    public float volume = 0f;

    public Collider2D entryLeft;
    public Collider2D entryRight;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        SetVolume(volume);
    }

    private void Update()
    {
        HandleTitleBarDrag();
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        volumeIndicator.RevealAmount(volume);
    }
    public bool IsEnterable(Collider2D other)
    {
        return other == entryLeft || other == entryRight;
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
}
