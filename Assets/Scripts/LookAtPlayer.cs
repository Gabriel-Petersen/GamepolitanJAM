using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Camera pc;
    Vector3 direction;
    void Start()
    {
         pc = FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = pc.gameObject.transform.position - transform.position;

        // Only look at the player on the Y axis (horizontal), keep X axis fixed
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(transform.eulerAngles.x, 0, 0);
    }
}
