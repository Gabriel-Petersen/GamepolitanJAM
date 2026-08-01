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
        GameObject obj = collision.gameObject;
        Debug.Log("Collision detected with: " + obj.name);
        HealthSystem healthSystem = obj.GetComponent<HealthSystem>();
        if (healthSystem != null && obj.CompareTag("Player"))
        {
            healthSystem.ChangeHp(-1);
        }
    }
}
