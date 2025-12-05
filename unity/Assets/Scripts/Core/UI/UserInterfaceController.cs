using System;
using System.Collections.Generic;
using Core.Computer;
using Core.Input;
using Core.MergeLibrary;
using Core.Pause;
using Framework.Controller;
using Framework.Controller.Interfaces;
using UnityEngine;

namespace Core.UI
{
    public class UserInterfaceController : BaseController<UserInterfaceController>
    {
        private List<IInterfaceController> interfaces = new List<IInterfaceController>();

        private void Start()
        {
            interfaces = new List<IInterfaceController>()
            {
                ComputerInterface.Instance,
                MergeLibraryInterface.Instance
            };

            InputDatabase.Instance.pauseAction.action.performed += context => CallPauseMenu();
        }

        public void CallPauseMenu()
        {
            var openedInterface = interfaces.Find(_interface => _interface.IsOpen);

            if (openedInterface != null)
            {
                openedInterface.ClosePanel();
            }
            else
            {
                PauseMenu.Instance.CallPause();
            }
        }
    }
}