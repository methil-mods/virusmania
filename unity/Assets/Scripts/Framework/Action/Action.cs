using System;
using System.Collections.Generic;

namespace Framework.Action
{
	[Serializable]
    public abstract class Action
    {
        public abstract void Execute();
    }
}