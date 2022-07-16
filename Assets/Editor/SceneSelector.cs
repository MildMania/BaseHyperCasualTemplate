using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;


public class SceneSelector : EditorWindow
{
    public class FileModificationProcessor : AssetModificationProcessor
    {
        private static bool IsStartWithScenePrefix(string path)
        {
            return path.StartsWith(_pathToScenesPrefix);
        }

        static string[] OnWillSaveAssets(string[] paths)
        {
            foreach (var path in paths)
            {
                _isUpdated = IsStartWithScenePrefix(path);
                if (_isUpdated)
                {
                    break;
                }
            }

            return paths;
        }

        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions rao)
        {
            _isUpdated = IsStartWithScenePrefix(path);
            return AssetDeleteResult.DidNotDelete;
        }

        private static AssetMoveResult OnWillMoveAsset(string path, string destinationPath)
        {
            _isUpdated = IsStartWithScenePrefix(destinationPath) || IsStartWithScenePrefix(path);
            return AssetMoveResult.DidNotMove;
        }
    }


    private static string _pathToScenesPrefix = "Assets/Scenes";
    private static string _managerSceneName = "ManagerScene";
    private static string _openButtonName = "Open";
    private static string _playButtonName = "Play";
    private static string _stopButtonName = "Stop";
    private static string _filter = "t:Scene";
    private static bool _isUpdated;
    private static string[] _folderPaths = {_pathToScenesPrefix};

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
        String[] assetGuids = AssetDatabase.FindAssets(_filter, _folderPaths);
        assetGuids.ForEach(assetGuid =>
        {
            String scenePath = AssetDatabase.GUIDToAssetPath(assetGuid);
            rootVisualElement.Add(CreateSceneButton(scenePath));
        });
    }

    private static VisualElement CreateSceneButton(string scenePath)
    {
        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        CreateButtonGroup(sceneAsset, out VisualElement buttonGroup);
        CreateButton(scenePath, buttonGroup);
        return buttonGroup;
    }


    private static void CreateButton(String scenePath, VisualElement buttonGroup)
    {
        Button openButton = new Button(() =>
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        })
        {
            text = _openButtonName
        };
        buttonGroup.Add(openButton);

        var playButton = new Button(() =>
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            EditorApplication.EnterPlaymode();
        })
        {
            text = _playButtonName
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
            text = _stopButtonName
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