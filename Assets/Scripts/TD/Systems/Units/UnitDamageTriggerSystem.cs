using TD.Data.Common;
using TD.Data.Units;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace TD.Systems.Units
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class UnitDamageTriggerSystem : JobComponentSystem
    {
        private BuildPhysicsWorld _buildPhysicsWorldSystem;
        private StepPhysicsWorld _stepPhysicsWorldSystem;
        private BeginInitializationEntityCommandBufferSystem _commandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            _buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
            _commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        [BurstCompile]
        struct TriggerCollisonCheckJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<UnitEnemy> Enemies;
            [ReadOnly] public ComponentDataFromEntity<ProjectileData> Projectiles;
            public ComponentDataFromEntity<HealthData> AllSystemsHealthData;

            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.Entities.EntityA;
                Entity entityB = triggerEvent.Entities.EntityB;

                bool isEntityAProjectile = Projectiles.Exists(entityA);
                bool isEntityBProjectile = Projectiles.Exists(entityB);

                bool isEntityAEnemy = Enemies.Exists(entityA);
                bool isEntityBEnemy = Enemies.Exists(entityB);

                // This means that EntityA is a player projectile
                // If EntityB is an enemy and !projectile reduce it's health
                if (isEntityAProjectile && !isEntityAEnemy)
                {
                    if (isEntityBEnemy && !isEntityBProjectile)
                    {
                        var healthComponent = AllSystemsHealthData[entityB];
                        var projectileComponent = Projectiles[entityA];

                        healthComponent.currentHelth -= projectileComponent.projectileDamageAmount;
                        AllSystemsHealthData[entityB] = healthComponent;

                        if (healthComponent.currentHelth <= 0)
                        {
                            // TODO: Play Death Effect When Enemy Dies
                            EntityCommandBuffer.DestroyEntity(entityB);
                        }
                    }
                }
                // This means that EntityB is a player projectile
                // If EntityA is an enemy and !projectile reduce it's health
                else if (isEntityBProjectile && !isEntityBEnemy)
                {
                    if (isEntityAEnemy && !isEntityAProjectile)
                    {
                        var healthComponent = AllSystemsHealthData[entityA];
                        var projectileComponent = Projectiles[entityB];

                        healthComponent.currentHelth -= projectileComponent.projectileDamageAmount;
                        AllSystemsHealthData[entityA] = healthComponent;

                        if (healthComponent.currentHelth <= 0)
                        {
                            // TODO: Play Death Effect When Enemy Dies
                            EntityCommandBuffer.DestroyEntity(entityA);
                        }
                    }
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle jobHandle = new TriggerCollisonCheckJob
            {
                Enemies = GetComponentDataFromEntity<UnitEnemy>(true),
                Projectiles = GetComponentDataFromEntity<ProjectileData>(true),
                AllSystemsHealthData = GetComponentDataFromEntity<HealthData>(),

                EntityCommandBuffer = _commandBufferSystem.CreateCommandBuffer()
            }.Schedule(_stepPhysicsWorldSystem.Simulation, ref _buildPhysicsWorldSystem.PhysicsWorld, inputDeps);

            _commandBufferSystem.AddJobHandleForProducer(jobHandle);

            return jobHandle;
        }
    }
}