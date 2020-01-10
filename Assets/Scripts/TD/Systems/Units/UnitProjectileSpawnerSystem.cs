using TD.Data.Units;
using Unity.Entities;
using Unity.Jobs;

namespace TD.Systems.Units
{
    public class UnitProjectileSpawnerSystem : JobComponentSystem
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
            var entityCommnadBuffer = _commandBufferSystem.CreateCommandBuffer().ToConcurrent();

            JobHandle jobHandle = Entities.ForEach(
                (Entity entity, int entityInQueryIndex, ref UnitProjectileSpawnerData unitProjectileSpawner) =>
                {
                    if (unitProjectileSpawner._timeLeftBetweenShot <= 0)
                    {
                        entityCommnadBuffer.Instantiate(entityInQueryIndex, unitProjectileSpawner.projectile);
                        unitProjectileSpawner._timeLeftBetweenShot = unitProjectileSpawner.timeBetweenShots;
                    }
                    else
                    {
                        unitProjectileSpawner._timeLeftBetweenShot -= deltaTime;
                    }
                }).Schedule(inputDeps);

            _commandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}