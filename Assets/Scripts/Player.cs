using UnityEngine;
using UnityEngine.InputSystem;

public class SquareMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float h = 0f;
        float v = 0f;

        // ---- Keyboard input ----
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1f;
        }

        // ---- Gamepad input ----
        if (Gamepad.current != null)
        {
            Vector2 gp = Gamepad.current.leftStick.ReadValue();
            h += gp.x;
            v += gp.y;
        }

        // ---- Movement ----
        Vector3 movement = new Vector3(h, v, 0).normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // ---- Flip ----
        if (h < 0)
            sr.flipX = true;
        else if (h > 0)
            sr.flipX = false;

        // ---- Animation Run ----
        bool isRunning = (h != 0 || v != 0);
        anim.SetBool("isRun", isRunning);

        // ---- Attack bằng chuột ----
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            anim.SetTrigger("isAttack");
        }
    }
}
