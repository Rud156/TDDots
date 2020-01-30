using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TD.Data.Player
{
    [GenerateAuthoringComponent]
    public struct PlayerMovementInputData : IComponentData
    {
        [Header("Movement")] public float velocityChangeRate;
        public float maxVelocity;
        [Range(0, 0.5f)] public float movementInputThreshold;

        [Header("Rotation")] public float rotationSpeed;

        [Header("Internal Data")] public float2 _movementData;
        public float _currentVelocity;
    }
}