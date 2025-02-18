# MeowFaceVRCFTInterface Module

> This module is an unofficial enhancement of the official [VRCFaceTracking-MeowFace](https://github.com/regzo2/VRCFaceTracking-MeowFace) module.

The module is a bridge between the [MeowFace](https://suvidriel.itch.io/meowface) Android app and the [VRCFaceTracking](https://docs.vrcft.io/) app.

With this module, you can use your real face to control your avatar's face in VRChat.

The MeowFaceVRCFTInterface is a good starting point as it doesn't require you to invest any money if you have an Android phone (Or its emulation) and a Windows computer.

## Step 0

1. Make sure you've installed [MeowFace](https://suvidriel.itch.io/meowface).
2. Make sure you've installed [VRCFaceTracking](https://docs.vrcft.io).
3. Make sure you find an avatar that supports face tracking or head movement. You **won't be able** to check if it works without this/third-party module enabled. Here's a video tutorial: [link](https://youtu.be/aitYy5H9YTM)
4. The **most important step** is to make sure that you have enabled [OSC](https://docs.vrcft.io/docs/intro/getting-started#3%EF%B8%8F-enable-osc-in-vrchat) in the avatar settings and enabled tracking of individual parts of the face/head; by default, this is all turned off.

## Installation

- Instructions can be found here: [click here](https://github.com/Jeka8833/MeowFaceVRCFTInterface/wiki/Install-Module)

## Connecting MeowFace to VRCFT

- Instructions can be found here: [click here](https://github.com/Jeka8833/MeowFaceVRCFTInterface/wiki/Connecting-MeowFace-to-VRCFT)

## Calibrating

Camera position, facial structure and other parameters are different for everyone, so the module needs to be customized and calibrated.

- Instructions on how to configure the module can be found here: [click here](https://github.com/Jeka8833/MeowFaceVRCFTInterface/wiki/Configuring-the-module)
- Instructions on how to calibrate an avatar face from the MeowFace app can be found here: [click here](https://github.com/Jeka8833/MeowFaceVRCFTInterface/wiki/Calibrate-From-the-MeowFace-App)
## Other Documentation

- All instructions can be found here: [click here](https://github.com/Jeka8833/MeowFaceVRCFTInterface/wiki)

## Build your module

> Feel free to create your modules to achieve the tracking results you want; perhaps some of them will be merged with this project.

I think you can figure it out yourself if you have already decided to modify the project. However, when you **clone** the repository using the **IDE**, the project will be **already configured and ready to build**.
Simply cloning (`git clone`) without `--recurse-submodules` or downloading a Zip archive from GitHub **won't work** because the repository uses **submodules**!

Instructions on where to put the module and in general on developing modules for VRCFT can be found [here](https://docs.vrcft.io/docs/vrcft-software/vrcft-sdk/tracking-module).

## License

MeowFaceVRCFTInterface code is licensed under [MIT License](https://github.com/Jeka8833/MeowFaceVRCFTInterface/blob/master/LICENSE).

MeowFaceVRCFTInterface uses code from third-party developers under license:
1. License for VRCFaceTracking: [Apache License 2.0](https://github.com/benaclejames/VRCFaceTracking/blob/master/LICENSE)