#!/bin/bash

TEMPDIR="./autoTestOutput"

# Iterate over each temporary result file in the directory
for TEMPFILE in "$TEMPDIR"/*_result_temp.txt
do
    if [ -f "$TEMPFILE" ]; then
        echo $TEMPFILE
        # Get the base name of the file (without path and extension)
        BASENAME=$(basename "$TEMPFILE")
        BASENAME_WITHOUT_EXT="${BASENAME%_result_temp.txt}"
        
        # Initialize variables
        TIME=""
        STATUS=""

        # Read the temporary result file line by line
       while IFS= read -r LINE
        do
            # Extract time from the JSON messages
            if [[ "$LINE" =~ \"time\":([0-9]+) ]]; then
                TIME="${BASH_REMATCH[1]}"
            fi

            # Extract status from the JSON messages
            if [[ "$LINE" =~ \"status\":\"([a-zA-Z]+)\" ]]; then
                STATUS="${BASH_REMATCH[1]}"
            fi
        done < "$TEMPFILE"

        # Write the extracted time and status to the final result file
        if [ -n "$TIME" ] && [ -n "$STATUS" ]; then
            echo "$TIME $STATUS $BASENAME_WITHOUT_EXT" > "$TEMPDIR/${BASENAME_WITHOUT_EXT}_result.txt"
        fi
    fi
done

echo "Extraction script execution completed."
