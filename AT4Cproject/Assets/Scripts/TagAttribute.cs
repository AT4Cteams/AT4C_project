using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// �^�O�̐�pUI��\�������邽�߂̑���
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class TagAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
/// <summary>
/// �^�O���̐�pUI��\�������邽�߂�PropertyDrawer
/// </summary>
[CustomPropertyDrawer(typeof(TagAttribute))]
public class TagAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // �Ώۂ̃v���p�e�B�������񂩂ǂ���
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        // �^�O�t�B�[���h��\��
        var tag = EditorGUI.TagField(position, label, property.stringValue);

        // �^�O���𔽉f
        property.stringValue = tag;
    }
}
#endif