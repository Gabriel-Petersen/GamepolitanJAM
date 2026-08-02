using UnityEngine;

public class WaveTorusEffect : MonoBehaviour
{
    private Vector3 originPosition;
    private float maxRadius;
    private float totalDuration;
    private float elapsed = 0f;
    private Vector3 travelDirection;

    public void Initialize(Vector3 origin, Vector3 direction, float targetRadius, float duration)
    {
        originPosition = origin;
        maxRadius = targetRadius;
        totalDuration = 3*duration;
        travelDirection = direction.normalized;

        transform.position = originPosition;
        transform.localScale = Vector3.zero;

        if (travelDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(travelDirection);
            // Se o modelo do toro estiver modelado deitado por padrão no eixo Z/Y, 
            // descomente a linha abaixo para girá-lo em 90 graus para ficar em pé:
            // transform.Rotate(90f, 0f, 0f);
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsed / totalDuration);

        float currentRadius = Mathf.Lerp(0f, maxRadius, progress);
        float currentScale = currentRadius * 2f;
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        transform.position = originPosition + travelDirection * currentRadius;

        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }
}