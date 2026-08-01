using UnityEngine;

public class WeakBarrier : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float returnSpeed;

    private float currentHealth;
    private Vector3 originalPosition;
    private bool isShaking;

    private void Start()
    {
        currentHealth = maxHealth;
        originalPosition = transform.position;
    }
}
