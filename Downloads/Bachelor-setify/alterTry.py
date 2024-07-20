import re
import warnings

def interpretSum(result):
    indexVariable = result[1]
    lowerBound = result[2]
    upperBound= result[3]
    innerFunction= result[4]
    match innerFunction:
        case 'n':
            interpretedSum = f"simplesum({indexVariable},{lowerBound},{upperBound})"
        case 'n^2':
            interpretedSum = f"squaredsum({indexVariable},{lowerBound},{upperBound})"
        case 'C*n':
            interpretedSum = f"coefficientsum({indexVariable},{lowerBound},{upperBound})"
        # case _"+"_:
        #     interpretedSum = str(result)
        case _:
            interpretedSum = str(result)
            print("ERROR")
    return interpretedSum

    
def splitInnerFunc(func):
    totalFunc = ""
    if len(func.split('+')) != 1:
        for f in func.split('+'): totalFunc = totalFunc + "+{}".format(f)
    else: return func
    return totalFunc

def search_and_replace(filePath):
   open('newFile.txt', 'w').close()  # Creates/Clears a newFile where the sums of the program are interpreted 
   writeFile = open("newFile.txt",'a') #opens newfile in append mode
   writeFile.write("import \"sumdomains.vpr\" \n") # imports what will be the domain defining axioms
   with open(filePath, 'r') as readFile: #Reads the input file
        readFile = readFile.read()
        searchString = "(sum\[(.?)\]\((.*?),(.*?),(.*?)\))" #Search string for regular expression
        result = re.findall(searchString,readFile) 
        for sum in result:
            # readFile = re.sub(sum[0],interpretSum(sum[1:]),readFile)
            newSum=interpretSum(sum)
            # createDomain(newSum)
            print(newSum)
            readFile = readFile.replace(sum[0],newSum)
        writeFile.write(readFile)
        




# Example usage
filePath = 'example.txt'
search_and_replace(filePath)