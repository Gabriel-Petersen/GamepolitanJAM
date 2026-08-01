using UnityEngine;

public class enemyDamageArea : MonoBehaviour
{

   
    public float cooldownTime = 1f;
    private float lastUsedTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastUsedTime = -cooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collision)
    {
        if(lastUsedTime + cooldownTime < Time.time)
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
            if (healthSystem != null && healthSystem.isPlayer)
            {
                healthSystem.ChangeHp(-1);
            }
            lastUsedTime = Time.time;
        }
        
    }
}
