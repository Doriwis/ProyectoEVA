using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratones : MonoBehaviour
{
    Enemy scripE;
    float vida;
    [SerializeField] float waiTime;
    float damaFisi;
    [SerializeField] float damaExplo;
    Animator anim;
    Rigidbody2D rb;
    Collider2D coll;
    [Header("MOV")]
    [SerializeField] bool mov;
    float velocidad;
    Transform posPlayer;

    [Header("Overlap")]
    [SerializeField] bool preview;
    [SerializeField] LayerMask maskP;
    [SerializeField] Transform spawnOverlap;
    [SerializeField] float radio;

    [Header("Area")]
    [SerializeField] bool previewArea;
    [SerializeField] float distacia;
    [SerializeField] GameObject area;

    // Start is called before the first frame update
    void Start()
    {
        posPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        anim = GetComponent<Animator>();

        scripE = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DatesRefesh();


        if (mov)
        {
            Movimiento();
        }

        AreaActive();

        Orientar();

        if (vida <= 0)
        {
            anim.SetTrigger("Death");
            rb.bodyType = RigidbodyType2D.Static;
            coll.isTrigger = true;
            mov = false;
        }
    }
    void AreaActive()
    {
        if (previewArea)
        {
            area.SetActive(true);
            area.transform.localScale = new Vector2(distacia * 2, distacia * 2);
        }
        else
        {
            area.SetActive(false);
        }
    }

    void Movimiento()
    {
        if (Vector2.Distance(transform.position, posPlayer.position) < distacia)
        {
            var acercamiento = velocidad * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, posPlayer.position, acercamiento);
        }
    }

    void Orientar()
    {
        if (transform.position.x<posPlayer.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    void DatesRefesh()
    {
        vida = scripE.vida;
        velocidad = scripE.velcidad;
        damaFisi = scripE.dano;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("choco player");
            mov = false;
            rb.bodyType = RigidbodyType2D.Static;
            coll.isTrigger = true;
            collision.gameObject.GetComponent<Player>().RecibierDano(damaFisi);
            StartCoroutine(SwitchMov());

        }
    }
    IEnumerator SwitchMov()
    {
        yield return new WaitForSeconds(waiTime);
        rb.bodyType = RigidbodyType2D.Dynamic;
        coll.isTrigger = false;
        mov = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }
    private void OnDrawGizmos()
    {
        if (preview)
        {
            Gizmos.DrawSphere(spawnOverlap.position, radio);
        }
        


    }

    void OverlapExplocion()
    {
        Collider2D cesta = Physics2D.OverlapCircle(spawnOverlap.position, radio, maskP);

        if (cesta&&cesta.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("DETECTO PLAYER EXPLOCION");
            cesta.gameObject.GetComponent<Player>().RecibierDano(damaExplo);
        }
    }
    void Death()
    {
        Destroy(this.gameObject);
    }
    
}
