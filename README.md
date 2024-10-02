# Augmented Reality (AR) Application

## Project Overview

This is a simple AR application built using Unity and the AR Foundation framework. It demonstrates basic AR functionalities, including placing, moving, rotating, and scaling 3D objects in an augmented reality environment. The app uses the New Input System to handle touch interactions.

## Features

- Place 3D objects in the real world using the device's camera.
- Move objects by dragging them across the AR scene.
- Rotate objects using two-finger rotation gestures.
- Scale objects using pinch-to-zoom gestures.
- Simple UI with buttons to reset the AR scene.
  
## Technologies Used

- **Unity 2021.3.X** (LTS version)
- **AR Foundation 4.X** (AR framework for Unity)
- **ARCore XR Plugin** (Google ARCore support)
- **New Input System** (`UnityEngine.InputSystem.EnhancedTouch`) for touch input handling
- **C#** for scripting
- **Android Build Support** for Unity to compile APK files

## Testing the project

### Download the .apk file
- you can find the .apk file of the project in the releases or [AR App.apk](https://github.com/DyaaEdDeen/AR-App/releases/download/v0.01/AR.APP.apk)
 simply download it and run the app

## How to Build the Project for Android

### Prerequisites
- **Unity 2021.3.X** (or later) installed with Android Build Support
- **Android SDK, NDK**, and **JDK** properly configured (Unity handles this in `Preferences > External Tools`)
- A **physical Android device** to run the AR application (ARCore compatible)

### Build Steps

1. **Open the Project in Unity**:
   - Open Unity Hub.
   - Click on "Open" and select the folder containing this project.

2. **Switch Platform to Android**:
   - In Unity, go to `File > Build Settings`.
   - Select **Android** from the platform list.
   - Click on **Switch Platform**.

3. **Player Settings**:
   - In the Build Settings window, click **Player Settings**.
   - Under **Other Settings**, ensure that the **Minimum API Level** is set to **Android 7.0 'Nougat' (API Level 24)** or higher.
   - Under **XR Plug-in Management**, make sure **ARCore** is enabled.

4. **Configure Input System**:
   - Go to `Edit > Project Settings > Player`.
   - Under **Active Input Handling**, select **Both** (for New Input System and Old Input support).

5. **Build the APK**:
   - Go to `File > Build Settings`.
   - Ensure **Android** is selected.
   - Set the build output path to a folder named **Builds** (this can be anywhere on your computer).
   - Click **Build** to create the APK file.

6. **Deploy APK to Device**:
   - Once the APK is built, copy the file from the **Builds** folder to your Android device or use **ADB** (Android Debug Bridge) to install it.

   ```bash
   adb install path_to_your_apk.apk

## Additional Notes

- The app uses the device's camera to display AR content, so make sure your Android device is ARCore compatible.
- Make sure to give the app camera permissions to run AR functionalities.

## Challenges & Improvements

- **Challenges:** Handling AR object interactions with multitouch input and ensuring smooth performance across different devices were key challenges during development.
- **Future Improvements:** If more time is available, I would like to implement features such as saving the AR scene, adding multiple objects, and improving gesture handling for more complex interactions.
