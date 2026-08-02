using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private static readonly int VerticalHash = Animator.StringToHash("Vertical");
    private static readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    [SerializeField] private List<Song> songs = new();

    private CharacterController controller;
    private Animator animator;
    private Transform cameraTransform;
    private Vector3 velocity;
    private Vector3 move;
    private bool isGrounded;

    [SerializeField] private float singSpeed = 1.0f;
    [SerializeField] private float wkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = 9.81f;

    private Vector3 pushForce = Vector3.zero;
    private float pushDuration = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.transform;
        foreach (var component in GetComponents<Song>())
        {
            songs.Add(component);
        }

        if (gravity > 0)
            gravity *= -1;
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        animator.SetFloat(HorizontalHash, moveX);
        animator.SetFloat(VerticalHash, moveZ);

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

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        move = forward * moveZ + right * moveX;

        controller.Move(move * (speed * Time.deltaTime));

        if (move.sqrMagnitude > 0.01f)
        {
            controller.Move(move * (speed * Time.deltaTime));
            animator.SetBool(IsWalkingHash, true);
        }
        else
        {
            animator.SetBool(IsWalkingHash, false);
        }

        if (forward.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(forward);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (pushForce.sqrMagnitude > 0.01f)
        {
            controller.Move(pushForce);
            pushDuration -= Time.deltaTime;
            if (pushDuration <= 0f)
            {
                pushForce = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// Pushes the player in a direction with the specified force.
    /// </summary>
    /// <param name="pushDirection">The direction to push the player (will be normalized)</param>
    /// <param name="pushMagnitude">The magnitude of the push force</param>
    /// <param name="duration">How long the push should be applied (in seconds)</param>
    public void PushPlayer(Vector3 pushDirection, float pushMagnitude, float duration = 0.1f)
    {
        Vector3 normalizedDirection = pushDirection.normalized;
        normalizedDirection.y = 0f; // Keep push horizontal to prevent unintended vertical movement

        pushForce = normalizedDirection * pushMagnitude;
        pushDuration = duration;
    }
}