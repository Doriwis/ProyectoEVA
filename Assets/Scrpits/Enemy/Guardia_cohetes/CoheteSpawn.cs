using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoheteSpawn : MonoBehaviour
{
    
    //va en el rango de la camara 
    Rigidbody2D rb;
    Animator anim;
    [SerializeField]Transform posPlayer;

    [Header("Radio")]
    [SerializeField] bool setArea;
    [SerializeField] bool detect;
    [SerializeField] float radioDect;
    [SerializeField] Transform area;

    [Header("Shoot")]
    [SerializeField] float time;
    [SerializeField] GameObject prefabCohetes;
    [SerializeField] Transform trSpawn;

    [SerializeField] float tempo;
    [SerializeField] float limiTempo;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        posPlayer = player.GetComponent<Transform>();

        StartCoroutine(DispararCohete());

    }

    // Update is called once per frame
    void Update()
    {
        AreaDetect();

        
    }

    void AreaDetect()
    {
        area.localScale = new Vector2(radioDect * 2, radioDect * 2);
        if (setArea)
        {
            area.gameObject.SetActive(true);
        }
        else
        {
            area.gameObject.SetActive(false);
        }

        if (Vector2.Distance(transform.position,posPlayer.position) < radioDect)
        {
            
            detect = true;
        }
        else
        {
            detect = false;
        }
    }

    void Temporizador()
    {
        tempo += 1*Time.deltaTime;
        if (tempo>=limiTempo)
        {
            tempo = 0;
        }
    }

    IEnumerator DispararCohete()
    {
        while (true)
        {
            if (detect)
            {
                
                Debug.LogWarning("DETECTO MONGOLO");
                anim.SetTrigger("Put");
                yield return new WaitForSeconds(time);
                Debug.LogWarning("espere time");
            }
           
            yield return null;
        }
        

    }

    public void InstanciarCohete()
    {
        Instantiate(prefabCohetes, trSpawn.position, Quaternion.identity); //poner la posicion de los spawns que devuelva el trigger stay
    }


    

   
}
