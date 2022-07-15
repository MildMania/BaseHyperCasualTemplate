using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;


public class SceneSelector : EditorWindow
{
    public class FileModificationProcessor : AssetModificationProcessor
    {
        static string[] OnWillSaveAssets(string[] paths)
        {
            _isUpdated = true;
            return paths;
        }

        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions rao)
        {
            _isUpdated = true;
            return AssetDeleteResult.DidNotDelete;
        }

        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            _isUpdated = true;
            return AssetMoveResult.DidNotMove;
        }
    }


    private static string _pathToScenesPrefix = "Assets/Scenes";
    private static string _managerSceneName = "ManagerScene";
    private static bool _isUpdated;

    [MenuItem("Tools/Scene Selector %#o")]
    static void OpenWindow()
    {
        GetWindow<SceneSelector>();
    }

    private void OnGUI()
    {
        if (!_isUpdated)
        {
            return;
        }

        _isUpdated = false;
        CreateGUI();
    }

    private void CreateGUI()
    {
        rootVisualElement.Clear();
        String[] assetGuids = AssetDatabase.FindAssets("t:Scene");
        assetGuids.ForEach(assetGuid =>
        {
            String scenePath = AssetDatabase.GUIDToAssetPath(assetGuid);
            rootVisualElement.Add(CreateSceneButton(scenePath));
        });
    }

    private static VisualElement CreateSceneButton(string scenePath)
    {
        if (!scenePath.StartsWith(_pathToScenesPrefix))
        {
            return null;
        }

        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        CreateButtonGroup(sceneAsset, out VisualElement buttonGroup);
        CreateButton(scenePath, buttonGroup);
        return buttonGroup;
    }


    private static void CreateButton(String scenePath, VisualElement buttonGroup)
    {
        Button openButton = new Button(() => { EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single); })
        {
            text = "Open"
        };
        buttonGroup.Add(openButton);

        var playButton = new Button(() =>
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            EditorApplication.EnterPlaymode();
        })
        {
            text = "Play"
        };
        buttonGroup.Add(playButton);

        var stopButton = new Button(() =>
        {
            if (EditorSceneManager.loadedSceneCount == 1 &&
                EditorSceneManager.GetActiveScene().path.Equals(scenePath) ||
                EditorSceneManager.loadedSceneCount == 2 && scenePath.Contains(_managerSceneName))
            {
                EditorApplication.ExitPlaymode();
            }
        })
        {
            text = "Stop"
        };
        buttonGroup.Add(stopButton);
    }

    private static void CreateButtonGroup(SceneAsset sceneAsset, out VisualElement buttonGroup)
    {
        var label = new Label($"{sceneAsset.name}")
        {
            style =
            {
                width = 125
            }
        };

        buttonGroup = new VisualElement
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                marginLeft = 3
            }
        };
        buttonGroup.Add(label);
    }
}