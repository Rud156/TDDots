using TD.Data.Units;
using TD.Utils;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;
using Random = Unity.Mathematics.Random;

namespace TD.Systems.Units
{
    public class UnitSpawnerSystem : JobComponentSystem
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
            uint seed = (uint) (UnityEngine.Random.value * 10000);
            Random random = new Random(seed);
            var entityCommandBuffer = _commandBufferSystem.CreateCommandBuffer().ToConcurrent();

            JobHandle jobHandle = Entities.ForEach(
                (Entity entity, int entityInQueryIndex, ref UnitSpawnerData spawnerData) =>
                {
                    if (spawnerData._spawnInActive)
                    {
                        return;
                    }

                    // Spawn Light Enemy Update Time
                    if (spawnerData._currentLightUnitTime <= 0)
                    {
                        if (spawnerData._currentLightEntityInBetweenTime <= 0)
                        {
                            float value = random.NextFloat();
                            if (value <= 0.5f)
                            {
                                entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.lightEntityA);
                            }
                            else
                            {
                                entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.lightEntityB);
                            }

                            spawnerData._currentLightEntityCount += 1;
                            if (spawnerData._currentLightEntityCount >= spawnerData.lightEntityBurstCount)
                            {
                                spawnerData._currentLightUnitTime = spawnerData
                                    ._currentDeadPerSec
                                    .Map(spawnerData.minDeathPerSec, spawnerData.maxDeathPerSec,
                                        spawnerData.maxTimeBetweenLightEntity, spawnerData.minTimeBetweenLightEntity);

                                spawnerData._currentLightEntityCount = 0;
                            }
                            else
                            {
                                spawnerData._currentLightEntityInBetweenTime = spawnerData.lightBurstTimeBetween;
                            }
                        }
                        else
                        {
                            spawnerData._currentLightEntityInBetweenTime -= deltaTime;
                        }
                    }
                    else
                    {
                        spawnerData._currentLightUnitTime -= deltaTime;
                    }

                    // Spawn Heavy Enemy Update Time
                    if (spawnerData._currentHeavyUnitTime <= 0)
                    {
                        float value = random.NextFloat();
                        if (value <= 0.5f)
                        {
                            entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.heavyEntityA);
                        }
                        else
                        {
                            entityCommandBuffer.Instantiate(entityInQueryIndex, spawnerData.heavyEntityB);
                        }

                        spawnerData._currentHeavyUnitTime = spawnerData
                            ._currentDeadPerSec
                            .Map(spawnerData.minDeathPerSec, spawnerData.maxDeathPerSec,
                                spawnerData.maxTimeBetweenHeavyEntity, spawnerData.maxTimeBetweenHeavyEntity);
                    }
                    else
                    {
                        spawnerData._currentHeavyUnitTime -= deltaTime;
                    }
                }).Schedule(inputDeps);

            _commandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}