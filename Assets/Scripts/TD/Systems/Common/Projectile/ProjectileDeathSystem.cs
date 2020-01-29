using System;
using TD.Data.Common.Projectile;
using TD.Managers.MainScene;
using TD.Managers.MainScene.Effects;
using Unity.Entities;
using Unity.Jobs;

namespace TD.Systems.Common.Projectile
{
    [AlwaysSynchronizeSystem]
    public class ProjectileDeathSystem : JobComponentSystem
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
            var entityCommandBuffer = _commandBufferSystem.CreateCommandBuffer();

            Entities
                .WithoutBurst()
                .ForEach((
                    Entity entity, int entityInQueryIndex,
                    ref ProjectileDeathData projectileDeath, in ProjectileSyncData projectileSync) =>
                {
                    if (projectileDeath._timeInitialized)
                    {
                        if (projectileDeath._currentLifetimeLeft <= 0)
                        {
                            switch (projectileSync.effectType)
                            {
                                case ProjectileSyncData.EffectType.None:
                                    break;

                                case ProjectileSyncData.EffectType.BlueBullTrail:
                                    BlueBullTrailSyncManager.Instance.StopTrailEffect(projectileSync._effectIndex);
                                    break;

                                case ProjectileSyncData.EffectType.PrototypeZeroEffect:
                                    PrototypeZeroEffectSyncManager.Instance
                                        .StopParticleEffect(projectileSync._effectIndex);
                                    break;

                                case ProjectileSyncData.EffectType.RockerTrail:
                                    RockerTrailSyncManager.Instance.StopTrailEffect(projectileSync._effectIndex);
                                    break;

                                case ProjectileSyncData.EffectType.Max:
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            entityCommandBuffer.DestroyEntity(entity);
                        }
                        else
                        {
                            projectileDeath._currentLifetimeLeft -= deltaTime;
                        }
                    }
                    else
                    {
                        projectileDeath._currentLifetimeLeft = projectileDeath.projectileLifeTime;
                        projectileDeath._timeInitialized = true;
                    }
                }).Run();

            return default;
        }
    }
}