﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class ResetOnRange : MonoBehaviour
{
    public float MaxRange = 10;
    public Action OverMaxRangeAction = Action.ResetToEndPoint;
    public bool UseWrongParticle = false;
    public bool StopVelocity = true;
    public Transform End;

    private VRTK_InteractableObject io;
    private VRTK_InteractableObject io2;

    private Vector3 startPos1;
    private Vector3 startPos2;
    private Quaternion startRot1;
    private Quaternion startRot2;

    public enum Action
    {
        ResetToEndPoint,
        ResetBothToMiddle,
        ResetBothToStart,
        ResetToEndPointOnUngrab
    }

    void Start()
    {
        io = GetComponent<VRTK_InteractableObject>();
        if (OverMaxRangeAction == Action.ResetBothToMiddle)
            io2 = End.GetComponent<VRTK_InteractableObject>();
        if (OverMaxRangeAction == Action.ResetBothToStart)
        {
            io2 = End.GetComponent<VRTK_InteractableObject>();
            startPos1 = transform.position;
            startPos2 = End.transform.position;
            startRot1 = transform.rotation;
            startRot2 = End.transform.rotation;
        }
    }

    void Update()
    {
        if (OverMaxRangeAction == Action.ResetToEndPointOnUngrab && io.IsGrabbed()) return;
        if (OverMaxRangeAction == Action.ResetBothToStart)
        {
            if (Vector3.Distance(transform.position, startPos1) > MaxRange ||
                Vector3.Distance(End.position, startPos2) > MaxRange)
            {
                SpawnWrongPartcile();
                Unsnap(transform.parent);
                Unsnap(End.transform.parent);
                io.ForceStopInteracting();
                io2.ForceStopInteracting();
                transform.position = startPos1;
                End.transform.position = startPos2;
                transform.rotation = startRot1;
                End.transform.rotation = startRot2;
                if (StopVelocity)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    End.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }
        else if (Vector3.Distance(End.position, transform.position) > MaxRange)
        {
            io.ForceStopInteracting();
            if (OverMaxRangeAction == Action.ResetBothToMiddle)
            {
                SpawnWrongPartcile();
                Unsnap(transform.parent);
                Unsnap(End.transform.parent);
                io2.ForceStopInteracting();

                var a = Vector3.Lerp(transform.position, End.position, 1f / 3);
                var b = Vector3.Lerp(transform.position, End.position, 2f / 3);
                transform.position = a;
                End.position = b;
                if (StopVelocity)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    End.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
            else //Action.ResetToEndPointOnUngrab and Action.ResetToEndPoint
            {
                SpawnWrongPartcile();
                Unsnap(transform.parent);
                transform.position = End.position;
                transform.rotation = End.rotation;
                if (StopVelocity)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }
    }

    void Unsnap(Transform parent)
    {
        while (parent != null)
        {
            if (parent.GetComponent<VRTK_SnapDropZone>() != null)
            {
                parent.GetComponent<VRTK_SnapDropZone>().ForceUnsnap();
                break;
            }
            parent = parent.parent;
        }
    }

    void SpawnWrongPartcile()
    {
        if (UseWrongParticle)
        {
            GameController.Instance.Particles.CreateParticle(Particles.NaszeParticle.Small_Wrong, transform.position);
            GameController.Instance.Particles.CreateParticle(Particles.NaszeParticle.Small_Wrong, End.transform.position);
            GameController.Instance.SoundManager.Play2D(SamplesList.Error, 0.01f);
        }
        else
        {
            GameController.Instance.Particles.CreateParticle(Particles.NaszeParticle.Poof, transform.position);
            GameController.Instance.Particles.CreateParticle(Particles.NaszeParticle.Poof, End.transform.position);
            GameController.Instance.SoundManager.Play2D(SamplesList.ShortPoof,0.1f);
        }
    }
}