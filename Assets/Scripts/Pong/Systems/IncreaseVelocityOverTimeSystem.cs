using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

namespace Pong
{
    [AlwaysSynchronizeSystem]
    public class IncreaseVelocityOverTimeSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((ref PhysicsVelocity velocity, in SpeedIncreaseOverTimeData speedData) =>
            {
                float2 modifier = new float2(speedData.increasePerSecond * deltaTime);

                float2 newVelocity = velocity.Linear.xy;
                newVelocity += math.lerp(-modifier, modifier, math.sign(newVelocity));
                velocity.Linear.xy = newVelocity;
            }).Run();

            return default;
        }
    }
}