using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInvaderMovement : MonoBehaviour
{
    public float speed = 2; // velocidad de movimiento en eje x
    public float despAbajo = 1f; // distancia que baja al cambiar de dirección 
    private int dir = 1;
    [HideInInspector]
    public float originalSpeed = 3f; // Velocidad 

    public bool canSwitch = true; // Bool que indica si puede girarse
    public float switchDelay = 0.5f; // Tiempo que debe pasar despues de girar, para poder volver a hacerlo
    public bool canMove = true; // Bool que indica di puede moverse
    public float moveStunTime = 0.5f;


    private GameManager gm;

    /*
     * 1 - Despues de girar, pongo canSwitch a false
     * 2 - Crear una función que ponga canSwitch a true
     * 3 - A la vez que pongo canSwitch a false, hago Invoke del método que cree antes, con tiempo switchDelay
     * 4 - El switchDir, el código no se ejecuta si el canSwitch es true, es decir, if (canSwitch==true)
     */ 


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.gameOver && canMove)
        {
            Movement();
        }
    }
    private void Movement()
    {
        transform.position += new Vector3(speed,0,0) * dir * Time.deltaTime;
    }
    public void SwitchDirection()
    {
        if (canSwitch == true) // Solo gira si canSwitch == true
           {
                 dir *= -1; // Invierto la dirección (1 y -1)
                 transform.position += new Vector3(0, -despAbajo, 0);// Desplazarme hacia abajo
                 canSwitch = false; // Desactivo el giro
                 Invoke("enableSwitch", switchDelay); // Reactivo el giro en switchDelay segundos
           }
    }

    public void enableSwitch()
    {
        canSwitch = true;
    }

    public void EnableMovement()
    {
        canMove = true;
        gm.SetInvadersAnim(true); 
    }

    public void AlienDestroyedStun()    // Metodo que se llama cuando se destrye un alien y que para su moviemento un tiempo
    {
        gm.SetInvadersAnim(false);
        canMove = false; // Paramos el movimiento
        Invoke("EnableMovement", moveStunTime);//Reactivamos el movimiento tras un tiempo 
    }
}
