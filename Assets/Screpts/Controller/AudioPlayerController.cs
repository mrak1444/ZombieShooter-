using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerController : MonoBehaviour
{
    [SerializeField] private AudioSource _stepLeft;
    [SerializeField] private AudioSource _stepRight;
    [SerializeField] private AudioSource _runLeft;
    [SerializeField] private AudioSource _runRight;

    public void StepLeft()
    {
        _stepLeft.Play();
    }

    public void StepRight()
    {
        _stepRight.Play();
    }

    public void RunLeft()
    {
        _runLeft.Play();
    }

    public void RunRight()
    {
        _runRight.Play();
    }
}
