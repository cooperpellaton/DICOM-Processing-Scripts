#!/bin/bash
#
#----------------------------------------------------------------------
# convert_dcmto4dfp.sh
# Converts dicom 3.0 compatible files to 4dfp, by way of the analyze
# image format.
#
# NOTE: This is a wrapper script for the standard preprocessing stream.
#
# Modified analyzeto4dfp for t2 and BOLD data to convert data inferior to 
# superior (-yz)- 02.14.17 MW
#
# Modified analyzeto4dfp for t2 and BOLD data to deal with orientation
# of data collected on GT Trio- 05.26.15 MW
#
# Revision 1.2: JT
# 05.07.2013
#  Updated the analyzeto4dfp path to use the nil-tools RELEASE variable
#   instead of a hard path.
#  Updated to work with the new paths.sh file in the main preproc dir.
#
# Revision 1.1: JT
# 01.18.2012
#  Added logic to determine orientation of mp-rage, so
#   analyzeto4dfp will now flip the images correctly.
#  Added loop to find the real raw directory in case the specified dir
#   is just the top dir. For example, if user species 'raw', but data
#   are actually in raw/MRCTR/.
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
VERSION=1.2
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

# assume this is where the paths file is:
#preproc=$( dirname $(which preproc.sh) )
preproc="/home/mw/wheeler_shared/usb3/home/jt/scripts/preproc"
if [ -e $preproc/paths.sh ] ; then
	source $preproc/paths.sh
else
	printf "\nERROR: $preproc/paths.sh not found. Make sure that the\n\
	preprocessing directory is in the global PATH environment var.\n\
	Check your .bashrc file or the config files in\n\
	/etc/profile.d/ named fidl.sh or fidl.csh\n"
fi
medcon="/usr/local/xmedcon/bin/medcon"
# Check that we can find medcon
if [ ! -d $(dirname $medcon) ]; then
	printf "\nERROR: $medcon not found.\n\n"
	exit 1
fi

# Set path to analyzeto4dfp, check if it exists
anzTo4dfp=$RELEASE/analyzeto4dfp
if [ ! -d $(dirname $anzTo4dfp) ]; then
	printf "\nERROR: $anzTo4dfp not found. Check that nil-tool's RELEASE env var is set.\n\n"
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
# Find the real raw directory if it seems like specified is not right.
# Traverse directories until we hit some ambiguity
#----------------------------------------------------------------------
hasRawPath=1
curDir=$(pwd)
cd $rawPath > /dev/null
while [ $hasRawPath -eq 1 ]; do
	nDirs=$( find * -maxdepth 0 -type d | wc -l )
	nFiles=$( find * -maxdepth 0 -type f | wc -l )
	# If there's only one directory and no files, keep going...
	if [ $nDirs -eq 1 -a $nFiles -eq 0 ]; then
		cd $( find * -maxdepth 0 -type d ) >/dev/null
		rawPath=$(pwd)
	else
		hasRawPath=0
		cd $curDir >/dev/null
	fi
	unset nDirs; unset nFiles
done
unset curDir; unset hasRawPath

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
	
	
	# Determine orientation of mprage so we can flip correctly
	#+ Use 4dfp conventions: 2=axial, 4=sagittal (coronal not supported)
	#+ This is kind of a hack, but hey, it works...
	$anzTo4dfp atlas/${subjID}_mprage
	nOrient=$( grep "orientation" atlas/${subjID}_mprage.4dfp.ifh | awk '{print $3}' )
	/bin/rm atlas/${subjID}_mprage.4dfp.img
	/bin/rm atlas/${subjID}_mprage.4dfp.ifh
	/bin/rm atlas/${subjID}_mprage.4dfp.img.rec
	/bin/rm atlas/${subjID}_mprage.4dfp.hdr
	
	# Use analyzeto4dfp to convert ANZ format stack to 4dfp stack
	#+	-xy flag flips orientation to match that of atlas image
	#+ 	(Note: this is for sagittal-acquired mprage from the NIC)
	#+ 	(try -yz for axial mprage)
	#+	Support will (hopefully) soon be added for images collected elsewhere
	printf "\nConverting ANZ to 4dfp\n"
	if [ $nOrient -eq 2 ]; then
		printf "\nApplying flip for AXIAL-acquired MP-RAGE\n\n"
		$anzTo4dfp -yz atlas/${subjID}_mprage
	elif [ $nOrient -eq 4 ]; then
		printf "\nApplying flip for SAGITTAL-acquired MP-RAGE\n\n"
		$anzTo4dfp -xyz atlas/${subjID}_mprage
	else
		printf "\nCould not determine orientation of image. Assuming Sagittal acquisition.\n\n"
		$anzTo4dfp -xy atlas/${subjID}_mprage
	fi
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
	#+  This will always assume these are axial images and flip accordingly.
	#+ 	(Note: this is for data acquired at the NIC)
        #+  -xy flag will flip sagittal data, acquired on Trio
	printf "\nConverting ANZ to 4dfp\n\n"
        #$anzTo4dfp -yz atlas/${subjID}_t2w
        $anzTo4dfp -xy atlas/${subjID}_t2w
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
	#$anzTo4dfp ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}
        # ****use below for pre-CABI****
        #$anzTo4dfp -yz ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}
        # -y onlyOB worked for Austin's data
        # ****use below for CABI****
        $anzTo4dfp -y ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}
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
