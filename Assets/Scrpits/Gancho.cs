using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    //clone la cadena 

    [SerializeField]float velocy;
    bool touch=false;
    public float angulo;


   Transform spawMano;
    float cotaDista;
    [SerializeField] GameObject cadeO;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Nazco");

        GameObject shit = GameObject.FindGameObjectWithTag("SpawnMano");
        Debug.Log("" + shit);
        spawMano = shit.GetComponent<Transform>();

        cotaDista = Vector3.Distance(spawMano.position, transform.position);

        StartCoroutine(Mover());
       

       
        
    }

    // Update is called once per frame
    
    IEnumerator Mover()
    {
        Debug.Log("Entro corrutina");
        while (!touch)
        {
            Debug.Log("Me muevo a alante ");
           transform.Translate(new Vector2(1, 0) * velocy * Time.deltaTime);
            //calcular distacncia

            


            if (Vector3.Distance(spawMano.position,transform.position)>= cotaDista+0.55f)
            {
                GameObject.Instantiate(cadeO, spawMano.position, Quaternion.Euler(0, 0, angulo), transform);
                cotaDista += 0.55f;
            }
            yield return null;
        }
        while (touch)
        {
            Debug.Log("Me muevo a atras ");
            transform.Translate(new Vector2(-1, 0) * velocy * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Termina corrutina");
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("toco algo ");
        if (collision.gameObject.CompareTag("Suelo"))
        {
            Debug.Log("Toco suelo");
            touch = true;
        }
        else if (collision.gameObject.CompareTag("Catch"))
        {
            Debug.Log("Toco algo");
            touch = true;
        }
    }
    public void Morir()
    {
        
        Destroy(this.gameObject);
    }
}
