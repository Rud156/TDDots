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
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;

            Entities.WithAll<PlayerTag>()
                .WithoutBurst()
                .WithStructuralChanges()
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
                        Entity projectileInstance = EntityManager.Instantiate(playerShootingInputData.projectile);

                        Entity shootingPoint = playerShootingInputData.shootingPoint;
                        float3 shootingPosition = EntityManager.GetComponentData<LocalToWorld>(shootingPoint).Position;

                        EntityManager.SetComponentData(projectileInstance, new Translation()
                        {
                            Value = shootingPosition
                        });

                        float3 forward = math.forward(rotation.Value);
                        EntityManager.SetComponentData(projectileInstance, new PhysicsVelocity()
                        {
                            Linear = forward * playerShootingInputData.projectileLaunchVelcoity
                        });
                    }
                }).Run();

            return default;
        }
    }
}