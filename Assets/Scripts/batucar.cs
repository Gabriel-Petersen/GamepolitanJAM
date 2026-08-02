using UnityEngine;

public class batucar : MonoBehaviour
{
    public EnemyDamageArea eda;
    void Batuque()
    {
        eda.StartAttack();
    }
}
