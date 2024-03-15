using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BookShaderManager : MonoBehaviour
{
    public Material material;
    public float fadeSpeed;
    public float breathDuration = 1;
    private float Override = 0;
    private float Dissolve = 1;
    private Coroutine deselectCoroutine;
    private bool handleSelectCalledThisFrame = false;

    private void Start()
    {
        material.SetFloat("_EmissionOveride", 0);
        material.SetFloat("_DissolveAmount", 1);
    }
    public void HandleSelect()
    {
        Override = Mathf.Lerp(Override, 0.9f, 0.2f);
        material.SetFloat("_EmissionOveride", Override);

        float dissolveFactor = Mathf.Sin(Time.time / breathDuration * 2 * Mathf.PI);

        // Map the sine wave values from (-1 to 1) to (0.5 to 1)
        Dissolve = Mathf.Lerp(0.3f, 1f, (dissolveFactor + 1f) / 2f);

        // Set the dissolve amount in the material
        material.SetFloat("_DissolveAmount", Dissolve);
        handleSelectCalledThisFrame = true;
        if (deselectCoroutine != null)
        {
            StopCoroutine(deselectCoroutine);
        }

        // Start a new coroutine to delay the OnDeselect call
        deselectCoroutine = StartCoroutine(DelayedDeselect());

    }
    public void OnDeselect()
    {
        handleSelectCalledThisFrame = false;
        if (Override != 0 || Dissolve != 1)
        {
            Override = Mathf.MoveTowards(Override, 0, 2f * Time.deltaTime);
            Dissolve = Mathf.MoveTowards(Dissolve, 1, 2f * Time.deltaTime);
            material.SetFloat("_EmissionOveride", Override);
            material.SetFloat("_DissolveAmount", Dissolve);
        }
    }
    private IEnumerator DelayedDeselect()
    {
        yield return new WaitForSeconds(0.05f); // Adjust the delay time as needed

        // Call OnDeselect after the delay
        OnDeselect();
    }
    private void Update()
    {
        if (!handleSelectCalledThisFrame)
        {
            OnDeselect();
        }
    }
}
