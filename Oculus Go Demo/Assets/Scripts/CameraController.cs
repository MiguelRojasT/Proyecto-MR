using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public float factor_escala = 1;
    public float factor_offset_y = 0;
    private Vector3 offset;

    void Start()
    {
        offset = Vector3.zero;// transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        
        Vector3 posicion_nueva = new Vector3();

        posicion_nueva.x = player.transform.position.x * factor_escala;
        posicion_nueva.z = player.transform.position.z * factor_escala;
        posicion_nueva.y = player.transform.position.y + factor_offset_y;

        Debug.Log("==================================================================");
        Debug.Log("==================================================================");
        Debug.Log("==================================================================");
        Debug.Log("Moviendo:   ");
        Debug.Log("Moviendo referencia:   ");
        Debug.Log(player.name);
        Debug.Log(posicion_nueva);
        Debug.Log("==================================================================");
        Debug.Log("==================================================================");
        Debug.Log("==================================================================");


        transform.position = posicion_nueva;


       // transform.rotation = player.transform.rotation;
    }
}