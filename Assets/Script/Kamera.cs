using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] public Vector3 Target_offset;

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position + Target_offset,.125f);
    }
}
