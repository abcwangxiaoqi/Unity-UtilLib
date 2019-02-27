using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptNodeFlow
{
    public class RuntimeNodeCanvas : NodeCanvas
    {
        static NodeController target;
        public static void Open(NodeController obj)
        {
            if (obj == null)
                return;

            if (window != null)
                window.Close();

            window = null;

            target = obj;
            window = GetWindow<RuntimeNodeCanvas>(string.Format("{0}({1})", obj.name, obj.nodeFlowData.name));
        }

        protected override void Awake()
        {
            base.Awake();

            EditorApplication.playModeStateChanged -= playModeStateChanged;
            EditorApplication.playModeStateChanged += playModeStateChanged;

            generateWindowData(target.nodeFlowData);
        }

        // close the window when playModeStateChanged
        private void playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingPlayMode)
            {
                window.Close();
            }
        }

        protected override void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                Close();
            }

            if (windowList == null)
                return;

            foreach (var item in windowList)
            {
                if (target.finished)
                {
                    if (!string.IsNullOrEmpty(target.error) && item.Id == target.currentID)
                    {
                        item.SetState(State.Error);
                    }
                    else
                    {
                        item.SetState(State.Idle);
                    }
                }
                else
                {
                    if (item.Id == target.currentID)
                    {
                        item.SetState(State.Running);
                    }
                    else
                    {
                        item.SetState(State.Idle);
                    }
                }
            }

            foreach (var entityId in target.nodePathMessage)
            {
                windowList.Find((BaseWindow w) =>
                {
                    return w.Id == entityId;
                }).Pass();
            }

            foreach (var router in target.routerPathMessage)
            {
                windowList.Find((BaseWindow w) =>
                {
                    return w.Id == router.id;
                }).Pass(router.conditionIndex);
            }


            base.OnGUI();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }

}