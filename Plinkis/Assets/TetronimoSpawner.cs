using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetronimoSpawner : MonoBehaviour
{
    Object[] tetronimos;
    int index = 0;
    private void Awake()
    {
        tetronimos = Resources.LoadAll("Tetronimos");
        
    }
    private void Start()
    {
        Shuffle();
        SpawnTetronimo();
    }
    public void SpawnTetronimo()//spawn next tetronimo
    {
        if (index >= tetronimos.Length)
        {
            Shuffle();
            index = 0;
            print(tetronimos);
        }
        Instantiate(tetronimos[index], transform.localPosition, Quaternion.identity);
        print(tetronimos[index]);
        index++;
    }
    void Shuffle()//shuffle the set so a new random order is created
    {
        for (int i = 0; i < tetronimos.Length; i++)
        {
            Object obj = tetronimos[i];
            int random = Random.Range(0, i);
            tetronimos[i] = tetronimos[random];
            tetronimos[random] = obj;
        }
    }
}
