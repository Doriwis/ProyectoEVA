using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public Rigidbody2D rb;
    [SerializeField] Vector2 rbVelocy;
    [Header("Cursor")]
    Vector2 cursorPos;

    [SerializeField]GameObject cursorObj;
    [Header("VARIABLES BASIC")]
    [SerializeField] float vidas = 100;
    public int capGas;
    int gitones;

    [Header("MOVIMIENTO")]
    public bool mov;
    [Header("Desplazameinto Simple ")]
    [SerializeField] float velocityMovimiento;
    [SerializeField] float velocidadMaxima;
    Vector3 ajuste;
    [SerializeField] float h;
    [SerializeField] float v;
    

    [Header("Correción de las rampas")]
    float gravityInicial;

    [Header("Salto")]
    public bool saltar=true;
    [SerializeField] LayerMask Suelo;
    [SerializeField] int saltosMax;
    [SerializeField] int saltosRes;
    [SerializeField] float forceJump;
    [SerializeField] float distSuelo;

    [Header("ANIMACION")]

    Animator anim;

    [Header("ATAQUES")]

    [SerializeField] float damage;

    [SerializeField]Transform transOverlap;
    [SerializeField]float oveRadio;
    [SerializeField] LayerMask maskEnemy;

    [Header("Gancho")]
    [SerializeField] Transform area;
    public bool power;
    [SerializeField] bool canShoot;
    [SerializeField]GameObject gancho;
    [SerializeField] Camera camara;
    [SerializeField] Transform spawnGanch;
    [SerializeField] GameObject instGancho;
    Vector2 posGanFin;
    float anguloRot;
    public float longCad=6;
    [Header("Metal")]
    [SerializeField]bool metal;

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
        rbVelocy = rb.velocity;

        RevelarCursor();
        LimiteGancho();
        ItsGrounded();
        Saltar();
        Metal();
        Atttack();
        if (mov)
        {
            
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
                
                saltosRes = saltosMax;
                anim.SetBool("Falling", true);
                Debug.Log("estoy en el suelo ");
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

    //Para que no resbale en las cuestas

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

    IEnumerator FalseGancho()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Gancho", false);
    }
    IEnumerator TrueShoot()
    {
        yield return new WaitForSeconds(0.4f);
        canShoot = true;
    }
    void Saltar()
    {

        if (Input.GetKeyDown(KeyCode.Space) && saltosRes > 0&&saltar) //&& anim.GetBool("attacking") == false)
        {
            if (instGancho)
            {
                RestaurarProGancho();
               
                instGancho.GetComponent<Gancho>().Morir();
                anim.SetTrigger("Jumping2");
                Impulso();
                StartCoroutine(FalseGancho());
            }
           else if (saltosRes > 1 && ItsGrounded() == true)
            {
                Debug.LogWarning("Activo salto 1");

                canShoot = false;
                anim.SetTrigger("Jumping");
                StartCoroutine(TrueShoot());
            }
            else
            {
                anim.SetTrigger("Jumping2");
                Impulso();
            }
        }
    }
    
    public void Impulso()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector3(0, 1, 0) * forceJump, ForceMode2D.Impulse);
        saltosRes--;
    }

    void RevelarCursor()
    {
        cursorPos = camara.ScreenToWorldPoint(Input.mousePosition);
        cursorObj.GetComponent<Transform>().position = cursorPos;
        if (ValidarAngulo())
        {
            cursorObj.SetActive(true);
        }
        else
        {
            cursorObj.SetActive(false);
        }
    }

    public void DisparoGancho()
    {
        
        
        

        instGancho = GameObject.Instantiate(gancho,spawnGanch.position , Quaternion.Euler(0,0,anguloRot));
        Gancho scriptG= instGancho.GetComponent<Gancho>();
        if (transform.localScale.x < 0)
        {
            instGancho.GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
        }

        scriptG.destino = cursorPos;
        posGanFin = cursorPos;
        scriptG.angulo = anguloRot;
        scriptG.limitRadio=longCad;
        scriptG.scriP = this.gameObject.GetComponent<Player>();


    }

    void AccionarGancho()
    {
        if (Input.GetMouseButtonDown(1) && ValidarAngulo() && ItsGrounded()&&power&&canShoot    )
        {
            Debug.LogWarning("Boton gancho");
            mov = false;
            power = false;
            saltar = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector3.zero;
            
            anim.SetBool("Gancho", true);

        }
    }

    void LimiteGancho()
    {
        area = GameObject.FindGameObjectWithTag("RadioLimite ").GetComponent<Transform>();
        area.localScale = new Vector3(longCad * 2, longCad * 2, longCad * 2);
    }

    bool ValidarAngulo()
    {
        

        float anguloRad = Mathf.Atan2(cursorPos.y - transform.position.y, cursorPos.x - transform.position.x);
        float anguloReal = ((180 * anguloRad) / Mathf.PI);
        float anguloClamp;

        //Debug.Log("ANGULO " + anguloReal);

        if (transform.localScale.x > 0)
        {
           // Debug.Log("DERECHA");
            if (anguloReal < 60 && anguloReal > -60)
            {
                //Debug.Log("DENTRO ENTRE 60 Y -60");
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
                
                //Debug.Log("DENTRO ENTRE 120 Y -120");
                if (anguloReal > 0)
                {
                    anguloClamp = (180 - anguloReal) * -1;
                    anguloRot = anguloClamp;
                   // Debug.Log("POSITIVO Y ABSOLUTO ES " + anguloClamp);
                }
                else
                {

                    anguloClamp = 180 + anguloReal;
                    anguloRot = anguloClamp;
                    //Debug.Log("NEGATVIO Y ABSOLUTO ES " + anguloClamp);
                }
                
                return true;
            }
            else
            {
                
                return false;
            }
            
        }
    }

    public void RestaurarProGancho()
    {
        mov = true;
        power = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        saltar = true;
        //dinamico shoot mov false
    }

    public IEnumerator IrGancho(float velocy)
    {
        while (Vector2.Distance(transform.position,posGanFin)>2)
        {
            Debug.Log("voy al gancho");
            transform.position=Vector2.MoveTowards(transform.position, posGanFin, 15*Time.deltaTime );

            yield return null;
         }
       
        
        RestaurarProGancho();
        anim.SetBool("Gancho", false);
        instGancho.GetComponent<Gancho>().Morir();
     }

    void Metal() 
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!metal && power)
            {
                Debug.LogWarning("me transformo en metal");
                anim.SetBool("Metal", true);
                BlockeoMetal();
            }
            else if(metal&&!power)      
            {
                Debug.LogWarning("me transformo en NORMAL");
                anim.SetBool("Metal", false);
            }
            else
            {
                Debug.LogWarning("ESTA EL GANCHO ACTIVO ");
            }
            

        }

    }
    public void BlockeoMetal()
    {
        if (!metal)
        {
            Debug.LogWarning("BLOCKEOMETAL");
            mov = false;
            power = false;
            saltar = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 8;
            metal = true;
        }
        else
        {
            Debug.LogWarning("DESBLOCKEOMETAL");
            mov = true;
            power = true;
            saltar = true;
            rb.gravityScale = gravityInicial;
            metal = false;

        }
        
    }
    IEnumerator RestauroAtaque()
    {
        yield return new WaitForSeconds(0.2f);
        mov = true;
        power = true;
        saltar = true;
    }
    void Atttack()
    {
        if (Input.GetMouseButtonDown(0)&&power&&canShoot)
        {
            Debug.LogWarning("CLCIK IZQUIERDO");

            anim.SetBool("Attacking", true);
            mov = false;
            power = false;
            saltar = false;
            rb.velocity = Vector2.zero;
        }
    }
    public void SigoAtacando()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            Debug.LogWarning("TABULADOR");
            anim.SetBool("Attacking", true);
        }
        else
        {
            anim.SetBool("Attacking", false);
            StartCoroutine(RestauroAtaque());
        }
        

    }
   
    public void FinAtaque()
    {
        anim.SetBool("Attacking", false);
        StartCoroutine(RestauroAtaque());
    }
    private void OnDrawGizmos()
    { //Detec duelo
        Gizmos.DrawSphere(transOverlap.position, oveRadio);
       
        
    }
    public void OverlabAttack()
    {
        Collider2D cesta=Physics2D.OverlapCircle(transOverlap.position, oveRadio, maskEnemy);
        if (cesta)
        {
            cesta.gameObject.GetComponent<Enemy>().RecibirDano(damage);
        }
       

    }
    public void OverlabAttack2()
    {
        Collider2D cesta = Physics2D.OverlapCircle(transOverlap.position, oveRadio, maskEnemy);
        if (cesta)
        {
            cesta.gameObject.GetComponent<Enemy>().RecibirDano(damage + 5);
        }
            

    }
    public void OverlabAttack3()
    {
        Collider2D cesta = Physics2D.OverlapCircle(transOverlap.position, oveRadio, maskEnemy);
        if (cesta)
        {
            cesta.gameObject.GetComponent<Enemy>().RecibirDano(damage + 10);
        }
        

    }
    public void RecibierDano(float exterDano)
    {
        
        if (!metal)
        {
            Debug.LogError("Entro en daño ");
            vidas -=exterDano;
        }
        
    }
}
