using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rayo : MonoBehaviour
{
    [SerializeField] GameObject prefabParteRayo;
    [SerializeField] Transform spawnRayo;
    [SerializeField] LayerMask suelo;
    [SerializeField] float radio;
    // Start is called before the first frame update
    void Start()
    {
        NuevoRayo();
    }
    void OnDrawGizmos()
    { //Detec duelo
        Gizmos.DrawSphere(spawnRayo.position, radio);


    }
    void   NuevoRayo()
    {
        bool cesta = Physics2D.OverlapCircle(spawnRayo.position, radio, suelo);
        if (!cesta)
        {
            
            Instantiate(prefabParteRayo, spawnRayo.position, Quaternion.identity);
            Debug.LogWarning("PUEDO SPAUN HIJO");
        }
    }
}

  
