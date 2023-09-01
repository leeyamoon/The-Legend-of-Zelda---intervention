using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeOfMap : MonoBehaviour
{
    public enum Direction
    { 
        Left, Right, Up, Down
    }
    private const float EPSILON = 0.1f;
    private Vector3 _transformInCameraMove;
    [SerializeField] private Direction direction;
    
    // Start is called before the first frame update
    private void Start()
    {
        switch (direction)
        {
            case Direction.Left:
                _transformInCameraMove = Vector3.right;
                break;
            case Direction.Right:
                _transformInCameraMove = Vector3.left;
                break;
            case Direction.Up:
                _transformInCameraMove = Vector3.down;
                break;
            case Direction.Down:
                _transformInCameraMove = Vector3.up;
                break;
        }
    }
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (GameManager.Shared.isWorldActionActive)
            {
                StartCoroutine(DragPlayer(col.gameObject));
            }
            CinemachineFunctionality.Shared.MoveCameraToSide(direction);
        }
    }

    private IEnumerator DragPlayer(GameObject player)
    {
        player.transform.SetParent(transform);
        yield return new WaitWhile(() => GameManager.Shared.isWorldActionActive);
        player.transform.position += _transformInCameraMove*EPSILON;
        player.transform.SetParent(null);
    }
}
