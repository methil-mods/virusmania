using System;
using Core.Item.Holder;
using Core.Player;
using Framework.Controller;
using Framework.Extensions;

namespace Core.Item
{
    public class ItemHoldInterface : BaseController<ItemHoldInterface>
    {
        public void Start()
        {
            var playerInteraction = PlayerController.Instance.updatables.FirstOfType<PlayerInteraction>();

            playerInteraction.OnItemAdded += UpdateInterface;
            playerInteraction.OnItemRemoved += UpdateInterface;
        }

        public void OnDisable()
        {
            var playerInteraction = PlayerController.Instance.updatables.FirstOfType<PlayerInteraction>();
            
            playerInteraction.OnItemAdded -= UpdateInterface;
            playerInteraction.OnItemRemoved -= UpdateInterface;
        }

        public void UpdateInterface(HoldItem holdItem)
        {
            
        }
    }
}