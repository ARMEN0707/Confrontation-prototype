#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class BuildingEditorWindow : EditorWindow
{
    private Vector3 _pos;
    private ReorderableList _buildings;

    private int _selectedType;
    private List<BuildingType> _freeTypes = new List<BuildingType>();
    private List<string> _buildingOptions = new List<string>();

    [MenuItem("Window/BuildingEditorWindow")]
    private static void Init() => GetWindow<BuildingEditorWindow>("BuildingEditorWindow", true);

    private void OnUpdate()
    {
        _freeTypes.Clear();
        foreach (var t in Enum.GetValues(typeof(BuildingType))) 
            _freeTypes.Add((BuildingType) t);
        
        foreach (var b in LevelManager.BuildingsConfig.BuildingConfigs)
        {
            if (_freeTypes.Contains(b.BuildingType))
                _freeTypes.Remove(b.BuildingType);
        }

        _buildingOptions.Clear();
        _buildingOptions.Add("None");
        foreach (var t in _freeTypes)
        {
            _buildingOptions.Add(t.ToString());
        }
    }

    private void OnEnable()
    {
        OnUpdate();
        
        _buildings = new ReorderableList(LevelManager.BuildingsConfig.BuildingConfigs, typeof(BuildingConfig), 
            false, true, true, true)
        {
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 2;
                var conf = LevelManager.BuildingsConfig.BuildingConfigs[index];
                
                rect.xMax /= 2;
                EditorGUI.LabelField(rect, conf.BuildingType.ToString());
                
                rect.x = rect.xMax;
                conf.Cost = EditorGUI.IntField(rect, conf.Cost);
            },
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Buildings");
            },
            onAddCallback = list =>
            {
                if (_freeTypes.Count != 0 && _selectedType != 0)
                    LevelManager.BuildingsConfig.BuildingConfigs.Add(new BuildingConfig() 
                        {BuildingType = _freeTypes[_selectedType - 1]});
                
                OnUpdate();
            },
            onRemoveCallback = list =>
            {
                LevelManager.BuildingsConfig.BuildingConfigs.RemoveAt(list.index);
                OnUpdate();
            }
        };
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        _pos = EditorGUILayout.BeginScrollView(_pos);

        GUILayout.Label("Building Settings", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        _selectedType = EditorGUILayout.Popup("Choose Building", _selectedType, _buildingOptions.ToArray());
        EditorGUI.EndChangeCheck();
        
        _buildings?.DoLayoutList();
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
#endif