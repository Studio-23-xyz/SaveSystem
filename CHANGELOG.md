# Changelog

All notable changes to this Unity package will be documented in this file.

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


