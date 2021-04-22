using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPerfecta : MonoBehaviour
{
    public float sem = 10.0f;
    public Transform target;
    Vector3 mouseDelta = Vector3.zero;
    Vector3 amount = Vector3.zero;
    Quaternion cameraRotation = Quaternion.identity;
    Vector3 up = Vector3.zero;
    Vector3 right = Vector3.zero;
    private float x, y;

    public Vector3 addPos = new Vector3(0, 1.63f, 0);
    RaycastHit hit;
    float hitDistance = 0;
    float tanFOV;
    Camera cam;
    Vector3 lookAt = Vector3.zero;
    Vector3 cameraPosition = Vector3.zero;
    Vector3 cameraPositionNotOcc = Vector3.zero;
   
    Vector3 screenCenter = Vector3.zero;
    Vector3[] corners = new Vector3[5];

    public bool mirar;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        float halfFOV = cam.fieldOfView * 0.5f * Mathf.Deg2Rad;
        tanFOV = Mathf.Tan(halfFOV) * cam.nearClipPlane;
        mirar = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (y >= 1 || y <= -1 || x <= -1 || x >= 1)
        {
            mirar = false;
        }
        else
        {
            mirar = true;
        }

        if (Input.GetMouseButton(1))
        {
            mirar = true;
        }

        screenCenter = (cameraRotation * Vector3.forward) * cam.nearClipPlane;
        up = (cameraRotation * Vector3.up) * tanFOV;
        right = (cameraRotation * Vector3.right) * tanFOV * cam.aspect;

        corners[0] = cameraPosition + screenCenter - up - right;
        corners[1] = cameraPosition + screenCenter + up - right;
        corners[2] = cameraPosition + screenCenter + up + right;
        corners[3] = cameraPosition + screenCenter - up + right;
        corners[4] = cameraPosition + screenCenter;

        hitDistance = 1000000;
        for (int i = 0; i < 5; i++)
        {
            if (Physics.Linecast(target.transform.position + addPos, corners[i], out hit))
            {
                Debug.DrawLine(target.transform.position + addPos, corners[i], Color.red);
                Debug.DrawRay(hit.point, Vector3.up * 0.05f, Color.white);
                hitDistance = Mathf.Min(hitDistance, hit.distance);

            }
            else
            {
                
                Debug.DrawLine(target.transform.position + addPos, corners[i], Color.blue);
                
            }
        }

        if (hitDistance > 999999)
        {
            hitDistance = 0;
        }

        if (!mirar)
        {
            cameraRotation = (transform.parent.rotation = Quaternion.AngleAxis(-amount.x, Vector3.up)) *
            Quaternion.AngleAxis(amount.y, Vector3.right);
        }

        if (mirar == true)
        {
            cameraRotation = Quaternion.AngleAxis(-amount.x, Vector3.up) *
            Quaternion.AngleAxis(amount.y, Vector3.right);
        }

    }

    private void LateUpdate()
    {
       

        mouseDelta.Set(Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y"),
            Input.GetAxisRaw("Mouse ScrollWheel"));

        amount += -mouseDelta * sem;
        amount.z = Mathf.Clamp(amount.z, 2, 80);
        amount.y = Mathf.Clamp(amount.y, -10, 40);
       
        cameraRotation = Quaternion.AngleAxis(-amount.x, Vector3.up) *
            Quaternion.AngleAxis(amount.y, Vector3.right);

        lookAt = cameraRotation * Vector3.forward;

        cameraPosition = target.transform.position + addPos - lookAt * amount.z * 0.5f;

        cameraPositionNotOcc = target.transform.position + addPos - lookAt * hitDistance;

        if (hitDistance < cam.nearClipPlane * 2.5f)
        {
            cameraPositionNotOcc -= lookAt * cam.nearClipPlane;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraRotation, Time.deltaTime * 10.0f);

        if (hitDistance > 0)
        {     
            transform.position = Vector3.Lerp(transform.position, cameraPositionNotOcc, Time.deltaTime * 10.0f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * 10.0f);
        }
        
       
    }
}
