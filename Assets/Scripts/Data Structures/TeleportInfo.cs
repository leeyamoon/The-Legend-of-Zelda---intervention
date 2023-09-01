using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Teleport Information")]
public class TeleportInfo : ScriptableObject
{
    public Vector3 cameraLocation;
    public Vector3 avatarLocation;
    public bool isGoInside;
}
