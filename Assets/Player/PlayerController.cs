using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<Song> songs = new();
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 move;

    [SerializeField] private float singSpeed = 1.0f;
    [SerializeField] private float wkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float rotSpeed = 15f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
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

        float speed = (Input.GetButton("Fire1") ? runSpeed : wkSpeed);
        foreach (var song in songs)
        {
            if (song.IsSinging())
            {
                speed = singSpeed;
                break;
            }
        };

        move.Set(moveX, 0f, moveZ);

        controller.Move(move * (speed * Time.deltaTime));

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}