using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TD.Data.Units
{
    [GenerateAuthoringComponent]
    public struct UnitProjectileSpawnerData : IComponentData
    {
        [Header("Projectile")] public Entity projectile;
        public float projectileLaunchVelocity;

        [Header("Shooting Offsets")] public float3 shootingOffsetA;
        public float3 shootingOffsetB;
        public bool useBothShootPoints;

        [Header("Timer")] public float timeBetweenShots;

        [Header("Internal Data")] public float _timeLeftBetweenShot;
    }
}