using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TitleBar : MonoBehaviour
{
    Window window;
    public Collider2D closeButtonCollider;

    void Awake()
    {
        window = GetComponentInParent<Window>();
    }

    void Update()
    {
        if (closeButtonCollider == null) return;

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (closeButtonCollider.OverlapPoint(mouseWorld) && Input.GetMouseButtonUp(0))
            window.Close();
    }

    void OnMouseDown() => window.OnTitleBarMouseDown();
    void OnMouseUp() => window.OnTitleBarMouseUp();
}
