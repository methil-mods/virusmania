using System;
using Core.Item;
using Core.Money;
using Framework.Controller;
using Framework.Extensions;
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
            Brief newBrief = BriefDatabase.Instance.Database.GetRandom();
            actualBrief = newBrief;
            briefInterface.SetupNewBriefShow(newBrief);
        }

        public bool TryToCompleteBrief(HoldItem itemToValidate)
        {
            if (actualBrief != null && actualBrief.wantedItem == itemToValidate.Item)
            {
                MoneyController.Instance.AddMoney(actualBrief.moneyGiven);
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