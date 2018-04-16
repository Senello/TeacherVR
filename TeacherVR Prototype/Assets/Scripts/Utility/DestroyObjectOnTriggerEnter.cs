﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOnTriggerEnter : MonoBehaviour
{
    public string tag;
    public bool Event = true;

    private GameController gameController;

    void Start()
    {
        gameController = GameController.Instance;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals(tag))
        {
            if (Event)
            {
                gameController.SoundManager.Play3DAt(SamplesList.WaterSplash, transform);
                gameController.Particles.CreateOnePoint(transform.position, 0);
                gameController.ScoreBoard.PointsAdd(100);
            }
            Destroy(transform.root.gameObject);
        }
    }
}