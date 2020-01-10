using Unity.Entities;
using UnityEngine;

namespace TD.Data.Units
{
    [GenerateAuthoringComponent]
    public struct UnitProjectileSpawnerData : IComponentData
    {
        public Entity projectile;
        public float timeBetweenShots;

        [Header("Internal Data")] public float _timeLeftBetweenShot;
    }
}