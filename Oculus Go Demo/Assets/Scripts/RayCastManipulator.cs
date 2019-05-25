
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastManipulator : MonoBehaviour
{
    private LineRenderer lineaSeleccion;

    public GameObject origen;
    //public ArcRaycaster arcRaycaster;
    public List<GameObject> objetos_admitidos;

    public Text textoInfo;

    public Color c1 = Color.blue;
    public Color c2 = Color.red;
    private int lengthOfLineRenderer = 2;

    private GameObject objetivo;
    private GameObject objeto_capturado;


    public GameObject agarre;


    private bool got_object = false;
    private bool lastTriggerState = false;


    // Start is called before the first frame update
    void Start()
    {

        //objetivo = new GameObject();

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.positionCount = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

        textoInfo.text = "Ningún Objeto seleccionado";
    }

    // Update is called once per frame
    void FixedUpdate()
    {  

        Vector3 posicion_apuntador = origen.transform.position;

        c1 = Color.blue;
        c2 = Color.red;


        bool detectado = ObjetoDetectado(0f);
        bool gatillo = DetectarEstadoGatillo();

        try
        {
            if (got_object)
            {
                c1 = Color.cyan;
                c2 = Color.blue;
                posicion_apuntador = objeto_capturado.transform.position;
                TrazarLinea(posicion_apuntador);

                if (gatillo)
                {
                    EscribirInfo("Liberado: " + objeto_capturado.name);
                    objeto_capturado.GetComponent<Rigidbody>().useGravity = true;
                    objeto_capturado = new GameObject();
                    objetivo = null;
                    got_object = false;

                }
                else
                {
                    EscribirInfo("En movimiento: " + objeto_capturado.name);
                    Vector3 vectorMovimiento = agarre.transform.position;
                    objeto_capturado.GetComponent<Rigidbody>().useGravity = false;
                    objeto_capturado.GetComponent<Rigidbody>().MovePosition(vectorMovimiento);
                }

            }
            else
            {
                if (detectado)
                {

                    posicion_apuntador = objetivo.transform.position;
                    TrazarLinea(posicion_apuntador);
                    EscribirInfo("Detectado: " + objetivo.name);

                    if (gatillo)
                    {
                        objeto_capturado = objetivo;
                        objetivo.GetComponent<Rigidbody>().useGravity = false;
                        got_object = true;
                        EscribirInfo("Capturado: " + objetivo.name);
                    }

                }
                else
                {
                    EscribirInfo("Sin acciones");
                    posicion_apuntador = origen.transform.position;
                    TrazarLinea(posicion_apuntador);
                }
            }

        }
        catch
        {
            //Debug.Log(ex.Message);
        }
    }

    private bool DetectarEstadoGatillo()
    {
        
        bool currentTriggerState = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        bool accion = false;

        // If the trigger was released this frame
        if (lastTriggerState && !currentTriggerState)
        {
            accion = true;
        }

        lastTriggerState = currentTriggerState;

        return accion;

    }


    //Invoked when a button is pressed.
    public void SetParent(GameObject objetivo, GameObject control)
    {
        //Makes the GameObject "newParent" the parent of the GameObject "player".
        objetivo.transform.SetParent(control.transform);

        //Display the parent's name in the console.
        //Debug.Log("Player's Parent: " + control.transform.parent.name);

        // Check if the new parent has a parent GameObject.
        if (control.transform.parent != null)
        {
            //Display the name of the grand parent of the player.
            Debug.Log("Player's Grand parent: " + control.transform.parent.parent.name);
        }
    }

    public void DetachFromParent(GameObject objetivo)
    {
        // Detaches the transform from its parent.
        objetivo.transform.SetParent(null);
    }
    

    private bool ObjetoDetectado(float range)
    {
        RaycastHit hit;
        bool objValido = false;

        Rigidbody objeto = new Rigidbody();

        if (range == 0) range = Mathf.Infinity;

        bool valor = false;

        // Bit shift the index of the layer (8) to get a bit mask
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        valor = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask);

        if (valor)
        {

            objeto = hit.rigidbody;

            foreach (GameObject go in objetos_admitidos)
            {
                if (objeto.name == go.name)
                {
                    objetivo = go;
                    objValido = true;
                }
            }

        }

        return objValido;
    }


    private void TrazarLinea(Vector3 fin)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var points = new Vector3[2];

        lineRenderer.startColor = c1;
        lineRenderer.endColor = c2;

        points[0] = origen.transform.position;
        points[1] = fin; //new Vector3(posicion_apuntador.x, posicion_apuntador.y, posicion_apuntador.z);

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPositions(points);
    }

    void EscribirInfo(string texto)
    {
        textoInfo.text = "Info: " + texto;
    }

}


