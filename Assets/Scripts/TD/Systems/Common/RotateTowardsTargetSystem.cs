using TD.Data.Common;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TD.Systems.Common
{
    public class RotateTowardsTargetSystem : JobComponentSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            Entities.ForEach((ref RotateTowardsTargetData rotateTowardsTarget, in Translation translation) =>
            {
                rotateTowardsTarget._lastPosition = translation.Value;
            }).Run();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;

            JobHandle jobHandle = Entities.ForEach((ref RotateTowardsTargetData rotateTowardsTarget,
                ref Rotation rotation,
                in Translation translation) =>
            {
                float3 previousPosition = rotateTowardsTarget._lastPosition;
                float3 currentPosition = translation.Value;

                float3 directionVector = (currentPosition - previousPosition);
                quaternion rotationValue = quaternion.LookRotation(directionVector, math.up());

                rotation.Value = math.slerp(
                    rotation.Value,
                    rotationValue,
                    deltaTime * rotateTowardsTarget.rotationSpeed
                );

                rotateTowardsTarget._lastPosition = currentPosition;
            }).Schedule(inputDeps);

            return jobHandle;
        }
    }
}