using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{

    TextAsset csvFile; // CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    public Dictionary<string, int[]> pointDict = new Dictionary<string, int[]>();

    void Start()
    {
        csvFile = Resources.Load("Datas/pointShiftTable") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        string _key;
        int[] _value;
        while (reader.Peek() != -1) // reader.Peekが-1になるまで
        {
            _key = "";
            _value = new int[2];
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
            string[] eles = line.Split(',');
            _key = eles[0] + "_" + eles[1] + "_" + eles[2] + "_" + eles[3];
            _value[0] = int.Parse(eles[4]);
            _value[1] = int.Parse(eles[5]);
            pointDict.Add(_key, _value);

        }
        //foreach(var key in pointDict.Keys) 
        //{
            //rint(key);
        //}
        
    }
}

