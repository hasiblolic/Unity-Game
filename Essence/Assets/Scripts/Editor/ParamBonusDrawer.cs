using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WeaponItem))]
public class ParamBonusDrawer : Editor
{
    bool showItemValues = true;
    bool showWeaponValues = true;
    bool showParams = true;
    bool showStrength = true;
    bool showAgility = true;
    bool showFaith = true;
    bool showIntelligence = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WeaponItem w = (WeaponItem)target;
        /*
        #region Item Values
        showItemValues = EditorGUILayout.BeginFoldoutHeaderGroup(showItemValues, "Item Values");
        if (showItemValues)
        {
            EditorGUI.indentLevel++;
            w.itemName = EditorGUILayout.TextField("Name: ", w.itemName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Description: ");
            w.itemDescription = EditorGUILayout.TextArea(w.itemDescription);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            w.itemID = EditorGUILayout.TextField("ID: ", w.itemID);
            w.itemValue = EditorGUILayout.IntField("Value: ", w.itemValue);
            w.itemWeight = EditorGUILayout.IntField("Weight: ", w.itemWeight);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorGUILayout.Space();

        #region Weapon Values
        showWeaponValues = EditorGUILayout.BeginFoldoutHeaderGroup(showWeaponValues, "Weapon Values");
        if (showWeaponValues)
        {
            EditorGUI.indentLevel++;
            w.itemModel = (GameObject) EditorGUILayout.ObjectField("Model/Prefab", w.itemModel, typeof(GameObject), false);

            w.baseDamage = EditorGUILayout.IntField("Base Damage: ", w.baseDamage);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Is Two Handed");
            w.isTwoHanded = EditorGUILayout.Toggle(w.isTwoHanded);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion
        */
        EditorGUILayout.Space();

        #region Param Values
        showParams = EditorGUILayout.BeginFoldoutHeaderGroup(showParams, "Param Bonuses");
        if (showParams)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.Space();
            showStrength = EditorGUILayout.Foldout(showStrength, "Strength");
            if (showStrength)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scaling Rating");
                w.strength.scalingRating = (ScalingValues)EditorGUILayout.EnumPopup(w.strength.scalingRating);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scaling");
                w.strength.scaling = EditorGUILayout.Slider(w.strength.scaling, GetMinScaling(w.strength.scalingRating), GetMaxScaling(w.strength.scalingRating));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            showAgility = EditorGUILayout.Foldout(showAgility, "Agility");
            if (showAgility)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scaling Rating");
                w.dexterity.scalingRating = (ScalingValues)EditorGUILayout.EnumPopup(w.dexterity.scalingRating);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scaling");
                w.dexterity.scaling = EditorGUILayout.Slider(w.dexterity.scaling, GetMinScaling(w.dexterity.scalingRating), GetMaxScaling(w.dexterity.scalingRating));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            showFaith = EditorGUILayout.Foldout(showFaith, "Faith");
            if (showFaith)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scaling Rating");
                w.faith.scalingRating = (ScalingValues)EditorGUILayout.EnumPopup(w.faith.scalingRating);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scaling");
                w.faith.scaling = EditorGUILayout.Slider(w.faith.scaling, GetMinScaling(w.faith.scalingRating), GetMaxScaling(w.faith.scalingRating));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            showIntelligence = EditorGUILayout.Foldout(showIntelligence, "Intelligence");
            if (showIntelligence)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scaling Rating");
                w.intelligence.scalingRating = (ScalingValues)EditorGUILayout.EnumPopup(w.intelligence.scalingRating);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Scaling");
                w.intelligence.scaling = EditorGUILayout.Slider(w.intelligence.scaling, GetMinScaling(w.intelligence.scalingRating), GetMaxScaling(w.intelligence.scalingRating));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion
    }

    private float GetMinScaling(ScalingValues scalingRating)
    {
        switch (scalingRating)
        {
            case ScalingValues.S:
                return 1.41f;
            case ScalingValues.A:
                return 1.01f;
            case ScalingValues.B:
                return 0.81f;
            case ScalingValues.C:
                return 0.51f;
            case ScalingValues.D:
                return 0.25f;
            case ScalingValues.E:
                return 0.01f;
            default: return 0;
        }
    }

    private float GetMaxScaling(ScalingValues scalingRating)
    {
        switch (scalingRating)
        {
            case ScalingValues.S:
                return 2f;
            case ScalingValues.A:
                return 1.40f;
            case ScalingValues.B:
                return 1.0f;
            case ScalingValues.C:
                return 0.80f;
            case ScalingValues.D:
                return 0.50f;
            case ScalingValues.E:
                return 0.24f;
            default: return 0;
        }
    }
}
