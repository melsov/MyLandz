using UnityEngine;

public enum MLGameSavedStateType
{
    DONT_SAVE,
    SAVE_INT,
    SAVE_FLOAT,
    SAVE_BOOL
}

[System.Serializable]
public struct MLGameStateSaver
{
    public MLGameSavedStateType type;
    
    public void writeToPrefs(MLGameState mlgs) {
        switch(type) {
            case MLGameSavedStateType.DONT_SAVE:
                break;
            case MLGameSavedStateType.SAVE_BOOL:
            case MLGameSavedStateType.SAVE_INT:
                PlayerPrefs.SetInt(mlgs.key, mlgs.param);
                break;
            case MLGameSavedStateType.SAVE_FLOAT:
            default:
                PlayerPrefs.SetFloat(mlgs.key, mlgs.param);
                break;
        }
    }

    public void reinstateFromPrefs(MLGameState mlGameState) {
        if(type == MLGameSavedStateType.DONT_SAVE) {
            return;
        }
        mlGameState.enforce(fromPrefs(mlGameState));
    }

    public MLNumericParam fromPrefs(MLGameState mlGameState) {
        MLNumericParam param = default(MLNumericParam);
        switch(type) {
            case MLGameSavedStateType.DONT_SAVE:
                break;
            case MLGameSavedStateType.SAVE_BOOL:
            case MLGameSavedStateType.SAVE_INT:
                param = new MLNumericParam(PlayerPrefs.GetInt(mlGameState.key));
                break;
            case MLGameSavedStateType.SAVE_FLOAT:
            default:
                param = new MLNumericParam(PlayerPrefs.GetFloat(mlGameState.key));
                break;
        }
        return param;
    }
}