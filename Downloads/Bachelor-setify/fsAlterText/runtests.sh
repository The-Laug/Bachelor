#!/bin/bash

# Directory containing the files
DIRECTORY="/Users/lauge/Desktop/bachelor/fsAlterText/autorun"

# Path to the F# script
FSHARP_SCRIPT="/Users/lauge/Desktop/bachelor/fsAlterText/interpretSumsAuto.fsx"

# Iterate over each file in the directory
for FILE in "$DIRECTORY"/*
do
    # Check if it is a file (not a directory)
    if [ -f "$FILE" ]; then
        # Run the F# script on the file
        dotnet fsi "$FSHARP_SCRIPT" "$FILE"
    fi
done