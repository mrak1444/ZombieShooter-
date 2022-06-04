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
    private Animator _anim;
    private Transform _rightHandObj = null;
    private Transform _leftHandObj = null;

    void Start()
    {
        _gunObj = Instantiate(_gunPref, _gunPoint.transform.position, Quaternion.identity);
        _gunObj.transform.SetParent(_parent.transform);
        _anim = GetComponent<Animator>();
        _rightHandObj = _gunObj.GetComponent<GunModel>().RightHandObj;
        _leftHandObj = _gunObj.GetComponent<GunModel>().LeftHandObj;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cam.transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            Debug.DrawRay(_cam.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(_cam.transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }

        _gunObj.transform.LookAt(_lookPoint.transform);
    }

    private void OnAnimatorIK()
    {
        if (_anim)
        {
            if (_rightHandObj != null)
            {
                _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);
                _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.5f);
                _anim.SetIKPosition(AvatarIKGoal.RightHand, _rightHandObj.position);
                _anim.SetIKRotation(AvatarIKGoal.RightHand, _rightHandObj.rotation);
            }

            if (_leftHandObj != null)
            {
                _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
                _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.5f);
                _anim.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandObj.position);
                _anim.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandObj.rotation);
            }
        }
    }
}
