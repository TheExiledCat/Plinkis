﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetronimo_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float autoFloatTimer=0.8f;
    [SerializeField] float horizontalTimer = 0.3f;
    [SerializeField] Vector3 pivot;
    float lastTimer;
    float lastHorizontalTimer;
    

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
    }
    void Rotate()
    {
        transform.RotateAround(transform.TransformPoint(pivot),Vector3.forward, -90);
        if (CheckOutOfBounds())
        {
            if (transform.position.x < Field.Grid.width / 2)
            {
                MoveX(true);
            }
            else MoveX(false);
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
        }
    }
    bool CheckOutOfBounds()
    {
        foreach(Transform t in transform)
        {
            int x = Mathf.RoundToInt(t.position.x);
            int y = Mathf.RoundToInt(t.position.y);
            

            if (x <0 || x >= Field.Grid.width || y <0 || y >= Field.Grid.height)
            {
                return true;
            }
            
        }
        return false;
    }
}
