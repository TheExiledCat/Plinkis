using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Grid;
    public int width =10;
    public int height=20;
    
    void Awake()
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
    }
    void Start()
    {
        transform.localScale = new Vector3(width, height, 1);
        transform.position += Vector3.right * width / 2+Vector3.up*height/2;
        
    }
    void OnDrawGizmos()
    {
        
        Gizmos.DrawWireCube(transform.position+new Vector3(width/2,height/2,1), new Vector3(width, height));
    }
}
