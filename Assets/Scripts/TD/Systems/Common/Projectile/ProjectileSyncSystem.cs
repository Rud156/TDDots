using System;
using TD.Data.Common.Projectile;
using TD.Managers.MainScene.Effects;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TD.Systems.Common.Projectile
{
    [AlwaysSynchronizeSystem]
    public class ProjectileSyncSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Entities
                .WithoutBurst()
                .ForEach((ref ProjectileSyncData unitProjectileSync, in Translation translation) =>
                {
                    ProjectileSyncData.EffectType effectType = unitProjectileSync.effectType;
                    if (unitProjectileSync._effectInitialized)
                    {
                        switch (effectType)
                        {
                            case ProjectileSyncData.EffectType.None:
                                Debug.Log("Invalid Effect Type");
                                break;

                            case ProjectileSyncData.EffectType.BlueBullTrail:
                            {
                                float3 position = translation.Value;
                                BlueBullTrailSyncManager.Instance.UpdateTrailPosition(
                                    position, unitProjectileSync._effectIndex
                                );
                            }
                                break;

                            case ProjectileSyncData.EffectType.PrototypeZeroEffect:
                            {
                                float3 position = translation.Value;
                                PrototypeZeroEffectSyncManager.Instance.UpdateEffectPosition(
                                    position, unitProjectileSync._effectIndex
                                );
                            }
                                break;

                            case ProjectileSyncData.EffectType.RockerTrail:
                            {
                                float3 position = translation.Value;
                                RockerTrailSyncManager.Instance.UpdateTrailPosition(
                                    position, unitProjectileSync._effectIndex
                                );
                            }
                                break;

                            case ProjectileSyncData.EffectType.Max:
                                Debug.Log("Invalid Effect Type");
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        switch (effectType)
                        {
                            case ProjectileSyncData.EffectType.None:
                                Debug.Log("Invalid Effect Requested");
                                break;

                            case ProjectileSyncData.EffectType.BlueBullTrail:
                            {
                                int trailIndex = BlueBullTrailSyncManager.Instance
                                    .GetTrailEffectIndex(translation.Value);

                                unitProjectileSync._effectIndex = trailIndex;
                                unitProjectileSync._effectInitialized = true;
                            }
                                break;

                            case ProjectileSyncData.EffectType.PrototypeZeroEffect:
                            {
                                int effectIndex = PrototypeZeroEffectSyncManager.Instance
                                    .GetEffectParticleIndex(translation.Value);

                                unitProjectileSync._effectIndex = effectIndex;
                                unitProjectileSync._effectInitialized = true;
                            }
                                break;

                            case ProjectileSyncData.EffectType.RockerTrail:
                            {
                                int trailIndex = RockerTrailSyncManager.Instance
                                    .GetTrailEffectIndex(translation.Value);

                                unitProjectileSync._effectIndex = trailIndex;
                                unitProjectileSync._effectInitialized = true;
                            }
                                break;

                            case ProjectileSyncData.EffectType.Max:
                                Debug.Log("Invalid Effect Requested");
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }).Run();

            return default;
        }
    }
}