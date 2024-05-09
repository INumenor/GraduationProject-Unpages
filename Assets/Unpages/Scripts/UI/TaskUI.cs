using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public Transform taskMaterialScrollViewContent;
    public Image taskRawImage;
    public GameObject taskMaterialPrefab;
    Image itemImage;
    public void Init(Sprite tex, List<FoodType> foodTypes)
    {
        taskRawImage.sprite = tex;
        foreach (FoodType foodtype in foodTypes)
        {
            GameObject taskMaterial = Instantiate(taskMaterialPrefab,taskMaterialScrollViewContent);
             itemImage.sprite = GameService.Instance.networkItems.GetImageFoodItem(foodtype);
            Debug.Log(GameService.Instance.networkItems.GetImageFoodItem(foodtype)+"  rjgdýorjgjg");
        }
    }
}
