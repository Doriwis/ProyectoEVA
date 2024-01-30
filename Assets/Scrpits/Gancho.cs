using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    //clone la cadena 
    //se mueva a frond
    [SerializeField]float velocy;
    bool touch=false;
    // se pare cuando toque algo 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Mover());
        Debug.Log("Nazco");
        
        
    }

    // Update is called once per frame
    
    IEnumerator Mover()
    {
        Debug.Log("Entro corrutina");
        while (!touch)
        {
            Debug.Log("Me muevo a alante ");
           transform.Translate(new Vector2(1, 0) * velocy * Time.deltaTime);
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
