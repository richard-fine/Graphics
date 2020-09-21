using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.Universal.Internal
{
    /// <summary>
    /// Contains a database of built-in resource GUIds. These are used to load built-in resource files.
    /// </summary>
    public static class ResourceGuid
    {
        /// <summary>
        /// GUId for the <c>ScriptableRendererFeature</c> template file.
        /// </summary>
        public static readonly string rendererTemplate = "51493ed8d97d3c24b94c6cffe834630b";
    }
}

namespace UnityEditor.Rendering.Universal
{
    static class EditorUtils
    {
        // Each group is separate in the menu by a menu bar
        public const int lwrpAssetCreateMenuPriorityGroup1 = CoreUtils.assetCreateMenuPriority1;
        public const int lwrpAssetCreateMenuPriorityGroup2 = CoreUtils.assetCreateMenuPriority1 + 50;
        public const int lwrpAssetCreateMenuPriorityGroup3 = lwrpAssetCreateMenuPriorityGroup2 + 50;

        private static string[] m_DefaultRenderingLayerNames;
        internal static string[] defaultRenderingLayerNames
        {
            get
            {
                if (m_DefaultRenderingLayerNames == null)
                {
                    m_DefaultRenderingLayerNames = new string[32];
                    for (int i = 0; i < m_DefaultRenderingLayerNames.Length; ++i)
                    {
                        m_DefaultRenderingLayerNames[i] = string.Format("Layer{0}", i + 1);
                    }
                }
                return m_DefaultRenderingLayerNames;
            }
        }

        internal class Styles
        {
            //Measurements
            public static float defaultLineSpace = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            public static float defaultIndentWidth = 12;
        }

        public static void DrawCascadeSplitGUI<T>(ref SerializedProperty shadowCascadeSplit)
        {
            float[] cascadePartitionSizes = null;
            Type type = typeof(T);
            if (type == typeof(float))
            {
                cascadePartitionSizes = new float[] { shadowCascadeSplit.floatValue };
            }
            else if (type == typeof(Vector3))
            {
                Vector3 splits = shadowCascadeSplit.vector3Value;
                cascadePartitionSizes = new float[]
                {
                    Mathf.Clamp(splits[0], 0.0f, 1.0f),
                    Mathf.Clamp(splits[1] - splits[0], 0.0f, 1.0f),
                    Mathf.Clamp(splits[2] - splits[1], 0.0f, 1.0f)
                };
            }
            if (cascadePartitionSizes != null)
            {
                EditorGUI.BeginChangeCheck();
                ShadowCascadeSplitGUI.HandleCascadeSliderGUI(ref cascadePartitionSizes);
                if (EditorGUI.EndChangeCheck())
                {
                    if (type == typeof(float))
                        shadowCascadeSplit.floatValue = cascadePartitionSizes[0];
                    else
                    {
                        Vector3 updatedValue = new Vector3();
                        updatedValue[0] = cascadePartitionSizes[0];
                        updatedValue[1] = updatedValue[0] + cascadePartitionSizes[1];
                        updatedValue[2] = updatedValue[1] + cascadePartitionSizes[2];
                        shadowCascadeSplit.vector3Value = updatedValue;
                    }
                }
            }
        }
    }
}
