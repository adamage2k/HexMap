using UnityEngine;
using System.Collections;
 
public class CameraController : MonoBehaviour
{
    public Transform target;
    public float panSpeed = 5f;

    private Vector3 position;
 
    void Start() { Init(); }
 
    public void Init()
    {
        GameObject go = new GameObject("Fake Cam Target");
        go.transform.position = transform.position;
        target = go.transform;
 
        position = transform.position;
    }
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            target.Translate(Vector3.left * Input.GetAxis("Mouse X") * panSpeed);
            target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed);
        }

        position = target.position;
        transform.position = position;
    }
}