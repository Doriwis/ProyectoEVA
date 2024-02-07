using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gancho : MonoBehaviour
{
   

    [Header("Movimiento")]
    public Vector2 destino;
    public float angulo;

   [SerializeField] bool touch=false;
    [SerializeField]float velocy;
    [SerializeField]float limitRadio;

    [Header("Area Limite ")]
    [SerializeField] Transform area;
    [SerializeField] Transform punto;

    [Header("Limite de distancia")]
   [SerializeField] float origidistan;
   [SerializeField] float actDista;
   [SerializeField] float recDistan;


    [Header("Cadena")]
   Transform spawMano;
    float cotaDista;
    [SerializeField] GameObject cadeO;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Nazco");

        origidistan = Vector2.Distance(transform.position, destino);
        //spawn cadena 
        GameObject shit = GameObject.FindGameObjectWithTag("SpawnMano");
        spawMano = shit.GetComponent<Transform>();
        cotaDista = Vector3.Distance(spawMano.position, transform.position);
        //areas visuales
        //area = GameObject.FindGameObjectWithTag("RadioLimite ").GetComponent<Transform>();
        //area.localScale = new Vector3(limitRadio, limitRadio, limitRadio);

        punto = GameObject.FindGameObjectWithTag("Point").GetComponent<Transform>();
        punto.position= destino;

        StartCoroutine(Mover());


       

       
        
    }
    
    IEnumerator Mover()
    {
        Debug.Log("Entro corrutina");
        while (!touch)
        {
            actDista = Vector3.Distance(transform.position, destino);
            recDistan = origidistan - actDista;
            Debug.Log("Me muevo a alante ");
            if (actDista>0.3 && recDistan<limitRadio)
            {
                if (transform.localScale.x > 0)
                {
                    transform.Translate(Vector2.right * velocy * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.left * velocy * Time.deltaTime);
                }

                //calcular distacncia

                if (Vector3.Distance(spawMano.position, transform.position) >= cotaDista + 0.55f)
                {
                    GameObject.Instantiate(cadeO, spawMano.position, Quaternion.Euler(0, 0, angulo), transform);
                    cotaDista += 0.55f;
                }
            }
            else
            {
                touch = true;
            }
            
            yield return null;
        }
        while (touch)
        {
            Debug.Log("Me muevo a atras ");
            if (transform.localScale.x > 0)
            {
                transform.Translate(Vector2.left * velocy * Time.deltaTime);
            }
            else
            {
                 transform.Translate(Vector2.right * velocy * Time.deltaTime);
            }
            

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
