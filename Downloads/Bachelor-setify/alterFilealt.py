import re
import warnings
functionsFound = []
def interpretSum(result,lineNumber) :
    indexVariable = result.group(1)
    lowerBound = result.group(2)
    upperBound= result.group(3)
    innerFunction= result.group(4)
    if innerFunction == 'n':
        interpretedSum = "simplesum({},{},{})".format(indexVariable,lowerBound,upperBound)
    elif innerFunction == "n^2":
        interpretedSum = "squaredsum({},{},{})".format(indexVariable,lowerBound,upperBound)
    else :
        interpretedSum = str(result.group(0))
        print("A sum was not interpreted on line {} char {}".format(lineNumber,result.start(0)))
    functionsFound.append(innerFunction)
    # print(innerFunction)
    return interpretedSum


def search_and_replace(filePath):
   open('newFile.txt', 'w').close()  # Creates/Clears a newFile where the sums of the program are interpreted 
   writeFile = open("newFile.txt",'a') #opens newfile in append mode
   writeFile.write("import \"sumdomains.vpr\" \n") # imports what will be the domain defining axioms
   lineNumber=0 #Keeps track of lineNumber for debugging
   with open(filePath, 'r') as readFile: #Reads the input file
        for line in readFile: #looping through the input file
            lineNumber+=1 # Incrementing line index
            searchString = 'sum\[(.)\]\((.*),(.*),(.*)?\)' #Search string for regular expression
            searchStringComp = re.compile(searchString)
            result = searchStringComp.findall(line)
            result = re.findall(searchString,line)
            if len(result)!=0:
                for res in result:
                    originalSum = res.group(0)
                    interpretedSum = interpretSum(res,lineNumber)
                    updated_contents = line.replace(originalSum, interpretedSum)
            else: 
                updated_contents = line
            writeFile.write(updated_contents)
            line = readFile.readline





# Example usage
filePath = 'example.txt'
search_and_replace(filePath)
print(functionsFound)