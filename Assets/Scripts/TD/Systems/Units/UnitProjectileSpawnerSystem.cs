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
        private BeginInitializationEntityCommandBufferSystem _commandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            _commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            var entityCommandBuffer = _commandBufferSystem.CreateCommandBuffer().ToConcurrent();

            JobHandle jobHandle = Entities.ForEach(
                (Entity entity, int entityInQueryIndex, ref UnitProjectileSpawnerData unitProjectileSpawner,
                    in Translation translation, in Rotation rotation) =>
                {
                    if (unitProjectileSpawner._timeLeftBetweenShot <= 0)
                    {
                        float3 forwardDirection =
                            math.forward(rotation.Value) * unitProjectileSpawner.projectileLaunchVelocity;

                        Entity projectileInstanceA =
                            entityCommandBuffer.Instantiate(entityInQueryIndex, unitProjectileSpawner.projectile);
                        entityCommandBuffer.SetComponent(entityInQueryIndex, projectileInstanceA, new Translation()
                        {
                            Value = unitProjectileSpawner.shootingOffsetA + translation.Value
                        });

                        entityCommandBuffer.SetComponent(entityInQueryIndex, projectileInstanceA, new PhysicsVelocity()
                        {
                            Linear = forwardDirection
                        });

                        if (unitProjectileSpawner.useBothShootPoints)
                        {
                            Entity projectileInstanceB =
                                entityCommandBuffer.Instantiate(entityInQueryIndex, unitProjectileSpawner.projectile);
                            entityCommandBuffer.SetComponent(entityInQueryIndex, projectileInstanceB, new Translation()
                            {
                                Value = unitProjectileSpawner.shootingOffsetB + translation.Value
                            });
                            entityCommandBuffer.SetComponent(entityInQueryIndex, projectileInstanceB,
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
                }).Schedule(inputDeps);

            _commandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}