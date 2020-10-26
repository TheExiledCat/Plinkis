using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlinkoGame : MonoBehaviour
{
    
    GameObject[,] dotsUneven;
    GameObject[,] dotsEven;
    [SerializeField]
    PlinkoSpawn[] spawns;
    [SerializeField]
    GameObject[] drops;
    int amount = 5;
    int rowsPerDots = 4;
    float[] multipliers;
    bool canPlay = false;
    GameObject Ball;
    float ballSpeed = 2;
    [SerializeField]
    int currentRowUneven=0,currentRowEven = 0;
    bool even=false;
    int currentCollumn = 0;
    GameObject target;
    public event Action<float> OnPlinkoCompleted;
    // Start is called before the first frame update
    
    void Start()
    {
        Field.Grid.OnPlinko += StartPlinko;
        OnPlinkoCompleted += Field.Grid.StopPlinko;
        
      
        
        for(int i =0; i < spawns.Length; i++)
        {
          spawns[i].OnClick += SpawnBall;
        }
        multipliers = new float[amount];
        
        GameObject[] Uneven = GameObject.FindGameObjectsWithTag("RowUneven");
        GameObject[] Even = GameObject.FindGameObjectsWithTag("RowEven");
        dotsUneven = new GameObject[amount, rowsPerDots];
        dotsEven = new GameObject[amount - 1, rowsPerDots];
        for (int y=0;y< Uneven.Length;y++)
        {
            for (int x = 0; x < dotsUneven.GetLength(0); x++)
            {
                dotsUneven[x, y] = Uneven[y].transform.GetChild(x).gameObject;
            }
        }
        for (int y = 0; y < Even.Length; y++)
        {
            for (int x = 0; x < dotsEven.GetLength(0); x++)
            {
                dotsEven[x, y] = Even[y].transform.GetChild(x).gameObject;
            }
        }
        
    }
    void InitializeMultipliers()
    {
        for(int i = 0; i < multipliers.Length; i++)
        {
            multipliers[i] = (0.25f * UnityEngine.Random.Range(1, 5));
        }
        for (int i = 0; i < drops.Length; i++)
        {
            drops[i].transform.GetChild(0).GetComponent<TMP_Text>().text = multipliers[i].ToString();
        }
    }
    void StartPlinko()
    {
        print("starting");
        InitializeMultipliers();
        canPlay = true;
        currentCollumn = 0;
        currentRowEven = 0;
        currentRowUneven = 0;
        Camera.main.GetComponent<Lerper>().LerpTo(transform.position,3f);
    }
    void SpawnBall(int collumn)
    {
        if (canPlay)
        {
            print("spawning ball at " + collumn);
            Ball = Instantiate(Resources.Load<GameObject>("PlinkoObjects/Ball"), spawns[collumn].transform.position, Quaternion.identity);
            canPlay = false;
            currentCollumn = collumn;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (currentRowUneven == 0)
        {
            print("initialize");
            target = dotsUneven[currentCollumn, currentRowUneven];
        }
        if (Ball)
        {
            Ball.transform.position = Vector3.MoveTowards(Ball.transform.position, target.transform.position, ballSpeed / 50);
            if (Ball.transform.position == target.transform.position)
            {
                if (target.CompareTag("BallDrop"))
                {
                    ChooseMultiplier(currentCollumn);
                }
                bool left = (UnityEngine.Random.value < 0.5f) ? true : false;
                
                if (currentRowEven+currentRowUneven == 7)
                {
                    target = drops[currentCollumn];
                    target = left ? drops[currentCollumn - 1] : drops[currentCollumn];
                    
                }
                
                print("changing");
                
                if(currentRowEven+currentRowUneven<7)
                if (!even)
                {

                    if (currentCollumn == 0)
                    {
                        left = false;
                    }
                    else if (currentCollumn == dotsUneven.GetLength(0))
                    {
                        left = true;
                    }
                    target = left ? dotsEven[currentCollumn , currentRowEven] : dotsEven[currentCollumn+1, currentRowEven];

                        currentCollumn = left ? currentCollumn  : currentCollumn+1;
                        even = true;
                        currentRowUneven++;
                    
                    
                }
                else
                {
                    
                    target = left ? dotsUneven[currentCollumn - 1, currentRowUneven ] : dotsUneven[currentCollumn, currentRowUneven ];
                    
                    currentCollumn = left ? currentCollumn - 1 : currentCollumn;
                    even = false;
                    currentRowEven++;
                }
                
            }
            
        }
        
    }
    void ChooseMultiplier(int i)
    {
        AddMultiplier(multipliers[i]);
        Destroy(Ball);
        Camera.main.GetComponent<Lerper>().LerpBack();
    }
    void AddMultiplier(float multiplier)
    {
        OnPlinkoCompleted?.Invoke(multiplier);
    }
}
