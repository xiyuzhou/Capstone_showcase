using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimation : MonoBehaviour
{
    public Light lightObject;
    [Range(0.0f, 10.0f)]
    public float duration;

    private float currentTime;
    private float targetIntensity;

    public float minBreathDuration = 1f;
    public float maxBreathDuration = 5f;
    public float minIntensityMultiplier = 1f;
    public float maxIntensityMultiplier = 3f;
    void Start()
    {
        lightObject = GetComponent<Light>();
        SetRandomTargetIntensity();
    }

    void FixedUpdate()
    {
        if (currentTime < duration)
        {
            // Increment time
            currentTime += Time.fixedDeltaTime;
            float lerpValue = currentTime / duration;

            // Lerp between current intensity and target intensity
            lightObject.intensity = Mathf.Lerp(lightObject.intensity, targetIntensity, lerpValue/10f);
        }
        else
        {
            // Reset time and set a new random target intensity
            currentTime = 0f;
            SetRandomTargetIntensity();
        }
    }

    void SetRandomTargetIntensity()
    {
        duration = Random.Range(minBreathDuration, maxBreathDuration);
        targetIntensity = Random.Range(minIntensityMultiplier, maxIntensityMultiplier);
    }
}
