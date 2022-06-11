using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private GameObject _zombiePref;
    private GameObject[] _spawnPointZombie;
    private System.Random _rnd = new System.Random();
    private RndCheckpoint _rndCheckpoint;

    public SpawnController(GameObject zombiePref, GameObject[] spawnPointZombie, RndCheckpoint rndCheckpoint)
    {
        _zombiePref = zombiePref;
        _spawnPointZombie = spawnPointZombie;
        _rndCheckpoint = rndCheckpoint;
    }

    public IZombie SpawnZombie()
    {
        var zombieobj = Instantiate(_zombiePref, _spawnPointZombie[_rnd.Next(_spawnPointZombie.Length)].transform.position, Quaternion.identity);
        var zombie = zombieobj.GetComponent<IZombie>();
        zombie.nextPosition = _rndCheckpoint.RND().position;
        return zombie;
    }
}
