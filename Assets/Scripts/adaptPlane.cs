using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adaptPlane : MonoBehaviour {
    private static int REDUCTION_FACTOR = 1000;
    public Texture mapTexture;
    private Renderer plane_renderer;

	// Use this for initialization
	void Start () {
        mapTexture = (Texture2D)Resources.Load("maps/map");
        plane_renderer = GetComponent<MeshRenderer>();

        if(mapTexture)
        {
            transform.localScale += new Vector3(mapTexture.width/REDUCTION_FACTOR, 0, mapTexture.height/REDUCTION_FACTOR);
        }
        else
        {
            Debug.Log("Map not found");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
