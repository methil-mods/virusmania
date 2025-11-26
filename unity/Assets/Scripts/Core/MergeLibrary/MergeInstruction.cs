using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MergeLibrary
{
    public class MergeInstruction: MonoBehaviour
    {
        [SerializeField] private GameObject imageItem;
        [SerializeField] private GameObject arrowImage;
        
        public void SetupItem(List<Item.Item> itemOnLeft, Item.Item itemOnRight)
        {
            foreach (Transform children in this.transform)
                Destroy(children.gameObject);
            
            foreach (var item in itemOnLeft)
            {
                // Left Item
                var goItem = Instantiate(imageItem, this.transform);
                goItem.transform.GetComponent<MergeItem>().SetupItem(item);

            }
            
            Instantiate(arrowImage, this.transform);
            
            // Right Item
            var goItemRight = Instantiate(imageItem, this.transform);
            goItemRight.transform.GetComponent<MergeItem>().SetupItem(itemOnRight);
        }
    }
}