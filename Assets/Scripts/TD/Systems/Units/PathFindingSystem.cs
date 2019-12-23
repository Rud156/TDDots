using TD.Data.Units;
using TD.Managers.MainScene;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TD.Systems.Units
{
    [AlwaysSynchronizeSystem]
    public class PathFindingSystem : JobComponentSystem
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
                            bool currentPathValid = PathManager.Instance.GetCurrentPointInPath(
                                unitMovementData._currentPathIndex, out currentTargetPoint
                            );

                            if (currentPathValid)
                            {
                                unitMovementData._targetPosition = currentTargetPoint;
                                unitMovementData._initalPathFindingLaunched = true;

                                unitMovementData._startPosition = translation.Value;
                                unitMovementData._lerpAmount = 0;
                            }
                        }
                        else if (math.distance(translation.Value, unitMovementData._targetPosition) <=
                                 unitMovementData.minNextPositionDistacne)
                        {
                            float3 nextTargetPoint;
                            bool nextPathValid = PathManager.Instance.GetNextPointInPath(
                                unitMovementData._currentPathIndex, out nextTargetPoint
                            );

                            if (nextPathValid)
                            {
                                unitMovementData._targetPosition = nextTargetPoint;
                                unitMovementData._currentPathIndex += 1;

                                unitMovementData._startPosition = translation.Value;
                                unitMovementData._lerpAmount = 0;
                            }
                            else
                            {
                                unitMovementData._reachedEndPoint = true;
                            }
                        }

                        unitMovementData._lerpAmount += unitMovementData.lerpSpeed * deltaTime;
                        unitMovementData._lerpAmount = math.clamp(unitMovementData._lerpAmount, 0, 1);
                        float3 lerpDistance = math.lerp(
                            unitMovementData._startPosition,
                            unitMovementData._targetPosition,
                            unitMovementData._lerpAmount
                        );
                        translation.Value = lerpDistance;
                    }
                }).Run();

            return default;
        }
    }
}