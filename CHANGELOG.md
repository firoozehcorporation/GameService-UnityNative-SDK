# Changelog

All notable changes to this project will be documented in this file.



## [8.8.0] Stable - 2021-09-15

### Changed

- **Increased Min Observer Threshold** From 8 To 12 Per Second in RealTime System



### Fixed

- Fixed Reconnect in WebSocket Connections
- Fixed Reconnect in RealTime System
- Updated GProtocol To Version 3.2.0





## [8.7.0] Stable - 2021-09-10

### Added

- Added Ability To **Get Other Player Connection State in RealTime System**



### Fixed

- Fixed Check Availability in TurnBased and RealTime Systems
- Updated GProtocol To Version 3.1.0





## [8.6.0] Stable - 2021-08-29

### Added

- Added Ability To **Edit Current Room in TurnBased System**
- Added Ability To **Edit Current Room in RealTime System**



### Changed

- Removed Min Player in **Create Room Option ** in TurnBased System
- Removed Min Player in **Create Room Option ** in RealTime System



### Fixed

- Fixed PhoneNumber Serialize In MemberInfo class
- Fixed Label and GlobalProperty Constraints Checks



## [8.5.0] Stable - 2021-08-26

### Added

- Added Ability To **Get Rooms Info in TurnBased System**
- Added Ability To **Get Rooms Info in RealTime System**
- Added **IsPersist** Property in Room Data Model



### Fixed

- Fixed Codes





## [8.4.0] Stable - 2021-08-18

### Added

- Added Ability To **Send Public Message in TurnBased System**
- Added Ability To **Send Private Message in TurnBased System**
- Added Ability To **Send Global Property in Chat System**



### Fixed

- Fixed Codes





## [8.3.1] Stable - 2021-08-01

### Added

- Added **Creator Member** To PushEvent Data Model



### Fixed

- Fixed Codes





## [8.3.0] Stable - 2021-07-30

### Added

- Added Ability To **Push an Event By Member Id**
- Added Ability To **Push a Scheduled Event By Member Id**
- Added Ability To **Push an Event By Member Tag**
- Added Ability To **Push a Scheduled Event By Member Tag**
- Added Ability To **Get Buffered Push Events**



### Fixed

- Fixed Accessibility In GameService Main Data Models
- Fixed Accessibility In Configuration Classes





## [8.2.0] Stable - 2021-07-24

### Added

- Added Ability To **Edit a Message in Public Channel **
- Added Ability To **Remove a Message in Public Channel**
- Added Ability To **Remove Member Chats in Public Channel**
- Added Ability To **Remove All Chats in a Public Channel**
- Added Ability To **Remove All Public Channels Messages**
- Added Ability To **Edit Chat a Private Message**
- Added Ability To **Remove a Private Message**
- Added Ability To **Remove All Private Messages That's Sent To a Member**
- Added Ability To **Remove All Private Messages**
- Added Ability To **Get All Private Recent Messages With MemberContact**
- Added **Chat id** And **Chat Property** To Chat Data Model
- Added Connectivity Checker in Turn-Based System
- Added WebSocket ConnectionType Support
- Added Throw Exception When Send Function Limited in GSLive Systems



### Changed

- Removed **Async/Await Functions** In Command and Turn-Based and Real-Time Systems To Improve Performance
- Removed **Pending Messages** and Replaced With **Private Recent Messages**
- Renamed All **Chats Event Handlers** With Correct Format (Added Received at the End of Names)



### Fixed

- Fixed Reconnecting Issues
- Fixed Multiple Auth Issue
- Fixed Reconnect Event Handler Context Issue
- Fixed GsTcpClient To Work better on Lossy Network





## [8.1.0] Stable - 2021-06-26

### Added

- Added Adaptive Serialization To Work Better On Lossy Networks
- Added Check CurrentPlayerObserving To Avoid Update In Observer Player



### Fixed

- Fixed LoginOrSignUp in Multiple Times
- Fixed AutoMatch in Multiple Times
- Fixed Dispose Issue





## [8.0.0] Alpha - 2021-06-22

### Added

- Added **Member Tags**
- Added **Member Label**
- Added **Member Global Property**
- Added **RTT (RoundTripTime)** To RealTime
- Added **PacketLost** To RealTime
- Added State Check in RealTime and TurnBased
- Added Encryption System



### Changed

- Renamed **Complete to AcceptVote** in TurnBased System
- Renamed **MemberFinish To Submitter** in Vote Model in TurnBased System
- Upgrade **Max Players** (10 Players For TurnBased and 50 Players For RealTime Systems)
- Removed Ping From RealTime
- Removed RoundTripTime(RTT) From MessageInfo Class in RealTime System
- Update GProtocol to Ver 3



### Fixed

- Fixed Multiple Connection Issues
- Fixed Disposing on StopReceiving in GsTcpClient Class
- Fixed NaughtyAttributes Lib Working With IL2CPP





## [7.0.0] Alpha - 2021-03-18

### Added

- Added **CloudComputing**
- Added Description and LeaderboardOrderTypes To LeaderBoard
- Added Description To Achievement
- Added GetVariables and Delete Them By Creator and Admins to Parties
- Added SaveName To Save System



### Changed

- Migrate **Functions To Providers**
- Renamed LeftParty To LeaveParty
- Renamed ChannelsRecentMessages to ChannelRecentMessages in Chat System
- Renamed SetProperty to SetOrUpdateProperty in TurnBased System
- Renamed Creator to CreatorId in RoomData Class
- Removed Deprecated Functions in RealTime
- Achievements and LeaderBoards **Only Works With ID**
- Changed Ping System



### Fixed

- Update GProtocol
- Fixed Timeout Misbehaving
- Fixed Reported Issues



## [6.1.1] Stable - 2021-02-13

### Added

- Added Server Leave Current Player When Room Members Reached To Min Players

- Added Party Delete Function

  

### Fixed

- Fixed Reported Issues





## [6.1.0] Alpha - 2021-01-22

### Added

- Added New Bucket Aggregation System



### Fixed

- Fixed Reported Issues





## [6.0.1] Alpha - 2020-12-24

### Added

- Added Current Device to ActiveDevice model
- Added ValidationUtil to BucketOptions



### Changed

- Renamed Room Variables to Properties



### Fixed

- Fixed Reported Issues





## [6.0.0] Alpha - 2020-12-19

### Added

- Added Friend System
- Added Party System
- Added Debugger System
- Added RoomPassword in Create Room
- Added RoomInfo To TurnBased and RealTime
- Added Devices And Ability To Revoke Them
- Added Change Password Function
- Added Get Current Game Function
- Added Get Buckets in Global Mode
- Added Friend Option To LeaderBoard
- Added Reconnect Event To TurnBased and RealTime
- Added Errors to GameServiceErrors Class



### Changed

- Renamed Finish Function To Vote in TurnBased System
- Removed Default Value Ignore in Update Bucket
- Increase HttpRequest Limit To 20 Request Per 3 Seconds



### Fixed

- Fixed Reconnect Command and TurnBased and RealTime
- Fixed Plugin Warnings 
- Fixed GProtocol
- Fixed Reported Issues






## [5.4.3] Stable - 2020-11-03

### Fixed

- Fixed OnDestroy In GameServiceInitializer Script
- Fixed Dispose Issues In Realtime And TurnedBase System
- Fixed Some Issues


## [5.4.2] Stable - 2020-11-01

### Changed

- Changed HttpRequest Limiter to 15 Requests per 3 Secs
- Changed OnUserJoined To OnUserUpdated Stat in AutoMatch



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