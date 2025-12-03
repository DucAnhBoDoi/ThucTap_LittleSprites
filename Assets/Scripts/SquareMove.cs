using UnityEngine;
using UnityEngine.InputSystem;

public class SquareMove : MonoBehaviour
{
    public float moveSpeed = 10f; // điều chỉnh trong Inspector

    private void Update()
    {
        float h = 0f;
        float v = 0f;

        // Keyboard
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1f;
        }

        // Gamepad
        if (Gamepad.current != null)
        {
            Vector2 gp = Gamepad.current.leftStick.ReadValue();
            h += gp.x;
            v += gp.y;
        }

        // Apply movement
        Vector3 movement = new Vector3(h, v, 0f).normalized * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
}
