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
    [SerializeField] LayerMask player;

    Rigidbody2D rbAraña;
    Animator anim;
    [SerializeField] float distanciaRayo;
    Vector3 position;
    Transform posPlayer;
    bool arañaEnSuelo;

    [SerializeField] bool bajar;
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
    [SerializeField] float radio;
    [SerializeField] LayerMask suelo;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rbAraña = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        posPlayer = player.GetComponent<Transform>();

        StartCoroutine(MovimientoEnPlataforma());
    }

    // Update is called once per frame
    void Update()
    {
        Bajar();
        SeguirPlayer();
    }
    void Voltear()
    {
        Vector2 localScale = transform.localScale;
        localScale.y *= -1;
        transform.localScale = localScale;
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
            return true;
        }
        else
        {
            detect=false;
            return false;
        }
    }

    IEnumerator MovimientoEnPlataforma()
    {
        while (true)
        {
            if (patrulla)
            {
                anim.SetBool("Running", true);

                Debug.LogWarning("moevo patrol");
                if (destino == 1)
                {

                    if (transform.position.x >= punto1.position.x)
                    {
                        destino = 2;
                        Debug.Log("Cambio de lado a 2");
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
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
                        transform.localScale = new Vector3(1, 1, 1);

                        rbAraña.velocity = new Vector2(velocidad * -1, rbAraña.velocity.y);
                    }

                }
            }



            yield return null;


        }
    }
    //que si esta debajo el player, la araña baje 
    void Bajar() 
    {
        bool estaPlayer = Physics2D.Raycast(transform.position, Vector2.down, distanciaRayo,player);  //mirar si el rayo sale para abajo o arriba

        // si golpea algo
        if (estaPlayer)
        {
            StartCoroutine(itsGrounder());
            Voltear();
            rbAraña.gravityScale = 1;
            bajar = true;
        }
    }

    IEnumerator itsGrounder()
    {
        while(rbAraña.velocity.y>=0 && bajar)
        {
            patrulla = false;
            Collider2D call = Physics2D.OverlapCircle(transform.position, radio, suelo);
            if (call)
            {
                bajar = false;
            }
            yield return null;
        }

    }
    void SeguirPlayer() 
    {
        if (seguir) //si araña en el suelo
        {
            var acercamiento = velocidad * Time.deltaTime; // calculate distance to move
            transform.position=Vector2.MoveTowards(position, posPlayer.position, acercamiento);
        }
    }

    
}



