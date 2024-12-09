using UnityEngine;

public class TokenBlinker : MonoBehaviour
{
    public float pulseSpeed = 2.0f; // Speed of the pulsating effect
    public float maxScale = 1.2f;  // Maximum scale factor
    public float minScale = 0.8f;  // Minimum scale factor
    private bool isScaling = false; // Whether the token is currently scaling
    private Vector3 originalScale;  // Original scale of the token

    private void Start()
    {
        originalScale = transform.localScale; // Store the original scale
    }

    public void StartScaling()
    {
        if (!isScaling)
        {
            isScaling = true;
            StartCoroutine(ScaleEffect());
        }
    }

    public void StopScaling()
    {
        isScaling = false;
        StopAllCoroutines();
        transform.localScale = originalScale; // Reset to original size
    }

    private System.Collections.IEnumerator ScaleEffect()
    {
        float scaleDirection = 1.0f; // 1 for growing, -1 for shrinking
        float currentScale = 1.0f;

        while (isScaling)
        {
            // Adjust the scale based on direction
            currentScale += scaleDirection * pulseSpeed * Time.deltaTime;

            // Switch directions if limits are reached
            if (currentScale >= maxScale)
            {
                currentScale = maxScale;
                scaleDirection = -1.0f;
            }
            else if (currentScale <= minScale)
            {
                currentScale = minScale;
                scaleDirection = 1.0f;
            }

            // Apply the new scale
            transform.localScale = originalScale * currentScale;

            yield return null; // Wait for the next frame
        }
    }
}
