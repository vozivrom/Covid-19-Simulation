using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public ushort sensibility = 175;
    public ushort speed = 75;
    public ushort boost = 150;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(0, Input.GetAxisRaw("Mouse X") * sensibility * Time.deltaTime, 0, Space.World);
            transform.Rotate(-Input.GetAxisRaw("Mouse Y") * sensibility * Time.deltaTime, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) speed += boost;
        if (Input.GetKeyUp(KeyCode.LeftControl)) speed -= boost;

        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")) * (speed * Time.deltaTime));
    }
}