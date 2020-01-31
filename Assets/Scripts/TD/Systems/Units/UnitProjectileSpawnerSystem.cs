using TD.Data.Units;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace TD.Systems.Units
{
    public class UnitProjectileSpawnerSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;

            Entities
                .WithStructuralChanges()
                .ForEach(
                    (Entity entity, int entityInQueryIndex, ref UnitProjectileSpawnerData unitProjectileSpawner,
                        in Translation translation, in Rotation rotation) =>
                    {
                        if (unitProjectileSpawner._timeLeftBetweenShot <= 0)
                        {
                            float3 forwardDirection =
                                math.forward(rotation.Value) * unitProjectileSpawner.projectileLaunchVelocity;

                            Entity projectileInstanceA = EntityManager.Instantiate(unitProjectileSpawner.projectile);
                            float3 shootingOffsetA = EntityManager.GetComponentData<LocalToWorld>(unitProjectileSpawner.shootingPointA).Position;
                            EntityManager.SetComponentData(projectileInstanceA, new Translation()
                            {
                                Value = shootingOffsetA
                            });

                            EntityManager.SetComponentData(projectileInstanceA, new PhysicsVelocity()
                            {
                                Linear = forwardDirection
                            });

                            if (unitProjectileSpawner.useBothShootPoints)
                            {
                                Entity projectileInstanceB = EntityManager.Instantiate(unitProjectileSpawner.projectile);
                                float3 shootingOffsetB = EntityManager.GetComponentData<LocalToWorld>(unitProjectileSpawner.shootingPointB).Position;
                                EntityManager.SetComponentData(projectileInstanceB, new Translation()
                                {
                                    Value = shootingOffsetB
                                });
                                EntityManager.SetComponentData(projectileInstanceB,
                                    new PhysicsVelocity()
                                    {
                                        Linear = forwardDirection
                                    });
                            }

                            unitProjectileSpawner._timeLeftBetweenShot = unitProjectileSpawner.timeBetweenShots;
                        }
                        else
                        {
                            unitProjectileSpawner._timeLeftBetweenShot -= deltaTime;
                        }
                    }).Run();

            return default;
        }
    }
}