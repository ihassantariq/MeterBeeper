# MeterBeeper

Specification
This is the design spec for Nuclyus’s Meter Beeper app.
Introduction
Workers on busy construction sites routinely forget social distancing rules. We want an app to give them an audible reminder when they approach within a set range. The distance should be an integer number of meters that defaults to 2 and can be changed on the device.
The sound will be a sample of The Police - Don't Stand So Close To Me. We’ll provide the sound file in mp3 format and the graphics as bitmaps.
Requirement
We require a cross-platform app to run on Android and iOS that performs that simple function. It will use Bluetooth LE or some other distance finding mechanism to measure the distance between the device and other devices in range running the app. When the distance is less than the set distance the app sounds an alarm by playing a sound file.
The user can stop the sound by moving a slider on the app front screen.
Behavior
•	Begins to work on startup.
•	Sliding the virus [or some other deliberate action] causes it to pause or restart.
•	Using the numeric slider [or equivalent] increases and decreases the set distance.
Appearance
We envisage a simple front screen with:
•	The virus image, 
•	A control for the distance.
•	Continues to work when backgrounded.
 
The numeric typeface is Montserrat Black.
