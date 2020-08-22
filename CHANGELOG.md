# Changelog

All notable changes to this project will be documented in this file.





## [5.4.1] Stable - 2020-08-22

### Added

- Added Email To Edit User
- Added Email and Phone Number To User Data Model





## [5.4.0] Stable - 2020-08-20

### Added

- Added Phone Number To Edit User
- Added Extra Data To Send With AutoMatch & Create Room & Invites
- Added GetLastLoginMemberInfo Function



### Fixed

- Fixed CanLoginWithPhoneNumber Return Value



## [5.3.0] Alpha - 2020-08-11

### Added

- Added SendLoginCodeWithSms Function
- Added LoginWithPhoneNumber Function
- Added CanLoginWithPhoneNumber Function
- Added Download With Certificates (Secure Download)



### Fixed

- Fixed RealTime & GProtocol System 



## [5.2.0] Stable - 2020-08-04

### Added

- Added GetCurrentPlayerScore Function

- Added Some Array Serializers

  

### Changed

- Changed HttpRequestObserver to 9 Requests per 3 Secs



### Fixed

- Fixed RealTime System To Improved Network Transmit







## [5.1.1] Alpha - 2020-07-30

### Added

- Added Ability To Parallel Download in Download Manager
- Added Some Download EventHandlers
- Added Cancel Download Functions


### Fixed

- Fixed ReadTime System



## [5.1.0] Alpha - 2020-07-29

### Added

- Added Ability To Set Property
- Added Some Error Handling



### Changed

- Changed Unity Serializers Classes  To Improve Performance and Network Transmit



### Fixed

- Fixed RigidBody2D Observer



## [5.0.0] Alpha - 2020-07-25

### Added

- Added New RealTime Utility System (Version Alpha 1.0.0) 

- Added Http Request Observer

- Added GetPing() To RealTime Functions

  

### Changed

- Migrate To Binary System in RealTime Core



### Fixed

- Fixed GsLive on Critical Sections
- Fixed Code



## [4.1.2] - 2020-07-03

### Changed

- Migrate DataPack To New Version





## [4.1.1] - 2020-06-05

### Fixed

- Fixed MaxPlayer in GSLiveOption



## [4.1.0] - 2020-06-03

### Added

- Added Message Info To RealTime Message Receive Event
- Added Round-Trip Time (RTT) to RealTime Message Info

### Fixed

- Fixed GetCurrent Time Function

- Fixed GSLive System

  



## [4.0.1] - 2020-05-31

### Fixed

- Fixed GSLive System 





## [4.0.0 alpha] - 2020-05-29

### Added

- Added Cryptography To GProtocol
- Added Congestion Control System To GProtocol

### Changed

- Updated GProtocol To Version 2.0.0

### Fixed

- Fixed GSLive System
- Fixed Sending Continuous Packets issue





## [3.0.1] - 2020-05-28

### Fixed

- Fixed Download Manager





## [3.0.0] - 2020-05-05

### Added

- Added Ability To Build on IL2CPP Scripting Backend





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