using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Araña : MonoBehaviour
{
    //sin gravedad

    //arañas en plataformas llendo de un lado a otra y si entra en el rango de la camara suelta bomba
    //bomba tarda en explotar 3 segundos

    //con raycast que sale de la araña
    //si player pasa por debajo (raycast que sale de la araña y detecta el rb de player)
    //se suelta, se gira y cae en la plataforma de debajo
    //si player toca la araña recibe daño

    
    [SerializeField] float vida;
    [SerializeField] float velocidad;
    [SerializeField] bool dead;
    [SerializeField] float dano;
    [SerializeField]bool mov;
    Rigidbody2D rbAraña;
    Collider2D coll;
    Animator anim;
    Transform posPlayer;
    [SerializeField]Enemy scripE;
    enum Estado { techo,volando,suelo};
    [SerializeField]Estado miPosicion;

    [Header("Radio")]
    [SerializeField] bool setArea;
    [SerializeField] bool detect;
    [SerializeField] float radioDect;
    [SerializeField] Transform area;

    [Header("Patrulla")]
    [SerializeField]bool patrulla;
    [SerializeField] int destino;
    [SerializeField] Transform punto1;
    [SerializeField] Transform punto2;

    [Header("Seguimiento")]
    [SerializeField] bool seguir;

    [Header("Overlap")]
    [SerializeField] float distR2;
    [SerializeField] LayerMask suelo;

    [Header("Caida")]
    [SerializeField] bool bajar;
    [SerializeField] LayerMask player;
    [SerializeField] float distanciaRayo;

    [Header("Bombardeo")]
    [SerializeField] bool animboom;
    [SerializeField] float secons;
    [SerializeField] Transform spawn;
    [SerializeField] Transform spawn2;
    [SerializeField] GameObject bomba;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rbAraña = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        posPlayer = player.GetComponent<Transform>();
        
        StartCoroutine(MovimientoEnPlataforma());
        StartCoroutine(CicloBomba());
    }

    // Update is called once per frame
    void Update()
    {
        DatesRefesh();
        Dead();
        AreaDetect();

        if (miPosicion==Estado.techo&&mov)
        {
            Bajar();
        }
        if (miPosicion==Estado.suelo&&mov)
        {
            SeguirPlayer();
        }
        
    }
    void DatesRefesh()
    {
        vida = scripE.vida;
        velocidad = scripE.velcidad;
        dano = scripE.dano;
    }
    void Dead()
	{
        if (vida<=0)
        {
            mov = false;
            dead = true;
            anim.SetTrigger("Dead");
        }
	}
    void ResetVelocy()
    {
        rbAraña.velocity = Vector2.zero;
    }
    void Voltear()
    {
        transform.localScale = new Vector2(1, -1);
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

        if (Vector2.Distance(transform.position, posPlayer.position) < radioDect)
        {
            detect=true;
            
            if (miPosicion==Estado.suelo)
            {
                patrulla = false;
            }
            return true;
        }
        else
        {
            detect=false;
            if (miPosicion == Estado.suelo)
            {
                patrulla = true;
            }
            return false;
        }
    }

    IEnumerator MovimientoEnPlataforma()
    {
        while (true)
        {
            if (miPosicion==Estado.techo|| miPosicion == Estado.suelo)
            {
                Debug.Log("estoy en el techo o suelo");
                if (patrulla&&mov)
                {
                    

                    if (destino == 1)
                    {

                        if (transform.position.x >= punto1.position.x)
                        {
                            destino = 2;
                        }
                        else
                        {
                            if (miPosicion == Estado.techo)
                            {
                                transform.localScale = new Vector3(-1, 1, 1);
                            }
                            else
                            {
                                transform.localScale = new Vector3(-1, -1, 1);
                            }
                            
                            rbAraña.velocity = new Vector2(velocidad, rbAraña.velocity.y);
                        }


                    }
                    else if (destino == 2)
                    {
                        if (transform.position.x <= punto2.position.x)
                        {
                            destino = 1;
                            Debug.Log("Cambio de lado a 1");
                        }
                        else
                        {
                            if (miPosicion == Estado.techo)
                            {
                                transform.localScale = new Vector3(1, 1, 1);
                            }
                            else
                            {
                                transform.localScale = new Vector3(1, -1, 1);
                            }

                            rbAraña.velocity = new Vector2(velocidad * -1, rbAraña.velocity.y);
                        }

                    }


                }
            }



            yield return null;
        }

        
    }
    IEnumerator CicloBomba()
    {
        while (miPosicion==Estado.techo)
        {
            if (!animboom&&detect)
            {
                anim.SetTrigger("boom");
                animboom = true;
                patrulla = false;
                ResetVelocy();
                yield return new WaitForSeconds(secons);
            }

            yield return null;
        }
    }
    public void AnimBoomFalse()
    {
        anim.SetBool("boom", false);
        animboom = false;
        patrulla = true;
    }
    public void SpawnBoom()
    {
        GameObject.Instantiate(bomba,spawn.position,Quaternion.identity);
    }
    //que si esta debajo el player, la araña baje 
    void Bajar() 
    {
        bool estaPlayer = Physics2D.Raycast(transform.position, Vector2.down, distanciaRayo,player);  //mirar si el rayo sale para abajo o arriba
        
        // si golpea algo
        if (estaPlayer)
        {
            anim.SetBool("boom", false);
            ResetVelocy();
            Voltear();
            rbAraña.gravityScale = 1;
            bajar = true;
            miPosicion = Estado.volando;
            StartCoroutine(itsGrounder());
            
        }
    }

    IEnumerator itsGrounder()
    {
        while( bajar)
        {
            patrulla = false;
            bool cesta = Physics2D.Raycast(transform.position, Vector2.down, distR2, suelo);  //mirar si el rayo sale para abajo o arriba
            Debug.DrawRay(transform.position, Vector2.down * distR2, Color.cyan);
            if (cesta)
            {
                miPosicion = Estado.suelo;
                bajar = false;
            }
            yield return null;
        }

    }
    void Orientar()
    {
        if (transform.position.x < posPlayer.position.x)
        {
            transform.localScale = new Vector2(-1, -1);
        }
        else
        {
            transform.localScale = new Vector2(1, -1);
        }
    }
    void SeguirPlayer() 
    {
        if (detect) //si araña en el suelo
        {
            var acercamiento = velocidad * Time.deltaTime; // calculate distance to move
            transform.position=Vector2.MoveTowards(transform.position, posPlayer.position, acercamiento);
            Orientar();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&!dead)
        {
            Debug.LogWarning("choco player");
            mov = false;
            rbAraña.bodyType = RigidbodyType2D.Static;
            coll.isTrigger = true;
            collision.gameObject.GetComponent<Player>().RecibirDano(dano);
            Invoke("SwitchMov", 0.5f);

        }
    }
    void SwitchMov()
    {
        rbAraña.bodyType = RigidbodyType2D.Dynamic;
        coll.isTrigger = false;
        mov =true;
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
    public void BoomPosDead()
    {
        GameObject.Instantiate(bomba, spawn2.position, Quaternion.identity);
    }
}



