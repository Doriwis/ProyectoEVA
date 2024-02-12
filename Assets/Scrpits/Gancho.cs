using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gancho : MonoBehaviour
{

    public  Player scriP;
    [SerializeField]float damage;
    [Header("Movimiento")]
    public Vector2 destino;
    public float angulo;

    [SerializeField]float velocy;
   public float limitRadio;

    [Header("Area Limite ")]
    
    [SerializeField] Transform punto;

    [Header("Limite de distancia")]
   [SerializeField] float origidistan;
   [SerializeField] float actDista;
   [SerializeField] float recDistan;


    [Header("Cadena")]
   Transform spawMano;
    [SerializeField]float cotaDista;
    [SerializeField] GameObject cadeO;
    [SerializeField]List<GameObject> cadenas;
    enum movimiento { avanzar,regresar,transladar,final};
    movimiento miEstado;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("Point").GetComponent<Transform>().position = destino;
        Debug.Log("Nazco");
        miEstado = movimiento.avanzar;
        scriP.saltar = false;
        origidistan = Vector2.Distance(transform.position, destino);
        //spawn cadena 
        GameObject shit = GameObject.FindGameObjectWithTag("SpawnMano");
        spawMano = shit.GetComponent<Transform>();

        cotaDista = Vector3.Distance(spawMano.position, transform.position);
       
        StartCoroutine(Mover());

    }
    
    IEnumerator Mover()
    {
        Debug.Log("Entro corrutina");
        while (miEstado==movimiento.avanzar)
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
                    GameObject cadeAux=GameObject.Instantiate(cadeO, spawMano.position, Quaternion.Euler(0, 0, angulo), transform);
                    cadenas.Add(cadeAux);
                    cotaDista += 0.55f;
                }
            }
            else
            {
                cotaDista = Vector3.Distance(spawMano.position, transform.position);
                miEstado =movimiento.regresar;
            }
            
            yield return null;
        }
        while (miEstado == movimiento.regresar)
        {
            Debug.Log("Me muevo a atras ");

            

            if (Vector3.Distance(transform.position , spawMano.position) >0.3f)
            {
                
                if (transform.localScale.x > 0)
                {
                    transform.Translate(Vector2.left * velocy * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.right * velocy * Time.deltaTime);
                }

                if (Vector3.Distance(spawMano.position, transform.position) <= cotaDista - 0.50f)
                {

                    Debug.LogWarning("elimino cadenas ");
                    Destroy(cadenas[cadenas.Count - 1].gameObject);
                    cadenas.RemoveAt(cadenas.Count - 1);
                    cotaDista -= 0.53f;
                    
                }
            }
            else
            {
                miEstado = movimiento.final;

                scriP.GetComponent<Animator>().SetBool("Gancho", false);
                Morir();
            }
                
            
            

            yield return null;
        }
        if (miEstado == movimiento.transladar)
        {
            StartCoroutine(scriP.IrGancho(velocy));
            while (miEstado == movimiento.transladar)
            {



                if (Vector3.Distance(spawMano.position, transform.position) <= cotaDista - 0.50f)
                {

                    Debug.LogWarning("elimino cadenas ");
                    Destroy(cadenas[cadenas.Count - 1].gameObject);
                    cadenas.RemoveAt(cadenas.Count - 1);
                    cotaDista -= 0.53f;

                }
                yield return null;
            }
        
           
        }
        Debug.Log("Termina corrutina");
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("toco algo ");
        if (collision.gameObject.CompareTag("Suelo"))
        {
            Debug.Log("Toco suelo");
            miEstado = movimiento.regresar;

        }
        else if (collision.gameObject.CompareTag("Catch"))
        {
            miEstado = movimiento.transladar;
            scriP.saltar = true;
            Debug.Log("Toco algo");
            
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            
            Debug.Log("Toco algo");
           // collision.gameObject.GetComponent<Enemy>().RecibirDano(damage);
            miEstado = movimiento.regresar;
        }
    }
    
    public void Morir()
    {
        
        Destroy(this.gameObject);
    }
    
}
