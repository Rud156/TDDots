using Unity.Entities;
using UnityEngine;

namespace TD.Data.Units
{
    [GenerateAuthoringComponent]
    public struct UnitSpawnerData : IComponentData
    {
        public Entity lightEntityA;
        public Entity lightEntityB;
        public Entity heavyEntityA;
        public Entity heavyEntityB;

        [Header("Spawn Timers")] public float minTimeBetweenLightEntity;
        public float maxTimeBetweenLightEntity;
        public float minTimeBetweenHeavyEntity;
        public float maxTimeBetweenHeavyEntity;

        [Header("Light Entity")] public int lightEntityBurstCount;
        public float lightBurstTimeBetween;

        [Header("Death Count Per Sec")] public float minDeathPerSec;
        public float maxDeathPerSec;

        [Header("Internal Data")] public float _currentDeadPerSec;
        public int _currentLightEntityCount;
        public float _currentLightEntityInBetweenTime;
        public float _currentLightUnitTime;
        public float _currentHeavyUnitTime;
        public bool _spawnInActive;
    }
}