using UnityEngine;

public class BarreiraDestruidaComInimigo : MonoBehaviour
{

    public float velocityBreakThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {

        if(collision.rigidbody.linearVelocity.sqrMagnitude > velocityBreakThreshold * velocityBreakThreshold)
        {
            // throwEnemy is available here if needed
            BreakWall(collision);
        }
        
    }

    public void BreakWall(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<EnemyAi>(out EnemyAi enemy))
        {
            enemy.ThrowEnemyAtBarrier();
        }
        Destroy(gameObject);
    }

}
