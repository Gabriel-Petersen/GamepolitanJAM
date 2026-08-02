using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private Slider healthBar;
    private Renderer damage_renderer;

    public float maxHp = 3f;
    public float currentHp;

    [Space(10)]
    public bool blinkWhenDamaged = true;
    public Material damagedMaterial;
    private Material defaultMaterial;
    public float blinkDuration = 0.1f;
    public Color defaultSpriteColor = Color.white;
    public Color damagedSpriteColor = Color.red;
    private bool isSpriteRenderer = false;

    //definir cor apenas se o alvo for um sprite renderer
    //definir material apenas se o alvo for um objeto 3D
    void Start()
    {
        currentHp = maxHp;
        healthBar.maxValue = maxHp;
        healthBar.value = currentHp;

        if (damage_renderer == null)
            damage_renderer = GetComponentInChildren<Renderer>();
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
        if (healthBar != null)
            healthBar.value = currentHp;
        DamageBlink();
        Debug.Log(gameObject.name + " HP: " + currentHp);

        if (currentHp <= 0)
        {
            Debug.Log(gameObject.name + " has died.");
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    public void DamageBlink()
    {
        if (blinkWhenDamaged)
        {
            ChangeMaterial(damagedMaterial, damagedSpriteColor);
          
            Invoke(nameof(ResetMaterial), blinkDuration);
            Debug.Log(gameObject.name + " is blinking due to damage.");
        }
    }

    public void ResetMaterial()
    {
        ChangeMaterial(defaultMaterial, defaultSpriteColor);
    }


    
    private void ChangeMaterial(Material newMaterial = null, Color newColor = default)
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
