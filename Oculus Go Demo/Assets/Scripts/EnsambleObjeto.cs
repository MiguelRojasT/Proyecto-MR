using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnsambleObjeto : MonoBehaviour
{
    public GameObject Parte_Referente;
    public GameObject Parte_Actual;
    public GameObject Suelo;
    public GameObject Paredes;

    public float offset_y = 2F;
    public float offset_x = 0.2F;
    public float offset_z = 0.2F;

    public float ang_x = 0;
    public float ang_y = 0;
    public float ang_z = 0;


    private Vector3 posicion_inicial;
    private Vector3 posicion_ensamble;
    private Quaternion angulo_inicial;
    private Vector3 posicion_final;
    private Quaternion angulo_final;

    private bool posicion_correcta;
    
    private bool ensamblado;
    private bool caido;

    // Start is called before the first frame update
    void Start()
    {
        posicion_inicial = Parte_Actual.transform.position;

        angulo_inicial = Quaternion.Euler(ang_x, ang_y, ang_z);

        posicion_ensamble = Parte_Referente.transform.position;
        posicion_ensamble.y = posicion_ensamble.y + offset_y;
        posicion_correcta = false;
        ensamblado = false;
        caido = false;

    }

    void Update()
    {
        posicion_ensamble = Parte_Referente.transform.position;
        posicion_ensamble.y = posicion_ensamble.y + offset_y;

        if (posicion_correcta && !ensamblado)
        {


            Parte_Actual.GetComponent<Rigidbody>().MoveRotation(angulo_inicial);
            Parte_Actual.GetComponent<Rigidbody>().MovePosition(posicion_ensamble);

           
            
            //Destroy(Parte_Actual.GetComponent<Rigidbody>());

            ensamblado = true;
        }

        if (caido)
        {
            Parte_Actual.GetComponent<Rigidbody>().MovePosition(posicion_inicial);
            Parte_Actual.GetComponent<Rigidbody>().MoveRotation(angulo_inicial);
            caido = false;
        }

        if (ensamblado)
        {
            Parte_Actual.GetComponent<Rigidbody>().MoveRotation(angulo_inicial);
            Parte_Actual.GetComponent<Rigidbody>().MovePosition(posicion_ensamble);
        }

    }

    void OnCollisionEnter(Collision col)
    {

        if (!ensamblado)
        {


            if (col.gameObject.name == Parte_Referente.name)
            {
                posicion_correcta = true;
                Debug.Log(posicion_correcta.ToString());

                Parte_Actual.GetComponent<Rigidbody>().useGravity = false;
                //Parte_Actual.GetComponent<Rigidbody>().detectCollisions = false;
                Parte_Actual.GetComponent<Rigidbody>().isKinematic = true;

                

            }

            if (col.gameObject.name == Suelo.name)
            {
                caido = true;
            }
            if (col.gameObject.name == Paredes.name)
            {
                caido = true;
            }

        }

    }
    void OnCollisionStay(Collision col)
    {
        // Debug.Log(col.gameObject.name);

    }
    void OnCollisionExit(Collision col)
    {
        // Debug.Log(col.gameObject.name);

    }
}