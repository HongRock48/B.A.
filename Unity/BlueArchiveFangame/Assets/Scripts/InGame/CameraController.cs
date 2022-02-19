using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private float SmoothTime = 0.3f;

    public Vector3 Offset;
    private Vector3 velocity = Vector3.zero;

    public void MoveCamera() {
        Vector3 targetPosition = Target.position + Offset;
        gameObject.transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);

        transform.LookAt(Target);
    }

    // Start is called before the first frame update
    void Start() {
        Offset = gameObject.transform.position - Target.position;
    }


    // Update is called once per frame
    void FixedUpdate() {
        MoveCamera();
    }
}
