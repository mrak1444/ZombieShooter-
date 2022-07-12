using TMPro;
using UnityEngine;

public class NameZombieController : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameZombie;

    private void Start()
    {
        _nameZombie.text = gameObject.name;
    }


}
