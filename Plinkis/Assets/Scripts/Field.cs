﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Grid;
    [HideInInspector]
    public Transform[,] grid2D;
    public int width =10;
    public int height=20;
    [HideInInspector]
    public TetronimoSpawner spawner;
    string HeldID="";
    public event Action<string> OnHold;
    public event Action<int> OnTetris;
    public event Action OnPlinko;
    public bool hasHeld = false;
    GameObject currentTetronimo;
    void Awake()//create singleton
    {
        
        if (Grid == null)
        {
            Grid = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        grid2D = new Transform[width, height];
        spawner = transform.GetChild(0).GetComponent<TetronimoSpawner>();
        
    }
    void Start()
    {
        Invoke("PlayPlinko", 1);
        transform.localScale = new Vector3(width, height, 1);
        transform.position += Vector3.right * width / 2+Vector3.up*height/2;
        
        
    }
    public void SetActive(GameObject tetronimo)
    {
        currentTetronimo = tetronimo;
        print(currentTetronimo);
    }
    public void HoldTetronimo(string t)
    {
        if (hasHeld) return;
        

        
        OnHold?.Invoke(t);
        
        if (HeldID != "")
        {
            spawner.SpawnTetronimo(HeldID);
        }
        else
        {
            spawner.SpawnTetronimo();
        }
        hasHeld = true;
        HeldID = t;
    }
    public void HoldReset()
    {
        hasHeld = false;
    }
    public void AddToGrid(Transform t)//add every block to the grid for calculation
    {
        currentTetronimo = null;
        foreach (Transform child in t)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            grid2D[x, y] = child;
        }
        SwipeGrid();
    }
    void PlayPlinko()
    {
        spawner.enabled = false;
        OnPlinko?.Invoke();
        print(currentTetronimo);
        Destroy(currentTetronimo,0.4f);
        
    }
    public void StopPlinko(float multiplier)
    {
        spawner.enabled = true;
        spawner.SpawnTetronimo();
    }
    void SwipeGrid()//checks for lines
    {
        int lineCount=0;
        for(int i = height-1; i >= 0; i--)
        {
            if (ContainsLine(i))
            {
                lineCount++;
                DeleteLine(i);
                Fall(i);
            }
        }
        if (lineCount >= 4)
        {
            Invoke("PlayPlinko", 0.1f);
        }
        OnTetris?.Invoke(lineCount);
        print(lineCount);
    }
    bool ContainsLine(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if (grid2D[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }
    void DeleteLine(int y)
    {
        for(int x = 0; x < width; x++)
        {
            Destroy(grid2D[x, y].gameObject);
            grid2D[x, y] = null;
        }
    }
    void Fall(int i)
    {
        for(int y=i;y<height;y++)
        {
            for(int x = 0; x < width; x++)
            {
                if (grid2D[x, y] != null)
                {
                    grid2D[x, y - 1] = grid2D[x, y];
                    grid2D[x, y] = null;
                    grid2D[x, y-1].transform.position += Vector3.down;
                }
                
                
            }
        }

    }
    public void Die()
    {
        Pause();
        
    }
    public void Pause()
    {
        spawner.enabled = false;
    }
    void OnDrawGizmos()
    {
        
        Gizmos.DrawWireCube(transform.position+new Vector3(width/2,height/2,1), new Vector3(width, height));
    }
}
