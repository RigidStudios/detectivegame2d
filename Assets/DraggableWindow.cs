using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableWindow : MonoBehaviour
{
    Vector3 hitOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Camera.current.ScreenToWorldPoint(Input.mousePosition);
        print(mousePos);
        hitOffset = transform.position - mousePos;
    }

    private void OnMouseUp()
    {
        
    }
}
