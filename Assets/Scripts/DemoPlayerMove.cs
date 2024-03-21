using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayerMove : MonoBehaviour
{
    [Tooltip("Prefab de la bala")]
    public GameObject prefabBullet;

    [Tooltip("Velocidad del jugador en unidades de unity / segundo")]
    public float speed = 2;
    // Teclas para input configurable
    public KeyCode shootKey = KeyCode.Space;
    public KeyCode moveLeft = KeyCode.D;
    public KeyCode moveRight = KeyCode.A;
    public float limiteHorizontal = 8f;

    public Transform posDisparo;

    public bool canShoot = true;
    public bool canMove = true;

    public Animator pAnimator;
    private Vector3 posInicial;
    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position;
        pAnimator.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            InputPlayer();    
        }
    }
    private void InputPlayer()
    {
        if (canShoot && Input.GetKeyDown(shootKey))
        {
            //DISPARA
            shoot();
        }
        else if (Input.GetKey(moveLeft))
        {
            //Voy a la izquierda
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
            if (transform.position.x < -limiteHorizontal)
            {
                Vector3 aux = transform.position;
                aux.x = -limiteHorizontal;
                transform.position = aux;
            }
        }
        else if (Input.GetKey(moveRight))
        {
            //Voy a la derecha
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
            if (transform.position.x > limiteHorizontal)
            {
                Vector3 aux = transform.position;
                aux.x = limiteHorizontal;
                transform.position = aux;   
            }
        }
    }
    private void shoot()
    {
        Instantiate(prefabBullet, posDisparo.position, Quaternion.identity).GetComponent<DemoPlayerBullet>().player = this ;
        canShoot = false;
    }

    public void PlayerDamaged()
    {
        pAnimator.Play("player_death");
        canMove = false;
    }

    private void OnDestroy()
    {
        Debug.Log("Destruido");
        // El jugador puede volver a disparar
        // player.canShoot = true;
    }
    public void PlayerReset()
    {
        pAnimator.Play("player_idle");
        canMove = true;
        transform.position = posInicial;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetCanMove(bool b)
    {
        canMove = b;
    }
}
