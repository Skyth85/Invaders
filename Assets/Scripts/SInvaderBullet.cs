using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInvaderBullet : MonoBehaviour
{
    public float speed = 3;
    [HideInInspector]
    public DemoPlayerMove player;
    public GameObject bulletExplosion;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SBorder")  //Choca con una barrera
        {
            Destroy(this.gameObject); //Se destruye la bala
            Instantiate(bulletExplosion, transform.position, Quaternion.identity); // Crea efecto de explosion de bala
        }
        else if (collision.tag == "Player")  //Choca con una barrera
        {
            GameManager.instance.DamagePlayer();
            Destroy(gameObject); //Se destruye el alien
                                           // SGameManager.instance
        }
        else if (collision.tag == "SBarrier")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject); //Se destruye la barrera
            Instantiate(bulletExplosion, transform.position, Quaternion.identity); //Se destruye la bala
        }
    }
}