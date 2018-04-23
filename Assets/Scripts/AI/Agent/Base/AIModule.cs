﻿using Framework.Debugging;
using UnityEngine;

namespace AI
{
    public abstract class AIModule : MonoBehaviour
    {
        #region Variables

        protected Blackboard _blackboard;
        protected WorkingMemory _memory;

        #endregion

        private void Awake()
        {
            _blackboard = GetComponentInParent<Blackboard>();
            _memory = GetComponentInParent<WorkingMemory>();

            if (!_blackboard)
            {
                Debugger.LogFormat(LOG_TYPE.ERROR,
                   "{0}: {1} missing!\n",
                    gameObject.name, typeof(Blackboard).Name);
            }

            if (!_memory)
            {
                Debugger.LogFormat(LOG_TYPE.ERROR,
                   "{0}: {1} missing!\n",
                    gameObject.name, typeof(WorkingMemory).Name);
            }
        }
    }
}