using UnityEngine;

public class enemyDamageArea : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
        if (healthSystem != null && healthSystem.isPlayer)
        {
            healthSystem.ChangeHp(-1);
        }
    }
}
