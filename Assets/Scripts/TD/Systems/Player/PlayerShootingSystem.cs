using TD.Data.Player;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace TD.Systems.Player
{
    public class PlayerShootingSystem : JobComponentSystem
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

            JobHandle jobHandle = Entities.WithAll<PlayerTag>()
                .ForEach((Entity entity, int entityInQueryIndex,
                    ref PlayerShootingInputData playerShootingInputData,
                    in Rotation rotation, in Translation translation) =>
                {
                    if (playerShootingInputData._currentShootTimer > 0)
                    {
                        playerShootingInputData._currentShootTimer -= deltaTime;
                    }

                    // Shoot only if the shooting button is down and the timer is over. Then reset the timer
                    if (playerShootingInputData._lastFrameShot && playerShootingInputData._currentShootTimer <= 0)
                    {
                        playerShootingInputData._currentShootTimer = playerShootingInputData.delayBetweenShots;
                        Entity projectileInstance = entityCommandBuffer.Instantiate(entityInQueryIndex, playerShootingInputData.projectile);

                        entityCommandBuffer.SetComponent(entityInQueryIndex, projectileInstance, new Translation()
                        {
                            Value = playerShootingInputData.shootingOffset + translation.Value
                        });

                        float3 forward = math.forward(rotation.Value);
                        entityCommandBuffer.SetComponent(entityInQueryIndex, projectileInstance, new PhysicsVelocity()
                        {
                            Linear = forward * playerShootingInputData.projectileLaunchVelcoity
                        });
                    }
                }).Schedule(inputDeps);

            _commandBufferSystem.AddJobHandleForProducer(jobHandle);
            return jobHandle;
        }
    }
}