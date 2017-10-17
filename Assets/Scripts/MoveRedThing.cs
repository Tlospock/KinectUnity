using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRedThing : MonoBehaviour {

    public Transform target;
    Camera cam;

    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Debug.Log("Mouse down" + Input.mousePosition.x + " " + Input.mousePosition.y);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;
    }

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        target = GetComponent<SpriteRenderer>().transform;
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        Debug.Log("target is " + screenPos.x + " pixels from the left");
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
    }
}
