using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeInvader : MonoBehaviour
{
    public float speed = 3f; // Velocidad  a la que s emueve
    public int points = 100; // Puntos que da al derrotarlo
    public int dir = 1; // Dirección del ovni (1 ---> derecha, -1 -> izquierda)
    public float deathAnimTime = 1f; 
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "SBorder") // Borde de la pantalla
        {
            Destroy(gameObject); // Se destruye
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SPlayerbullet") // Borde de la pantalla
        {
            Destroy (gameObject);
            GameManager.instance.AddScore(points); // Sumar puntos
            animator.Play("OVNI_Death");
            speed = 0;  
            Destroy(gameObject, deathAnimTime); // Se destruye
        }
    }
    public void DerribaOVNI()
    {
        GameManager.instance.AddScore(points); // Sumar puntos
        Destroy(gameObject);
    }
}
