using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Markers : MonoBehaviour
{
    public Whiteboard whiteBoard;
    private RaycastHit touch;

    // Start is called before the first frame update
    void Start()
    {
        this.whiteBoard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();    
    }

    // Update is called once per frame
    void Update()
    {
        float tipHeight = transform.Find("Tip").transform.localScale.y;
        Vector3 tip = transform.Find("Tip").transform.position;

        if(Physics.Raycast(tip, transform.up, out touch, tipHeight))
        {
            if(!(touch.collider.tag == "Whiteboard"))
                    return;

            this.whiteBoard = touch.collider.GetComponent<Whiteboard>();
            Debug.Log("touching");
        }
    }
}
