using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private GameObject _gunPref;
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _gunPoint;
    [SerializeField] private GameObject _lookPoint;
    [SerializeField] private Camera _cam;

    private GameObject _gunObj;

    void Start()
    {
        _gunObj = Instantiate(_gunPref, _gunPoint.transform.position, Quaternion.identity);
        _gunObj.transform.SetParent(_parent.transform);
        
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cam.transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            Debug.DrawRay(_cam.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log(hit.transform.name);
        }
        else
        {
            Debug.DrawRay(_cam.transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        _gunObj.transform.LookAt(_lookPoint.transform);
    }
}
