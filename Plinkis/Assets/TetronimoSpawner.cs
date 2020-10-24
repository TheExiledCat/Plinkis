using System;
using System.Collections.Generic;
using UnityEngine;

public class TetronimoSpawner : MonoBehaviour
{
    [SerializeField]
    UnityEngine.Object[] tetronimos,NextList ;
    [SerializeField]
    int index = 0;
    bool first=true;
    public event Action OnSpawn;
    private void Awake()
    {
        tetronimos = Resources.LoadAll("Tetronimos");
        NextList = new UnityEngine.Object[7];
    }
    private void Start()
    {
        for (int i = 0; i < tetronimos.Length; i++)
        {
            NextList[i] = tetronimos[i];
        }
        Shuffle();
        OnSpawn?.Invoke();
        
        SpawnTetronimo();
    }
    public void SpawnTetronimo()//spawn next tetronimo
    {
        
        OnSpawn?.Invoke();
        if (index >= tetronimos.Length)
        {
            Shuffle();
            index = 0;
        }
        Instantiate(tetronimos[index], transform.localPosition, Quaternion.identity);
        print(tetronimos[index]);
        index++;
        OnSpawn?.Invoke();
    }
    public void SpawnTetronimo(string id)
    {
        OnSpawn?.Invoke();
        Instantiate(Resources.Load<GameObject>("Tetronimos/" + id+" Block"), transform.localPosition, Quaternion.identity);
        
    }
    void Shuffle()//shuffle the set so a new random order is created
    {
        if (first)
        {
            
            for (int i = 0; i < tetronimos.Length; i++)
            {
                UnityEngine.Object obj = tetronimos[i];
                int random = UnityEngine.Random.Range(0, i);
                tetronimos[i] = tetronimos[random];
                tetronimos[random] = obj;
            }
            for (int i = 0; i < tetronimos.Length; i++)
            {
                UnityEngine.Object obj = NextList[i];
                int random = UnityEngine.Random.Range(0, i);
                NextList[i] = NextList[random];
                NextList[random] = obj;
            }
        }
        else
        {
            for (int i = 0; i < tetronimos.Length; i++)
            {
                tetronimos[i] = NextList[i];
            }
            for (int i = 0; i < tetronimos.Length; i++)
            {
                UnityEngine.Object obj = NextList[i];
                int random = UnityEngine.Random.Range(0, i);
                NextList[i] = NextList[random];
                NextList[random] = obj;
            }
            
        }
        
        





        first = false;

        OnSpawn?.Invoke();
    }
    public string[] GetSpawns()
    {
        

        
        GameObject A = (index + 1 <= tetronimos.Length - 1 ? tetronimos[index ] : NextList[0]) as GameObject;
        print(A);
        
        GameObject B = (index + 2 <= tetronimos.Length - 1 ? tetronimos[index + 1] : NextList[index-tetronimos.Length+2]) as GameObject;
        
        string[] ids = { A.GetComponent<Tetronimo>().tetronimoID, B.GetComponent<Tetronimo>().tetronimoID};
        return ids;
    }
}
