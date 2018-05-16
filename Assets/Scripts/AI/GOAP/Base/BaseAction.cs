﻿using System.Collections.Generic;

namespace AI.GOAP
{
    public abstract class BaseAction :
        IGOAPImmutable<BaseAction>,
        IEqualityComparer<BaseAction>
    {
        #region Variables

        protected bool _complete;

        #endregion

        #region Properties

        public string ID { get; set; }

        public string Dialog { get; set; }

        public int Cost { get; set; }

        public int TimeInMinutes { get; set; }

        public WorldState Preconditions { get; set; }

        public WorldState Effects { get; set; }

        public bool LastAction { get; set; }

        #endregion

        public abstract void Update(AIModule module);

        public abstract bool CheckContext();

        public virtual bool Validate(WorldState current)
        {
            return CheckPreconditions(current);
        }

        public virtual WorldState ApplyEffects(WorldState state)
        {
            var temp = state.Copy();

            for (int i = 0; i < WorldState.SymbolCount; i++)
                if (Effects[i] != STATE_SYMBOL.UNSET)
                    temp[i] = Effects[i];

            return temp;
        }

        public virtual WorldState ApplyPreconditions(WorldState state)
        {
            var temp = state.Copy();

            for (int i = 0; i < WorldState.SymbolCount; i++)
                if (Preconditions[i] == STATE_SYMBOL.SATISFIED)
                    temp[i] = STATE_SYMBOL.SATISFIED;

            return temp;
        }

        public virtual bool CheckPreconditions(WorldState state)
        {
            for (int i = 0; i < WorldState.SymbolCount; i++)
            {
                var s = Preconditions[i];

                if (s == STATE_SYMBOL.UNSET)
                    continue;

                if (s != state[i])
                    return false;
            }

            return true;
        }

        public virtual bool CheckEffects(WorldState state)
        {
            for (int i = 0; i < WorldState.SymbolCount; i++)
            {
                var s = Effects[i];

                if (s == STATE_SYMBOL.UNSET)
                    continue;

                if (s != state[i])
                    return true;
            }

            return false;
        }

        public virtual void Activate(AIModule module)
        {
            module.Board.Dialog = Dialog;

            if (TimeInMinutes > 0)
            {
                int hours = TimeInMinutes / 60;
                int minutes = TimeInMinutes % 60;

                Timer.StartNew(hours, minutes, () =>
                {
                    _complete = true;
                });
            }
        }

        public virtual WorldState Deactivate(AIModule module, WorldState current)
        {
            module.Board.Dialog = string.Empty;
            return ApplyEffects(current);
        }

        public virtual bool IsComplete()
        {
            return _complete;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseAction))
                return false;

            var action = (BaseAction)obj;

            return string.CompareOrdinal(ID, action.ID) == 0;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public bool Equals(BaseAction x, BaseAction y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(BaseAction obj)
        {
            return obj.GetHashCode();
        }

        public abstract BaseAction Copy();

        protected void Setup(BaseAction action)
        {
            action.Cost = Cost;
            action.Dialog = Dialog;
            action.Effects = Effects;
            action.ID = ID;
            action.Preconditions = Preconditions;
            action.TimeInMinutes = TimeInMinutes;
            LastAction = false;
        }
    }
}