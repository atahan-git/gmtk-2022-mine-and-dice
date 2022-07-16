using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu]
public class ProductionCardRecipe : ScriptableObject
{
    [Serializable]
    public class ItemWithSpawnChance {
        public GameObject item;
        public int chance;
    }


    public ItemWithSpawnChance[] myResults;


    public GameObject GetRandomItem() {
        var chances = new List<int>();

        for (int i = 0; i < myResults.Length; i++) {
            for (int j = 0; j < myResults[i].chance; j++) {
                chances.Add(i);
            }
        }

        var result = chances[Random.Range(0, chances.Count)];

        return myResults[result].item;
    }
}
