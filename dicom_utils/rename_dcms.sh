#!/bin/bash

# rename_dcms.sh
# 
# 02.07.2011
# Josh Tremel
# University of Pittsburgh

program=$0
program=$program:t

MINARGS=2

if [ $# -lt $MINARGS ]; then
	echo -e "Script to sort dicom files from Siemens scanners. This takes an"
	echo -e "input directory containing all files to be sorted/renamed. Output"
	echo -e "directory will contain folders for each series in which all files"
	echo -e "for that series will be named 'series_acquisition_instance.dcm'"
	echo -e ""
	echo -e "NOTE: \t Since this is a shell script, it will take a LONG time to"
	echo -e "\t run. Only use this if the compiled programs are not working..."
	echo -e "\t -It is recommended to run the command as:"
	echo -e "\t nice +19 ./rename_dcms.sh"
	echo -e ""
	echo -e "NOTE: \t This only copies files and does not alter file contents."
	echo -e ""
	echo -e "Usage: ./rename_dcms.sh <indir> <outdir>"
	echo -e "\t -<indir> should contain ALL files to rename (no sub-dirs)"
	echo -e "\t -make sure all files in <indir> are truly dicom files"
	exit 1
fi

# grab cmd line params
indir=$1
outdir=$2

# set path to medcon
dcmdump=/usr/local/bin/dcmdump

# Make <outdir> if it doesn't exist
if [ ! -d $outdir ]; then mkdir $outdir ; fi

pushd $indir >/dev/null
	# Count files in <indir>
	nfiles=`find . -type f | wc -l`

	# Make array listing all files
	files=(`ls -1`)
	
popd >/dev/null


echo "found $nfiles files in $indir"

# push into <indir>, start processing files
pushd $indir >/dev/null
filecount=1
errfiles=0
iter=0

while [ $iter -lt $nfiles ]; do
	# Read dicom info in first file; set vars for series, acquisition, instance
	series=`$dcmdump ${files[$iter]} | grep "SeriesNumber" \
	        | cut -d "[" -f 2 | cut -d "]" -f 1`
	acquisition=`$dcmdump ${files[$iter]} | grep "AcquisitionNumber" \
	        | cut -d "[" -f 2 | cut -d "]" -f 1`
	instance=`$dcmdump ${files[$iter]} | grep "InstanceNumber" \
	        | cut -d "[" -f 2 | cut -d "]" -f 1`

	# Pad numbers to 2, 3, and 4 digits, respectively
	series=`printf %02d $series`
	acquisition=`printf %04d $acquisition`
	instance=`printf %06d $instance`
	
	# make filename variable
	filenm="${series}_${acquisition}_${instance}.dcm"
	
	# Check if series dir exists, if not, make it.
	if [ ! -d ../$outdir/$series ] ; then mkdir "../$outdir/$series" ; fi
	
	#  Make sure destination file does not exist.
	#+ If it does, keep track of how many ($errfiles) and skip.
	#+ Otherwise, use system cp cmd to cp to outdir with new filename.
	if [ -e ../$outdir/$series/$filenm ] ; then
		(( errfiles++ ))
	else
		/bin/cp ${files[$iter]} ../$outdir/$series/$filenm
		if [ $? -ne 0 ]; then
			echo "Error on cp. Skipping file."
			(( errfiles++ )) 
		fi
	fi
	
	# echo count of files processed every hundred files
	if [ "${filecount:(-2)}" == '00' ] || [ "${filecount:(-2)}" == '50' ] ; then echo "$filecount files renamed" ; fi
	
	# advance count vars
	(( iter++ ))
	(( filecount++ ))

done

(( filecount-- ))
echo "${filecount} files read successfully."
echo ""

if [ $errfiles -gt 0 ]; then
	echo "$errfiles files could not be renamed because"
	echo "destination file already exists or cp error."
	echo ""
else
	echo "No errors detected."
	echo ""
fi

popd >/dev/null

echo "Script completed successfully by `whoami` on `date`"

exit 0
