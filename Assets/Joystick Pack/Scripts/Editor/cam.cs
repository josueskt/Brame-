using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    public float distancia, altura, orbit_speed, vertical_speed;
    public Transform player_cam, center_punto;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        center_punto.position = gameObject.transform.position + new Vector3(0, 1.57f, 0);
        center_punto.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * Time.deltaTime * orbit_speed, 0);
        altura += Input.GetAxis("Mouse Y");
    }

    private void LateUpdate()
    {
        player_cam.position = center_punto.position + center_punto.forward * -1 * distancia + Vector3.up * altura;
        player_cam.LookAt(center_punto);





    }
}
