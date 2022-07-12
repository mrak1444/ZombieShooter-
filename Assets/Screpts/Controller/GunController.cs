using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviourPunCallbacks
{
    [SerializeField] private LayerMask _layermask;
    [SerializeField] private GameObject[] _gunPref;
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _gunPoint;
    [SerializeField] private GameObject _lookPoint;
    [SerializeField] private Camera _cam;
    [SerializeField] private AudioSource _shot;


    private GameObject _gunObj;
    private GunModel _gunModel;
    private Animator _anim;
    private Transform _rightHandObj = null;
    private Transform _leftHandObj = null;
    private Dictionary<string, GameObject> _gunPrefDict = new Dictionary<string, GameObject>();
    private bool _flagFire = false;

    private void Start()
    {
        foreach (var item in _gunPref)
        {
            _gunPrefDict.Add(item.name, item);
        }

        _gunObj = Instantiate(_gunPrefDict[GameProfile.GunId], _gunPoint.transform.position, Quaternion.identity);
        _gunObj.transform.SetParent(_parent.transform);
        _anim = GetComponent<Animator>();
        _gunModel = _gunObj.GetComponent<GunModel>();
        _rightHandObj = _gunModel.RightHandObj;
        _leftHandObj = _gunModel.LeftHandObj;
    }

    private void Update()
    {
        Fire();

        _gunObj.transform.LookAt(_lookPoint.transform);
    }

    private void Fire()
    {
        RaycastHit hit;

        if (Input.GetButtonDown("Fire1"))
        {
            _shot.Play();
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, (float)GameProfile.GunRange, _layermask))
            {
                Debug.DrawRay(_cam.transform.position, _cam.transform.forward, Color.yellow, 100f);

                if (hit.transform.gameObject.CompareTag("Zombie"))
                {
                    GameProfile.DamageForZombie.Value = new SubscriptionData(gameObject.name, hit.transform.gameObject.name, _gunModel.Damage);
                }
            }
            else
            {
                Debug.DrawRay(_cam.transform.position, _cam.transform.forward, Color.yellow, 10f);
            }
        }
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
