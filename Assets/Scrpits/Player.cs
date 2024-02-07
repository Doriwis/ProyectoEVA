using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float vidas = 100;
    Rigidbody2D rb;

    [Header("MOVIMIENTO")]
    [SerializeField]bool mov;
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
    [SerializeField] float distSuelo;

    [Header("ANIMACION")]
    Animator anim;

    [Header("ATAQUES")]

    [Header("Gancho")]
    [SerializeField] bool shoot;
    [SerializeField]GameObject gancho;
    [SerializeField] Camera camara;
    [SerializeField] Transform spawnGanch;
    Vector2 objetivo;
    float anguloRot;

    // AREA DE DETECCION
    // CADENA
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



        ItsGrounded();
        if (mov)
        {
            Saltar();
            Animacio();
            AccionarGancho();
        }
        
    }

    //apica fuerzas al player
    private void FixedUpdate()
    {
        if (vidas > 0 && mov)
        {
            rb.velocity = new Vector3(h * velocityMovimiento, rb.velocity.y);
            ajuste = Vector3.ClampMagnitude(rb.velocity, velocidadMaxima);
            rb.velocity = new Vector3(ajuste.x, rb.velocity.y);
        }

    }
    void Animacio()
    {
        h = Input.GetAxisRaw("Horizontal");
        if (h > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            Debug.Log("Derecha");

            anim.SetBool("Runnig", true);
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            Debug.Log("IZQUIERDA");
            anim.SetBool("Runnig", true);
        }
        else
        {
            anim.SetBool("Runnig", false);
        }

    }

    //Detecta si esta en suelo
    bool ItsGrounded()
    {

        RaycastHit2D cesta = Physics2D.Raycast(transform.position, Vector2.down, distSuelo, Suelo);
        Debug.DrawRay(transform.position, Vector2.down * distSuelo, Color.red);
        if (cesta)
        {
            if (rb.velocity.y <= 0)
            {
                Debug.Log("estoy en el suelo ");
                saltosRes = saltosMax;
                anim.SetBool("Falling", true);

                anim.SetBool("Falling", false);
            }

            
            return true;


        }
        else
        {
            
            if (rb.velocity.y < 0)
            {
                Debug.Log("estoy en el aire a bajo");
                anim.SetBool("Falling", true);
            }
            else
            {
                Debug.Log("estoy en el aire arriba");
                anim.SetBool("Falling", false);
            }
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
               

                anim.SetTrigger("Jumping");

            }
            else
            {
                


                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector3(0, 1, 0) * forceJump, ForceMode2D.Impulse);

                anim.SetTrigger("Jumping2");
                saltosRes--;
            }
        }
    }

    public void Impulso()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector3(0, 1, 0) * forceJump, ForceMode2D.Impulse);
        saltosRes--;
    }

    public void DisparoGancho()
    {
        
        
        

        GameObject newHand = GameObject.Instantiate(gancho,spawnGanch.position , Quaternion.Euler(0,0,anguloRot));
        
        if (transform.localScale.x < 0)
        {
            newHand.GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
        }

        newHand.GetComponent<Gancho>().destino=objetivo;
        newHand.GetComponent<Gancho>().angulo = anguloRot;


    }
    void AccionarGancho()
    {
        if (Input.GetMouseButtonDown(1) && ValidarAngulo()&& ItsGrounded()&&shoot)
        {
            mov = false;
            shoot = false;
            
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector3.zero;
            
            anim.SetBool("Gancho", true);

        }
    }
    bool ValidarAngulo()
    {
        objetivo = camara.ScreenToWorldPoint(Input.mousePosition);

        float anguloRad = Mathf.Atan2(objetivo.y - transform.position.y, objetivo.x - transform.position.x);
        float anguloReal = ((180 * anguloRad) / Mathf.PI);
        float anguloClamp;

        Debug.Log("ANGULO " + anguloReal);

        if (transform.localScale.x > 0)
        {
            Debug.Log("DERECHA");
            if (anguloReal < 60 && anguloReal > -60)
            {
                Debug.Log("DENTRO ENTRE 60 Y -60");
                anguloClamp = anguloReal;
                anguloRot = anguloClamp;

                return true;
            }
            else
            {
                
                return false;
            }
        }
        else
        {
            Debug.Log("IZQUIERDA");
            if (anguloReal < -120 && anguloReal > -180 || anguloReal > 120 && anguloReal < 180)
            {
                
                Debug.Log("DENTRO ENTRE 120 Y -120");
                if (anguloReal > 0)
                {
                    anguloClamp = (180 - anguloReal) * -1;
                    anguloRot = anguloClamp;
                    Debug.Log("POSITIVO Y ABSOLUTO ES " + anguloClamp);
                }
                else
                {

                    anguloClamp = 180 + anguloReal;
                    anguloRot = anguloClamp;
                    Debug.Log("NEGATVIO Y ABSOLUTO ES " + anguloClamp);
                }

                return true;
            }
            else
            {
                return false;
            }
            
        }
    }

    public void RecibierDaño(float exterDano)
    {
        vidas = -exterDano;
    }
}
