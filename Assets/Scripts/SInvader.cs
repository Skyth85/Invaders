using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InvaderType {SQUID, CRAB, OCTOPUS};

public class SInvader : MonoBehaviour
{
    public InvaderType tipo = InvaderType.SQUID;
    public GameObject particulaMuerte;
    public bool isQuitting = false;
    public SInvaderMovement padre;
    public GameObject invaderBullet;
    public float bulletSpawnYOffset = -0.5f;
    public int puntosGanados = 10;
    public  Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot() // El alien dispara una bala 
    {
        Vector3 aux = transform.position + new Vector3(0, bulletSpawnYOffset, 0);
        Instantiate(invaderBullet, transform.position, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SBorder") // Choca con borde de pantalla
        {
            //Debug.Log("Colision alien");
            //Debug.Log("Colision con borde");
            padre.SwitchDirection();
        }
        else if (collision.tag == "SGameOverBarrier")
        {
            GameManager.instance.PlayerGameOver();
        }
        else if (collision.tag == "SPlayerBullet") // Chocacon un alien
        {
            GameManager.instance.AlienDestroyed(); 

            GameObject particula = Instantiate(particulaMuerte, transform.position, Quaternion.identity);
            // Destroy (particula. 0.3f); destruiamos la partícula dentor de 0,3f segundos
            // Stun a los alien (movimiento)
            padre.AlienDestroyedStun();
            //Suma puntos
            GameManager.instance.AddScore(puntosGanados);
            Destroy(collision.gameObject); // Destruye bala
            Destroy(gameObject);
        }
    }


    public void MovementAnimation()
    {
        if (tipo == InvaderType.SQUID)
        {
            animator.Play("alien1_idle");
        }
        else if (tipo == InvaderType.CRAB)
        {
            animator.Play("alien2_idle");
        }
        else if (tipo == InvaderType.OCTOPUS)
        {
            animator.Play("alien3_idle");
        }

        animator.Play("alien" + ((int)tipo + 1).ToString() + "_idle");

    }

    public void StunAnimation()
    {
        {
            if (tipo == InvaderType.SQUID)
            {
                animator.Play("alien1_stun");
            }
            else if (tipo == InvaderType.CRAB)
            {
                animator.Play("alien2_stun");
            }
            else if (tipo == InvaderType.OCTOPUS)
            {
                animator.Play("alien3_stun");
            }

            animator.Play("alien" + ((int)tipo + 1).ToString() + "_idle");
        }
    }
}