using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.Controller;

namespace Core.Computer
{
    public class ComputerInterface : InterfaceController<ComputerInterface>
    {
        public List<ComputerApplication> applications;

        private void Start()
        {
            foreach (var app in applications)
            {
                app.applicationPanel.SetActive(false);
                app.applicationButton.onClick.AddListener(() => ShowPanel(app));
            }
        }

        private void ShowPanel(ComputerApplication app)
        {
            foreach (var otherApp in applications)
            {
                otherApp.applicationPanel.SetActive(false);
            }
            app.applicationPanel.SetActive(true);
        }
    }

    [Serializable]
    public class ComputerApplication
    {
        public Button applicationButton;
        public GameObject applicationPanel;
    }
}