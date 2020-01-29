using Unity.Entities;

namespace TD.Data.Common.Projectile
{
    [GenerateAuthoringComponent]
    public struct ProjectileData : IComponentData
    {
        public float projectileDamageAmount;
    }
}