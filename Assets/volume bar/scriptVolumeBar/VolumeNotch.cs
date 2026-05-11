using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class VolumeNotch : MonoBehaviour
{
    //public
    [Header("References")]
    public VolumeWindow window;
    public Transform barTop;
    public Transform barBottom;

    [Header("Visuals")]
    public Color colorNormal = new Color(0.35f, 0.35f, 0.35f);
    public Color colorHover = new Color(0.22f, 0.22f, 0.22f);
    public Color colorDrag = new Color(0.15f, 0.15f, 0.15f);

    //priv
    SpriteRenderer sr;
    Camera mainCam;

    public bool isDragging = false;
    bool isHovered = false;
    float dragOffsetY;

    float barMinY;
    float barMaxY;



    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
    }

    void Start()
    {
        RecalculateBounds();
        ApplyVisualState();

        //notch to pos
        float startY = Mathf.Lerp(barMinY, barMaxY, window.volume);
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }
//add 
    public void RecalculateBounds()
    {
        float halfNotch = transform.localScale.y * 0.5f; //stupid bar scaling don't work without this
        barMinY = barBottom.position.y + halfNotch;
        barMaxY = barTop.position.y - halfNotch;
    }

    //mouse settings
    void OnMouseEnter()
    {
        isHovered = true;
        ApplyVisualState();
        print("mouse-enter");
    }

    void OnMouseExit()
    {
        if (!isDragging) isHovered = false;
        ApplyVisualState();
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 worldClick = mainCam.ScreenToWorldPoint(Input.mousePosition);
        dragOffsetY = transform.position.y - worldClick.y;
        ApplyVisualState();
    }

    void OnMouseDrag()
    {
        Vector3 worldMouse = mainCam.ScreenToWorldPoint(Input.mousePosition);
        float targetY = worldMouse.y + dragOffsetY;
        float clampedY = Mathf.Clamp(targetY, barMinY, barMaxY);
        //notch movement down here
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);

        float t = Mathf.InverseLerp(barMinY, barMaxY, clampedY);
        window.SetVolume(t);
    }

    void OnMouseUp()
    {
        isDragging = false;
        isHovered = false;
        ApplyVisualState();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDragging)
        {
            GetComponent<Rigidbody2D>();

        }
    }

    //and dragging indicators
    void ApplyVisualState()
    {
        if (isDragging) sr.color = colorDrag;
        else if (isHovered) sr.color = colorHover;
        else sr.color = colorNormal;
    }
}
