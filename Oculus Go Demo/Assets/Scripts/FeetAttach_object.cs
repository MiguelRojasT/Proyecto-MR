using UnityEngine;

public class FeetAttach_object : MonoBehaviour
{
    public GameObject Mesa_construccion;
    public GameObject Plano_Construccion;
    public GameObject Parte_actual;

    public GameObject Suelo;
    public GameObject Paredes;

    private Vector3 posicion_inicial;
    private Vector3 posicion_ensamble;
    private Quaternion angulo_inicial;
    private bool posicion_correcta;
    private bool caido;


    public float angulo_x;
    public float angulo_y;
    public float angulo_z;

    // Start is called before the first frame update
    void Start()
    {
        float offset = 0.65F;

        posicion_inicial = Parte_actual.transform.position;

        angulo_inicial = Quaternion.Euler(angulo_x, angulo_y,angulo_z);

        posicion_ensamble = Plano_Construccion.transform.position;
        posicion_ensamble.y = posicion_ensamble.y + offset;
        posicion_correcta = false;
        caido= false;

}

    void Update()
    {
        if (posicion_correcta)
        {
            Parte_actual.GetComponent<Rigidbody>().MoveRotation(angulo_inicial);
            Parte_actual.GetComponent<Rigidbody>().MovePosition(posicion_ensamble);
        }

        if (caido)
        {
            Parte_actual.GetComponent<Rigidbody>().MovePosition(posicion_inicial);
            Parte_actual.GetComponent<Rigidbody>().MoveRotation(angulo_inicial);
            caido = false;
        }

    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Tocando la mesa");
        Debug.Log(col.gameObject.name);



        if (col.gameObject.name == Mesa_construccion.name)
        {
           
            posicion_correcta = true;

            DetachFromParent(Parte_actual);

            Parte_actual.GetComponent<Rigidbody>().useGravity = false;
            //Parte_actual.GetComponent<Rigidbody>().detectCollisions = false;
            Parte_actual.GetComponent<Rigidbody>().isKinematic = true;
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
    void OnCollisionStay(Collision col)
    {
        // Debug.Log(col.gameObject.name);

    }
    void OnCollisionExit(Collision col)
    {
        // Debug.Log(col.gameObject.name);

    }

    public void DetachFromParent(GameObject objetivo)
    {
        // Detaches the transform from its parent.
        objetivo.transform.SetParent(null);
    }
}
