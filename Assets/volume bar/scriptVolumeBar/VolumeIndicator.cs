using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeIndicator : MonoBehaviour
{
    [Header("EQ Bar")]
    public Transform[] eqBars;
    public float barMaxHeight;

    [Header("Bar Spring")]
    public float springStrength;
    public float damping;
    public float retargetRate;

    [Header("Gradient")]
    public Gradient barGradient;

    [Header("Mask")]
    public SpriteMask revealMask;
    public float maskFullHeight;

    //dont touch
    float revealAmount = 0f;
    float[] current;
    float[] targets;
    float[] velocities;
    float retargetTimer;
    //dont touch this either
    SpriteRenderer[] barRenderers;

    void Awake()
    {
        int n = eqBars.Length;
        current = new float[n];
        targets = new float[n];
        velocities = new float[n];
        barRenderers = new SpriteRenderer[n];

        for (int i = 0; i < n; i++)
        {
            current[i] = 0.1f;
            targets[i] = 0.1f;
            barRenderers[i] = eqBars[i].GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        AnimateEQBars();
        UpdateRetarget();
    }

    public void RevealAmount(float volume)
    {
        revealAmount = volume;
        if (revealMask != null)
        {
            Vector3 s = revealMask.transform.localScale;
            s.y = Mathf.Lerp(0f, maskFullHeight, volume);
            revealMask.transform.localScale = s;
        }
    }
    void AnimateEQBars()
    {
        for (int i = 0; i < eqBars.Length; i++)
        {
            float scaledTarget = targets[i] * revealAmount;
            velocities[i] += (scaledTarget - current[i]) * springStrength;
            velocities[i] *= damping;
            current[i] = Mathf.Clamp01(current[i] + velocities[i]);

            Vector3 scale = eqBars[i].localScale;
            scale.y = Mathf.Max(0.05f, current[i] * barMaxHeight);
            eqBars[i].localScale = scale;

            if (barRenderers[i] != null)
                barRenderers[i].color = barGradient.Evaluate(current[i]);
        }
    }

    void UpdateRetarget()
    {
        retargetTimer -= Time.deltaTime;
        if (retargetTimer > 0f) return;

        retargetTimer = retargetRate + Random.Range(-0.03f, 0.03f);
        for (int i = 0; i < targets.Length; i++)
            targets[i] = Random.Range(0.05f, 1f);
    }
}