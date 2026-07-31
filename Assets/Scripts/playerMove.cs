using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void Update()
    {
        // 1. Digital Keys (true only on the frame it is pressed)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        // 3. Virtual Axes (Returns a smoothed float value between -1.0 and 1.0)
        // Uses presets configured in Edit > Project Settings > Input Manager
        float horizontal = Input.GetAxis("Horizontal"); // Smooth filtering (WASD/Left Stick)
        float vertical = Input.GetAxis("Vertical"); 
        //float vertical = Input.GetAxisRaw("Vertical");    // Snappy, unfiltered (-1, 0, or 1)

    void Jump()
        {

        }

    void Walk()
        {

        }
