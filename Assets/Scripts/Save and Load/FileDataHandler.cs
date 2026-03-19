using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
    }

    public void Save(GameData _data)//就是先组合路径，然后确保路径存在，不存在就创建，然后把数据转化再写入文件里面
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            using(FileStream stream =new FileStream(fullPath,FileMode.Create))
            {
                using (StreamWriter writer =new StreamWriter (stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }

        catch(Exception e)
        {
            Debug.LogError("保存文件错误"+fullPath+"\n"+e);
        }
    }

    public GameData Load()
    {
        string fullPath=Path.Combine(dataDirPath, dataFileName);
        GameData loadData=null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream=new FileStream(fullPath,FileMode.Open))
                {
                    using (StreamReader reader =new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            
            }
            catch(Exception e)//防止报错
            {
                Debug.LogError("未能从文件加载存档"+fullPath+"\n"+e);
            }
        }
        return loadData;
    }




}
