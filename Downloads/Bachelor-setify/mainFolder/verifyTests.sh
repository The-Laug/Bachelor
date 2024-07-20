# Directory containing the files
cd benchmarksThesis
OUTPUTDIR="outputs"
echo "The F Sharp script has now run on all the files and saved the outputs in benchmarksThesis/$OUTPUTDIR"
# Iterate over each file in the directory
for FILE in "$OUTPUTDIR"/*
do
    # Check if it is a file (not a directory)
    if [[ -f "$FILE" && "$FILE" == *_output.vpr ]]; then
        BASENAMESWITHEXT=$(basename "$FILE")
        BASENAMES="${BASENAMESWITHEXT%.*}"

        echo "Verifying $BASENAMES"

        # Specifying output directory
        TMPOUTPUT="../autoTestOutput/${BASENAMES}_result_temp.txt"
        
        if [ -f $TMPOUTPUT ]; then 
        rm $TMPOUTPUT
        fi
        ../viper_client-master/viperenv/bin/python3 ../viper_client-master/client.py -p 51882 -f $FILE -x="--disableCaching" >> $TMPOUTPUT
    fi
done
echo "The verification has now been run. The verifications can now be processed to one file using the script ./processTests.sh"
