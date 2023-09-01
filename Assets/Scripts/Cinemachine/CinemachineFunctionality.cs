using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineFunctionality : MonoBehaviour
{
    #region Cameras and Movement

        private const string MIDDLE_CAMERA_NAME = "Middle Camera";
        private const string UP_CAMERA_NAME = "Up Camera";
        private const string DOWN_CAMERA_NAME = "Down Camera";
        private const string RIGHT_CAMERA_NAME = "Right Camera";
        private const string LEFT_CAMERA_NAME = "Left Camera";
        private const float CHANGE_CAMERA_TIME = 1.5f;
        private Animator _animator;
        [SerializeField] private CinemachineVirtualCamera middleCamera;
        [SerializeField] private CinemachineVirtualCamera rightCamera;
        [SerializeField] private CinemachineVirtualCamera leftCamera;
        [SerializeField] private CinemachineVirtualCamera upCamera;
        [SerializeField] private CinemachineVirtualCamera downCamera;

    #endregion


    #region Teleportation of Cameras
    
        [SerializeField, Min(0.05f)] private float timeToMoveTeleport;
        private Vector3 _distanceBetweenCamerasHorizontal;
        private Vector3 _distanceBetweenCamerasVertical;
        
    #endregion
    
    public static CinemachineFunctionality Shared { get; private set; }
    
    
    private void Awake()
    {
        Shared = this;
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        var positionMid = middleCamera.transform.position;
        _distanceBetweenCamerasHorizontal = rightCamera.transform.position - positionMid;
        _distanceBetweenCamerasVertical = upCamera.transform.position - positionMid;
    }

    public float GetTimeToMoveTeleport()
    {
        return timeToMoveTeleport;
    }

    public void MoveCameraToSide(EdgeOfMap.Direction direction)
    {
        if (GameManager.Shared.isWorldActionActive) return;
        switch (direction)
        {
            case EdgeOfMap.Direction.Right:
                StartCoroutine(WaitUntilCameraSet(CHANGE_CAMERA_TIME, RIGHT_CAMERA_NAME, rightCamera));
                GameManager.Shared.ChangeLocationInMapGrid(1,0);
                break;
            case EdgeOfMap.Direction.Left:
                StartCoroutine(WaitUntilCameraSet(CHANGE_CAMERA_TIME, LEFT_CAMERA_NAME, leftCamera));
                GameManager.Shared.ChangeLocationInMapGrid(-1,0);
                break;
            case EdgeOfMap.Direction.Up:
                StartCoroutine(WaitUntilCameraSet(CHANGE_CAMERA_TIME, UP_CAMERA_NAME, upCamera));
                GameManager.Shared.ChangeLocationInMapGrid(0,-1);
                break;
            case EdgeOfMap.Direction.Down:
                StartCoroutine(WaitUntilCameraSet(CHANGE_CAMERA_TIME, DOWN_CAMERA_NAME, downCamera));
                GameManager.Shared.ChangeLocationInMapGrid(0,1);
                break;
        }
    }

    public IEnumerator MoveCameraToPlace(GameObject player, TeleportInfo teleportInfo)
    {
        GameManager.Shared.isWorldActionActive = true;
        if (teleportInfo.isGoInside)
        {
            yield return new WaitForSeconds(timeToMoveTeleport);
            ChangeMiddleCameraPos(teleportInfo.cameraLocation);
        }
        else
        {
            ChangeMiddleCameraPos(teleportInfo.cameraLocation);
            yield return new WaitForSeconds(timeToMoveTeleport);
        }
        player.transform.position = teleportInfo.avatarLocation;
        GameManager.Shared.isWorldActionActive = false;
    }
    
    private IEnumerator WaitUntilCameraSet(float seconds, string stateName, CinemachineVirtualCamera otherCamera)
    {
        GameManager.Shared.isScreenMoves = true;
        GameManager.Shared.isWorldActionActive = true;
        _animator.Play(stateName);
        yield return new WaitForSeconds(seconds);
        middleCamera.transform.position = otherCamera.transform.position;
        _animator.Play(MIDDLE_CAMERA_NAME);
        SetAllCamerasNewPositions(middleCamera);
        GameManager.Shared.isWorldActionActive = false;
        GameManager.Shared.isScreenMoves = false;
    }
    
    private void SetAllCamerasNewPositions(CinemachineVirtualCamera cameraInTheMiddle)
    {
        var middlePositions = cameraInTheMiddle.transform.position;
        rightCamera.transform.position = middlePositions + _distanceBetweenCamerasHorizontal;
        leftCamera.transform.position = middlePositions - _distanceBetweenCamerasHorizontal;
        upCamera.transform.position = middlePositions + _distanceBetweenCamerasVertical;
        downCamera.transform.position = middlePositions - _distanceBetweenCamerasVertical;
    }

    private void ChangeMiddleCameraPos(Vector3 pos)
    {
        middleCamera.transform.position = pos;
        SetAllCamerasNewPositions(middleCamera);
    }
}
