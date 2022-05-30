using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RndCheckpoint
{
    private CheckpointModel _checkpointModel;
    private System.Random _rnd = new System.Random();
    public RndCheckpoint(CheckpointModel checkpointModel)
    {
        _checkpointModel = checkpointModel;
    }

    public Transform RND()
    {
        return _checkpointModel.Checkpoints[_rnd.Next(_checkpointModel.Checkpoints.Length)];
    }
}
