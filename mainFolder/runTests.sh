#!/bin/bash


# Create output dir
if [ ! -d "autoTestOutput" ]; then
    mkdir autoTestOutput
fi


# Directory containing the files
DIRECTORY="./benchmarksThesis"

# Path to the F# script
FSHARP_SCRIPT="./interpretsums.fsx"

# Iterate over each file in the directory
for FILE in "$DIRECTORY"/*
do
    # Check if it is a file (not a directory)
    if [ -f "$FILE" ]; then
        # Run the F# script on the file
        dotnet fsi "$FSHARP_SCRIPT" "$FILE"
    fi
done

# Directory containing the files
OUTPUTDIR="./benchmarksThesis/outputs"

# Iterate over each file in the directory
for FILE in "$OUTPUTDIR"/*
do
    # Check if it is a file (not a directory)
    if [[ -f "$FILE" && "$FILE" == *_output.vpr ]]; then
        BASENAMESWITHEXT=$(basename "$FILE")
        BASENAMES="${BASENAMESWITHEXT%.*}"

        echo ${BASENAMES}

        # Run the F# script on the file
        ./viper_client-master/viperenv/bin/python3 ./viper_client-master/client.py -p 51882 -f $FILE -x="--disableCaching" >> ./autoTestOutput/${BASENAMES}_result_temp.txt
    fi
done