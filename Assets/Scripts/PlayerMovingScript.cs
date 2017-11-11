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

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rBody2D;
    private float inverseMoveTime;

    //public int moveRange, initialX, initialY;
    //public bool[,] reachableCases; 

    // Use this for initialization
    protected virtual void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rBody2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1 / moveTime;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        int inputX = x < 0 ? -1 : (x == 0 ? 0 : 1);
        int inputY = y < 0 ? -1 : (y == 0 ? 0 : 1);

        keyoffset = new Vector2(inputX, inputY);
        Thread.Sleep(450);
        transform.position = new Vector2((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y)) + keyoffset;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = (Vector2)transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = false;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove;
        canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rBody2D.position, end, inverseMoveTime * Time.deltaTime);
            rBody2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected void OnCantMove<T>(T component) where T : Component
    {
        // TODO
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
