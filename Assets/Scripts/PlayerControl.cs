using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {

        // Check if the player is within the interaction radius.
        

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 1, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, -1, 0));
        }
    }
}
