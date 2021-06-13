using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustment : MonoBehaviour
{
    [SerializeField] private MeshRenderer _boundMesh;
    private void Awake()
    {
        float orthoSize = _boundMesh.bounds.size.x * Screen.height / Screen.width * .5f;
        if (Camera.main != null) 
            Camera.main.orthographicSize = orthoSize;
    }
}
