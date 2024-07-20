The main program is interpretsums.fsx
This can be run with dotnet using:
dotnet fsi interpretsums.fsx VIPERSCRIPT
Where VIPERSCRIPT is substituted for the script you want to verify.

The automated testing can be done using the shell script ./runTests.sh
Here you are asked to specify a directory, in which the testing will be run. (Please use the relative path from mainFolder)
Afterwards, the automated testing can be run by starting a viperserver on port 51882 and running the ./verifyTests.sh
After this the tests can be processesed and plotted by running ./processTests.sh

The tests shown in the evaluation of the thesis can be found in the tests directory
