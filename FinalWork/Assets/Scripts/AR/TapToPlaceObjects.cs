using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class TapToPlaceObjects : MonoBehaviour
{

    [SerializeField] private GameObject gameObjectToInstantiate, confirmLocationPanel;
    private GameObject ARUI;
    [SerializeField] private NetworkManagerPlus networkManagerPlus;

    private GameObject spawnedObject;
    private ARRaycastManager arRaycastManager;
    private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    public void ConfirmPosition()
    {
        Debug.Log("Confirm Position");
        NetworkManagerPlus.AREvents.OnHostGameLocationPicked.Invoke(spawnedObject);
    }
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                ARUI = GameObject.Find("GameManager").GetComponent<GameManager>().uIManager.ArHostUI;
                ARUI.SetActive(true);
                ARUI.GetComponent<ARHostUI>().spawnedObject = spawnedObject;
                ARUI.GetComponent<ARHostUI>().EnableConfirmationButton();
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }
}
