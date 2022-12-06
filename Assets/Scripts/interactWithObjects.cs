using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactWithObjects : MonoBehaviour
{
    public GameObject lastHit;
    Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f);
    float rayLength = 5f;
    Ray ray;
    public LayerMask Interactible;
    public Vector3 collision = Vector3.zero;
    private GameObject crosshair;
    private float defaultScale = 1;
    private bool scaleUp = true;

    private float currentScale;
    public GameObject CameraTransform;
    public GameObject inFrontCam;
    public GameObject newObj;
    // Start is called before the first frame update
    void Start()
    {
        currentScale = defaultScale;
        crosshair = GameObject.Find("crosshair");
    }

    // Update is called once per frame
    void Update()
    {

        ray = Camera.main.ViewportPointToRay(rayOrigin);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength, 1 << LayerMask.NameToLayer("Interactible")))
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject hitObj = hit.transform.gameObject;
                newObj = (GameObject)Instantiate(hitObj);
                newObj.transform.parent = inFrontCam.transform;
                newObj.transform.localPosition = Vector3.zero.normalized;
                Debug.Log(newObj.transform.rotation);
                newObj.transform.localRotation = Quaternion.Euler(90, 180, 0);
            }
            else if (scaleUp == true)
            {
                currentScale += .01f;
                if (crosshair.transform.localScale.x >= defaultScale * 1.5)
                {
                    scaleUp = false;
                }
            }
            else
            {
                currentScale -= .01f;
                if (crosshair.transform.localScale.x <= defaultScale * 0.75)
                {
                    scaleUp = true;
                }
            }
            crosshair.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
        
        else if(crosshair.transform.localScale.x != defaultScale)
        {
            if(crosshair.transform.localScale.x > 1)
                currentScale -= .01f;
            else
                currentScale += .01f;
            crosshair.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
        else
        {
            scaleUp = true;
        }
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
    }
}
