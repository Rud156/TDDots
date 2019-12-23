using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TD.Data.Units
{
    [GenerateAuthoringComponent]
    public struct UnitMovementData : IComponentData
    {
        public float3 positionOffset;
        public float lerpSpeed;
        public float minNextPositionDistacne;

        [Header("Internal Data")] public float3 _targetPosition;
        public int _currentPathIndex;
        public bool _reachedEndPoint;
        public bool _initalPathFindingLaunched;

        public float3 _startPosition;
        public float _lerpAmount;
    }
}