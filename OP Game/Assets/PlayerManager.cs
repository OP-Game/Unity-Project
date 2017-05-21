using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public Camera mainCam;
    public GameObject exploder;
    public Exploder.ExploderObject exploderObject;
    public LineRenderer shotLine;
    public Material barkMat;

    private Color startColor, endColor;
    private float alphaValue = 255;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        //  On click, performs a raycast in the direction of the camera
        //  Checks for hits, on a hit with any object tagged "Exploder", triggers explosion in the direction of the raycast.

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
            if (Physics.Raycast(ray, out hit))                                                     
            {
                shotLine.SetPosition(0, shotLine.transform.position);                                   //sets the start and end positions of the line renderer
                shotLine.SetPosition(1, hit.point);

                exploder.transform.position = hit.point;
                
                if (hit.transform.tag == "Exploder")
                {
                    if(hit.transform.parent.tag == "TerrainObj")                                        //if the hit object's parent has the tag TerrainObj, sets the fragment material to Bark.
                    {
                        exploderObject.FragmentOptions.FragmentMaterial = barkMat;
                    }
                    else if(hit.transform.parent == null || hit.transform.parent.tag != "TerrainObj")   //else if there is no parent, or the parent isnt tagged TerrainObj, use default material
                    {
                        exploderObject.FragmentOptions.FragmentMaterial = null;
                    }
                    exploderObject.ForceVector = ray.direction;
                    exploderObject.ExplodeObject(hit.transform.gameObject);
                }
            }
        }
        alphaValue -= 1;
        startColor.a = alphaValue;
        endColor = new Color(1, 0, 0, alphaValue);
        shotLine.startColor = startColor;

        if(startColor.a == 0)
        {
            startColor.a = 255;
        }

    }


}
