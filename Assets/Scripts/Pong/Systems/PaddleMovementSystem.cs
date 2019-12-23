using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Pong
{
    [AlwaysSynchronizeSystem]
    public class PaddleMovementSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            float yBound = GameManager.main.yBound;

            // JobHandle myJob = Entities.ForEach((ref Translation translation, in PaddleMovementData movementData) =>
            // {
            //     translation.Value.y =
            //         math.clamp(translation.Value.y + (movementData.speed * movementData.direction * deltaTime),
            //             yBound,
            //             -yBound);
            // }).Schedule(inputDeps);

            Entities.ForEach((ref Translation translation, in PaddleMovementData movementData) =>
            {
                translation.Value.y =
                    math.clamp(translation.Value.y + (movementData.speed * movementData.direction * deltaTime),
                        -yBound,
                        yBound);
            }).Run();

            return default;
        }
    }
}