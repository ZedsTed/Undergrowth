//#if UNITY_EDITOR

//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//using System;


////[CustomEditor(typeof(ShopManifest))]
//public class ShopManifestInspector : Editor
//{
//    protected ShopManifest Target {  get { return target as ShopManifest; } }

//    protected List<KeyValuePair<ItemDefinition, int>> Items => Target.stockedItems;

//    protected static KeyValuePair<ItemDefinition, int> item;

//    protected ItemDefinition newDef = null;

//    protected int newQuantity = 0;


//    public override void OnInspectorGUI()
//    {
//        bool changed = false;


//        EditorGUI.BeginChangeCheck();

//        if (Items == null)
//            Target.stockedItems = new List<KeyValuePair<ItemDefinition, int>>();

//        for (int i = 0, iC = Items.Count; i < iC; ++i)
//        {
//            if (i > 0)
//                GUILayout.Space(8f);

//            if (DrawShopEntry(Items, i))
//            {
//                changed = true;
//                break;
//            }

//            if (i == (iC - 1))
//                GUILayout.Space(36f);
//        }


//        changed |= EditorGUI.EndChangeCheck();

//        EditorGUILayout.BeginHorizontal();
//        changed |= DrawAdd();
//        EditorGUILayout.EndHorizontal();

//        if (GUILayout.Button("Save", GUILayout.ExpandWidth(true)))
//        {
//            EditorUtility.SetDirty(target);
//            AssetDatabase.SaveAssets();
//            AssetDatabase.Refresh();
//        }

//            if (changed)
//        {
//            EditorUtility.SetDirty(target);
//            GUI.FocusControl(null);
//        }

//    }


//    protected bool DrawAdd()
//    {
//       ItemDefinition selectedDef = EditorGUILayout.ObjectField("Item Definition", newDef, typeof(ItemDefinition), false, GUILayout.ExpandWidth(true)) as ItemDefinition;

//        newQuantity = EditorGUILayout.IntField(newQuantity, GUILayout.Width(40f));

//        if (selectedDef != newDef)
//        {
//            if (selectedDef == null)
//            {
//                newQuantity = 0;
//            }
//            else
//            {
//                newDef = selectedDef;
//            }
//        }

//        GUILayout.Space(8f);

//        if (GUILayout.Button("+", GUILayout.Width(20f)))
//        {
//            if (newQuantity == 0)
//            {
//                EditorUtility.DisplayDialog("Shop Manifest", "Every shop manifest entry must have a non-zero quantity.", "Fiiiine");
//            }
//            else if (newQuantity > 99)
//            {
//                EditorUtility.DisplayDialog("Shop Manifest", "Every shop manifest entry must have a quantity less than the maximum.", "Fiiiine");
//            }
//            else
//            {
//                Debug.Log("shopmaniinpext" + newQuantity);
//                Target.AddItemDefinition(newDef, newQuantity);
//                newDef = null;
//                newQuantity = 0;

//                return true;
//            }
//        }

//        return false;
//    }


//    protected bool DrawShopEntry(List<KeyValuePair<ItemDefinition, int>> items, int index)
//    {
//        KeyValuePair<ItemDefinition, int> kvp = items[index];

//        EditorGUILayout.BeginHorizontal();

//        ItemDefinition item = EditorGUILayout.ObjectField("Item Definition", kvp.Key, typeof(ItemDefinition), false, GUILayout.ExpandWidth(true)) as ItemDefinition;
        
//        int quantity = EditorGUILayout.IntField(kvp.Value, GUILayout.Width(40f));


//        KeyValuePair<ItemDefinition, int> newKvp = new KeyValuePair<ItemDefinition, int>(item, quantity);

//        Items[index] = newKvp;

//        //if (!kvp.Equals(newKvp))
//        //{
//        //    Items.RemoveAt(index);
//        //    Items.Insert(index, newKvp);
//        //    return true;
//        //}

//        GUILayout.Space(8f);

//        if (GUILayout.Button("-", GUILayout.Width(20f)))
//        {
//            Items.RemoveAt(index);
//            EditorGUILayout.EndHorizontal();
//            return true;
//        }

//        EditorGUILayout.EndHorizontal();

//        return false;
//    }
//}

//#endif