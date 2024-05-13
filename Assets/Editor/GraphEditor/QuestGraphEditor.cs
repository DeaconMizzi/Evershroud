using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace QuestSystem.QuestEditor
{
    public class QuestGraphEditor : GraphViewEditorWindow
    {
        public static Quest questForGraph;

        private QuestGraphView _questGraph;
        private bool _mouseClicked;

        [MenuItem("Tools/QuestGraph")]
        public static void OpenQuestGraphWindow()
        {
            questForGraph = null;
            var window = GetWindow<QuestGraphEditor>();
            window.titleContent = new GUIContent("QuestGraph");

        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolBar(); 
        }

        private void ConstructGraphView()
        {
            _questGraph = new QuestGraphView(this)
            {
                name = "Quest Graph"
            };

            if (questForGraph != null)
                _questGraph.questName = questForGraph.misionName;

            _questGraph.StretchToParentSize();

            rootVisualElement.Add(_questGraph);
        }

        private void GenerateToolBar()
        {
            var toolbar = new Toolbar();

            var nodeCreateButton = new Button(clickEvent: () => { _questGraph.CreateNode("NodeQuest", Vector2.zero); });
            nodeCreateButton.text = "Create Quest Node";

            toolbar.Add(nodeCreateButton);

            //Save
            toolbar.Add(new Button(clickEvent: () => SaveQuestData()) { text = "Save Quest Data" });
            toolbar.Add(new Button(clickEvent: () => LoadQuestData()) { text = "Load Quest Data" });

            //Current quest
            var ins = new ObjectField("Quest editing");
            ins.objectType = typeof(Quest);
            ins.RegisterValueChangedCallback(evt =>
            {
                questForGraph = evt.newValue as Quest;
            });

            toolbar.Add(ins);

            rootVisualElement.Add(toolbar);
        }

        private void CreateQuest()
        {
            // create new scriptableObject 

            Quest newQuest = ScriptableObject.CreateInstance<Quest>();

            NodeQuestGraph entryNode = _questGraph.GetEntryPointNode();
            newQuest.misionName = entryNode.misionName;
            newQuest.isMain = entryNode.isMainQuest;

            questForGraph = newQuest;

            var saveUtility = QuestGraphSaveUtility.GetInstance(_questGraph);

            saveUtility.CheckFolders(questForGraph);
                AssetDatabase.CreateAsset(newQuest, $"{QuestConstants.MISIONS_FOLDER}/{newQuest.misionName}/{newQuest.misionName}.asset");
            
        }

        private void LoadQuestData()
        {
            if (questForGraph == null)
            {
                EditorUtility.DisplayDialog("Error!!", "No quest to load!", "OK");
                return;
            }

            var saveUtility = QuestGraphSaveUtility.GetInstance(_questGraph);
            saveUtility.LoadGraph(questForGraph);
        }

        private void SaveQuestData()
        {
            if (questForGraph == null)
            {
                CreateQuest();
            }


            var saveUtility = QuestGraphSaveUtility.GetInstance(_questGraph);
            Debug.Log(questForGraph.misionName);
            saveUtility.SaveGraph(questForGraph);
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_questGraph);
        }
    }
}