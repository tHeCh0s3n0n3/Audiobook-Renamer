# Audiobook Renamer

## Introduction
This tool is designed to make use of the ID3 tags of MP3 audiobook files to organize the files into a library.

This tool _copies_ files to their new destination. You will need to remove the old files by hand. This is intentional and serves 2 purposes:
* I don't want to be responsible for loss of data
* If you use a tool to acquire your audiobooks which doesn't already do this, it probably expects the files to remain where it downloaded them to (or it will redownload them).

## Front-ends
* UI: Mainly used to preview the changes
* CLI: For quick renaming of new files

## How to use
1. Start the application
2. Click on the "Parse Directory" button
    * This will parse all mp3 files in the specified directory (non-recursive) 
3. Click on the "Mass Rename" button
    * This will create the following directory structure
```
    Author
    |- Series Name [for books in a series]
    |-- Book # - Book Name
    |--- Book Name.mp3
    |- Book Name [for books not part of a series]
    |-- Book Name.mp3
```

## Additional Functionality
The following functionality is also available:
* Preview the following fields from the parsed ID3 tags:
  * TItle
  * Author
  * Series Name
  * Book #
  * JSON representation (if available)
* You can delete books which have a series name, but no book #
  * Useful for when you have a tool which recently introduces the book # functionality 