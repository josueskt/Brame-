using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float distancia, altura, orbit_speed, vertical_speed;
    public Transform player_cam, center_punto;
    public FixedTouchField touch;
    public float maxima_altura, minima_altura;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        center_punto.position = gameObject.transform.position + new Vector3(0, 1.57f, 0);
        center_punto.eulerAngles += new Vector3(0, touch.TouchDist.x * Time.deltaTime * orbit_speed, 0);
        altura += touch.TouchDist.y * Time.deltaTime * vertical_speed;
        altura = Mathf.Clamp(altura, minima_altura, maxima_altura);
    }

    private void LateUpdate()
    {
        player_cam.position = center_punto.position + center_punto.forward * 1 * distancia + Vector3.up * altura;
        player_cam.LookAt(center_punto);





    }
}
