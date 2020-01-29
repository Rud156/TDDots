using Unity.Entities;
using UnityEngine;

namespace TD.Data.Common.Projectile
{
    [GenerateAuthoringComponent]
    public struct ProjectileDeathData : IComponentData
    {
        public float projectileLifeTime;

        [Header("Internal Data")] public float _currentLifetimeLeft;
        public bool _timeInitialized;
    }
}