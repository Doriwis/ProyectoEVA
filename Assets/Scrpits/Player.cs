using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float vidas = 100;
    Rigidbody2D rb;

    [Header("MOVIMIENTO")]

    [Header("Desplazameinto Simple ")]
    [SerializeField] float velocityMovimiento;
    [SerializeField] float velocidadMaxima;
    Vector3 ajuste;
    [SerializeField] float h;
    [SerializeField] float v;
   [SerializeField] float daño;

    [Header("Correción de las rampas")]
    float gravityInicial;

    [Header("Salto")]
    [SerializeField] LayerMask Suelo;
    [SerializeField] int saltosMax;
    [SerializeField] int saltosRes;
    [SerializeField] float forceJump;
    [SerializeField]float distSuelo;
    [Header("ANIMACION")]
    Animator anim;
    private enum estado { IDEL,run, caer,saltar,saltar2 ,dead};
    private estado estadoPaleyer=estado.IDEL;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        saltosRes = saltosMax;
        gravityInicial = rb.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        ItsGrounded();
        Saltar();
    }

    //apica fuerzas al player
    private void FixedUpdate()
    {
        if (vidas > 0)
        {
            rb.velocity = new Vector3(h * velocityMovimiento, rb.velocity.y);
            ajuste = Vector3.ClampMagnitude(rb.velocity, velocidadMaxima);
            rb.velocity = new Vector3(ajuste.x, rb.velocity.y);
        }
        
    }

    //Detecta si esta en suelo
    bool ItsGrounded()
    {   
        
        RaycastHit2D cesta = Physics2D.Raycast(transform.position, Vector2.down, distSuelo, Suelo);
        Debug.DrawRay(transform.position, Vector2.down*distSuelo, Color.red);
        if (cesta)
        {
            saltosRes = saltosMax;
            Debug.Log("estoy en el suelo ");
            return true;
            
            
        }
        else
        {
            Debug.Log("estoy en el aire");
                return false;
        }
        
        
    }

    //Para que no resvale en las cuestas

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            ContactPoint2D puntoConatcto = collision.GetContact(0);
            Vector2 normalDelPlano = puntoConatcto.normal;
            float dot = Vector3.Dot(normalDelPlano, Vector3.up);

            if (ItsGrounded() && dot != 1)
            {
                Debug.Log("Desactiva Gravidad");
                rb.gravityScale = 0;
            }
            else
            {
                Debug.Log("Activar Gravidad");
                rb.gravityScale = gravityInicial;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            Debug.Log("Activar Gravidad x salida");
            rb.gravityScale = gravityInicial;

        }
    }


    void Saltar()
    {

        if (Input.GetKeyDown(KeyCode.Space) && saltosRes > 0) //&& anim.GetBool("attacking") == false)
        {


            if (saltosRes > 1 && ItsGrounded() == true)
            {
                estadoPaleyer = estado.saltar;

                rb.velocity = new Vector2(rb.velocity.x, 0);

                rb.AddForce(new Vector3(0, 1, 0) * forceJump, ForceMode2D.Impulse);
            }
            else
            {
                estadoPaleyer = estado.saltar2;
                

                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector3(0, 1, 0) * forceJump, ForceMode2D.Impulse);

                saltosRes--;
            }
        }
    }
    void Animaciones()
    {
        switch (estadoPaleyer)
        {
            case estado.IDEL:
                Debug.Log("ESTYO QUIETO");
                break;
            case estado.run:
                Debug.Log("ESTYO CORRIENDO");
                break;
            case estado.caer:
                Debug.Log("ESTYO CAYENDO");
                break;
            case estado.saltar:
                Debug.Log("ESTYO SALTANDO");
                break;
            case estado.saltar2:
                Debug.Log("ESTYO SALTANDO X2");
                break;
            case estado.dead:
                Debug.Log("ESTYO MUERTO");
                break;
            default:
                break;
        }
    }

    public void RecibierDaño()
    {

    }
}
