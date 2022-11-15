using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;


public class XMLManager
{
    public Leaderboard leaderboard;
    private static XMLManager _instance;
    public  static XMLManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new XMLManager();
            }
            return _instance;
        }
    }
    public XMLManager() {

        leaderboard = new Leaderboard();
        if (!Directory.Exists(Application.dataPath + "/SavedData/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/SavedData/");
        }
    }

    

    public void saveScores(List<HighScore> scoresToSave)
    {
        leaderboard.list = scoresToSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        FileStream stream = new FileStream(Application.dataPath + "/SavedData/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        stream.Close();
    }
    public List<HighScore> loadScores()
    {

        if (File.Exists(Application.dataPath + "/SavedData/highscores.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            FileStream stream = new FileStream(Application.dataPath + "/SavedData/highscores.xml", FileMode.Open);
            leaderboard = serializer.Deserialize(stream) as Leaderboard;
        }
        else
        {
            return new List<HighScore>();
        }
        return leaderboard.list;
    }
    
}
[System.Serializable]
public class Leaderboard
{
    public List<HighScore> list = new List<HighScore>();
}