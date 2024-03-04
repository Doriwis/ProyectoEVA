using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float vida;
    public float velcidad;
    public float dano;

    public void RecibirDano(float danOut)
    {
        Debug.LogWarning("RECIBODAÑO E");
        vida -= danOut;
    }
}
