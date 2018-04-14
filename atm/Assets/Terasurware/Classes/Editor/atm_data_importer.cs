using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class atm_data_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Data/atm_data.xls";
	private static readonly string exportPath = "Assets/Resources/Data/atm_data.asset";
	private static readonly string[] sheetNames = { "StatusData", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_StatusData data = (Entity_StatusData)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_StatusData));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_StatusData> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Entity_StatusData.Sheet s = new Entity_StatusData.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_StatusData.Param p = new Entity_StatusData.Param ();
						
					cell = row.GetCell(1); p.level = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.nextSumExp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.power = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.maxPerson = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
