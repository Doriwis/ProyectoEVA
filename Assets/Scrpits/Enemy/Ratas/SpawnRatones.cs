using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRatones : MonoBehaviour
{

    // VA EN el espacio de la camara
    // only sent if one of the colliders also has a rigidbody attached. (el trigger stay)
    //la corrutina se activa cuando  el rango de la camara esta en contacto con el spawn
    Rigidbody2D rb;
    [SerializeField] GameObject prefabRats;
    Transform player;
    [SerializeField]int limtRats;
    [SerializeField] float time;

    [SerializeField] bool libre= true;
    [SerializeField] bool detect = false;

    [SerializeField]Transform area;
    [SerializeField]float radio;
    [SerializeField] bool setArea;


    //poner los spawns //PONER TAG A TODOS LOS SPAWNS



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        StartCoroutine(CorrutinaSalidaRatones());
        //rangoCamara = GetComponent<Rigidbody2D>();
        //position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        AreaDetect();
    }

    

    void AreaDetect()
    {
        area.localScale = new Vector2(radio * 2, radio * 2);
        if (setArea)
        {
            area.gameObject.SetActive(true);
        }
        else
        {
            area.gameObject.SetActive(false);
        }

        if (Vector2.Distance(transform.position, player.position) < radio)
        {
            Debug.LogWarning("Estoy en area");
            detect = true;
        }
        else
        {
            detect = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")||collision.gameObject.CompareTag("Enemy"))
        {
            libre = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            libre = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            libre = false;
        }
    }

    IEnumerator CorrutinaSalidaRatones()
    {
        while (true)
         {
            if (libre&&limtRats>0&&detect) //mirar si en el rango de la camara del player hay algun spawn
            {
                limtRats--;
                Instantiate(prefabRats, transform.position, Quaternion.identity); //poner la posicion de los spawns que devuelva el trigger stay
            }

            yield return new WaitForSeconds(time);
        }

    }

}
