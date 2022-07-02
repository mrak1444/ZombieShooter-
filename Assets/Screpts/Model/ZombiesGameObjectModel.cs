using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesGameObjectModel : MonoBehaviour
{
    [SerializeField] private GameObject[] _zombiesGameObject;

    public GameObject[] ZombiesGameObject => _zombiesGameObject;
}