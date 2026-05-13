using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apps : MonoBehaviour
{
    [Header("Da Window")]
    public Window targetWindow;
    [Header("Da Clicking")]
    public float doubleClickThreshold = 0.3f;
    [Header("Da Looks & Glamour")]
    public Color colorNormal = Color.white;
    public Color colorHovered = new Color(0.8f, 0.8f, 0.8f);
    public Color colorSelected = new Color(0.6f, 0.8f, 1f);

    //priv
    SpriteRenderer sr;
    float lastClickTime = -1f;
    bool isSelected = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = colorNormal;
    }

    void OnMouseEnter()
    {
        if (!isSelected) sr.color = colorHovered;
    }

    void OnMouseExit()
    {
        if (!isSelected) sr.color = colorNormal;
    }

    void OnMouseDown()
    {
        float timeSinceLast = Time.time - lastClickTime;

        if (timeSinceLast <= doubleClickThreshold)
        {
            OnDoubleClick();
        }
        else
        {
            isSelected = true;
            sr.color = colorSelected;
        }

        lastClickTime = Time.time;
    }

    void OnMouseUpAsButton()
    {
        if (targetWindow != null && targetWindow.IsOpen)
        {
            isSelected = false;
            sr.color = colorNormal;
        }
    }

    void OnDoubleClick()
    {
        isSelected = false;
        sr.color = colorNormal;

        if (targetWindow != null)
            targetWindow.Open();
    }
}
