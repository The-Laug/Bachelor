#!/bin/bash


# Create output dir
if [ ! -d "autoTestOutput" ]; then
    mkdir autoTestOutput
fi

# Directory containing the files
# DIRECTORY="./benchmarksThesis"
echo "Please give directory on which the script will run tests on all files"
read DIRECTORY


# Path to the F# script
FSHARP_SCRIPT="../interpretsums.fsx"
cd ./benchmarksThesis/
# Iterate over each file in the directory
for FILE in "../$DIRECTORY"/*
do
    # Check if it is a file (not a directory)
    if [[ -f "$FILE" && "$FILE" == *.vpr ]]; then
        # Run the F# script on the file
        echo " Running script on $FILE"
        dotnet fsi "$FSHARP_SCRIPT" "$FILE"
    fi
done


# # Directory containing the files
# OUTPUTDIR="outputs"
# echo "The F Sharp script has now run on all the files and saved the outputs in benchmarksThesis/$OUTPUTDIR"
# # Iterate over each file in the directory
# for FILE in "$OUTPUTDIR"/*
# do
#     # Check if it is a file (not a directory)
#     if [[ -f "$FILE" && "$FILE" == *_output.vpr ]]; then
#         BASENAMESWITHEXT=$(basename "$FILE")
#         BASENAMES="${BASENAMESWITHEXT%.*}"

#         echo "Verifying $BASENAMES"

#         # Specifying output directory
#         TMPOUTPUT="../autoTestOutput/${BASENAMES}_result_temp.txt"
        
#         if [ -f $TMPOUTPUT ]; then 
#         rm $TMPOUTPUT
#         fi
#         ../viper_client-master/viperenv/bin/python3 ../viper_client-master/client.py -p 51882 -f $FILE -x="--disableCaching" >> $TMPOUTPUT
#     fi
# done
# echo "The verification has now been run. The verifications can now be processed to one file using the script ./processTests.sh"
