
# Save System for Unity

Save System is a framework to manage local and cloud saves. It provides simple interface to save files and configure cloud saves for Steam or Xbox and other platforms via extensions.

## Table of Contents

1. [Installation](#installation)
2. [Getting Started](#getting-started)
   - [Initialization](#initialization)
   - [Creating Save Slots](#creating-save-slots)
3. [Saving and Loading Data](#saving-and-loading-data)
   - [Save Data](#save-data)
   - [Load Data](#load-data)
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

### Save Data

You can save game data using the `SaveData` method. Here's how to use it:

```csharp
using Studio23.SS2.SaveSystem.Core;
using Cysharp.Threading.Tasks;

public class YourGameManager : MonoBehaviour
{
    private async void SaveGameData()
    {
        // Your game data
        YourGameData data = ...

        // Save the data to the selected slot
        await SaveSystem.Instance.SaveData(data, "your_data_id");
    }
}
```

### Load Data

To load saved game data, use the `LoadData` method as shown below:

```csharp
using Studio23.SS2.SaveSystem.Core;
using Cysharp.Threading.Tasks;

public class YourGameManager : MonoBehaviour
{
    private async void LoadGameData()
    {
        // Load the data from the selected slot
        YourGameData loadedData = await SaveSystem.Instance.LoadData<YourGameData>("your_data_id");

        // Use the loaded data in your game
        ...
    }
}
```

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

That's it! You now have the basic information you need to use the **Studio23.SS2.SaveSystem** library in your Unity project. Explore the library's features and customize it according to your game's needs.