using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class TapToPlaceObjects : MonoBehaviour
{

    // [SerializeField] private GameObject gameObjectToInstantiate, confirmLocationPanel;
    // public GameObject ARUI;
    // private bool allowPlacement;
    // [SerializeField] private NetworkManagerPlus networkManagerPlus;

    // private GameObject spawnedObject;
    // private ARRaycastManager arRaycastManager;
    // private Vector2 touchPosition;

    // private GameManager gameManager;

    // static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // private void Awake()
    // {
    //     arRaycastManager = GetComponent<ARRaycastManager>();
    // }

    // public void SetUp(GameManager gm, GameObject arui)
    // {
    //     Debug.Log("TapToPlaceSETUP: " + gm);
    //     gameManager = gm;
    //     ARUI = arui;
    //     allowPlacement = true;
    // }

    // private bool TryGetTouchPosition(out Vector2 touchPosition)
    // {
    //     if (Input.touchCount > 0)
    //     {
    //         touchPosition = Input.GetTouch(0).position;
    //         return true;
    //     }
    //     touchPosition = default;
    //     return false;
    // }

    // private void GameLocationSet(Vector3 x, Quaternion rot)
    // {
    //     allowPlacement = false;
    // }
    // void Update()
    // {
    //     if (allowPlacement)
    //     {
    //         if (!TryGetTouchPosition(out Vector2 touchPosition))
    //         {
    //             return;
    //         }
    //         if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
    //         {
    //             var hitPose = hits[0].pose;

    //             if (spawnedObject == null)
    //             {
    //                 gameManager.AREvents.OnHostGameLocation = (gameLocationPos, rotation) => GameLocationSet(gameLocationPos, rotation);
    //                 spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);

    //                 ARUI.SetActive(true);
    //                 ARUI.GetComponent<ARHostUI>().spawnedObject = spawnedObject;
    //                 ARUI.GetComponent<ARHostUI>().networkManagerPlus = networkManagerPlus;
    //                 ARUI.GetComponent<ARHostUI>().EnableConfirmationButton();
    //             }
    //             else
    //             {
    //                 spawnedObject.transform.position = hitPose.position;
    //             }
    //         }
    //     }
    // }
}
