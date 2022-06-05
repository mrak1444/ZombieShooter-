using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private LayerMask _layermask;
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
        Cursor.lockState = CursorLockMode.Locked;

        _gunObj = Instantiate(_gunPref, _gunPoint.transform.position, Quaternion.identity);
        _gunObj.transform.SetParent(_parent.transform);
        _anim = GetComponent<Animator>();
        _rightHandObj = _gunObj.GetComponent<GunModel>().RightHandObj;
        _leftHandObj = _gunObj.GetComponent<GunModel>().LeftHandObj;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, 100f, _layermask))
            {
                Debug.DrawRay(_cam.transform.position, _cam.transform.forward, Color.yellow, 100f);
                
                if (hit.transform.gameObject.CompareTag("Zombie"))
                {
                    //Debug.Log($"{hit.collider.name} - {hit.transform.gameObject.GetComponent<IZombie>().Health - 1}");
                    hit.transform.gameObject.GetComponent<IZombie>().Health = 1;
                }
            }
            else
            {
                Debug.DrawRay(_cam.transform.position, _cam.transform.forward, Color.yellow, 10f);
            }
        }

        _gunObj.transform.LookAt(_lookPoint.transform);
    }

    private void OnAnimatorIK()
    {
        if (_anim)
        {
            if (_rightHandObj != null)
            {
                _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                _anim.SetIKPosition(AvatarIKGoal.RightHand, _rightHandObj.position);
                _anim.SetIKRotation(AvatarIKGoal.RightHand, _rightHandObj.rotation);
            }

            if (_leftHandObj != null)
            {
                _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                _anim.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandObj.position);
                _anim.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandObj.rotation);
            }
        }
    }
}
