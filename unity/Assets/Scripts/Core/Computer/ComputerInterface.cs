using System;
using System.Collections.Generic;
using Core.Input;
using UnityEngine;
using UnityEngine.UI;
using Framework.Controller;

namespace Core.Computer
{
    public class ComputerInterface : InterfaceController<ComputerInterface>
    {
        public void Start()
        {
            base.Start();

            this.OnPanelOpen += () => { 
                InputDatabase.Instance.moveAction.action.Disable();
            };

            this.OnPanelClose += () => { 
                InputDatabase.Instance.moveAction.action.Enable();
            };
        }
    }
}