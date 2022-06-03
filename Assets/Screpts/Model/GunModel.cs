using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModel : MonoBehaviour
{
    [SerializeField] private Transform _rightHandObj = null;
    [SerializeField] private Transform _leftHandObj = null;

    public Transform RightHandObj => _rightHandObj;
    public Transform LeftHandObj => _leftHandObj;
}
