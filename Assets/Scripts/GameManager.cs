using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Lista doble (matriz) de SInvaders
    public SInvader[,] matrizAliens;
    // Nº filas de invaders, alto
    public const int nFilas = 5; // Constante, no es variable, no se puede cambiar
    // Nº filas de invaders, ancho
    public const int nColumnas = 11;

    private DemoPlayerMove player;

    public TextMeshPro highScoreText;

    // Start is called before the first frame update
    // Prefab Alien
    public GameObject alien1Prefab;
    public GameObject alien2Prefab;
    public GameObject alien3Prefab;
    public int defeatedAliens = 0;


    //GameObject padre del los aliens (movimiento)
    public SInvaderMovement padreAliens;
    //Distancia entre aliens al spawnearlos
    public float distanciaAliens = 1;
    public int highScore;

    public float tiempoEntreDisparos = 2f;
    // Ciclo de juego
    // Fin de la partida
    public bool gameOver = false;
    // Vidas actuales del jugador
    public int vidas = 3;

    public TextMeshPro LivesText;
    public TextMeshPro ScoreText;
    public GameObject spriteVida2;
    public GameObject spriteVida3;
    public float incrVel;

    public static GameManager instance = null;

    public int score = 0;


    // Singleton
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public float playerDamageDelay = 1f;

    // OVNIS
    public GameObject prefabOVNI;       // Prefab del OVNI
    public Transform spawnIzqOVNI;      // Posicion de OVNI a la izquierda
    public Transform spawnDerOVNI;      // Posición de Ovni a la derecha
    public float spawnOVNITime = 15f;   // Tiempo que tarda un OVNI en aparecer

    void Start()
    {
        player = FindFirstObjectByType<DemoPlayerMove>();

        // Decimos que matrizAliens es una nueva matriz de SInvaders de nColumnas x nFilas
        // - INICIALIZACIÓN
        matrizAliens = new SInvader[nColumnas, nFilas];
        SpawnAliens();
        InvokeRepeating("SelectAlienShoot", 2f, 2f);
        
        // Saco la puntuación máxima guardada en el archivo PlayerPrefs
        highScore = PlayerPrefs.GetInt("HIGH-SCORE");
        highScoreText.text = "HI-SCORE'\n'" + highScore.ToString();
    }

    void SpawnAliens()
    {
        // Doble bucle (anidado) que recorra la matriz (de 11 x 5, rangos 0-10 y 0-4)
        for (int j = 0; j < nColumnas; j++)
        {
            for(int i = 0; i < nFilas; i++)
            {
                GameObject prefab;
                if (i == 4) prefab = alien1Prefab;
                else if (i < 2) prefab = alien3Prefab;
                else prefab = alien2Prefab;
                //Dentro de los dos bucles, intercambiamos un alien
                SInvader auxAlien = Instantiate(prefab, padreAliens.transform).GetComponent<SInvader>();
                matrizAliens[j, i] = auxAlien;
                //Colocamos  el alien
                auxAlien.transform.position += new Vector3(j-nColumnas/2,i-nFilas/2,0) * distanciaAliens;
                // Asignamos padre movement al alien
                auxAlien.padre = padreAliens;
            }
        }
        // Dentro de los dos bucles, instanciamos un alien
        // Lo guardamos en la posición de la matriz apropiada
        // Colocamos un alien
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Busca el alien más cercano al jugador en una columna aleatoria y le dice que dispare
    private void SelectAlienShoot()
    {
        bool encontrado = false;
        if (!gameOver)
        {
            while (!encontrado) // Se repito con columnas aleatorias hasta encontrar un alien
            {
                // 1 - Elegir una columna aleatoria que no esté vacia
                int randomCol = Random.Range(0, nColumnas); // Columna aleatoria

                // 2 - Buscar al alien más cercano al jugador en esa columna
                //En este for, tenemos dos condiciones, que j > -1 y que encontrado == false
                // como usamos && entre elllas (el AND), deben cumplirse las dos, o salimos del bucle for
                for (int i = 0; i < nFilas && !encontrado; i++) // Recorrer la columna aleatoria de delante a atrás
                {
                    // Compruebo si el alien existe (no se ha destruido)
                    if (matrizAliens[randomCol, i] != null) // Si la casilla no está vacia (null) el alien sigue vivo
                    {
                        // Si encuentro un alien vivo, es el más cercano de la columna al jugador
                        // porque la estoy recorriendo de abajo a arriba
                        matrizAliens[randomCol, i].Shoot(); // El alien dispara
                        encontrado = true; // He acabado la búsqueda
                    }
                }
            }
        }

    }

    public void DamagePlayer()
    {
        if (!gameOver && player.GetCanMove())
        {
            vidas--;
            UpdateLifeUI();
            // Animción de daño del jugador 
            player.PlayerDamaged();
            padreAliens.canMove = false; // Bloqueo los aliens
            SetInvadersAnim(false);
            Invoke("UnlockDamagedPlayer", playerDamageDelay);
            // avisarle

            if (vidas <= 0)
            {
                PlayerGameOver();
            }
        }
    }

    private void UnlockDamagedPlayer()
    {
        player.PlayerReset();
        padreAliens.canMove = true; // Desbloqueo los aliens
        SetInvadersAnim(true);
    }

    private void UpdateLifeUI()
    {
        // Actualiza el texto
        LivesText.text = vidas.ToString();
        // Actualizar los sprites de las vidas
        spriteVida2.SetActive(vidas >= 2); // Se activa si vidas >= 2
        spriteVida3.SetActive(vidas >= 3); // Se activa si vidas >= 3
    }
    public void PlayerGameOver()
    {
        gameOver = true;
        Invoke("ResetGame", 2);
        //UnityEngine.Debug.Log("el jugador ha perdido");
    }

    public void PlayerWin() 
    {
        gameOver = true;
        CancelInvoke(); // Interrumpinos los invokes de este componente (se deja de disparar)
        UnityEngine.Debug.Log("el jugador ha ganado");
        Invoke("ResetGame", 2); // Reinicio la partida en 2 segundos
    }
    public void CheckPlayerWin()
    {
        if(defeatedAliens >= nFilas * nColumnas)    // Si a derrotado a todos los aliens
        {
            PlayerWin(); // El jugador gana
        }
    }


    public void AlienDestroyed()
    {
        defeatedAliens++;
        // Actualizar velocidad de los aliens según cuantos quedan
        // Suma incrVelocidad / aliensTotales
        // (1 + (aliensDerrotados / alienTotales) * incVelocidad) * speedAliens
        // 1 + (0/55) * 3) * 2 = 2
        // 1 + (55/55) * 3) * 2 = 8

        padreAliens.speed += (1f / (float)defeatedAliens / (float)(nFilas * nColumnas) * incrVel) * incrVel;

        

        if(defeatedAliens >= nFilas * nColumnas) // Si ha derrotado a todos los aliens
        {
            PlayerWin();
        }
    }
    public void ResetGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void UpdateHighScore()
    {
        if (score > highScore)
        { // si mi puntuación es mayor que la máxima
            PlayerPrefs.SetInt("HIGH-SCORE", score);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        ScoreText.text = "SCORE\n" + score.ToString(); //Actualizar texto puntos
    }
    // Recorre la lista de alien, y les pone la animación indicada
    public void SetInvadersAnim(bool movement)
    {
        for (int j = 0; j < nColumnas; j++)
        {
            for (int i = 0; i < nFilas; i++)
            {
                if (matrizAliens[j, i] != null)
                {
                    if (movement) matrizAliens[j, i].MovementAnimation();
                    matrizAliens[j, i].StunAnimation();
                }
            }
        }
    }
    void SpawnOVNI()
    {
        // Elegir una direccion aleatoria
        int random = Random.Range(0, 2);    // Entre 0 y 1

        //Si sale 0, a la izquierda
        if(random == 0)
        {
            Instantiate(prefabOVNI, spawnIzqOVNI).GetComponent<JefeInvader>().dir = 1;// Crearlo
        }

        else if (random == 1) // Si sale 1, a la derecha
        {
            Instantiate(prefabOVNI, spawnDerOVNI).GetComponent<JefeInvader>().dir = -1;
        }
        //Ponerle la dirección apropiada
    }
}


//matrizAliens.GetLength(0)