using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratones : MonoBehaviour
{
    Enemy scripE;
    [SerializeField] public float vida;
    
    [SerializeField] float damaFisi;
    [SerializeField] float damaExplo;
    Animator anim;
    [Header("MOV")]
    [SerializeField] public float velocidad;
    Transform posPlayer;
    [Header("Overlap")]
    [SerializeField] bool preview;
    [SerializeField] LayerMask maskP;
    [SerializeField] Transform spawnOverlap;
    [SerializeField] float radio;
    
    // Start is called before the first frame update
    void Start()
    {
        posPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        anim = GetComponent<Animator>();

        scripE = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        DatesRefesh();



        Movimiento();
        Orientar();
        if (vida <= 0)
        {
            anim.SetTrigger("Death");
        }
    }


    void Movimiento()
    {
        if (Vector2.Distance(transform.position, posPlayer.position) < 7)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("choco player");
            collision.gameObject.GetComponent<Player>().RecibierDano(damaFisi);

            
        }
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
