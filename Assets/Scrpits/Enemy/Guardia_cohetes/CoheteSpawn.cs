using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoheteSpawn : MonoBehaviour
{
    
    //va en el rango de la camara 
    [SerializeField] GameObject prefabCohetes;
    bool estaEnRango = false;
    Transform trSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DispararCohete());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DispararCohete()
    {
        while (true)
        {
            if (estaEnRango) //mirar si en el rango de la camara del player hay algun enemigo
            {
                Instantiate(prefabCohetes, trSpawn.position, Quaternion.identity); //poner la posicion de los spawns que devuelva el trigger stay
                yield return new WaitForSeconds(3);
                //Animacion explosion bomba
                ExplosionCohete();
            }
            // yield return null;
        }
        

    }


    void OnTriggerStay2D(Collider2D other) //si el rango del player entra en contacto con este spawn y 
    {
        if (other.CompareTag("SpawnCohetes"))
        {
            trSpawn = other.GetComponent<Transform>();

            estaEnRango = true;
        }
        else
        {
            estaEnRango = false;
        }
    }

    void ExplosionCohete()
    {
        //animacion explosion cohete

    }
}
