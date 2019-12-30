using TD.Data.Units;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace TD.Systems.Testing
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class TestTriggerCollision : JobComponentSystem
    {
        private BuildPhysicsWorld _buildPhysicsWorldSystem;
        private StepPhysicsWorld _stepPhysicsWorldSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            _buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle jobHandle = new TriggerCollisonCheckJob
            {
                enemies = GetComponentDataFromEntity<UnitEnemy>(true)
            }.Schedule(_stepPhysicsWorldSystem.Simulation, ref _buildPhysicsWorldSystem.PhysicsWorld, inputDeps);

            return jobHandle;
        }

        struct TriggerCollisonCheckJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<UnitEnemy> enemies;

            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.Entities.EntityA;
                Entity entityB = triggerEvent.Entities.EntityB;
            }
        }
    }
}