using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEngine;


[CreateAssetMenu]
public class SmeltingRecipe : ScriptableObject {
    public Ingredients input;
    public GameObject output;
}
