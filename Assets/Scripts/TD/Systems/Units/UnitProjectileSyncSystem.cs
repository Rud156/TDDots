using System;
using TD.Data.Units;
using TD.Managers.MainScene;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TD.Systems.Units
{
    [AlwaysSynchronizeSystem]
    public class UnitProjectileSyncSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Entities
                .WithoutBurst()
                .ForEach((ref UnitProjectileSyncData unitProjectileSync, in Translation translation) =>
                {
                    UnitProjectileSyncData.EffectType effectType = unitProjectileSync.effectType;
                    if (unitProjectileSync._effectInitialized)
                    {
                        switch (effectType)
                        {
                            case UnitProjectileSyncData.EffectType.None:
                                Debug.Log("Invalid Effect Type");
                                break;

                            case UnitProjectileSyncData.EffectType.BlueBullTrail:
                            {
                                float3 position = translation.Value;
                                BlueBullTrailSyncManager.Instance.UpdateTrailPosition(
                                    position, unitProjectileSync._effectIndex
                                );
                            }
                                break;

                            case UnitProjectileSyncData.EffectType.PrototypeZeroEffect:
                            {
                                float3 position = translation.Value;
                                PrototypeZeroEffectSyncManager.Instance.UpdateEffectPosition(
                                    position, unitProjectileSync._effectIndex
                                );
                            }
                                break;

                            case UnitProjectileSyncData.EffectType.RockerTrail:
                            {
                                float3 position = translation.Value;
                                RockerTrailSyncManager.Instance.UpdateTrailPosition(
                                    position, unitProjectileSync._effectIndex
                                );
                            }
                                break;

                            case UnitProjectileSyncData.EffectType.Max:
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
                            case UnitProjectileSyncData.EffectType.None:
                                Debug.Log("Invalid Effect Requested");
                                break;

                            case UnitProjectileSyncData.EffectType.BlueBullTrail:
                            {
                                int trailIndex = BlueBullTrailSyncManager.Instance
                                    .GetTrailEffectIndex(translation.Value);

                                unitProjectileSync._effectIndex = trailIndex;
                                unitProjectileSync._effectInitialized = true;
                            }
                                break;

                            case UnitProjectileSyncData.EffectType.PrototypeZeroEffect:
                            {
                                int effectIndex = PrototypeZeroEffectSyncManager.Instance
                                    .GetEffectParticleIndex(translation.Value);

                                unitProjectileSync._effectIndex = effectIndex;
                                unitProjectileSync._effectInitialized = true;
                            }
                                break;

                            case UnitProjectileSyncData.EffectType.RockerTrail:
                            {
                                int trailIndex = RockerTrailSyncManager.Instance
                                    .GetTrailEffectIndex(translation.Value);

                                unitProjectileSync._effectIndex = trailIndex;
                                unitProjectileSync._effectInitialized = true;
                            }
                                break;

                            case UnitProjectileSyncData.EffectType.Max:
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