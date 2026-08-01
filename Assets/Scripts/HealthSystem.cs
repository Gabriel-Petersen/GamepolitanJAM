using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [Range(1f, 10f)]
    public float maxHp = 3f;
    public float currentHp;
    public bool isPlayer = false;
    public bool blinkWhenDamaged = true;
    public Material damagedMaterial;
    private Material defaultMaterial;
    public float blinkDuration = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        defaultMaterial = GetComponent<Renderer>().material;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHp(float amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        damageBlink();
        Debug.Log(gameObject.name + " HP: " + currentHp);
    }

    public void damageBlink()
    {
        if (blinkWhenDamaged)
        { 
            GetComponent<Renderer>().material = damagedMaterial;
            Invoke("resetMaterial", blinkDuration);
            Debug.Log(gameObject.name + " is blinking due to damage.");
        }
    }

    public void resetMaterial()
    {
        GetComponent<Renderer>().material = defaultMaterial;
    }


}
