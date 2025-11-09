using Framework.Controller;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Core.Money
{
    public class MoneyController : BaseController<MoneyController>
    {
        public int money;

        public Action<int> OnMoneyAdded;
        public Action<int> OnMoneyRemoved;
        public Action OnMoneyInsufficient;

        public void AddMoney(int amount)
        {
            Debug.LogWarning("AddMoney called on MoneyController, amount: " + amount);
            money += amount;
            OnMoneyAdded?.Invoke(amount);
        }

        public bool RemoveMoney(int amount)
        {
            if (CanRemoveMoney(amount))
            {
                money -= amount;
                OnMoneyRemoved?.Invoke(amount);
                return true;
            }
            else
            {
                OnMoneyInsufficient?.Invoke();
                return false;
            }
        }

        public bool CanRemoveMoney(int amount)
        {
            return money - amount >= 0;
        }
    }
}