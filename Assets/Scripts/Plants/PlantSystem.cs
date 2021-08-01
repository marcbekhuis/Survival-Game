using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantSystem : Singleton<PlantSystem>
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap secondTilemap;
    [SerializeField] private Transform focusTransform;
    [SerializeField][Min(0)] private float visibleRange = 10;

    [HideInInspector] public List<Plant> plantedPlants = new List<Plant>();

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused) return;

        GrowPlant();
        //PlantCrop();
    }

    //private void PlantCrop()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        if (plants.Length <= 0) return;

    //        Vector2Int gridPosition = Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0));
    //        if (HasPlant(gridPosition)) return;

    //        int index = Random.Range(0, plants.Length);
    //        plantedPlants.Add(plants[index].Create(plants[index], new Tilemap[] { tilemap, secondTilemap }, gridPosition, Random.Range(0, plants[index].growthTime)));
    //    }
    //}

    private bool HasPlant(Vector2Int gridPosition)
    {
        foreach (var plantedCrop in plantedPlants)
        {
            if(plantedCrop.GridPosition == gridPosition) return true;
        }
        return false;
    }

    private void GrowPlant()
    {
        foreach (var plantedCrop in plantedPlants)
        {
            plantedCrop.SetAge(plantedCrop.age + Time.deltaTime, Vector2.Distance(focusTransform.position, plantedCrop.GridPosition) <= visibleRange);
        }
    }
}
