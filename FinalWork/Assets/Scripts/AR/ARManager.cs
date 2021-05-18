using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    [SerializeField] GameObject gameLocationChooser;

    [Header("AR Foundation")]

    /// <summary>
    /// The active AR Session Origin used in the example.
    /// </summary>
    public ARSessionOrigin SessionOrigin;

    /// <summary>
    /// The AR Session used in the example.
    /// </summary>
    public GameObject SessionCore;

    /// <summary>
    /// The AR Extentions used in the example.
    /// </summary>
    public GameObject ARExtentions;

    /// <summary>
    /// The active AR Anchor Manager used in the example.
    /// </summary>
    public ARAnchorManager AnchorManager;

    /// <summary>
    /// The active AR Raycast Manager used in the example.
    /// </summary>
    public ARRaycastManager RaycastManager;

    public void Start()
    {
        gameObject.name = "AR";
    }

}
