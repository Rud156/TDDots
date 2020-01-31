using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TD.Data.Player
{
    [GenerateAuthoringComponent]
    public struct PlayerShootingInputData : IComponentData
    {
        [Header("Projectile")] public Entity projectile;
        public float projectileLaunchVelcoity;

        [Header("Shooting")] public float delayBetweenShots;
        public Entity shootingPoint;

        [Header("Internal Data")] public bool _lastFrameShot;
        public float _currentShootTimer;
    }
}