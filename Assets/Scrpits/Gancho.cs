using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gancho : MonoBehaviour
{
   

    [Header("Movimiento")]
   [SerializeField] bool touch=false;

    [SerializeField]float velocy;

    [SerializeField]float limitRadio;
    [SerializeField]Vector2 destino;
    float origidistan;
   [SerializeField] float actDista;

    public float angulo;

    [Header("Cadena")]
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
    
    IEnumerator Mover()
    {
        Debug.Log("Entro corrutina");
        while (!touch)
        {
            actDista = Vector3.Distance(transform.position, destino);

            Debug.Log("Me muevo a alante ");
            if (actDista>0.3||(origidistan-actDista)<limitRadio)
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
    public void OrientarGancho(Vector3 objetivo)
    {
       
        
        float anguloRad = Mathf.Atan2(objetivo.y - transform.position.y, objetivo.x - transform.position.x);
        float anguloReal = ((180 * anguloRad) / Mathf.PI);
        float anguloClamp;

        Debug.Log("ANGULO " + anguloReal);
        if (transform.localScale.x>0)
        {
            Debug.Log("DERECHA");
            if (anguloReal < 60 && anguloReal > -60)
            {
                Debug.Log("DENTRO ENTRE 60 Y -60");
                anguloClamp = anguloReal;
            }
            else if (anguloReal > 60)
            {
                Debug.Log("MAYOR QUE 60");
                anguloClamp = 60;
            }
            else
            {
                Debug.Log("MENOR QUE -60");
                anguloClamp = -60;
            }
        }
        else
        {
            Debug.Log("IZQUIERDA");
            if (anguloReal < -120 && anguloReal > -180|| anguloReal >120 && anguloReal < 180)
            {
                Debug.Log("DENTRO ENTRE 120 Y -120");
                if (anguloReal>0)
                {
                    anguloClamp = (180 - anguloReal)*-1;
                    Debug.Log("POSITIVO Y ABSOLUTO ES " + anguloClamp);
                }
                else
                {
                    
                    anguloClamp = 180+anguloReal;
                    Debug.Log("NEGATVIO Y ABSOLUTO ES " + anguloClamp);
                }
                
            }
            else if (anguloReal > -120&& anguloReal<0)
            {
                Debug.Log("MAYOR QUE -120");
                anguloClamp = 60;
            }
            else
            {
                Debug.Log("MENOR QUE 120");
                anguloClamp = -60;
            }

        }

        angulo = anguloClamp;

        transform.rotation = Quaternion.Euler(0, 0, angulo);
       
        destino = objetivo;

        origidistan = Vector2.Distance(transform.position, destino);
        
    }
    public void Morir()
    {
        
        Destroy(this.gameObject);
    }
}
