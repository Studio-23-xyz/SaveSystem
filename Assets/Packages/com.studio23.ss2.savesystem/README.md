# Save System for Unity

Save System is a framework to manage local and cloud saves. It provides simple interface to save files and configure cloud saves for Steam or Xbox and other platforms via extensions.

## Table of Contents

1. [Installation](#installation)
2. [Usage](#usage)
   - [Using LocalSaveManager Script](#Using LocalSaveManager script)
   - {About Sticher Script] (#About Sticher Script)
3. [Example](#example)
4. [License](#license)

## Installation

### Install via Git URL
You can also use the "Install from Git URL" option from Unity Package Manager to install the package.
```
https://github.com/Studio-23-xyz/SaveSystem.git#upm
```
## Usage

### Using LocalSaveManager script

1. In the LocalSaveManager class the constructor takes the path of the file.

2. enableEncryption = true will use encryption to the save file. It will also decrypt when the file will be loaded.

3. Save and Load functions are generic type parameters in case any content can be stored and load locally.

```csharp
// Example code to use the Save and Load function
LocalSaveManager Obj = new LocalSaveManager("savedata01",true,"encryptsampleKeyA","encryptIVkeyA")
Obj.Save(savedata);
var loaddata = Obj.Load()


### About Sticher Script

When trying to save if the directories do no exist it will attempt to create it then save it. If there will be numerous saveable files, it will combine into on directory and can also be extracted whenever 
it needs to be extracted.


