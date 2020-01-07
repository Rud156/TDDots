using TD.Data.Units;
using TD.Managers.MainScene;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TD.Systems.Units
{
    [AlwaysSynchronizeSystem]
    public class UnitMovementSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;

            Entities
                .WithoutBurst()
                .ForEach((ref Translation translation, ref UnitMovementData unitMovementData) =>
                {
                    if (!unitMovementData._reachedEndPoint)
                    {
                        if (!unitMovementData._initalPathFindingLaunched)
                        {
                            float3 currentTargetPoint;
                            bool currentPathValid = PathManager.Instance.GetCurrentPointInPathWithVariation(
                                unitMovementData._currentPathIndex, out currentTargetPoint
                            );

                            if (currentPathValid)
                            {
                                currentTargetPoint.y = unitMovementData.baseYOffset;
                                unitMovementData._targetPosition = currentTargetPoint;
                                unitMovementData._initalPathFindingLaunched = true;

                                float3 position = translation.Value;
                                unitMovementData._startPosition = new float3(
                                    position.x,
                                    unitMovementData.baseYOffset,
                                    position.z
                                );
                                unitMovementData._lerpAmount = 0;
                            }
                        }
                        else
                        {
                            float3 modifiedPosition = new float3(
                                translation.Value.x,
                                unitMovementData.baseYOffset,
                                translation.Value.z
                            );
                            if (math.distance(modifiedPosition, unitMovementData._targetPosition) <=
                                unitMovementData.minNextPositionDistacne)
                            {
                                float3 nextTargetPoint;
                                bool nextPathValid = PathManager.Instance.GetNextPointInPathWithVariation(
                                    unitMovementData._currentPathIndex, out nextTargetPoint
                                );

                                if (nextPathValid)
                                {
                                    nextTargetPoint.y = unitMovementData.baseYOffset;
                                    unitMovementData._targetPosition = nextTargetPoint;
                                    unitMovementData._currentPathIndex += 1;


                                    float3 position = translation.Value;
                                    unitMovementData._startPosition = new float3(
                                        position.x,
                                        unitMovementData.baseYOffset,
                                        position.z
                                    );
                                    unitMovementData._lerpAmount = 0;
                                }
                                else
                                {
                                    unitMovementData._reachedEndPoint = true;
                                }
                            }
                        }

                        unitMovementData._lerpAmount += unitMovementData.lerpSpeed * deltaTime;
                        unitMovementData._lerpAmount = math.clamp(unitMovementData._lerpAmount, 0, 1);
                        float3 lerpDistance = math.lerp(
                            unitMovementData._startPosition,
                            unitMovementData._targetPosition,
                            unitMovementData._lerpAmount
                        );
                        translation.Value = lerpDistance + new float3(0, unitMovementData.positionYOffset, 0);
                    }
                }).Run();

            return default;
        }
    }
}