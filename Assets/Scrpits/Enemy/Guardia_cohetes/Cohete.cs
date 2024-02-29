using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohete : MonoBehaviour
{
    // VA EN LOS COHETES
    Vector3 position;
    
    [SerializeField]Transform posPlayer;

    [SerializeField] float velocidad;
    [Header("OVERLAP")]
    [SerializeField] bool preview;
    [SerializeField] float radio;
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask enemy;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        posPlayer = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        

        var acercamiento = velocidad * Time.deltaTime; // calculate distance to move
        transform.position=Vector2.MoveTowards(transform.position, posPlayer.position, acercamiento);//mover los ratones hacia player con translate
        //que sigan al player
        Giro();
        GiroCohete();
    }

    void GiroCohete()
    {
        //conseguir el transform del player
        if (transform.position.x > posPlayer.transform.position.x) // si la x del cohete es mayor que el GOb cabeza de player que mire a la izuierda
        {

            transform.localScale = new Vector3(1, 1, 1);
        }
        else // si la x del cohete es menor que la de player que mire a la derecha
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent("Player"))
        {
            ExplocionPlayer();
        }
        else
        {
            ExplocionEnemy();
        }
    }
    public void ExplocionPlayer()
    {
        Collider2D aux=Physics2D.OverlapCircle(transform.position, radio, player);
        if (aux)
        {
            Morir();
            Debug.LogError("booom P");
        }
    }
    public void ExplocionEnemy()
    {
        Collider2D aux = Physics2D.OverlapCircle(transform.position, radio, enemy);
        if (aux)
        {
            
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
        float anguloReal = ((180 * anguloRad) / Mathf.PI);
        transform.rotation=Quaternion.Euler(0, 0, anguloReal);
    }
    public void Morir()
    {
        Destroy(this.gameObject);
    }

}
