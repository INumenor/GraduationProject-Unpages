using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public Transform taskMaterialScrollViewContent;
    public Image taskRawImage;
    public GameObject taskMaterialPrefab;
    public void Init(Sprite tex, List<FoodType> foodTypes)
    {
        taskRawImage.sprite = tex;
        foreach (FoodType foodtype in foodTypes)//bura yanlýþ çalýþýyo hatta çalýþmýyo
        {
            GameObject taskMaterial = Instantiate(taskMaterialPrefab,taskMaterialScrollViewContent);
            taskMaterial.GetComponent<Image>().sprite = GameService.Instance.networkItems.GetImageFoodItem(foodtype);
        }
    }
}
