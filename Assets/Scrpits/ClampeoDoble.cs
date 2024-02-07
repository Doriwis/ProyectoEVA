using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampeoDoble : MonoBehaviour
{
    Vector3 objetivo;
    Camera camara;

    
    void Clampeo2()
    {
        objetivo = camara.ScreenToWorldPoint(Input.mousePosition);
        float anguloRad = Mathf.Atan2(objetivo.y - transform.position.y, objetivo.x - transform.position.x);
        float anguloReal = ((180 * anguloRad) / Mathf.PI);
        float anguloClamp;

        Debug.Log("ANGULO " + anguloReal);
        if (transform.localScale.x > 0)
        {
            Debug.Log("DERECHA");
            if (anguloReal < 60 && anguloReal > -60)
            {
                Debug.Log("DENTRO ENTRE 60 Y -60");
                anguloClamp = anguloReal;
            }
            else if (anguloReal > 60)
            {
                Debug.Log("MAYOR QUE 60");
                anguloClamp = 60;
            }
            else
            {
                Debug.Log("MENOR QUE -60");
                anguloClamp = -60;
            }
        }
        else
        {
            Debug.Log("IZQUIERDA");
            if (anguloReal < -120 && anguloReal > -180 || anguloReal > 120 && anguloReal < 180)
            {
                Debug.Log("DENTRO ENTRE 120 Y -120");
                if (anguloReal > 0)
                {
                    anguloClamp = (180 - anguloReal) * -1;
                    Debug.Log("POSITIVO Y ABSOLUTO ES " + anguloClamp);
                }
                else
                {

                    anguloClamp = 180 + anguloReal;
                    Debug.Log("NEGATVIO Y ABSOLUTO ES " + anguloClamp);
                }

            }
            else if (anguloReal > -120 && anguloReal < 0)
            {
                Debug.Log("MAYOR QUE -120");
                anguloClamp = 60;
            }
            else
            {
                Debug.Log("MENOR QUE 120");
                anguloClamp = -60;
            }

        }
    }
}
