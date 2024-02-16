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
    [SerializeField] GameObject punto1;
    [SerializeField] GameObject punto2;
    [SerializeField] LayerMask player;

    Rigidbody2D rbAraña;
    Animator anim;
    Transform puntoActual;
    [SerializeField] float distanciaRayo;
    Vector3 position;
    Transform posPlayer;
    bool arañaEnSuelo;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rbAraña = GetComponent<Rigidbody2D>();
        puntoActual = punto2.transform;
        anim.SetBool("Running", true);

    }

    // Update is called once per frame
    void Update()
    {
        MovimientoEnPlataforma();
        Bajar();
        SeguirPlayer();
    }

    public void RecibirDanho(float danho) //ejecutar desde evento en la animacion
    {

        vida -= danho;
        if (vida <= 0)
        {
            Destroy(gameObject);

        }
    }

    void Girar()
    {
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    void Voltear()
    {
        Vector2 localScale = transform.localScale;
        localScale.y *= -1;
        transform.localScale = localScale;
    }

    void MovimientoEnPlataforma()
    {
        //que se mueva constantemente de un punto a otro
        Vector2 punto = puntoActual.position - transform.position;
        if (puntoActual == punto2.transform)
        {
            rbAraña.velocity = new Vector2(velocidad, rbAraña.velocity.y);
        }
        else
        {
            rbAraña.velocity = new Vector2(-velocidad, rbAraña.velocity.y);
        }

        if (Vector2.Distance(transform.position, puntoActual.position) < 0.5f && puntoActual == punto2.transform)
        {
            Girar();
            puntoActual = punto1.transform;
        }
        else if (Vector2.Distance(transform.position, puntoActual.position) < 0.5f && puntoActual == punto1.transform)
        {
            Girar();
            puntoActual = punto2.transform;
        }


    }

    //que si esta debajo el player, la araña baje 
    void Bajar() 
    {
        bool estaPlayer = Physics2D.Raycast(transform.position, -Vector2.up, distanciaRayo,player);  //mirar si el rayo sale para abajo o arriba

        // si golpea algo
        if (estaPlayer)
        {
            
            Voltear();
            rbAraña.gravityScale = 1;
            
        }
    }

    void SeguirPlayer() 
    {
        if (arañaEnSuelo) //si araña en el suelo
        {
            position = transform.position;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            posPlayer = player.GetComponent<Transform>();

            var acercamiento = velocidad * Time.deltaTime; // calculate distance to move
            Vector2.MoveTowards(position, posPlayer.position, acercamiento);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //
    {
        if (collision.CompareTag("Suelo"))
        {
            arañaEnSuelo = true;
        }
        else
        {
            arañaEnSuelo = false;
        }
    }
}



