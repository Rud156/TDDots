using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TD.Data.Player
{
    [GenerateAuthoringComponent]
    public struct PlayerInputData : IComponentData
    {
        [Header("Internal Data")] public float2 _movementData;
        public bool _lastFrameShot;
    }
}