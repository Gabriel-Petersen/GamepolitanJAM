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
    public Renderer damage_renderer;
    public Color defaultSpriteColor = Color.white;
    public Color damagedSpriteColor = Color.red;
    private bool isSpriteRenderer = false;

    //definir cor apenas se o alvo for um sprite renderer
    //definir material apenas se o alvo for um objeto 3D
    void Start()
    {
        currentHp = maxHp;
        
        if (damage_renderer == null)
            damage_renderer = GetComponent<Renderer>();
        defaultMaterial = damage_renderer.material;

        
        if (damage_renderer is SpriteRenderer)
        {
            Debug.Log("Sprite renderer detected.");
            isSpriteRenderer = true;
        }
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
            changeMaterial(damagedMaterial, damagedSpriteColor);
          
            Invoke("resetMaterial", blinkDuration);
            Debug.Log(gameObject.name + " is blinking due to damage.");
        }
    }

    public void resetMaterial()
    {
        changeMaterial(defaultMaterial, defaultSpriteColor);
    }


    
    private void changeMaterial(Material newMaterial = null, Color newColor = default(Color))
    {
        //so eh necessario definir cor apenas se o alvo for um sprite renderer
        //so eh necessario definir material apenas se o alvo for um objeto 3D

        if (isSpriteRenderer)
        {
            (damage_renderer as SpriteRenderer).color = newColor;
        }
        else
        {
            damage_renderer.material = newMaterial;
        }
    }
   

}
