using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohete : MonoBehaviour
{
    // VA EN LOS COHETES
    Vector3 position;
    
    Transform posPlayer;

    [SerializeField] float velocidad;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        anim = GetComponent<Animator>();
        anim.SetBool("Running", true);

       
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        posPlayer = player.GetComponent<Transform>();

        var acercamiento = velocidad * Time.deltaTime; // calculate distance to move
        Vector2.MoveTowards(position, posPlayer.position, acercamiento);//mover los ratones hacia player con translate
        //que sigan al player

        GiroCohete();
    }

    void Girar() // CUANDO la posicion en h de pleyer sea negativa?
    {
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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

}
