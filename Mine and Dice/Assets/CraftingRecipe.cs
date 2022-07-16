using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject {
    public Ingredients[] topLayer = new Ingredients[3];
    public Ingredients[] midLayer = new Ingredients[3];
    public Ingredients[] bottomLayer = new Ingredients[3];


    public GameObject result;


    private Ingredients[] finalArray = null;
    public Ingredients[] GetRecipe() {
        if (finalArray == null) {
            var ing = new List<Ingredients>();
            ing.AddRange(topLayer);
            ing.AddRange(midLayer);
            ing.AddRange(bottomLayer);

            finalArray = ing.ToArray();
        }

        return finalArray;
    }
}
