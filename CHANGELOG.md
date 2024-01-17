# Changelog

## [v2.0.0] - 2024-01-16

### Updated

+ Updated GitHub Actions workflow .github/workflows/UPMBranchUpdate.yml.
+ Modified SaveSystemInstallerWindow.cs script.
+ Updated SaveSystem.cs and SaveSlot.cs.
+ Updated ISaveable.cs interface.
+ Moved FakeData.cs from Tests/PlayMode/Data to Samples/Demo/Scripts and updated metadata.
+ Deleted test scripts BasicSaveLoadTests.cs, EncryptionTest.cs, and SlotMetaDataTest.cs.

### Added

+ Added SlotConfiguration.cs.
+ Added FileProcessor.cs , replacing the deleted SaveProcessor.cs.
+ Added SaveSlotProcessor.cs.
+ Added new samples for demonstration and their configurations.
+ Added Archiver directory and ArchiverBase.cs, ZipUtilityArchiver.cs.
+ Added Encryptor directory and EncryptorBase.cs, AESEncryptor.cs , and deleted Stitcher.cs.
+ Added ItemDataBehaviour.cs and PlayerDataBehaviour.cs scripts for demo.



## [v1.2.0] - 2023-12-14

### Added

+  Added Cloud Save Provider
+  Cloud Save Provider Has methods to upload and download files and events to be subscribed

### Updated
+  New API added: Delete Selected Slot
+  New Test added for the new API

## [v1.1.8] - 2023-12-13

### Updated

+  Made two internal paths public for extention libraries


## [v1.1.7] - 2023-12-7

### Updated

+  Now Bundling and Unbundling is based on SharpZipLib from unity

## [v1.1.6] - 2023-10-26

### Fixed

+  Fixed a bug regarding enabling encryption

## [v1.1.5] - 2023-10-26

### Updated

+  Save Method will now check if directory exists otherwise create it.


## [v1.1.3] - 2023-10-26

### Updated

+  Updated How Save and Load Methods work.


## [v1.1.2] - 2023-10-22

### Added

+  Added a new public property called SaveBundle Path. Which can be used by extentions to manage cloud save.

## [v1.1.2] - 2023-10-22

### Added

+  New Method added to get save metadata to show UI.
+  New Test Cases Added


## [v1.1.1] - 2023-10-22

### Added

+  Added SaveEvent delegate and corresponding events for tracking save and load operations.
+  Added methods to handle all components implementing the ISaveable interface, bundle save files into a single file, and extract bundled save files.

### Updated

+  Modified SaveSystem class to include new events and methods.
+  Updated SaveData and LoadData methods to invoke events and return loaded data respectively.
+  Updated bundleVersion field in the PlayerSettings structure from 1.1.0 to 1.1.1.



## [v1.1.0] - 2023-10-21

### Added

- New SaveSystem Class added which will be the new entrypoint of using this  library
- New Test Cases to ensure the library works as intented.

### Updated

- Updated how Bundling and unbundeling works.

### Fixed

- Now Bundling and unbundeling works properly

### Removed

- Bugs


