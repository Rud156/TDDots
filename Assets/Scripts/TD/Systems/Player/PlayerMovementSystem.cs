using TD.Data.Player;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class PlayerMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        JobHandle jobHandle = Entities
            .WithAll<PlayerTag>()
            .ForEach((ref PhysicsVelocity physicsVelocity,
                ref RotationEulerXYZ rotationEuler, ref PlayerInputData playerInputData,
                in Rotation rotation) =>
            {
                // Movement
                playerInputData._currentVelocity += playerInputData._movementData.y * playerInputData.velocityChangeRate * deltaTime;
                if (playerInputData._movementData.y == 0)
                {
                    playerInputData._currentVelocity -= playerInputData.velocityChangeRate * deltaTime;
                }

                playerInputData._currentVelocity = math.clamp(playerInputData._currentVelocity, 0, playerInputData.maxVelocity);
                physicsVelocity.Linear = math.forward(rotation.Value) * playerInputData._currentVelocity;

                // Rotation
                float3 currentRotation = rotationEuler.Value;
                currentRotation.y += playerInputData._movementData.x * playerInputData.rotationSpeed * deltaTime;
                rotationEuler.Value = currentRotation;
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}