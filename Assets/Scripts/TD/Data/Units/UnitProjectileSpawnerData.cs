using Unity.Entities;
using UnityEngine;

namespace TD.Data.Units
{
    [GenerateAuthoringComponent]
    public struct UnitProjectileSpawnerData : IComponentData
    {
        [Header("Projectile")] public Entity projectile;
        public float projectileLaunchVelocity;

        [Header("Shooting Offsets")] public bool useBothShootPoints;
        public Entity shootingPointA;
        public Entity shootingPointB;

        [Header("Timer")] public float timeBetweenShots;

        [Header("Internal Data")] public float _timeLeftBetweenShot;
    }
}