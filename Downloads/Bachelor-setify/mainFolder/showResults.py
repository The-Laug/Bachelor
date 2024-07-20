import matplotlib.pyplot as plt
import numpy as np

# Read the file and parse the data into lists
runtimes = []
statuses = []
test_names = []

with open('autoTestOutput/collected_result.txt', 'r') as file:
    for line in file:
        runtime, status, test_name = line.strip().split()
        runtimes.append(int(runtime))
        statuses.append(status)
        test_names.append(test_name)

# Combine the lists to sort them
data = list(zip(runtimes, statuses, test_names))

data.sort(key=lambda data: (data[1], data[0]),reverse=True)


# Making seperate lists again
runtimes, statuses, test_names = zip(*data)

# Finding proportional runtime
max_runtime = max(runtimes)
min_runtime = min(runtimes)
proportional_runtimes = [(r / max_runtime)*0.6 for r in runtimes]

# Creating the colors
colors = []
for i, status in enumerate(statuses):
    if status == 'success':
        intensity = 1 - proportional_runtimes[i] # Finding intensity
        colors.append((0.0, intensity, 0.0)) #green
    else:
        # Red color for failure
        colors.append((1.0, 0.0, 0.0)) #red

# Plotting
plt.figure(figsize=(12, 8))
bars = plt.barh(test_names, runtimes, color=colors)
plt.xlabel('Verifier Runtimes in ms')
plt.ylabel('Tests')
plt.title('Quantitative tests')



for i, (test_name, status) in enumerate(zip(test_names, statuses), 1):
    plt.text(max_runtime * 1.02, i-1, str(i), va='center', color='green' if status == 'success' else 'red')

plt.subplots_adjust(left=0.2) 
plt.yticks(fontsize=9)  

# Adding the legend
success_patch = plt.Rectangle((0, 0), 1, 1, fc='green', label='Success')
failure_patch = plt.Rectangle((0, 0), 1, 1, fc='red', label='Failure')
plt.legend(handles=[success_patch, failure_patch], loc='upper left', bbox_to_anchor=(1, 1))



plt.show()
