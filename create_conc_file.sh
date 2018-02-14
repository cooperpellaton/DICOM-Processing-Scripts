#! /bin/bash
#
#----------------------------------------------------------------------
# create_conc_file.sh
#
# Creates a conc file for FIDL
#
# Version 0.01: RK (Rachit Kumar)
# 9.22.2017
#  Initial script creation
#
#
# Rachit Kumar
# rkumar.biz@gmail.com
# Georgia Institute of Technology
# Wheeler Lab
#----------------------------------------------------------------------

#----------------------------------------------------------------------
# Program and User info
#----------------------------------------------------------------------
COMMAND=$0
PROGRAM=create_conc_file.sh
VERSION=0.01
USER=$(whoami)

#----------------------------------------------------------------------
# Script/Code
#----------------------------------------------------------------------
printf "How many files will be in your list?\n"
printf "Enter here: "
read numFiles

printf "\n\nCurrent working directory is $(pwd)\n"
printf "Leave the entry blank if you would like to use this directory as the base for the list.\n"
printf "Otherwise, enter a new directory.\n"
printf "Enter here: "
read baseDir

if [ -z "$baseDir" ]; then
    baseDir="$(pwd)"
fi

printf "\n\nWhat is the subject identifier (ex: S02, S03, FH0015)\n"
printf "Enter here: "
read subjID

printf "\n\nWhat would you like the name of the output file to be?\n"
printf "Output file will automatically have the .conc extension appended.\n"
printf "Enter here: "
read outName
outName="${outName}.conc"

if [[ -e "$outName" ]]; then
    printf "File already exists! Do you want to overwrite (y/n)?\n"
    printf "Enter here: "
    read fileExistsResp
    
    if [ "$fileExistsResp" != "y" ]; then
	printf "\nOverwrite denied; script exiting...\n\n"
	exit $1	
    fi 
fi

echo "number of files: $numFiles" > "$outName"

for (( i=1; i<=$numFiles; i++ ))
do
   outLine="file:/"
   outLine+="$baseDir"
   outLine+="/run$i"
   outLine+="/$subjID"
   outLine+="_run$i"
   outLine+="_faln_dbnd_xr3d_norm.4dfp.img"
   echo -e "\t$outLine"

done >> "$outName"

echo "number of files: $numFiles" >> "$outName"

printf "\nOutput conc file \" $outName \" created with provided parameters.\n\n"