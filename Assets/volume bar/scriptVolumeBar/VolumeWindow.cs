using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VolumeWindow : MonoBehaviour
{
    public VolumeNotch notch;
    public VolumeIndicator volumeIndicator;
    public float volume = 0f;

    public Collider2D entryLeft;
    public Collider2D entryRight;

    // Start is called before the first frame update
    void Start()
    {
        SetVolume(volume);
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
}
