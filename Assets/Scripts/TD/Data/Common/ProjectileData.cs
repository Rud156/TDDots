using Unity.Entities;

namespace TD.Data.Common
{
    [GenerateAuthoringComponent]
    public struct ProjectileData : IComponentData
    {
        public float projectileDamageAmount;
    }
}