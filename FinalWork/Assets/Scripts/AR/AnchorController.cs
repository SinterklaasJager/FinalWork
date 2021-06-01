//-----------------------------------------------------------------------
// <copyright file="AnchorController.cs" company="Google LLC">
//
// Copyright 2019 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using Google.XR.ARCoreExtensions;
using UnityEngine;
using Mirror;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

/// <summary>
/// A Controller for the Anchor object that handles hosting and resolving the
/// <see cref="ARCloudAnchor"/>.
/// </summary>
#pragma warning disable 618
public class AnchorController : NetworkBehaviour
#pragma warning restore 618
{
    [SerializeField] private List<GameObject> rocketComponents = new List<GameObject>();
    [SerializeField] private Material progressMaterial, sabotageMaterial;
    [SyncVar] private int currentActiveRocketComponents = 0;

    /// <summary>
    /// The customized timeout duration for resolving request to prevent retrying to resolve
    /// indefinitely.
    /// </summary>
    private const float _resolvingTimeout = 10.0f;

    /// <summary>
    /// The Cloud Anchor ID for the hosted anchor's <see cref="ARCloudAnchor"/>.
    /// This variable will be synchronized over all clients.
    /// </summary>
#pragma warning disable 618
    [SyncVar(hook = "OnChangeId")]
#pragma warning restore 618
    private string _cloudAnchorId = string.Empty;

    /// <summary>
    /// Indicates whether this script is running in the Host.
    /// </summary>
    private bool _isHost = false;

    /// <summary>
    /// Indicates whether an attempt to resolve the Cloud Anchor should be made.
    /// </summary>
    private bool _shouldResolve = false;

    /// <summary>
    /// Indicates whether to chekc Cloud Anchor state and update the anchor.
    /// </summary>
    private bool _shouldUpdatePoint = false;

    /// <summary>
    /// Record the time since resolving started. If the timeout has passed, display
    /// additional instructions.
    /// </summary>
    private float _timeSinceStartResolving = 0.0f;

    /// <summary>
    /// Indicates whether passes the resolving timeout duration or the anchor has been
    /// successfully resolved.
    /// </summary>
    private bool _passedResolvingTimeout = false;

    /// <summary>
    /// The anchor mesh object.
    /// In order to avoid placing the Anchor on identity pose, the mesh object should
    /// be disabled by default and enabled after hosted or resolved.
    /// </summary>
    private GameObject _anchorMesh;

    /// <summary>
    /// The Cloud Anchor created locally which is used to moniter whether the
    /// hosting or resolving process finished.
    /// </summary>
    private ARCloudAnchor _cloudAnchor;

    /// <summary>
    /// The Cloud Anchors example controller.
    /// </summary>
    private CloudAnchorController _cloudAnchorController;

    /// <summary>
    /// The AR Anchor Manager in the scene, used to host or resolve a Cloud Anchor.
    /// </summary>
    private ARAnchorManager _anchorManager;

    /// <summary>
    /// The Unity Awake() method.
    /// </summary>
    public void Awake()
    {
        Debug.Log("Anchor Controller Awake");
        gameObject.name = "AnchorController";
        _cloudAnchorController =
            GameObject.Find("CloudAnchorController")
            .GetComponent<CloudAnchorController>();
        _anchorManager = _cloudAnchorController.AnchorManager;
        // _anchorMesh = transform.Find("AnchorMesh").gameObject;
        // _anchorMesh.SetActive(false);
    }

    /// <summary>
    /// The Unity OnStartClient() method.
    /// </summary>
    public override void OnStartClient()
    {
        Debug.Log("OnStartClient id: " + _cloudAnchorId);
        if (_cloudAnchorId != string.Empty)
        {
            _shouldResolve = true;
        }
    }

    /// <summary>
    /// The Unity Update() method.
    /// </summary>
    public void Update()
    {
        if (_isHost)
        {
            if (_shouldUpdatePoint)
            {
                UpdateHostedCloudAnchor();
            }
        }
        else
        {
            if (_shouldResolve)
            {
                if (!_cloudAnchorController.IsResolvingPrepareTimePassed())
                {
                    return;
                }
                if (!_passedResolvingTimeout)
                {
                    _timeSinceStartResolving += Time.deltaTime;

                    if (_timeSinceStartResolving > _resolvingTimeout)
                    {
                        _passedResolvingTimeout = true;
                        _cloudAnchorController.OnResolvingTimeoutPassed();
                    }
                }

                if (!string.IsNullOrEmpty(_cloudAnchorId) && _cloudAnchor == null)
                {
                    Debug.Log("ResolveAnchor");
                    ResolveCloudAnchorId(_cloudAnchorId);
                }
            }

            if (_shouldUpdatePoint)
            {
                UpdateResolvedCloudAnchor();
            }
        }
    }

    [Server]
    public void ActivateRocketComponent(Enums.CardType type)
    {
        rocketComponents[currentActiveRocketComponents].SetActive(true);
        var mr = rocketComponents[currentActiveRocketComponents].GetComponent<MeshRenderer>();

        if (type == Enums.CardType.good)
        {
            mr.material = progressMaterial;
        }
        else
        {
            mr.material = sabotageMaterial;
        }

        UpdateRocketForClients(type, currentActiveRocketComponents);
        currentActiveRocketComponents++;
    }

    [ClientRpc]
    private void UpdateRocketForClients(Enums.CardType type, int currentComponentInt)
    {
        var currentComponent = rocketComponents[currentComponentInt];
        currentComponent.SetActive(true);
        var mr = currentComponent.GetComponent<MeshRenderer>();

        if (type == Enums.CardType.good)
        {
            mr.material = progressMaterial;
        }
        else
        {
            mr.material = sabotageMaterial;
        }

    }


    /// <summary>
    /// Command run on the server to set the Cloud Anchor Id.
    /// </summary>
    /// <param name="cloudAnchorId">The new Cloud Anchor Id.</param>
#pragma warning disable 618
    [Command(requiresAuthority = false)]
#pragma warning restore 618
    public void CmdSetCloudAnchorId(string cloudAnchorId)
    {
        Debug.Log("Update Cloud Anchor Id with: " + cloudAnchorId);
        _cloudAnchorId = cloudAnchorId;
    }

    /// <summary>
    /// Hosts the user placed cloud anchor and associates the resulting Id with this object.
    /// </summary>
    /// <param name="anchor">The last placed anchor.</param>
    public void HostAnchor(ARAnchor anchor)
    {
        Debug.Log("HostAnchor");
        _isHost = true;
        _shouldResolve = false;
        transform.SetParent(anchor.transform);
        //  _anchorMesh.SetActive(true);
        _cloudAnchor = _anchorManager.HostCloudAnchor(anchor);
        GameObject.Find("GameManager").GetComponent<GameManager>().SetWorldPosition(_cloudAnchor.pose.position, _cloudAnchor.pose.rotation, _cloudAnchor.transform);
        Debug.Log("_cloudAnchor(anchor controller): " + _cloudAnchor);
        if (_cloudAnchor == null)
        {
            Debug.LogError("Failed to add Cloud Anchor.");
            _cloudAnchorController.OnAnchorHosted(
                false, "Failed to add Cloud Anchor.");
            _shouldUpdatePoint = false;
        }
        else
        {
            _shouldUpdatePoint = true;
        }
    }

    /// <summary>
    /// Resolves the Cloud Anchor Id and instantiate a Cloud Anchor on it.
    /// </summary>
    /// <param name="cloudAnchorId">The Cloud Anchor Id to be resolved.</param>
    private void ResolveCloudAnchorId(string cloudAnchorId)
    {
        Debug.Log("ResolveAnchor with id: " + cloudAnchorId);
        Debug.Log("ResolveAnchor with anchor: " + _cloudAnchor);
        _cloudAnchorController.OnAnchorInstantiated(false);
        _cloudAnchor = _anchorManager.ResolveCloudAnchorId(cloudAnchorId);
        if (_cloudAnchor == null)
        {
            Debug.LogErrorFormat("Client could not resolve Cloud Anchor {0}.", cloudAnchorId);
            _cloudAnchorController.OnAnchorResolved(
                false, "Client could not resolve Cloud Anchor.");
            _shouldResolve = true;
            _shouldUpdatePoint = false;
        }
        else
        {
            _cloudAnchorController.OnAnchorResolved(
             true, "Client could resolve Cloud Anchor.");
            _shouldResolve = false;
            _shouldUpdatePoint = true;
        }
    }

    /// <summary>
    /// Update the anchor if hosting Cloud Anchor is success.
    /// </summary>
    private void UpdateHostedCloudAnchor()
    {
        Debug.Log("Update Hosted Anchor");
        if (_cloudAnchor == null)
        {
            Debug.LogError("No Cloud Anchor.");
            return;
        }

        CloudAnchorState cloudAnchorState = _cloudAnchor.cloudAnchorState;
        if (cloudAnchorState == CloudAnchorState.Success)
        {
            Debug.Log("Hosted Anchor succes");
            CmdSetCloudAnchorId(_cloudAnchor.cloudAnchorId);
            _cloudAnchorController.OnAnchorHosted(
                true, "Successfully hosted Cloud Anchor.");
            _shouldUpdatePoint = false;
        }
        else if (cloudAnchorState != CloudAnchorState.TaskInProgress)
        {
            Debug.Log("Hosted Anchor fail: " + cloudAnchorState);
            _cloudAnchorController.OnAnchorHosted(false,
                "Fail to host Cloud Anchor with state: " + cloudAnchorState);
            _shouldUpdatePoint = false;
        }
    }

    /// <summary>
    /// Update the anchor if resolving Cloud Anchor is success.
    /// </summary>
    private void UpdateResolvedCloudAnchor()
    {
        Debug.Log("Update Resolved Anchor");
        if (_cloudAnchor == null)
        {
            Debug.LogError("No Cloud Anchor.");
            return;
        }

        CloudAnchorState cloudAnchorState = _cloudAnchor.cloudAnchorState;
        if (cloudAnchorState == CloudAnchorState.Success)
        {
            Debug.Log(" Resolved Anchor succes");
            transform.SetParent(_cloudAnchor.transform, false);
            _cloudAnchorController.OnAnchorResolved(
                true,
                "Successfully resolved Cloud Anchor.");
            _cloudAnchorController.WorldOrigin = transform;


            // _anchorMesh.SetActive(true);

            // Mark resolving timeout passed so it won't fire OnResolvingTimeoutPassed event.
            _passedResolvingTimeout = true;
            _shouldUpdatePoint = false;
        }
        else if (cloudAnchorState != CloudAnchorState.TaskInProgress)
        {
            Debug.Log(" Resolved Anchor fail: " + cloudAnchorState);
            _cloudAnchorController.OnAnchorResolved(
                false, "Fail to resolve Cloud Anchor with state: " + cloudAnchorState);
            _shouldUpdatePoint = false;
        }
    }

    /// <summary>
    /// Callback invoked once the Cloud Anchor Id changes.
    /// </summary>
    /// <param name="newId">New Cloud Anchor Id.</param>
    private void OnChangeId(string oldID, string newId)
    {
        if (!_isHost && newId != string.Empty)
        {
            _cloudAnchorId = newId;
            _shouldResolve = true;
            _cloudAnchor = null;
        }
    }
}

