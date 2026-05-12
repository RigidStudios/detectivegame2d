using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxEffect : MonoBehaviour
{
    public GameObject player;
    public Camera cam;

    //start pos for parralax obj
    Vector2 startPos;
    float startZ;

    //cammove
    Vector2 camMoveSinceStart => (Vector2) player.transform.position - startPos;


    float zDistanceFromTarget => transform.position.z - player.transform.position.z;

    float clippingPlane => (player.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    void Start()
    {
        startPos = transform.position;
        startZ = transform.position.z;
    }

    void Update()
    {
        Vector2 newPos = startPos + camMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
