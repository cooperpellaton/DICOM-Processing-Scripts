# Knowledge Breakdown

## Pre-processing Variables
1. MP Rage or T1 Structural Number
2. T2 Weight Structural Series Number
3. Functional Series # + Label  
    Ex: `5 6 7 8`  
    These are space delimited. Any reason?

*Where are the above variables coming from?*


## What we don't know:
* Don't understand the slices/scans therefore don't understand what/why certain operations are being done.
* What is achieved by debanding and realigning? Putting things on axis?
* Matrix transforms occur after the measurement are reconfigured to a mutli-dimensional matrix instead of a one dimensional array but what is happening during the operations? Averaging?

## What we know:
* The first functional series is always the scout.
    * Use this to discern the next 2 questions
    * MPRage more than 1?
* Functional size = image dimensions x+y (64x64)
    * T2 => the number of slices (*is this decay over time?*)
* Frame -> Folder with the scouts (*not sure about this*)
* Collapsing the time space means we go from a 1 dimensional array to a 3D array.

## Goal:
Ultimately we want to go from the scanner to a docker image running on the server to the final file output ready for analysis.

## Other stuff:
T1 Strutural Scan  
MP-RAGE (subset of T1) 
* Both are types of structural brain scans before processing to get the structure

T2 measures decay.  
T1 initial alignment.

Localizer button presses.