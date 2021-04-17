using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplayer : MonoBehaviour
{

    public float horizonal_move;
    public float vertical_move;
    public CharacterController controler;
    public float speed_v;
    public Vector3 move_player;
    public Joystick joistik;
    public bool salto ;

    
    private Vector3 player_speed_imput; // limitar velocidad

    // camera
    public Camera cam;
    private Vector3 cam_mira_recto;
    private Vector3 cam_mira_derecha;

    //gravedad 
    public float gravedad = 9.8f;
    public float fall_v;
    public float jum_force;



    
    void Start()
    {
        controler = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        horizonal_move = joistik.Horizontal;    
        vertical_move = joistik.Vertical;
        player_speed_imput = new Vector3(horizonal_move, 0, vertical_move);
        player_speed_imput = Vector3.ClampMagnitude(player_speed_imput, 1);
       
        camDirection();

        move_player = player_speed_imput.x * cam_mira_derecha + player_speed_imput.z * cam_mira_recto;
        
        controler.transform.LookAt(controler.transform.position + move_player);
        SetGravity();
        if (controler.isGrounded && salto )
        {
            fall_v = jum_force;
            move_player.y = fall_v;
            Debug.Log("Hola");
           
        }
        else
        {
            salto = false;
        }


        move_player = move_player * speed_v;  // mobimiento del player
        controler.Move(move_player * Time.deltaTime);
       

        

        
        
    }
    void camDirection()    //reotacion del player  con respeco a una cam
    {
        cam_mira_recto = cam.transform.forward;
        cam_mira_derecha = cam.transform.right;

        cam_mira_recto.y = 0;
        cam_mira_derecha.y = 0;
        cam_mira_recto = cam_mira_recto.normalized;
        cam_mira_derecha = cam_mira_derecha.normalized;
        
    }

     public void PlayerSkills()
    {
        salto = true;   
    }

    

   
    
    void SetGravity()   //gravedad
    {
        if (controler.isGrounded)
        {
            fall_v = -gravedad * Time.deltaTime;
            move_player.y = fall_v;
        }
        else
        {
            fall_v -= gravedad * Time.deltaTime;
            move_player.y = fall_v;
        }
       
    }


   
}
