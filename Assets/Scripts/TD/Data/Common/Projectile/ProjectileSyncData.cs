using Unity.Entities;
using UnityEngine;

namespace TD.Data.Common.Projectile
{
    [GenerateAuthoringComponent]
    public struct ProjectileSyncData : IComponentData
    {
        public enum EffectType
        {
            None,
            BlueBullTrail,
            PrototypeZeroEffect,
            RockerTrail,
            Max
        }

        public EffectType effectType;

        [Header("Internal Data")] public int _effectIndex;
        public bool _effectInitialized;
    }
}