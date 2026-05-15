using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [Header("Window + Dragging")]
    public string windowTitle = "App";
    public bool startOpen = false;
    public Transform titleBar;
    public Vector2 dragOffset;

    [Header("Open/Close Animation")]
    public float openSpeed = 12f;
    public Vector3 closedScale = new Vector3(1f, 0f, 1f); 
    public Vector3 openScale = Vector3.one;

    [Header("Platform Snapping")]
    public float snapRange = 1.5f;
    public LayerMask platformLayer;
    public float windowHalfHeight = 1f;

    [Header("9-Slice Scaling")]
    public SpriteRenderer windowBackground;
    public float lockedWidth = 5f;
    public float defaultWidth = 2f;
    public float scaleSpeed = 8f;

    [Header("Interactability")]
    public Collider2D titleBarCollider;
    public Collider2D closeButtonCollider;
    public string playerTag = "Player";

    [Header("Adjust child Width")]
    public Transform[] leftAnchored; 
    public Transform[] rightAnchored;
    public Transform[] centreAnchored;
    public BoxCollider2D rootCollider;


    //priv 
    bool isOpen = false;
    bool isDragging = false;
    bool isLocked = false;
    bool isPlayerInside = false;
    float[] leftOffsets;
    float[] rightOffsets;
    float[] centreWidths;
    float targetWidth;
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
    void Start()
    {
        CacheAnchorOffsets();
        targetWidth = defaultWidth;
        if (windowBackground != null)
            windowBackground.size = new Vector2(defaultWidth, windowBackground.size.y);
    }

    void Update()
    {
        AnimateScale();
        HandleTitleBarDrag();
        UpdateWindowScale();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        isOpen = true;
        targetScale = openScale;
    }

    public void Close()
    {
        isOpen = false;
        targetScale = closedScale;
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
    void UpdateWindowScale()
    {
        if (windowBackground == null) return;

        float currentWidth = windowBackground.size.x;
        float newWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * scaleSpeed);
        windowBackground.size = new Vector2(newWidth, windowBackground.size.y);

        UpdateChildAnchors(newWidth);

        if (rootCollider != null)
        {
            Vector2 size = rootCollider.size;
            size.x = newWidth;
            rootCollider.size = size;
        }
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
        SnapToPlatform();
    }
    void SnapToPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            snapRange,
            platformLayer
        );

        if (hit.collider != null)
        {
            Vector3 snapped = transform.position;
            snapped.y = hit.point.y + windowHalfHeight;
            transform.position = snapped;
            isLocked = true;
            targetWidth = lockedWidth;
        }
        else
        {
            isLocked = false;
            targetWidth = defaultWidth;
        }
    }
    void CacheAnchorOffsets()
    {
        leftOffsets = new float[leftAnchored.Length];
        rightOffsets = new float[rightAnchored.Length];
        centreWidths = new float[centreAnchored.Length];

        for (int i = 0; i < leftAnchored.Length; i++)
            leftOffsets[i] = leftAnchored[i].localPosition.x + (defaultWidth * 0.5f);

        for (int i = 0; i < rightAnchored.Length; i++)
            rightOffsets[i] = rightAnchored[i].localPosition.x - (defaultWidth * 0.5f);

        for (int i = 0; i < centreAnchored.Length; i++)
        {
            BoxCollider2D bc = centreAnchored[i].GetComponent<BoxCollider2D>();
            centreWidths[i] = bc != null ? bc.size.x / defaultWidth : 1f;
        }
    }

    void UpdateChildAnchors(float currentWidth)
    {
        float halfWidth = currentWidth * 0.5f;

        for (int i = 0; i < leftAnchored.Length; i++)
        {
            Vector3 p = leftAnchored[i].localPosition;
            p.x = -halfWidth + leftOffsets[i];
            leftAnchored[i].localPosition = p;
        }
        for (int i = 0; i < rightAnchored.Length; i++)
        {
            Vector3 p = rightAnchored[i].localPosition;
            p.x = halfWidth + rightOffsets[i];
            rightAnchored[i].localPosition = p;
        }

        for (int i = 0; i < centreAnchored.Length; i++)
        {
            Vector3 p = centreAnchored[i].localPosition;
            p.x = 0f;
            centreAnchored[i].localPosition = p;

            BoxCollider2D bc = centreAnchored[i].GetComponent<BoxCollider2D>();
            if (bc != null)
            {
                Vector2 size = bc.size;
                size.x = currentWidth * centreWidths[i];
                bc.size = size;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInside = true;
            SetInteractable(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInside = false;
            SetInteractable(true);
        }
    }

    void SetInteractable(bool interactable)
    {
        if (titleBarCollider != null) titleBarCollider.enabled = interactable;
        if (closeButtonCollider != null) closeButtonCollider.enabled = true;
        if (!interactable) isDragging = false;
    }
}
