using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffect : MonoBehaviour
{
    [SerializeField] Transform followplayer;
    [SerializeField, Range(0f, 1f)] float paralaxStrength = 0.1f;
    [SerializeField] bool disableVerticalParalax;
    Vector3 targetPrevPos;

    private void Start()
    {
        if (!followplayer)
            followplayer = Camera.main.transform;

        targetPrevPos = followplayer.position;
    }

    private void Update()
    {
        var delta = followplayer.position - targetPrevPos;

        if (disableVerticalParalax)
            delta.y = 0;

        targetPrevPos = followplayer.position;
        transform.position += delta * paralaxStrength;
    }
}