using TD.Data.Player;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace TD.Systems.Player
{
    public class PlayerMovementSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;

            JobHandle jobHandle = Entities
                .WithAll<PlayerTag>()
                .ForEach((ref PhysicsVelocity physicsVelocity,
                    ref RotationEulerXYZ rotationEuler, ref PlayerMovementInputData playerMovementInputData,
                    in Rotation rotation) =>
                {
                    // Movement
                    playerMovementInputData._currentVelocity +=
                        playerMovementInputData._movementData.y * playerMovementInputData.velocityChangeRate * deltaTime;
                    if (playerMovementInputData._movementData.y == 0)
                    {
                        playerMovementInputData._currentVelocity -= playerMovementInputData.velocityChangeRate * deltaTime;
                    }

                    playerMovementInputData._currentVelocity = math.clamp(playerMovementInputData._currentVelocity, 0, playerMovementInputData.maxVelocity);
                    physicsVelocity.Linear = math.forward(rotation.Value) * playerMovementInputData._currentVelocity;

                    // Rotation
                    float3 currentRotation = rotationEuler.Value;
                    currentRotation.y += playerMovementInputData._movementData.x * playerMovementInputData.rotationSpeed * deltaTime;
                    rotationEuler.Value = currentRotation;
                })
                .Schedule(inputDeps);

            return jobHandle;
        }
    }
}