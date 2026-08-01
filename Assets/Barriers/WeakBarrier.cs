using UnityEngine;

public class WeakBarrier : MonoBehaviour, ISongResponsive
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

    private void Update()
    {
        if (isShaking)
        {
            TakeDamage(damagePerSecond * Time.deltaTime);
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = originalPosition + randomOffset;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * returnSpeed);
        }

        isShaking = false;
    }

    private void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Debug.Log($"A barreira {gameObject.name} foi destruída pela onda sonora!");
            Destroy(gameObject);
        }
    }

    public void OnSongListening(Song song)
    {
        isShaking = true;
    }
}
