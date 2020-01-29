using TD.Data.Player;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        JobHandle jobHandle = Entities
            .WithAll<PlayerTag>()
            .ForEach((ref PhysicsVelocity physicsVelocity, ref Rotation rotation,
                in PlayerInputData playerInputData) =>
            {
                quaternion rotationQuat = rotation.Value;
                float3 forwardDirection = math.forward(rotationQuat);

                physicsVelocity.Linear =
                    forwardDirection * playerInputData._movementData.y *
                    playerInputData.movementSpeed;

                // TODO: Change this. Very Important
                float3 currentRotation = ToEuler(rotation.Value);
                currentRotation.y += playerInputData._movementData.x * playerInputData.rotationSpeed;

                rotation.Value = quaternion.Euler(currentRotation);
            })
            .Schedule(inputDeps);

        return jobHandle;
    }

    #region Utility Functions

    public static float3 ToEuler(quaternion quaternion)
    {
        float4 q = quaternion.value;
        double3 res;

        double sinr_cosp = +2.0 * (q.w * q.x + q.y * q.z);
        double cosr_cosp = +1.0 - 2.0 * (q.x * q.x + q.y * q.y);
        res.x = math.atan2(sinr_cosp, cosr_cosp);

        double sinp = +2.0 * (q.w * q.y - q.z * q.x);
        if (math.abs(sinp) >= 1)
        {
            res.y = math.PI / 2 * math.sign(sinp);
        }
        else
        {
            res.y = math.asin(sinp);
        }

        double siny_cosp = +2.0 * (q.w * q.z + q.x * q.y);
        double cosy_cosp = +1.0 - 2.0 * (q.y * q.y + q.z * q.z);
        res.z = math.atan2(siny_cosp, cosy_cosp);

        return (float3) res;
    }

    #endregion
}