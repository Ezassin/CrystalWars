﻿using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

struct ConquerTransitionJob : IJobForEachWithEntity<State, Translation, TargetPos, ConquerRange>
{
    public void Execute(Entity entity, int index, ref State c0, [ReadOnly] ref Translation c1, [ReadOnly] ref TargetPos c2, [ReadOnly] ref ConquerRange c3)
    {
        if(TransitionRules.TransitionToConquer(c3, c1, c2.Value))
        {
            if(c0.Value != States.Conquer)
            {
                RemoveStateData(c0.Value, entity, index);
                c0.Value = States.Conquer;
                TransitionSystem.buffer.AddComponent(index, entity, UnitData.DefaultConquerData);
            }
        }
    }

    private void RemoveStateData(States type, Entity e, int jobIndex)
    {
        switch(type)
        {
            case States.Idle:
                TransitionSystem.buffer.RemoveComponent<IdleData>(jobIndex, e);
                return;
            case States.Build:
                TransitionSystem.buffer.RemoveComponent<BuildData>(jobIndex, e);
                return;
            case States.Attack:
                TransitionSystem.buffer.RemoveComponent<AttackData>(jobIndex, e);
                return;
            case States.Conquer:
                TransitionSystem.buffer.RemoveComponent<ConquerData>(jobIndex, e);
                return;
        }
    }
}
