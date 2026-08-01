using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<Song> songs = new();
    private CharacterController controller;
    private Transform cameraTransform;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 move;

    [SerializeField] private float singSpeed = 1.0f;
    [SerializeField] private float wkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float rotSpeed = 15f;
    [SerializeField] private float lookSpeed = 100f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        foreach (var component in GetComponents<Song>())
        {
            songs.Add(component);
        }
        gravity *= -1;
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float speed = (Input.GetKey(KeyCode.LeftShift) ? runSpeed : wkSpeed);
        foreach (var song in songs)
        {
            if (song.IsSinging())
            {
                speed = singSpeed;
                break;
            }
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        //move 1 - seta anda pra cada direção
        /*
        move = (forward * moveZ) + (right * moveX);
        move.y = 0f;
        move.Normalize();

          if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetPlayerRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetPlayerRotation, rotSpeed * Time.deltaTime);
        }
        */


        //move 2 - rotaciona o player com A e D
        move = (transform.forward * moveZ);
        transform.Rotate(0f, moveX * lookSpeed * Time.deltaTime, 0f);




        //resto do move
      
        controller.Move(move * (speed * Time.deltaTime));

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}