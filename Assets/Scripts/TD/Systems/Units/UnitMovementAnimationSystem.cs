using AnimatorSystems.Runtime.Authoring;
using TD.Data.Common;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace TD.Systems.Units
{
    [AlwaysSynchronizeSystem]
    public class UnitMovementAnimationSystem : JobComponentSystem
    {
        private static readonly int MovementAnimParam = Animator.StringToHash("Move");

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Entities
                .WithoutBurst()
                .ForEach((
                    in AnimatorAuthoring movementAnimation,
                    in RotateTowardsTargetData rotateTowardsTarget,
                    in Translation translation) =>
                { 
                    // movementAnimation.Animator.SetBool(
                    //     MovementAnimParam,
                    //     rotateTowardsTarget._lastPosition.Equals(translation.Value)
                    // );
                }).Run();

            return default;
        }
    }
}