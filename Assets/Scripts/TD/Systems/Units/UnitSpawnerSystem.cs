using TD.Data.Units;
using TD.Utils;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TD.Systems.Units
{
    public class UnitSpawnerSystem : JobComponentSystem
    {
        private BeginInitializationEntityCommandBufferSystem _commandBufferSystem;
        private Random _random;

        protected override void OnCreate()
        {
            base.OnCreate();

            _random = new Random((uint) UnityEngine.Random.Range(1, 1000));
            _commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            Random random = _random;
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
                        for (int i = 0; i < spawnerData.lightEntityBurstCount; i++)
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
                        }

                        spawnerData._currentLightUnitTime = spawnerData
                            ._currentDeadPerSec
                            .Map(spawnerData.minDeathPerSec, spawnerData.maxDeathPerSec,
                                spawnerData.maxTimeBetweenLightEntity, spawnerData.minTimeBetweenLightEntity);
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