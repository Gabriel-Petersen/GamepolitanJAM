using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(SongsHolder))]
public class PlayerController : MonoBehaviour
{
    private static readonly int VerticalHash = Animator.StringToHash("Vertical");
    private static readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    private CharacterController controller;
    private SongsHolder songsHolder;
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

    private Vector3 pushVelocity = Vector3.zero;
    private float pushTimer = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        songsHolder = GetComponent<SongsHolder>();
        cameraTransform = Camera.main.transform;

        if (gravity > 0)
            gravity *= -1;
    }

    

    private void Update()
    {
        if (pushTimer > 0f)
        {
            pushTimer -= Time.deltaTime;

            controller.Move(pushVelocity * Time.deltaTime);

            pushVelocity.y += gravity * Time.deltaTime;

            pushVelocity.x = Mathf.Lerp(pushVelocity.x, 0f, 5f * Time.deltaTime);
            pushVelocity.z = Mathf.Lerp(pushVelocity.z, 0f, 5f * Time.deltaTime);

            if (controller.isGrounded && pushVelocity.y < 0f)
            {
                pushTimer = 0f;
                pushVelocity = Vector3.zero;
            }
            return;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        animator.SetFloat(HorizontalHash, moveX);
        animator.SetFloat(VerticalHash, moveZ);

        float speed;
        if (songsHolder.IsAnySongSinging())
            speed = singSpeed;
        else
            speed = (Input.GetKey(KeyCode.LeftShift) ? runSpeed : wkSpeed);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        move = forward * moveZ + right * moveX;

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
    }

    public void PushPlayer(Vector3 pushDirection, Vector2 force)
    {
        Vector3 normalizedDirection = pushDirection.normalized;
        normalizedDirection.y = 0f;

        pushVelocity = (normalizedDirection * force.x) + (Vector3.up * force.y);

        pushTimer = 0.4f;
    }
}