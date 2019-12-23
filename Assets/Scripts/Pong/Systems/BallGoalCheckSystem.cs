using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pong
{
    [AlwaysSynchronizeSystem]
    public class BallGoalCheckSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);

            Entities
                .WithAll<BallTag>()
                // .WithStructuralChanges() // Don't use this
                .WithoutBurst()
                .ForEach((Entity entity, in Translation translation) =>
                {
                    float3 position = translation.Value;
                    float bound = GameManager.main.xBound;

                    if (position.x > bound)
                    {
                        GameManager.main.PlayerScored(0);
                        entityCommandBuffer.DestroyEntity(entity);
                    }
                    else if (position.x < -bound)
                    {
                        GameManager.main.PlayerScored(1);
                        entityCommandBuffer.DestroyEntity(entity);
                    }
                }).Run();

            entityCommandBuffer.Playback(EntityManager);
            entityCommandBuffer.Dispose();

            return default;
        }
    }
}