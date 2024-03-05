using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBombas : MonoBehaviour
{
    [SerializeField]float dano;
    Animator anim;
    Rigidbody2D rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
    }
    //va en el trigger del rango de las bombas
    private void OnTriggerEnter2D(Collider2D collision)
    {

        anim.SetTrigger("Dead");
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().RecibirDano(dano);
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().RecibirDano(dano);
        }

    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}



