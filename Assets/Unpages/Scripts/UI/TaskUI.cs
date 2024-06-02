using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public Transform taskMaterialScrollViewContent;
    public Image taskImage;
    public Image taskTimeImage;
    public GameObject taskMaterialPrefab;
    public Action<FoodRecipes,bool> taskTimeAction;
    public FoodRecipes foodRecipes;
    public void Init(FoodRecipes foodRecipes)
    {
        this.foodRecipes = foodRecipes;
        taskImage.sprite = foodRecipes.recipeImages;
        foreach (FoodType foodtype in foodRecipes.foodTypes)
        {
            GameObject taskMaterial = Instantiate(taskMaterialPrefab,taskMaterialScrollViewContent);
            taskMaterial.transform.GetChild(0).GetComponent<Image>().sprite = GameService.Instance.networkItems.GetImageFoodItem(foodtype);
            TaskTime(foodRecipes.recipeTime);
        }
    }
    public void TaskTime(float recipeTime)
    {
        Sequence taskSequence = DOTween.Sequence();
        taskSequence.Append(taskTimeImage.gameObject.transform.DOScale(new Vector3(0f, 0.6f, 1), recipeTime).SetEase(Ease.Linear)).OnComplete(() =>
        {
            taskTimeAction.Invoke(foodRecipes, false);
        });
        taskSequence.Insert(recipeTime / 2, taskTimeImage.DOColor(Color.red, recipeTime / 2));
        //if (taskTimeImage.transform.localScale.x < 0.5f)
        //{
            //taskSequence.Append(taskTimeImage.DOColor(Color.red, recipeTime / 2));
        //}
           
    }
}
