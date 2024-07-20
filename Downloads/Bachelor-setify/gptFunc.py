import re

def replace_sum(filepath):
    try:
        with open(filepath, 'r') as file:
            content = file.read()

            # Define a dictionary to map function names to their replacements
            function_replacements = {
                'f': 'interpreted_f',
                # Add more function replacements here if needed
            }

            # Define a regular expression pattern to match the desired string
            pattern = r'sum\((\w+),(\w+),(\w+)\)'

            # Define a function to replace matches
            def replace(match):
                # Extract the function name and its arguments
                func_name, arg1, arg2 = match.groups()

                # Check if the function name needs replacement
                new_func_name = function_replacements.get(func_name, func_name)

                # Replace the matched string
                return f'interpreted_sum({arg1},{arg2})'

            # Perform the replacement using regular expression
            new_content = re.sub(pattern, replace, content)

        # Write the modified content back to the file
        with open(filepath, 'w') as file:
            file.write(new_content)

        print(f'Successfully modified {filepath}')
    except FileNotFoundError:
        print(f'Error: File {filepath} not found')

# Example usage:
replace_sum('example_file.txt')
