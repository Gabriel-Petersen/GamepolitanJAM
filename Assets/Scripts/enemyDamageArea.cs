using UnityEngine;

public class enemyDamageArea : MonoBehaviour
{

   
    public float cooldownTime = 1f;
    private float lastUsedTime = 0f;
    private bool isAttacking = false;
    private bool attackConfirmed = false;
    public float pushForceX = 5f;
    public float pushForceY = 1f;
    public float attackDuration = 0.5f; // Duration of the attack animation or effect
    private float attackStartTime = 0f;
    private MeshRenderer meshr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastUsedTime = -cooldownTime;
        meshr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(isAttacking);
        //iniciar ataque
        if (lastUsedTime + cooldownTime < Time.time && !isAttacking)
        {
            StartAttack();
        }

        if(attackStartTime + attackDuration < Time.time && isAttacking)
        {
            EndAttack();
        }
    }

    void OnTriggerStay(Collider collision)
    {
        Debug.Log("isAttacking: " + isAttacking);
        Debug.Log("attackConfirmed: " + attackConfirmed);
        
        if (isAttacking && !attackConfirmed)
        {
            GameObject obj = collision.gameObject;

            //ataque
            if (obj.CompareTag("Player"))
            {
                Debug.Log("attacking: " + obj.name);
                if (obj.TryGetComponent<HealthSystem>(out var healthSystem))
                {
                    healthSystem.ChangeHp(-1);
                }
                PushAttack(collision);
                attackConfirmed = true;
            }
        }
        
    }

    private void PushAttack(Collider target)
    {
        Vector3 pushDirection = (target.transform.position - transform.position).normalized;
        if(target.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.PushPlayer(pushDirection, pushForceX, pushForceY);
        }
    }

    private void StartAttack(){
        isAttacking = true;

        attackStartTime = Time.time;
        //enable or disable mesh renderer
        //meshr.enabled = true;

    }

    private void EndAttack()
    {
        attackConfirmed = false;
        isAttacking =false;
        lastUsedTime = Time.time;
        //meshr.enabled = false;
    }



}
