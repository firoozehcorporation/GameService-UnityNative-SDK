# Changelog

All notable changes to this project will be documented in this file.





## [2.3.0] - 2020-05-02

### Added

- Added Ability To GetMemberData
- Added Ability To Send Private Message 
- Added Ability To Get Members Chat
- Added Ability To Get Pending Messages
- Added Ability To Get Recent Messages

### Changed

- Changed Return Value in GetCurrentPlayer Function
- Changed Score User Property to Submitter Member

### Fixed

- Fixed GSLive System





## [2.2.3] - 2020-04-26

### Fixed

- Fixed GSLive System





## [2.2.2] - 2020-04-23

### Added

- Added Ability to Get Channels Subscribed





## [2.2.1] - 2020-04-20

### Added

- Added Channel Name to Chat Class Model





## [2.2.0] - 2020-04-14

### Added

- Added Ability To Get User Data With UserID
- Added Local & Global Mode To Member Data 
- Added Ability to check if Command is available or not
- Added GSLiveOption XML Doc

### Changed

- Added Name & Logo To Member Class (for Local Mode)
- Changed FindUser Function to FindMember Function in GSLive
- Changed Inviter User to Inviter Member in Invite Class in GSLive

### Fixed

- Fixed Timeout When GSLive not Connected
- Fixed Http Request



## [2.1.1] - 2020-04-4

### Added

- Added Ignore Default Value To FaaS



### Fixed

- Fixed SDK





## [2.1.0] - 2020-03-28

### Added

- Added FaaS Feature (Earlier Version)
- Added Ignore Default Value For Update Bucket
- Added Check BucketCore Inheritance For Bucket Functions



### Fixed

- Fixed SDK





## [2.0.4] - 2020-03-21

### Added

-  Added AssetInfo To Check AssetDataInfo Before Download it
- Added DownloadAssets With AssetInfo 

### Changed

- Removed Unnecessary Classes

### Fixed

- Fixed Some XML Doc
- Fixed SDK





## [2.0.3] - 2020-03-10

### Added

- Added Ability To Cancel Current AutoMatch (In CancelAutoMatch Function)
- Added EventHandlers XML Documentation





## [2.0.2] - 2020-03-8

### Added

- Added Ability To Check Current Time (In GetCurrentTime Function)

### Fixed

- Fixed  Reconnect To GameService





## [2.0.1] - 2020-03-7

### Added

- Added Ability To Detect User Is Guest Or Not (In GetCurrentPlayer Function)
- Added Restrictions In Guest Mode

### Changed

- Removed Some SaveDetails Class Properties
- Removed IsMe Property In Score Class 

### Fixed

- Fixed LoginAsGuest





## [2.0.0] - 2020-03-6

### Added

- Added GameService Errors Class
- Added Google Login
- Added Ability To Stop Download Assets
- Added Leaderboard Score Details Limit
- Added BucketCore Class for Inheritance in Buckets

### Changed

- Change Bucket Functions Return Value
- Migrate To New Http API System
- Refactor Command Classes To GsLive.Command
- Remove SaveGame Description

### Fixed

- GsLive System
- DeviceID Error