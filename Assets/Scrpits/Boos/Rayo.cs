using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rayo : MonoBehaviour
{
    [SerializeField] float dano;
    [SerializeField] bool candamge;
    [SerializeField] GameObject prefabParteRayo;
    [SerializeField] Transform spawnRayo;
    [SerializeField] LayerMask suelo;
    [SerializeField] LayerMask laser;
    [SerializeField] float radio;
    [SerializeField] float dist;
    [SerializeField] Transform lateral1;
    [SerializeField] Transform lateral2;
    [SerializeField] bool primero;
    // Start is called before the first frame update
    void Start()
    {
        NuevoRayo();
    }
    void Update()
    {
        MeDestruyo();
        RestauroRayo();
    }
    void OnDrawGizmos()
    { //Detec duelo
        Gizmos.DrawSphere(spawnRayo.position, radio);


    }
    void MeDestruyo()
    {
        RaycastHit2D cesta = Physics2D.Raycast(lateral1.position, Vector2.down, dist, suelo);
        Debug.DrawRay(lateral1.position, Vector2.down * dist, Color.red);
        if (cesta)
        {
            Destroy(this.gameObject);

        }
        RaycastHit2D cesta2 = Physics2D.Raycast(lateral2.position, Vector2.down, dist, suelo);
        Debug.DrawRay(lateral2.position, Vector2.down * dist, Color.red);
        if (cesta2)
        {
            Destroy(this.gameObject);

        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("ENTÓ ALGUIEN");
        if (collision.gameObject.CompareTag("Player") && candamge)
        {
            Debug.LogWarning("esplayer");
            collision.gameObject.GetComponent<Player>().RecibirDano(dano);
            StartCoroutine(Esperaycambio());
        }
    }
    IEnumerator Esperaycambio()
    {
        candamge = false;
        yield return new WaitForSeconds(2);
        candamge = true;

    }
    void   NuevoRayo()
    {
        bool cesta = Physics2D.OverlapCircle(spawnRayo.position, radio, suelo);
        if (!cesta)
        {
            
            GameObject aux = Instantiate(prefabParteRayo, spawnRayo.position, Quaternion.identity);
            aux.transform.parent = transform;
            primero = true;
        }
    }
    void RestauroRayo()
    {
        bool cesta = Physics2D.OverlapCircle(spawnRayo.position, radio, suelo);
        if (!cesta)
        {
            Debug.LogWarning("no hay pared");

            bool cesta2 = Physics2D.OverlapCircle(spawnRayo.position, radio, laser);
            if (!cesta2)
            {
                Debug.LogWarning("no hay RAYO");
                GameObject aux = Instantiate(prefabParteRayo, spawnRayo.position, Quaternion.identity);
                aux.transform.parent = transform;
                
            }
            else
            {
                Debug.LogWarning("hay RAYO");
            }
            
        }
        else
        {
            Debug.LogWarning("hay pared");
        }
    }

    

}

  
