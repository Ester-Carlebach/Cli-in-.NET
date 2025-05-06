File Bundler
 
# File Bundler CLI
 
## Overview
This is a command-line interface (CLI) tool for bundling code files into a single text file. It
is designed to help users package multiple files of various programming languages into
one readable output.

### Parameters
- `--language <languages>`: Specify the programming languages to include (e.g., js, py).
- `--output <output_file>`: The name of the output file where the bundled code will be saved.
- `--note [true|false]`: Include notes in the output (default is no).
- `--sort [true|false]`: Sort the files before bundling (default is no).
- `--remove-empty-lines [true|false]`: Remove empty lines from the output (default is no).
- `--author <author_name>`: Specify the author's name to include in the output.
## Examples
fib bundle --language js,py --output bundled.txt --note y --sort n --remove-empty-lines y --author "Sara Choen"
## Bundling Command
To bundle your code files, use the following command structure:
```bash
fib bundle --language <languages split with ","> --output <output_file> --note [y|n] --sort
[y|n] --remove-empty-lines [y|n] --author <author_name>
