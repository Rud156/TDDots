using Unity.Entities;
using UnityEngine;

namespace TD.Data.Common
{
    [GenerateAuthoringComponent]
    public struct HealthData : IComponentData
    {
        public float maxHealth;

        [Header("Internal Data")] public float currentHelth;
    }
}