using System;
using Framework;
using Unity.Cinemachine;
using UnityEngine;

namespace Core.Player
{
    [Serializable]
    public class PlayerCamera : Updatable<PlayerController>
    {
        public CinemachineCamera cinemachineCamera;
        public CameraZone[] cameraZones;

        public override void OnTriggerEnter(PlayerController controller, Collider other)
        {
            foreach (var zone in cameraZones)
            {
                if (other == zone.enterCollider)
                {
                    cinemachineCamera.Follow = zone.target;
                    cinemachineCamera.LookAt = zone.target;
                    cinemachineCamera.GetComponent<CinemachineFollow>().FollowOffset
                        = zone.followOffset;
                    break;
                }
            }
        }
    }

    [Serializable]
    public class CameraZone
    {
        public Collider enterCollider;
        public Transform target;
        public Vector3 followOffset;
    }
}