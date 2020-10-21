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
    }
    void Start()
    {
        transform.localScale = new Vector3(width, height, 1);
        transform.position += Vector3.right * width / 2+Vector3.up*height/2;
        spawner = transform.GetChild(0).GetComponent<TetronimoSpawner>();
        
    }
    public void AddToGrid(Transform t)//add every block to the grid for calculation
    {
        foreach (Transform child in t)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            grid2D[x, y] = child;
        }
        SwipeGrid();
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
    void OnDrawGizmos()
    {
        
        Gizmos.DrawWireCube(transform.position+new Vector3(width/2,height/2,1), new Vector3(width, height));
    }
}
