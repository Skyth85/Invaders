using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuclesAnidados : MonoBehaviour
{
    public int[,] listaNumeros; //La coma es para que sea de dos dimensiones
    public List<int> listaDinamicaNumeros;
    public int linea;
    // Start is called before the first frame update
    void Start()
    {
        listaNumeros = new int[5, 10];
        Debug.Log(listaNumeros.Length);
        Debug.Log(listaNumeros.GetLength(0));
        Debug.Log(listaNumeros.GetLength(1));
        

        listaNumeros = new int[10, 10];

        for (int i = 0; i < listaNumeros.GetLength(0); i++)
        {
            string tablaDeI = "Esta es la tabla de" + (i+1).ToString() + "";
            for(int j = 0; j < listaNumeros.GetLength(1); j++) 
            {   
                listaNumeros[i, j] = (i+1)*(j+1);
                tablaDeI += listaNumeros[i, j].ToString() + "s";
                Debug.Log(listaNumeros[i, j]);  
            }
        }
        Debug.Log(listaNumeros);
    }

    public void DibujaMatriz(int[,] matriz)
    {
        string texto = "";
        for (int j = 0; j < matriz.GetLength(1); j++) // Recorro 2ª dimensión
        {
            for(int i = 0; i < matriz.GetLength(0); i++) // Recorro 1ª dimensión
            {
                texto += matriz[j, i].ToString() + " ";
            }
            texto += '\n';
        }
            Debug.Log(texto);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
