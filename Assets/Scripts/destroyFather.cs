using UnityEngine;

public class destroyFather : MonoBehaviour
{

    public void kaboom()
    {
        Destroy(transform.parent.gameObject);
    }
}
