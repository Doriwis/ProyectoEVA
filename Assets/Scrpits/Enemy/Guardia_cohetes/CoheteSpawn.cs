using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoheteSpawn : MonoBehaviour
{
    
    //va en el rango de la camara 
    Rigidbody2D rb;
    Animator anim;
    [SerializeField]Transform posPlayer;
    [SerializeField] Enemy stats;
    float velocidad;
    [SerializeField] float vida;
    [SerializeField]bool dead;

    [Header("Radio")]
    [SerializeField] bool setArea;
    [SerializeField] bool detect;
    [SerializeField] float radioDect;
    [SerializeField] Transform area;

    [Header("Shoot")]
    [SerializeField] float time;
    [SerializeField] GameObject prefabCohetes;
    [SerializeField] Transform trSpawn;


    [Header("Retroceder")]
    [SerializeField] bool retrocede;

    [Header("Patrulla")]
    [SerializeField] int destino;
    [SerializeField] Transform destinoAc1;
    [SerializeField] Transform destinoAc2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        posPlayer = player.GetComponent<Transform>();

        StartCoroutine(DispararCohete());
        StartCoroutine(Patrulla());

    }



    // Update is called once per frame
    void Update()
    {
        ActualStats();
        if (vida<=0)
        {
            Dead();
        }
        if (!dead)
        {
            AreaDetect();
            Retroceder();
        }
    }
    void ActualStats()
    {
        vida = stats.vida;
        velocidad = stats.velcidad;
    }

    bool AreaDetect()
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
            return true;
        }
        else
        {
            return false;
        }
    }
    void Retroceder()
    {
        if (detect && retrocede)
        {
            if (transform.position.x>posPlayer.position.x)
            {
                rb.velocity = new Vector2(velocidad, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                rb.velocity = new Vector2(velocidad * -1, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
            }
            
        }
        if (detect&&!retrocede)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    void RetrocederFalse()
    {
        retrocede = false;
    }
    void RetrocederTrue()
    {
        retrocede = true;
    }
    public void GiroAColocar()
    {
        if (transform.position.x > posPlayer.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
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
                RetrocederFalse();

            }
            if (AreaDetect())
            {
                detect = true;
            }
            else
            {
                detect = false;
            }
           
            yield return null;
        }
        

    }
    public void SwitchDestino()
    {
        if (destino==1)
        {
            destino = 2;
        }
        else
        {
            destino = 1;
        }
    }
    IEnumerator Patrulla()
    {
        while (true)
        {
            if (!detect)
            {
                Debug.LogWarning("moevo patrol");
                if (destino == 1)
                {

                    if (transform.position.x >= destinoAc1.position.x)
                    {
                        destino = 2;
                        Debug.Log("Cambio de lado a 2");
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        rb.velocity = new Vector2(velocidad, rb.velocity.y);
                    }


                }
                else if (destino == 2)
                {
                    if (transform.position.x <= destinoAc2.position.x)
                    {
                        destino = 1;
                        Debug.Log("Cambio de lado a 1");
                    }
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);

                        rb.velocity = new Vector2(velocidad*-1, rb.velocity.y);
                    }

                }
            }

            

            yield return null;
        }
    }
    public void InstanciarCohete()
    {
        Instantiate(prefabCohetes, trSpawn.position, Quaternion.identity); //poner la posicion de los spawns que devuelva el trigger stay
    }

    void Dead()
    {
        anim.SetBool("Dead",true);
        dead = true;
        StopAllCoroutines();
        rb.velocity = new Vector2(0, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().isTrigger = true;
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
