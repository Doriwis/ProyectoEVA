using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohete : MonoBehaviour
{
    // VA EN LOS COHETES
    Vector3 position;
    
    [SerializeField]Transform posPlayer;

    [SerializeField] float velocidad;
    [SerializeField] float daño;
    [Header("OVERLAP")]
    [SerializeField] bool preview;
    [SerializeField] float radio;
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask enemy;
    [SerializeField] bool up;
    [SerializeField] bool boom;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        position = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        posPlayer = player.GetComponent<Transform>();
        Invoke("Upfalse", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //que sigan al player
        if (!up)
        {
            Seguimiento();
            Giro();
        }
        
        
    }
    private void FixedUpdate()
    {
        if (up)
        {
            rb.velocity = new Vector3(rb.velocity.x, velocidad);
        }
        else
        {
            rb.velocity = new Vector3(0,0);
        }
        
    }
    void Upfalse()
    {
        up = false;
        boom = true;
    }

    void Seguimiento()
    {
        var acercamiento = velocidad * Time.deltaTime; // calculate distance to move
        transform.position = Vector2.MoveTowards(transform.position, posPlayer.position, acercamiento);//mover los ratones hacia player con translate
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COCHO");
        if (collision.gameObject.GetComponent("Player")&&boom)
        {
            ExplocionPlayer();
        }
        else if(boom)
        {
            ExplocionEnemy();
        }
        
    }
    
    public void ExplocionPlayer()
    {
        Collider2D aux=Physics2D.OverlapCircle(transform.position, radio, player);
        if (aux)
        {
            Debug.LogError("booom P");
            aux.gameObject.GetComponent<Player>().RecibirDano(daño);
            Morir();
        }
    }
    public void ExplocionEnemy()
    {
        Collider2D aux = Physics2D.OverlapCircle(transform.position, radio, enemy);
        if (aux)
        {
            aux.gameObject.GetComponent<Enemy>().RecibirDano(daño);
            Debug.LogError("booom E O ");
        }

        Debug.LogError("booom  O ");
        Morir();
    }
    private void OnDrawGizmos()
    {
        if (preview)
        {
            Gizmos.DrawSphere(transform.position, radio);
        }


    }
    void Giro()
    {
        float anguloRad = Mathf.Atan2(posPlayer.position.y - transform.position.y, posPlayer.position.x - transform.position.x);
        float anguloReal = ((180 / Mathf.PI) * anguloRad - 90);
        transform.rotation=Quaternion.Euler(0, 0, anguloReal);
    }

    public void Morir()
    {
        Destroy(this.gameObject);
    }

}
