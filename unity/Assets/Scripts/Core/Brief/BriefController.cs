using System;
using Core.Item;
using Framework.Controller;
using UnityEngine;

namespace Core.Brief
{
    public class BriefController : BaseController<BriefController>
    {
        [SerializeField]
        BriefInterface briefInterface;
        [SerializeField]
        public Brief actualBrief;

        public void Start()
        {
            NewBrief();
        }
        
        public void NewBrief()
        {
            Brief newBrief = BriefDatabase.Instance.GetRandom();
            actualBrief = newBrief;
            briefInterface.SetupNewBriefShow(newBrief);
        }

        public bool TryToCompleteBrief(HoldItem itemToValidate)
        {
            if (actualBrief != null && actualBrief.wantedItem == itemToValidate.Item)
            {
                actualBrief = null;
                NewBrief();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}