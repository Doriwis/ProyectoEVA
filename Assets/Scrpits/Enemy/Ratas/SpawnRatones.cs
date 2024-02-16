using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRatones : MonoBehaviour
{

    // VA EN el espacio de la camara
    // only sent if one of the colliders also has a rigidbody attached. (el trigger stay)
    //la corrutina se activa cuando  el rango de la camara esta en contacto con el spawn
    [SerializeField] GameObject prefabRats;
    bool estaEnRango = false;
    [SerializeField]int limtRats;
    Transform trSpawn;
    

    //poner los spawns //PONER TAG A TODOS LOS SPAWNS



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CorrutinaSalidaRatones());
        //rangoCamara = GetComponent<Rigidbody2D>();
        //position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator CorrutinaSalidaRatones()
    {
        while (true)
         {
            if (estaEnRango&&limtRats>0) //mirar si en el rango de la camara del player hay algun spawn
            {
                limtRats--;
                Instantiate(prefabRats, trSpawn.position, Quaternion.identity); //poner la posicion de los spawns que devuelva el trigger stay
            }
            yield return new WaitForSeconds(2);
        }

    }
}
