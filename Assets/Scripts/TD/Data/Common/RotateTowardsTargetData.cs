using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TD.Data.Common
{
    [GenerateAuthoringComponent]
    public struct RotateTowardsTargetData : IComponentData
    {
        public float rotationSpeed;

        [Header("Internal Data")] public float3 _lastPosition;
    }
}