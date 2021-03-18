# Walden VR Unity App

## Contents

- [Compatibility](#compatibility)
- [Setup](#setup)
- [Building](#building)
- [Releasing](#releasing)
- [Analytics](#analytics)
- [Testing](#testing)
- [Developer Tips](#developer-tips)

## Compatibility

- Android: Version 7.0+ (Nougat). API level 24.
- iOS: iOS 11+.
- Unity: Version 2019.2.12f
- Xcode: 11.2.1

## Setup

### Install Unity

1. Install Unity Hub
    * URL: https://unity3d.com/get-unity/download
2. Open Unity Hub and install the correct Unity version. Currently 2019.2.12f for this project.
    * Ensure that the following modules are checked:
        * iOS Build Support
        * Android Build Support
        * Android SDK & NDK Tools
        * OpenJDK

### Install Xcode

- Version 11.2.1
- URL: https://developer.apple.com/download
- More Information: [Unity iOS documentation](https://docs.unity3d.com/2018.2/Documentation/Manual/iphone-GettingStarted.html)

### Download the Source

Clone the repository:

        $ git clone https://github.com/livefront/gokart-walden-vr.git

## Building

**Note:** Switching between platforms can take a few minutes while Unity regenerates some files.

### Unity Hub

1. Open Unity Hub
2. Project > Add
3. Browse to the project's root folder > Open

### Android

#### Configuration

1. In Unity, select Edit > Project Settings
2. Select Player
3. Select the Android Tab (Droid Icon)
4. Expand the Other Settings section
5. Enter the following values:

        **Identification**
        Package Name: e.g. com.abc.net
        Version: [version number, e.g. 1.0]
        Bundle Version Code: [version code, e.g. 2019121002]

6. Expand the Publish Settings section
7. Ensure the keystore path is set to Suppport/upload.keystore

#### Building

1. In Unity, select File > Build Settings
2. Select "Android"
3. Select "Switch Platform" to switch to the Android platform
4. Select "Build"
5. Select a destination and filename for the APK and select "Save"

More Information: [Unity build process for Android](https://docs.unity3d.com/2018.2/Documentation/Manual/android-BuildProcess.html)

#### Running

To run the build, transfer the APK to your Android device. You can do this by manually copying the APK to the device or via the `adb` command-line tool included as part of the Android SDK.

### iOS

**Important:** The output of a Unity build for iOS is an Xcode project. To run or release the build, you need to open the project in Xcode and run it from there.

#### Configuration

1. In Unity, select Edit > Project Settings
2. Select Player
3. Select the iOS Tab (iPhone Icon)
4. Expand the Other Settings section


#### Building

1. In Unity, select File > Build Settings
2. Select "iOS"
3. Select "Switch Platform" to switch to the iOS platform
4. Select "Build"
5. Select a destination and filename for the Xcode project and select "Save"

More Information: [Unity build process for iOS](https://docs.unity3d.com/2018.2/Documentation/Manual/iphone-BuildProcess.html)

#### Running

##### Apple Setup For Debugging (First Time Only)

Before you can run the app on a device, you must configure a provisioning profile. Xcode can help you do this.

1. In Xcode, select `Unity-iPhone` in the project navigator
2. Select the `Unity-iPhone` Target
3. Select the Signing & Capabilities tab
4. Ensure the "Automatically manage signing" checkbox is checked
5. Under "Team" select "Laureate Education Inc"

More Information: [Xcode Signing & Capabilities](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7)

##### Run the App

1. Open Xcode version 11.2.1
2. In Xcode, open `Unity-iPhone.xcworkspace`
3. Connect a device, and select Run

## Releasing

### Android

1. Follow the steps in the "Building" section for Android above
2. Sign into the [Google Play Store Console]()
3. Select the "Walden VR" app
4. Select "Release Management"
5. App Releases > Production > Manage > Create Release
6. Select "Browse Files" and upload the APK you built in step 1
7. Optionally name the release, enter release notes, and submit the release

### iOS

#### Apple Setup For Release (First Time Only)

1. Install the distribution certificate. The distribution cert is on file as a password-protected `.p12` file.
2. Download and install the distribution provisioning profile from the Apple Developer Center.

More Information: [Xcode Signing & Capabilities](https://help.apple.com/xcode/mac/current/#/dev60b6fbbc7)

##### Upload to TestFlight

1. Open Xcode version 11.2.1
2. In Xcode, open `Unity-iPhone.xcworkspace`
3. Select Product > Destination > Generic iOS Device
4. Select Product > Archive
5. Follow the steps to upload the archive to TestFlight
6. Use App Store Connect to distribute the build via TestFlight

More Information: [Testing via TestFlight](https://help.apple.com/xcode/mac/current/#/dev2539d985f)

## Analytics

The project uses Firebase Analytics: https://firebase.google.com/docs/analytics/unity/start

Analytics events do not show up immediately in the online dashboard. To test analytics you can view log output from the devices:

### Android Analytics Logging

```
adb shell setprop log.tag.FA VERBOSE ; adb shell setprop log.tag.FA-SVC VERBOSE ; adb logcat -v time -s FA FA-SVC
```

### iOS Analytics Logging

Add these `Arguments Passed on Launch` under `Edit Scheme`:

```
-FIRDebugEnabled
-FIRAnalyticsDebugEnabled
```

## Testing

### TestRunner

To run the automated tests open the TestRunner. Go to `Window->General->Test Runner`. Make sure the game tab is not set to `Maximize on Play`. In the `Test Runner` tab select `PlayMode` and you can run all or selected tests as desired.

Test are located in `Assets/_tests`.

## Developer Tips

### Application Structure

This application contains two scenes in `Assets/scenes`:
  - LoginUI: 2D canvas objects for the initial login/menu selection.
  - CarboardVideoPlayer: contains VR viewer and videoplayer specific to cardboard.
    - The `DefaultMediaManager` attribute `Subfolder` can be edited to view each chapter without going through the LoginUI.
    - The videoplayer will not work until a chapter's mp4 files are downloaded to the device (`~/Library/Application\ Support/testing/test360VR/` when using the mac editor mode).
    - GoogleVR complains that 'SDK Cardboard is not supported in Editor Play Mode', but that warning message can be ignored.

### Troubleshooting

#### Upgrading Unity

Before opening the project with a new version of Unity you will need to manually delete the previous Unity version's cached files:

- `rm -rf Temp/* ; rm -rf Builds/* ; rm -rf Library/*`
- Unity will regenerate the necessary files.

You might also need to reimport the Cardboard VR:

- File -> Build Settings -> Player Settings -> XR Settings
- Remove Cardboard from the list of `Virtual Reality SDKs` and then re-add Cardboard.
- Do this for both Android and iOS players.
