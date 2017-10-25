using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovingScript : MonoBehaviour
{
    //private Vector2 movement;
    private Vector3 screenPoint;
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    private Vector2 keyoffset;         //Private variable to store the offset distance between the player and camera
    //public int moveRange, initialX, initialY;
    //public bool[,] reachableCases; 

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        int inputX = x < 0 ? -1 : (x == 0 ? 0 : 1);
        int inputY = y < 0 ? -1 : (y == 0 ? 0 : 1);

        keyoffset = new Vector2(inputX, inputY);
        Thread.Sleep(100);
        transform.position = new Vector2((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y)) + keyoffset;
    }


    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }


    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z); // hardcode the y and z for your use

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = new Vector2((float)Math.Round(curPosition.x), (float)Math.Round(curPosition.y));
    }
}
