using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovBoss : MonoBehaviour
{
    //Poner en Player variables de int vidas, bool metalActivo (cuando player se hace hierro)
    //hacer spawns de engranajes con el padre en 

    Player scriptPlayer;
    bool playerEnRango = false;
    [SerializeField]int vida = 100;
    GameObject player;
    Rigidbody2D rb;
    [SerializeField] float velocidad;

    [SerializeField] GameObject spawnInicial; //en la posicion inicial

    Animator anim;


    bool playerTocado;

    enum Fases {faseRayo,faseTp,faseIman}
    [SerializeField]Fases misFases = Fases.faseRayo;

    enum EstadosFase1 { volar, espera, pegar }
    EstadosFase1 estadoF1 = EstadosFase1.espera;
    [Header("Radio")]
    [SerializeField] bool setArea;
    [SerializeField] bool detect;
    [SerializeField] float radioDect;
    [SerializeField] Transform area;

    [Header("Volar")]
    Transform spawnRayo;
    GameObject goSpawnRayo;

    [SerializeField] int aux;
    [SerializeField]float registro=1;
    enum Destinos { arriba,abajo,derecha, izquierda,centro }
    [SerializeField]Destinos destinoAct=Destinos.arriba;
    [SerializeField] Transform puntoAlto;
    [SerializeField] Transform puntoR;
    [SerializeField] Transform puntoC;
    [SerializeField] Transform puntoL;

    Vector2 direccion;
    [SerializeField] int tiempoRayoVolar;
    [SerializeField] int dahnoRayo;
    bool volar;// ver cuando se activa
    [SerializeField] int pararVolar;

    [SerializeField] GameObject prefabParteRayo;
    [SerializeField] GameObject SpawnPrefabParteRayo; //va emparentado al spawn del rayo y se crea uno a la vez que se crea el trozo de rayo 
    //es donde se va a spaunear el siguiente asi que tiene qu eestar mas debajo para q sea el ce


    //ENGRANAJES
    [SerializeField] GameObject[] engranajes;
    Transform spwEngranajes;
    [SerializeField] int numEngranajes;
    [SerializeField] Vector3[] posEngranajes;
    [SerializeField] int rotZ; //numero que se usa para rotar los engranajes sumando esta variable cada vez a la rotacion actual
    bool engranajesActivados;

    //TIMER
    int temporizador;
    bool finTemporizador;

    [Header("ATAQUE BASTON")]

    [SerializeField] float danomele1;
    [SerializeField] float danomele2;
    bool tpActivado;
    

    //TP
    [SerializeField] GameObject spawnTpR1; //emparentados al palyer 1 respecto al player
    Transform posSpawnTpR1;
    [SerializeField] GameObject spawnTpR2; //emparentados al palyer 1 respecto al player
    Transform posSpawnTpR2;
    [SerializeField] GameObject spawnTpR3; //emparentados al palyer 1 respecto al player
    Transform posSpawnTpR3;

    [SerializeField] GameObject spawnTpL; //emparentados al palyer -1 respecto al player
    // Start is called before the first frame update
    void Start()
    {
        

        //posSpawnTpR1 = spawnTpR1.GetComponent<Transform>();
        //posSpawnTpR2 = spawnTpR2.GetComponent<Transform>();
        //posSpawnTpR3 = spawnTpR3.GetComponent<Transform>();

        player = GameObject.FindGameObjectWithTag("Player");
        scriptPlayer = player.GetComponent<Player>();
        //spwEngranajes = player.GetComponent<Transform>();

        //spawnRayo = goSpawnRayo.GetComponent<Transform>();

        anim = GetComponent<Animator>();

        //animacion 

        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MovFase1());
       
    }

    // Update is called once per frame
    void Update()
    {
        if (vida >0)
        {
            if (vida > 66) //vida a mas de 66% //rayo volar
            {
                if (volar)
                {
                    Fase1(); 
                }

            }
            else if (vida > 33) //vida a mas de 33% //baston
            {
                if (tpActivado)
                {
                   TP();
                   
                }
                
            }
            else
            {
                if (engranajesActivados)
                {
                    LanzarEngranajes(); 
                    Iman();
                }
            }
        }
        else
        {
            anim.SetTrigger("Muerte"); //que no se pueda mover el player
            //Animacion MUERTE
        }

        while (playerEnRango) //ataque de baston
        {
            StartCoroutine(Timer(2));
            anim.SetTrigger("Ataque");//dentro de la animacion si detecta player en el frame que le quite 


        }
    }

    void AtaqueBaston()
    {
        while (playerEnRango) //si el player se acerca a x distancia del boss el boss espera x seg y ataca, //si el player sigue en su rango de ataque ataca cada x seg 
        {
            StartCoroutine(Timer(2));
            anim.SetTrigger("AtaqueBaston");
        }
        
    }
    public void AtaqueDanoPlayer() //llamarlo en el frame de la animacion que impacte con player
    {
        if (playerTocado)
        {
            scriptPlayer.RecibirDano(danomele1);
        }

    }

    void Fase1() //FALTA EL PATRULLAJE
    {
        misFases = Fases.faseRayo;

    }

    public void Iman() //FALTAN LAS FISICAS
    {
        // (si el jugador se hace metal el boss tarda dos seg aprox en cambiar del de los engranajes al del imán y le atrae rapido asi que 
        //hay quitarse el metal rápido)

        //si detecta que el bool metalActivo está a true
        if (scriptPlayer.metal) //se activa a true cuando jugador pone cuerpo de metal
        {
            StartCoroutine(Timer(2)); //tiempo que tarda en cambiar de engranaje a iman 
            //activar physics iman
        }
        else
        {
            //desactivar physics imán
            //desactivar si está a false (pq cuando se vuelva a preguntar puede ser que el iman ya esté activado el iman pero el bool esta en false asi que se queda como está)

        }

    }
    IEnumerator MovFase1()
    {
                yield return new WaitForSeconds(3);
        while (true)
        {
            if (misFases==Fases.faseRayo)
            {
                        
                if (destinoAct==Destinos.arriba)
                {
                    Debug.LogWarning("ARRIBA");
                    if (transform.position.y >= puntoAlto.position.y)
                    {
                            
                        if (registro==1)
                        {
                            registro =-0.5f;
                            StartCoroutine(aquedireccion());
                        }
                        else if(registro==-1)
                        {
                            registro = 0.5F;
                            StartCoroutine(aquedireccion());
                        }
                        else if (registro == -0.5)
                        {
                            registro=-1;
                        }
                        else if (registro == 0.5)
                        {
                            registro = 1;
                        }
                        CambioDestino();

                        Debug.LogWarning("ESTOY arrriba");
                    }
                    else
                    {
                        anim.SetBool("StartLaser", true);
                        rb.velocity = new Vector2(0, velocidad);
                    }


                }
                else if (destinoAct==Destinos.abajo)
                {
                    Debug.LogWarning("ABAJO");

                    if (transform.position.y <= puntoR.position.y)
                    {
                        yield return new WaitForSeconds(5);
                        CambioDestino();
                        Debug.LogWarning("ESTOY ABAJO");
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, velocidad * -1);
                    }

                }
                else if (destinoAct == Destinos.derecha)
                {
                    Debug.LogWarning("Derecha");
                    if (transform.position.x >= puntoR.position.x)
                    {

                        CambioDestino();
                    }
                    else
                    {
                        rb.velocity = new Vector2(velocidad , 0);
                    }
                }
                else if (destinoAct == Destinos.izquierda)
                {
                    Debug.LogWarning("Izquierda");
                    if (transform.position.x <= puntoL.position.x)
                    {
                        CambioDestino();
                    }
                    else
                    {
                        rb.velocity = new Vector2(velocidad * -1, 0);
                    }
                }
                else if (destinoAct == Destinos.centro)
                {
                    Debug.LogWarning("Centro");
                    if (aux == 1)
                    {
                        if (transform.position.x >= puntoC.position.x)
                        {
                            CambioDestino();
                        }
                        else
                        {
                            Debug.LogError("DERECHA PA C");
                            rb.velocity = new Vector2(velocidad, 0);
                        }
                    }
                    else
                    {
                        if (aux == -1 && transform.position.x <= puntoC.position.x)
                        {
                            CambioDestino();
                        }
                        else
                        {
                            Debug.LogError("IXQUIERDA PA C");
                            rb.velocity = new Vector2(velocidad * -1, 0);
                        }
                    }
                }

                

            }
            yield return null;
        }
    }
    IEnumerator aquedireccion()
    {
        yield return null;

        if (transform.position.x >= puntoC.position.x)
        {
            
            aux = -1;
        }
        else if (transform.position.x <= puntoC.position.x)
        {
            aux = 1;
        }
    }
    void CambioDestino()
    {
        if (destinoAct == Destinos.arriba)
        {
            if (registro==-0.5||registro==0.5)
            {
                destinoAct = Destinos.centro;
            }
            else if (registro==-1)
            {
                destinoAct = Destinos.izquierda;
            }
            else if(registro == 1)
            {
                destinoAct = Destinos.derecha;
            }
        }

        else if (destinoAct == Destinos.abajo)
        {
            destinoAct = Destinos.arriba;
        }
        else if (destinoAct==Destinos.derecha|| destinoAct == Destinos.izquierda || destinoAct == Destinos.centro)
        {
            destinoAct = Destinos.abajo;
        }
    }
    bool AreaDetect()
    {
        area.localScale = new Vector2(radioDect * 2, radioDect * 2);
        if (setArea)
        {
            area.gameObject.SetActive(true);
        }
        else
        {
            area.gameObject.SetActive(false);
        }

        if (Vector2.Distance(transform.position, player.transform.position) < radioDect)
        {
            detect = true;
            return true;
        }
        else
        {
            detect = false;
            return false;
        }
    }
    void RotarEngranajes(int rotacionZ) 
    {
        //instanciar engranajes en la punta del baston
        for (int i = 0; i < numEngranajes; i++)
        {
            rotacionZ = rotZ;

            Instantiate(engranajes[i], spwEngranajes.position, Quaternion.Euler(0, 0, rotacionZ)); //cambiar la z por un numero variable para sumar despues de cada instancia
            rotacionZ += rotZ;

        }
        //instanciar engranajes con corrutina
      
    }
    void LanzarEngranajes() //FALTA translacion (que se disparen)
    {
        engranajesActivados = false;  //para que se active solo una vez en el start                       
        RotarEngranajes(rotZ); //instanciar engranajes y rotarlos
        StartCoroutine(Timer(2));//esperar un par de segundos
        //que salgan disparados a las nuevas posiciones
        StartCoroutine(Timer(2)); //esperar un par de segundos
        //desplazarlos  hacia adelante con move forward y while?
        StartCoroutine(Timer(2)); //esperar un par de segundos
        engranajesActivados = true;
           
    }
 
   void TP() 
    {
        //para que se active solo una vez en el start
        //dos tp a cierta distancia y uno cerca con ataque con 1 o 2 segundos de espera antes del ataque 
        //mientras tpActivado a true en el update
        
        tpActivado = false;

        //TP 1
        Vector3 posPlayer = player.transform.localScale; //saber hacia donde mira el player
        Vector3 posBoss = transform.position;
        posBoss = spawnTpR1.transform.position; //tp al spawn 1
        posBoss = posPlayer; //que los dos miren al mismo lado?
        anim.SetTrigger("TP");
        StartCoroutine(Timer(1));

        //TP 2
        posBoss = transform.position;
        posBoss = spawnTpR2.transform.position; //tp al spawn 2
        anim.SetTrigger("TP");
        StartCoroutine(Timer(1));
        //look at transform del player

        //TP 3
        
        posPlayer = player.transform.localScale;
        posBoss = transform.position;
        posBoss = spawnTpR3.transform.position; //tp al spawn 3
        posBoss = posPlayer; //que los dos miren al mismo lado?
        anim.SetTrigger("TP");
        StartCoroutine(Timer(1));
        AtaqueBaston();

       
        StartCoroutine(Timer(2));  //esperar x tiempo para volver a tp 
        posBoss = spawnInicial.transform.position; //tp al spawn inicial
        anim.SetTrigger("TP");
        tpActivado = true;

        StartCoroutine(Timer(4));

    }  

    IEnumerator Timer(int tiempo) // StartCoroutineTimer(int tiempo));
    {
        finTemporizador = false;
        while (tiempo > 0) //darle valor a cada temporizador cuando se llame la corrutina
        {
            yield return new WaitForSeconds(1);
            tiempo--;
        }
        finTemporizador = true;
       // StartCoroutine(PararVolar());
    }

    private void OnTriggerStay2D(Collider2D collision) 
    {
        Collider2D plColl = player.GetComponent<Collider2D>();
        if (collision = plColl)
        {
            playerTocado = true;
        }
        if (collision.CompareTag("PlayerEnRango"))
        {
            playerEnRango = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
       
        if (collision.CompareTag("PlayerEnRango"))
        {
            playerEnRango = false;

        }
    }

    public void RecibirDano(int dano)
    {
        vida =  -dano;
    }

    // para el baston poner en el baston o el boss - collider2D (is trigger)+ Riggid body (kinematic) + tag iman
    //https://www.youtube.com/watch?v=XyVFmlViB-w
}


