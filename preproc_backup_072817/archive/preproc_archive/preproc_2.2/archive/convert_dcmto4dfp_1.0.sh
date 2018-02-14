#!/bin/bash
#
#----------------------------------------------------------------------
# convert_dcmto4dfp.sh
# Converts dicom 3.0 compatible files to 4dfp, by way of the analyze
# image format.
#
# NOTE: This is a wrapper script for the standard preprocessing stream.
#
# Version 1.0
# 10.05.2011
#
# Josh Tremel
# (tremeljosh@gmail.com)
# University of Pittsburgh
#----------------------------------------------------------------------

#----------------------------------------------------------------------
# Program and User info
#----------------------------------------------------------------------
COMMAND=$0
PROGRAM=convert_dcmto4dfp.sh
VERSION=1.0
USER=$(whoami)

MINARGS=1

#----------------------------------------------------------------------
# Program Usage
#----------------------------------------------------------------------
usage() {
	cat << usageEOF
	
Usage: convert_dcmto4dfp.sh <string vars_file>
Description:
	Converts DICOM 3.0 standard files to analyze format and
	then to 4dfp for use with fidl.
	
	<vars_file> Preproc v2.0 vars file (usually passed from the actual
	preprocessing script)

usageEOF
	exit $1
}

# If there's no argument, return usage
if [ $# -ne $MINARGS ]; then
	usage 10 1>&2
fi

# Grab the vars file
varsFile=$1
# Read in variables
source $varsFile

# Set path to medcon, check to see it exists
medcon=/usr/local/pkg/xmedcon/bin/medcon
if [ ! -d $(dirname $medcon) ]; then
	printf "\nERROR: $medcon not found.\n\n"
	exit 1
fi

# Set path to analyzeto4dfp, check if it iexists
anzTo4dfp=/usr/local/pkg/nil-tools/bin/analyzeto4dfp
if [ ! -d $(dirname $anzTo4dfp) ]; then
	printf "\nERROR: $anzTo4dfp not found.\n\n"
	exit 1
fi

# Set up path variables for convenience
subjPath=${studyPath}/${subjID}
rawPath=${subjPath}/${rawDir}

# Push into subject directory
pushd $subjPath >/dev/null

# See if atlas dir exists; if not, make it
if [ ! -d $subjPath/atlas ]; then mkdir $subjPath/atlas ; fi

#----------------------------------------------------------------------
# CONVERT MPRAGE
#----------------------------------------------------------------------
# Check if there's a t1/mprage
if [ -n $t1Series ]; then
	printf "\n------------------"
	printf "\nConverting MP-RAGE"
	printf "\n------------------"
	printf "\nUser $USER on $(date)\n"
	
	# Pad series number to two digits
	mprDir=$(printf %02d ${t1Series})
	
	# Set full path to t1 raw series directory
	mprDir=$(printf ${rawPath}/${mprDir})
	printf "\nConverting MP-RAGE from $mprDir\n"
	
	# Use medcon to stack images and convert to analyze format.
	#+	-f: input files. Here, we're taking everything in mprDir with .sort.dcm extension.
	#+	These 'files' are links created with the link_files.sh script in the util/ directory.
	#+	-c anlz: sets conversion to analyze format
	#+	-sqr: squares up the image to it's largest dimension
	#+	-pad: pads edges of image to make square
	#+	-fb-dicom: sets 'fallback format' to dicom. Prevents some errors when using this...
	#+	-noprefix: tells medcon not to add anything to our output file name
	#+	-stack3d: puts all individual images into a single file. Each image is a slice.
	#+	-s: turn off verbosity (medcon sends some weird stuff through stderr)
	#+	-o: flag for specifying output file name
	printf "\nStacking images... Converting DCM to ANZ.\n"
	${medcon} -f $(ls -1 ${mprDir}/*.sort.dcm) -c anlz -sqr -pad -fb-dicom -noprefix -stack3d -s -o atlas/${subjID}_mprage
		# Make sure exit status of medcon command was 0 (i.e., successful, no errors).
		#+	If not, exit with error.
		if (( $? > 0 )); then
			printf "\n$PROGRAM: MEDCON ERROR\n"
			exit 1
		fi
	
	# Use analyzeto4dfp to convert ANZ format stack to 4dfp stack
	#+	-xy flag flips orientation to match that of atlas image
	#+ 	(Note: this is for sagittal-acquired mprage from the NIC)
	#+ 	(try -yz for axial mprage)
	#+	Support will (hopefully) soon be added for images collected elsewhere
	printf "\nConverting ANZ to 4dfp\n"
	$anzTo4dfp -xy atlas/${subjID}_mprage
		# Make sure exit status of medcon command was 0 (i.e., successful, no errors).
		#+	If not, exit with error.
		if (( $? > 0 )); then 
			printf "\n$PROGRAM: ANALYZETO4DFP ERROR\n"
			exit 1
		fi
	
	# Clean up
	if (( $econ > 1 )); then
		printf "\nCleaning up ./atlas/\n"
		printf "\tRemoving ANZ images...\n"
		/bin/rm atlas/${subjID}_mprage.img
		/bin/rm atlas/${subjID}_mprage.hdr
	fi
fi

#----------------------------------------------------------------------
# CONVERT T2
#----------------------------------------------------------------------
# Check if there's a t2 structural
if [ ! -z $t2Series ]; then
	printf "\n-------------"
	printf "\nConverting T2"
	printf "\n-------------"
	printf "\nUser $USER on $(date)\n"
	
	# Pad series number to two digits
	t2Dir=$(printf %02d ${t2Series})
	
	# Set full path to t2 raw series directory
	t2Dir=$(printf ${rawPath}/${t2Dir})
	printf "\nConverting T2 from $t2Dir\n"

	# Use medcon to stack images and convert to analyze format.
	#+	-f: input files. Here, we're taking everything in t2Dir with .sort.dcm extension.
	#+	These 'files' are links created with the link_files.sh script in the util/ directory.
	#+	-c anlz: sets conversion to analyze format
	#+	-sqr: squares up the image to it's largest dimension
	#+	-pad: pads edges of image to make square
	#+	-fb-dicom: sets 'fallback format' to dicom. Prevents some errors when using this...
	#+	-noprefix: tells medcon not to add anything to our output file name
	#+	-stack3d: puts all individual images into a single file. Each image is a slice.
	#+	-s: turn off verbosity (medcon sends some weird stuff through stderr)
	#+	-o: flag for specifying output file name	
	printf "\nStacking images... Converting DCM to ANZ.\n"
	${medcon} -f $(ls -1 ${t2Dir}/*.sort.dcm) -c anlz -sqr -pad -fb-dicom -noprefix -stack3d -s -o atlas/${subjID}_t2w
		# Make sure exit status of command was 0 (i.e., successful, no errors). If not, exit with error.
		if (( $? > 0 )); then 
			printf "\n$PROGRAM: MEDCON ERROR\n"
			exit 1
		fi
	
	# Use analyzeto4dfp to convert ANZ format stack to 4dfp stack
	#+	-yz flag will flip axial-aquired T2 so R=R and stack Inf to Sup
	#+ 	(Note: this is for data acquired at the NIC)
	#+	Support will (hopefully) soon be added for images collected elsewhere
	printf "\nConverting ANZ to 4dfp\n\n"
	$anzTo4dfp -yz atlas/${subjID}_t2w
		# Make sure exit status of command was 0 (i.e., successful, no errors). If not, exit with error.
		if (( $? > 0 )); then 
			printf "\n$PROGRAM: ANALYZETO4DFP ERROR\n"
			exit 1
		fi
	
	# Clean up
	if (( $econ > 1 )); then
		printf "\nCleaning up ./atlas/\n"
		printf "\tRemoving ANZ images...\n"
		/bin/rm atlas/${subjID}_t2w.img
		/bin/rm atlas/${subjID}_t2w.hdr
	fi
fi

#----------------------------------------------------------------------
# CONVERT FUNCTIONAL DATA
#----------------------------------------------------------------------
printf "\n--------------------------"
printf "\nConverting Functional Data"
printf "\n--------------------------"
printf "\nUser $USER on $(date)\n"

# Loop across all functional runs
for (( i=0; i<${#funcSeries[@]}; i++ )); do
	printf "\nConverting Series $( printf %02d ${funcSeries[$i]} )...\n"
	
	# If directory 'Label[index]' doesn't exist, make it.
	if [ ! -d ${funcLabel[$i]} ]; then mkdir ${funcLabel[$i]}; fi
	
	# Pad series number to two digits
	curSeriesDir=$( printf %02d ${funcSeries[$i]} )
	
	# Set full path to raw functional data directory
	curSeriesDir=$( printf ${rawPath}/${curSeriesDir} )
	printf "\nConverting files from $curSeriesDir\n"
	
	# Check if we need to evict any images 
	#+	(awk "FNR>2", for example, will print out the full list minus the first two images)
	if (( $evict > 0 )); then
		fileList=( $(ls -1 ${curSeriesDir}/*.sort.dcm | awk "FNR>$evict") )
	else
		fileList=( $(ls -1 ${curSeriesDir}/*.sort.dcm) )
	fi
	
	# Use medcon to stack images and convert to analyze format.
	#+	-f: input files. Here, we're taking everything in curSeriesDir with .sort.dcm extension.
	#+	These 'files' are links created with the link_files.sh script in the util/ directory.
	#+	-c anlz: sets conversion to analyze format
	#+	-stack4d: puts all images into a single file. Each image is a frame (TR).
	#+	-fb-dicom: sets 'fallback format' to dicom. Prevents some errors when using this...
	#+	-s: turn off verbosity (medcon sends some weird stuff through stderr)
	#+	-fmosaic=XxYxZ: dimensions of mosaic in each file. This is to split the slices and stack accordingly
	#+	-noprefix: tells medcon not to add anything to our output file name
	#+	-o: flag for specifying output file name	
	printf "\nStacking images... Converting DCM to ANZ.\n"
	${medcon} -f ${fileList[*]} -c anlz -stack4d -fb-dicom -s -fmosaic=${xDim}x${yDim}x${zDim} -noprefix -o ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}
		# Make sure exit status of command was 0 (i.e., successful, no errors). If not, exit with error.
		if (( $? > 0 )); then 
			printf "\n$PROGRAM: MEDCON ERROR\n"
			exit 1
		fi

	# Use analyzeto4dfp to convert ANZ format stack to 4dfp stack
	#+	-yz flag will flip axial-aquired image so R=R and stack Inf to Sup
	#+ 	(Note: this is for data acquired at the NIC)
	#+	Support will (hopefully) soon be added for images collected elsewhere
	printf "\nConverting ANZ to 4dfp\n\n"
	$anzTo4dfp -yz ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}
		# Make sure exit status of command was 0 (i.e., successful, no errors). If not, exit with error.
		if (( $? > 0 )); then 
			printf "\n$PROGRAM: ANALYZETO4DFP ERROR\n"
			exit 1
		fi

	# Clean up
	if (( $econ > 1 )); then
		printf "\nCleaning up ${func_label[$i]}\n"
		printf "\tRemoving ANZ images...\n"
		/bin/rm ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}.img
		/bin/rm ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}.hdr
	fi
	
	# clear variables before increment
	unset fileList; unset curSeriesDir
done

# Get rid of all those links we set up
if (( $econ > 1 )); then
	printf "\nCleaning up raw data directories...\n"
	find * -type l | xargs rm
fi

# Notify user that we're done
printf "\nConversions completed\n"

popd >/dev/null

# Program info and time
printf "\n$PROGRAM: Finished - User $USER on $(date)\n"

# Exit safely
exit 0
