using UnityEngine;

public class MouseController : MonoBehaviour
{
    private bool isCursorLocked = true;

    void Start()
    {
        LockMouse();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorLocked = !isCursorLocked;

            if (isCursorLocked)
                LockMouse();
            else
                UnlockMouse();
        }
    }

    void LockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UnlockMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}