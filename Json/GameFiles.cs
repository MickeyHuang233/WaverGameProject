using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameFiles
{
    //游戲存檔信息
    public List<GameFile> gameFiles;

    public GameFiles()
    {
        gameFiles = new List<GameFile>();
        for (int i = 0; i <= 5; i++)
        {
            GameFile gameFile = new GameFile();
            gameFile.gameFileId = i;
            gameFiles.Add(gameFile);
        }
    }
}
