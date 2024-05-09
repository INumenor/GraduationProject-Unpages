using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public Transform taskMaterialScrollViewContent;
    public RawImage taskRawImage;
    public GameObject taskMaterialPrefab;
    public void Init(Texture2D tex, List<FoodType> foodTypes)
    {
        taskRawImage.texture = tex;
        foreach (FoodType foodtype in foodTypes)
        {
            GameObject taskMaterial = Instantiate(taskMaterialPrefab,taskMaterialScrollViewContent);
            //taskMaterial.transform.SetParent(taskMaterialScrollViewContent);
            Texture2D itemImage = GameService.Instance.networkItems.GetImageFoodItem(foodtype);
        }
    }
}
