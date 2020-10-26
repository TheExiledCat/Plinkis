using System;
using System.Collections.Generic;
using UnityEngine;

public class Tetronimo : MonoBehaviour
{
    // Start is called before the first frame update
    float autoFloatTimer=1;
    float horizontalTimer = 0.2f;
    [SerializeField] Vector3 pivot;
    float lastTimer;
    float lastHorizontalTimer;
    public event Action OnLock;
    public event Action<Transform> OnStatic;
    public event Action OnDeath;
    public event Action<string> OnHold;
    public string tetronimoID;
    private void Start()
    {
        Field.Grid.SetActive(gameObject);
        OnLock += Field.Grid.spawner.SpawnTetronimo;
        OnStatic += Field.Grid.AddToGrid;
        OnDeath += Field.Grid.Die;
        OnHold += Field.Grid.HoldTetronimo;
        OnLock += Field.Grid.HoldReset;
        if (CheckOutOfBounds())
        {
            OnDeath?.Invoke();
            OnLock -= Field.Grid.spawner.SpawnTetronimo;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //move once
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveX(false);
            lastHorizontalTimer = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveX(true);
            lastHorizontalTimer = Time.time;
        }
        //hold movement
        if (Time.time - lastHorizontalTimer > horizontalTimer)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveX(false);
                lastHorizontalTimer = Time.time;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveX(true);
                lastHorizontalTimer = Time.time;
            }
        }
        //gravity
        if (Time.time - lastTimer > (Input.GetKey(KeyCode.DownArrow) ? autoFloatTimer /10: autoFloatTimer)){
            MoveY(true);
            lastTimer = Time.time;
        }
        //rotation
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
        //hold
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!Field.Grid.hasHeld)
                Destroy(gameObject);
            OnHold?.Invoke(tetronimoID);
            
            
        }
    }
    
    #region movement
    void Rotate()
    {
        transform.RotateAround(transform.TransformPoint(pivot),Vector3.forward, -90);
        PushBack();
    }
    void PushBack()
    {
        
        if (CheckTouchingWall())
        {
            if (transform.position.x < Field.Grid.width / 2)
            {
                transform.position += Vector3.right;

            }
            else
            {
                transform.position += Vector3.left;

            }
            if (CheckTouchingOther())
            {
                transform.RotateAround(transform.TransformPoint(pivot), Vector3.forward, 90);
            }
            if (CheckTouchingWall())
            {
                
                PushBack();
            }
            if (CheckTouchingFloor())
            {
                print("Floor touched");
                transform.position += Vector3.up;
                if (CheckOutOfBounds())
                {
                    transform.position += Vector3.down;
                    transform.RotateAround(transform.TransformPoint(pivot), Vector3.forward, 90);
                }
            }
        }
        
    }
    void MoveX(bool right)
    {
        transform.position += right ? Vector3.right : Vector3.left;
        if (CheckOutOfBounds())
        {
            transform.position += right ? Vector3.left : Vector3.right;
        }
    }
    void MoveY(bool down)
    {
        transform.position += down ? Vector3.down : Vector3.up;
        if (CheckOutOfBounds())
        {
            transform.position += down ? Vector3.up : Vector3.down;
            Lock();
            
        }
    }
    #endregion
    #region math
    void Lock()//freeze the tetronimo in place and call functions
    {
        OnLock?.Invoke();
        OnStatic?.Invoke(transform);
        this.enabled = false;
    }
    bool CheckOutOfBounds()//check if tetronimo block is touching a wall or another tetronimo
    {
        foreach(Transform t in transform)
        {
            int x = Mathf.RoundToInt(t.position.x);
            int y = Mathf.RoundToInt(t.position.y);
            

            if (x <0 || x >= Field.Grid.width || y <0 || y >= Field.Grid.height)
            {
                return true;
            }
            if (Field.Grid.grid2D[x, y] != null)
            {
                return true;
            }
            
        }
        return false;
    }
    bool CheckTouchingWall()
    {
        foreach (Transform t in transform)
        {
            int x = Mathf.RoundToInt(t.position.x);
            int y = Mathf.RoundToInt(t.position.y);


            if (x < 0 || x >= Field.Grid.width )
            {
                return true;
            }
            

        }
        return false;
    }
    bool CheckTouchingFloor()
    {
        foreach (Transform t in transform)
        {
            int y = Mathf.RoundToInt(t.position.y);
            if (y <= 0 || y >= Field.Grid.height)
            {
                return true;
            }


        }
        return false;
    }
    bool CheckTouchingOther()
    {
        foreach (Transform t in transform)
        {
            int x = Mathf.RoundToInt(t.position.x);
            int y = Mathf.RoundToInt(t.position.y);

            if (Field.Grid.grid2D[x, y] != null)
            {
                return true;
            }

        }
        return false;
    }
    #endregion
}
