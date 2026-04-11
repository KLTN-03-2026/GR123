using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorShoot;
    [SerializeField] private Texture2D cursorReLoad;

    private Vector2 hotspot = new Vector2(16, 16); // Thay 48 bằng 16 để trỏ vào trung tâm con trỏ

    void Start()
    {
        // Đặt con trỏ mặc định khi bắt đầu game
        Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        // Khi nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorShoot, hotspot, CursorMode.Auto);
        }
        // Khi thả chuột trái
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
        }

        // Khi nhấn phím R để nạp đạn
        if (Input.GetKeyDown(KeyCode.R))
        {
            Cursor.SetCursor(cursorReLoad, hotspot, CursorMode.Auto);
        }
        // Khi thả phím R
        else if (Input.GetKeyUp(KeyCode.R))
        {
            Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
        }
    }
}
