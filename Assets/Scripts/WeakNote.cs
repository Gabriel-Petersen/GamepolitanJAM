using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Rendering;

public class WeakNote : MonoBehaviour
{

    public Vector3 velocity = Vector3.zero;
    public WeakSong weakSong;
    public float speed;
    
    void Start()
    {
         
    }
  
    void Update()
    {
        if(weakSong == null) { return; }

        transform.position += (velocity * speed * Time.deltaTime) ;

        if(Vector3.Distance(transform.position, weakSong.transform.position) > weakSong.currentRadius)
        {
            Destroy(gameObject);
        }
        
    }
}
