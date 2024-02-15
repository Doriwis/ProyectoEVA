using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float vida;
    [SerializeField] public float velocidad;
    [SerializeField] float damaFisi;
    [SerializeField] float damaExplo;
    Animator anim;

    [Header("Overlap")]

    [SerializeField] LayerMask maskP;
    [SerializeField] Transform spawnOverlap;
    [SerializeField] float radio;

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vida<=0)
        {
            anim.SetTrigger("Death");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("choco player");
            collision.gameObject.GetComponent<Player>().RecibierDano(damaFisi);

            anim.SetTrigger("Death");
        }
    }
    private void OnDrawGizmos()
    { 
        Gizmos.DrawSphere(spawnOverlap.position, radio);


    }

    void OverlapDeath()
    {
        Collider2D cesta=Physics2D.OverlapCircle(spawnOverlap.position, radio, maskP);
        if (cesta.gameObject.CompareTag("Player"))
        {
            cesta.gameObject.GetComponent<Player>().RecibierDano(damaExplo);
        } 
    }

    public void RecibirDano(float danOut)
    {
        vida -= danOut;
    }
}
