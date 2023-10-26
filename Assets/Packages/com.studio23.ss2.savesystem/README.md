
<h1 align="center">SaveSystem</h1>

<p align="center">
<a href="https://openupm.com/packages/com.studio23.ss2.savesystem/"><img src="https://img.shields.io/npm/v/com.studio23.ss2.savesystem?label=openupm&amp;registry_uri=https://package.openupm.com" /></a>
</p>

# Save System for Unity

Save System is a framework to manage local and cloud saves. It provides simple interface to save files and configure cloud saves for Steam or Xbox and other platforms via extensions.

## Table of Contents

1. [Installation](#installation)
2. [Getting Started](#getting-started)
   - [Initialization](#initialization)
   - [Creating Save Slots](#creating-save-slots)
3. [Saving and Loading Data](#saving-and-loading-data)
   - [Save Data](#saving-data)
   - [Load Data](#loading-data)
4. [Managing Save Slots](#managing-save-slots)
   - [Selecting a Slot](#selecting-a-slot)
   - [Clearing Slots](#clearing-slots)
5. [Cloud Save and Restore](#cloud-save-and-restore)
   - [Bundle Save Files](#bundle-save-files)
   - [Unbundle Save Files](#unbundle-save-files)

## Installation


### Install via Git URL
You can also use the "Install from Git URL" option from Unity Package Manager to install the package.
```
https://github.com/Studio-23-xyz/SaveSystem.git#upm
```

## Getting Started

### Initialization

Create An Empty GameObject and attach the SaveSystem MonoBehaviour to it. And you are all set to use the Save System.
Or You can use the Installer From Top tool bar Studio-23/Save System/ Installer

### Creating Save Slots

Save slots are used to organize saved game data. You can create and manage save slots as follows:

```csharp
using Studio23.SS2.SaveSystem.Core;
using Cysharp.Threading.Tasks;

public class YourGameManager : MonoBehaviour
{
    private async void Start()
    {
        // Create save slots
        await SaveSystem.Instance.CreateSlotsAsync();
    }
}
```

## Saving and Loading Data

### Saving Data

To save data using the SaveSystem library, you can use the `SaveData` method. Here's how to use it:

```Csharp
// Example code for saving data
YourDataType yourData = // Your data here;
string saveFileName = "example_data"; // Choose a unique name for your data
string savefilePath = "Your/Save/Path"; // Choose a save directory path
string extention = ".dat"; // Choose a file extension for your data

await SaveSystem.Instance.SaveData(yourData, saveFileName, savefilePath, extention);
```

- `yourData`: Replace this with the data you want to save.
- `saveFileName`: Choose a unique name for your saved data.
- `savefilePath`: Specify the directory path where you want to save the data.
- `extention`: Choose a file extension for your data.

### Loading Data

To load data using the SaveSystem library, you can use the `LoadData` method. Here's how to use it:

```Csharp
// Example code for loading data
string saveFileName = "example_data"; // The same name used when saving
string savefilePath = "Your/Save/Path"; // The directory where the data was saved
string extention = ".dat"; // The file extension used for the saved data

YourDataType loadedData = await SaveSystem.Instance.LoadData<YourDataType>(saveFileName, savefilePath, extention);
```

- `saveFileName`: Use the same name that was used when saving the data.
- `savefilePath`: Specify the directory path where the data was saved.
- `extention`: Use the same file extension that was used for saving the data.
- `YourDataType`: Replace with the actual data type you're expecting to load.

These methods allow you to save and load custom data types using the SaveSystem library.

## Managing Save Slots

### Selecting a Slot

You can switch between save slots by using the `SelectSlot` method:

```csharp
using Studio23.SS2.SaveSystem.Core;

public class YourGameManager : MonoBehaviour
{
    private void SwitchSaveSlot(int slotIndex)
    {
        SaveSystem.Instance.SelectSlot(slotIndex);
    }
}
```

### Clearing Slots

To clear all save slots, you can use the `ClearSlotsAsync` method:

```csharp
using Studio23.SS2.SaveSystem.Core;
using Cysharp.Threading.Tasks;

public class YourGameManager : MonoBehaviour
{
    private async void ClearAllSlots()
    {
        // Clear all save slots
        await SaveSystem.Instance.ClearSlotsAsync();
    }
}
```

## Cloud Save and Restore

### Bundle Save Files

You can bundle save files for cloud storage with the `BundleSaveFiles` method:

```csharp
using Studio23.SS2.SaveSystem.Core;
using Cysharp.Threading.Tasks;

public class YourGameManager : MonoBehaviour
{
    private async void BundleSaveFilesForCloud()
    {
        // Bundle save files for cloud storage
        await SaveSystem.Instance.BundleSaveFiles();
    }
}
```

### Unbundle Save Files

To restore save files from cloud storage, use the `UnBundleSaveFiles` method:

```csharp
using Studio23.SS2.SaveSystem.Core;
using Cysharp.Threading.Tasks;

public class YourGameManager : MonoBehaviour
{
    private async void RestoreSaveFilesFromCloud()
    {
        // Restore save files from cloud storage
        await SaveSystem.Instance.UnBundleSaveFiles();
    }
}
```

That's it! You now have the basic information you need to use the **SaveSystem** library in your Unity project. Explore the library's features and customize it according to your game's needs.