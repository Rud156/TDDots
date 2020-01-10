using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace TD.Data.Units
{
    [GenerateAuthoringComponent]
    public struct UnitProjectileSyncData : IComponentData
    {
        public enum EffectType
        {
            None,
            BlueBullTrail,
            PrototypeZeroEffect,
            Max
        }

        public EffectType effectType;

        [Header("Internal Data")] public int _effectIndex;
        public bool _effectInitialized;
    }
}