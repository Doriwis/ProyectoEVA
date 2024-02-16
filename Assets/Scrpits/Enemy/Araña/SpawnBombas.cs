using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBombas : MonoBehaviour
{
    //va en el trigger del rango de las bombas
    [SerializeField] GameObject prefabBombas;
    bool estaEnRango = false;
    Transform trSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CorrutinaSoltarBombas());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CorrutinaSoltarBombas()
    {
        while (true)
        {
            if (estaEnRango) //mirar si en el rango de la camara del player hay algun spawn
            {
                Instantiate(prefabBombas, trSpawn.position, Quaternion.identity); //poner la posicion de los spawns que devuelva el trigger stay
                yield return new WaitForSeconds(3);
                //Animacion explosion bomba
                ExplosionBomba();
            }
            // yield return null;
        }

    }


    void OnTriggerStay2D(Collider2D collision) //si el rango del player entra en contacto con este spawn
    {
        if (collision.CompareTag("SpawnBombas"))
        {
            trSpawn = collision.GetComponent<Transform>();

            estaEnRango = true;
        }
        else
        {
            estaEnRango = false;
        }
    }

    void ExplosionBomba()
    {
        //animacion explosion bomba
        
    }

}



