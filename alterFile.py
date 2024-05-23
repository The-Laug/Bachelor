import re
import warnings
def interpretSum(result) :
    indexVariable = result[0]
    lowerBound = result[1]
    upperBound= result[2]
    innerFunction= result[3]
    if innerFunction == 'n':
        interpretedSum = "simplesum({},{},{})".format(indexVariable,lowerBound,upperBound)
    elif innerFunction == "n^2":
        interpretedSum = "squaredsum({},{},{})".format(indexVariable,lowerBound,upperBound)
    else :
        interpretedSum = str(result)
        print("ERROR")
    # print(innerFunction)
    return interpretedSum


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
            readFile = readFile.replace(sum[0],interpretSum(sum[1:]))
        writeFile.write(readFile)
        createDomain(result)
        
def domainRec(sum):
    if len(sum.split('+')) != 1:
        domainRec(sum.split('+'))
    if sum == 'n':
        return """
domain simplesums {
    // uninterpreted function
    function simplesum(i: Int, N: Int): Int

    axiom emptySum{
        simplesum(0,0) == 0
    }
    
    axiom iGreaterThanN {
        forall i:Int, N:Int::
            i > N ==> simplesum(i,N) == 0
    }

    axiom rightRecursion {
        forall i:Int, N: Int :: 
            0 <= i < N ==> simplesum(i,N) == simplesum(i,N-1) + N
    }

    axiom leftRecursion {
        forall i:Int, N: Int :: 
            0 <= i < N ==> simplesum(i,N) == i + simplesum(i+1,N)
    }
    

}
                """
    else :
        return "//No func found"

def createDomain(sumList):
   open('sumdomains.vpr', 'w').close()
   domainFile = open("sumdomains.vpr",'a')
   for sum in sumList:
        domainFile.write(domainRec(sum[-1]))






# Example usage
filePath = 'example.txt'
search_and_replace(filePath)