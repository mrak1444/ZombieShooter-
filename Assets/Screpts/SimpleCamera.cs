using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    //Цель
    [SerializeField] private Transform target;

    //Скорость слежения
    [SerializeField] private float speed = 4f;

    //Маска слоев препятствий
    [SerializeField] private LayerMask maskObstacles;

    private Vector3 _position;

    void Start()
    {
        _position = target.InverseTransformPoint(transform.position);
    }

    void Update()
    {
        var oldRotation = target.rotation;
        target.rotation = Quaternion.Euler(0, oldRotation.eulerAngles.y, 0);
        var currentPosition = target.TransformPoint(_position);
        target.rotation = oldRotation;

        transform.position = Vector3.Lerp(transform.position, currentPosition, speed * Time.deltaTime);
        var currentRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, speed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(target.position, transform.position - target.position, out hit, Vector3.Distance(transform.position, target.position), maskObstacles))
        {
            transform.position = hit.point;
            transform.LookAt(target);
        }
    }
}