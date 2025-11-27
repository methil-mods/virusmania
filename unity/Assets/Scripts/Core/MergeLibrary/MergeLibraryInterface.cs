using System.Collections.Generic;
using System.Linq;
using Core.Item.Cook;
using Core.Item.Merge;
using Framework.Controller;
using UnityEngine;

namespace Core.MergeLibrary
{
    public class MergeLibraryInterface : InterfaceController<MergeLibraryInterface>
    {
        public RectTransform mergeRecipeTransform;
        public RectTransform boilingRecipeTransform;

        [SerializeField] private GameObject instructionUiPrefab;
        [SerializeField] private bool onlyOnBoarding = false;
        
        public override void Start()
        {
            base.Start();
            
            foreach (Transform children in mergeRecipeTransform)
                Destroy(children.gameObject);
            foreach (Transform children in boilingRecipeTransform)
                Destroy(children.gameObject);
            
            MergeDatabase mergeDatabase = MergeDatabase.Instance;

            foreach (MergeRecipeBase mergeRecipe in mergeDatabase.Database
                         .FindAll(mergeRecipe => mergeRecipe.isOnBoarding == onlyOnBoarding))
            {
                if (mergeRecipe is MergeVirusRecipe mergeVirusRecipe)
                {
                    var go = Instantiate(instructionUiPrefab, mergeRecipeTransform);
                    go.name = "Instruction for : " + mergeRecipe.name;
                    go.GetComponent<MergeInstruction>().SetupItem(
                        mergeVirusRecipe.inputItems.ToList().Cast<Item.Item>().ToList(), 
                        mergeRecipe.GetResultItem().Item
                        );
                }
            }
            
            foreach (CookRecipe cookRecipe in CookDatabase.Instance.Database
                         .FindAll(cookRecipe => cookRecipe.isOnBoarding == onlyOnBoarding))
            {
                var go = Instantiate(instructionUiPrefab, boilingRecipeTransform);
                go.name = "Instruction for : " + cookRecipe.name;
                go.GetComponent<MergeInstruction>().SetupItem(
                    new List<Item.Item>()
                    {
                        cookRecipe.inputItem 
                    } , 
                    cookRecipe.resultItem
                );
                
            }
            
        }
    }
}