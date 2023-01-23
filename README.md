# Unity - Import/Export CSV and ScriptableObject
---
This editor script provide an way to generate multiple ScriptableObject from a CSV file, and to export them back into a CSV if you've made changed in the editor.

This script allows the fast iteration of ScriptableObject, while not requiring for designers to manage and install Unity. This can really benefit heavy data games like RPGs, deckbuilders or Roguelike.

- The two editor script you're gonna need to edit are the [CSVItemImport](Assets/Editor/CSVItemImport.cs) and the [CSVItemExport](Assets/Editor/CSVItemExport.cs). There, you'll be able to list the SciptableObject you want to import/export.
- The configuration for the CSV and ScriptableObject location is in the [CSV](Assets/SCRIPT/UTILS/CSV.cs) class
- The static class [CSV](Assets/SCRIPT/UTILS/CSV.cs) contain the parser to go back and forth between a CSV file and a list of dictionnaries that will be used by the ScriptableObjects
- Every ScriptableObject must use the [IDataObject](<Assets\SCRIPT\DATA\#1-SO_CLASS\Interface\IDataObject.cs>) interface
	- The init() function act as a constructor, and must validate the data and raise the flag *isValid* if everything initialized correctly
	- A unique field *id* must be present so that we can easely reference objects between sheet **(that's the only required field in your CSV)**
	- GetData() is used for exporting: it return the data used for the initialization, but by changing the field that could have been edited in the inspector
- To help you, I created [a minimal template for your ScriptableObject](Assets\SCRIPT\DATA\01-SO_CLASS\MinimalSO.cs). You can also refer to the two example class, [ItemSO](Assets\SCRIPT\DATA\01-SO_CLASS\ItemSO.cs) and [EnemySO](Assets\SCRIPT\DATA\01-SO_CLASS\EnemySO.cs).

## Optional utilities
- In your SO constructor, you can use [SOFileManagement.GetSOWithId<ItemSO>(int id, string SO_folder)](Assets\Editor\UtilsSO\SOFileManagement.cs) to reference another SO.
- You can use [SOFileManagement.LoadAssetFromFile<T>(string path, string prefab_name)](Assets\Editor\UtilsSO\SOFileManagement.cs) to load an asset using its name.