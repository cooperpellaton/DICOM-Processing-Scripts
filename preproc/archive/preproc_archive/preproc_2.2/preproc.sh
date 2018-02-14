#!/bin/bash
#
#----------------------------------------------------------------
#  preproc.sh
#  Preprocessing stream for fIDL
#
#  Version 2.2
#  01.17.2012
#
#  Josh Tremel
#  (tremeljosh@gmail.com)
#  University of Pittsburgh
#----------------------------------------------------------------
# REVISION HISTORY:
#----------------------------------------------------------------
# Revision 2.2: JT (current)
# 01.17.2012
#   Added a check for institution and slice number before frame 
#    alignment. If the slice number is even and the Institution ID
#    is "MRRC", then add a -N option to frame_align_4dfp to tell
#    it that funcs are even-odd interleaved instead of the normal
#    odd-even. This is only for the MRRC and only if there's an 
#    even total slice count.
#   Now figures out acquisition orientation for the mprage and
#    flips image correctly.
#
# Revision 2.1: JT
# 11.14.2011
#   added script to check raw directory structure and check
#   for data left over from previous or botched preprocessing.
#
# Revision 2.0: JT
# 10.04.2011
#   implemented subject.vars file input method
#   added some better error control
#   added log output to subjDir/QA directory
#   added support for .ima and Siemens Vision files
#   abstracted some blocks of code to separate scripts
#    to ease debugging
#   moved to new directory in /usr/local/pkg/fidl_preprocess/
#   cleaned up code
#
# Revision 1.2: JT
# 01.28.2011
#   Added some data checks to make sure script will run
#   correctly. Script should find input data more 
#   intelligently...
#
# Revision 1.1: JT
# 11.18.2010
#   Fixed a few annoyances.
#
# Version 1.0 (BASH): JT
# 09.29.2010
# Based on:
#   generic_NT_script (CSH) v1.7, 2007
#   Avi Snyder, Washington University, St. Louis, MO
#   Packaged with older release of fIDL
#
#----------------------------------------------------------------
# TO DO:
#----------------------------------------------------------------
# 01) Check if files are already organized into the current
#    directory structure format or if 4dfp files already exist, 
#    to avoid redundant processing
# 02) Add option for selective processing (i.e., just convert)
# 03) Test speed differences between dcmdump and medcon for 
#    reading files. Can dcmtk convert to anz? Don't want to 
#    add more dependencies...
# 04) Add in output of movement data file compatible with
#    plot_movt.sh to QA/ directory.
# 05) Add in option for gzip compression/decompression of raw
#    data.
# 06) Add option for varying degrees of verbosity
# 08) Move $econ checks and cleanup to a case statement at the
#    end of the script. Makes more sense, instead of checking
#    the econ setting all over the script.
# 09) Add in better support for multiple mprages and T2s
# 10) Subject readme/summary file output to QA/
# 11) QA checks (snr, movt, variance overlay) to QA/
# 12) Support for multiple atlas t4 file output
# 13) Support for single subj cross-day/multi-session processing

#----------------------------------------------------------------
# Program and User info
#----------------------------------------------------------------
COMMAND=$0
PROGRAM=preproc.sh
VERSION=2.2
USER=$(whoami)

# Number of required arguments
ARGS=1

#----------------------------------------------------------------
# Program Usage
#----------------------------------------------------------------
# Function to display program usage
usage() {
	cat << usageEOF
preproc.sh version $VERSION
  Usage: preproc.sh <subject_id.vars file>
  Description:
    Preprocessing stream for data analysis in FIDL

    <subject_id.vars> contains all the variables and
    settings needed to preprocess your data. 

    If you need to make one, copy the default file in 
    /usr/local/pkg/fidl_preprocess/files/preproc_default.vars,
    name it something informative like subjectID.vars, and
    change the settings in it. Alternatively, run 
    fidl_preprocess/create_vars_file.sh and follow the 
    instructions.
usageEOF
	exit $1
}

# Check for correct number of arguments; call usage(), if wrong
if (( $# != $ARGS )); then
	usage 10 1>&2
fi

#----------------------------------------------------------------
# Process Subject Vars File
#----------------------------------------------------------------
# Grab settings file
varsFile=$1

# Read in variables and settings
source $varsFile

# Print vars to screen, ask user if everything is right:
printf "\n---------------------------\n"
printf "Please check these values:"
printf "\n---------------------------\n"
printf "Subject ID: $subjID\n"
printf "Data are in: $studyPath/$subjID/$rawDir\n"
printf "Target atlas: $target\n"
printf "Series numbers: \n"
printf "\t MPRAGE/T1: $t1Series\n"
printf "\t T2: $t2Series\n"
printf "\t EPI/BOLD: (${funcLabel[*]})\n"
printf "\t EPI/BOLD: (${funcSeries[*]})\n"
printf "Your TR is $TR s\n"
printf "In-plane resolution is ${xDim}x${yDim}\n"
printf "Number of slices: $zDim\n"

# Fix evict and skip settings
if (( $evict < $skip )); then
	skip=$(( $skip - $evict ))
elif (( $evict >= $skip )); then
	skip=0
fi
printf "Set to skip $skip frames and evict $evict frames\n"

# Check and print normalization setting
if (( $shouldNorm == 1 )); then 
	printf "Set to normalize per-frame volume intensity\n"
else
	printf "Set to not normalize per-frame volume intensity\n"
fi

# Check and print atlas transform settings
case $epi2atl in
	0 )
		printf "Data stacks will be left in EPI/data space\n"
		;;
	1 | 2 )
		printf "Data stacks will be transformed to atlas space\n"
		;;
esac

# Ask if everything is correct; read user input
printf "\nProceed with the above settings? [RETURN assumes yes] (Y/n): "
read answer

# If they just hit return with no input, continue with script (assume yes)
if [ -z $answer ] ; then
	printf "\nPreproc started by user $USER on $(date)\n"
# Check for input
else
	case $answer in 
		# Start if answer is y or yes
		[yY] | [yY][Ee][Ss] )
			printf "\nPreproc started by user $USER on $(date)\n"
			;;
		# Exit if answer is n or no
		[nN] | [N|n][O|o] )
			printf "Exiting...\n"
			exit 11
			;;
		# Exit if there's anything else that might be confusing/ambiguous
		*)
			printf "Unrecognized input. Exiting...\n"
			exit 12
			;;
	esac
fi

varsFile=$(pwd)/$varsFile

#----------------------------------------------------------------
# Setup
#----------------------------------------------------------------
	#----------------------------------------------------------------
	# Variables for convenience and readability...
	#----------------------------------------------------------------
	# subjPath is path to main subject directory
	subjPath=${studyPath}/${subjID}

	# rawPath is path to raw data directory
	rawPath=${subjPath}/${rawDir}

	#----------------------------------------------------------------
	# Set up log file and QA dir, tee stderr/out to log file
	#----------------------------------------------------------------
	if [ ! -d $subjPath/QA ]; then mkdir $subjPath/QA ; fi
	preprocLog=$subjPath/QA/${subjID}_preproc.log
	if [ -e $preprocLog ]; then /bin/rm $preprocLog ; fi
	exec > >(tee $preprocLog)
	exec 2>&1

	#----------------------------------------------------------------
	# Make sure we have the programs we need
	#----------------------------------------------------------------
	# Set path to medcon
	medcon=/usr/local/pkg/xmedcon/bin/medcon
	if [ ! -d $(dirname $medcon) ]; then
		printf "\nERROR: $medcon not found.\n\n"
		exit 1
	fi

	# Set path to preproc dir
	preproc=/usr/local/pkg/fidl_preprocess/
	if [ ! -d $preproc ]; then
		printf "\nERROR: $preproc not found.\n\n"
		exit 1
	fi

	# Set path to afni directory
	afni=/usr/local/pkg/afni/
	if [ ! -d $afni ]; then
		printf "\nERROR: $afni not found.\n\n"
		exit 1
	fi
	
	#----------------------------------------------------------------
	# Make sure our number of functional runs is correct
	#----------------------------------------------------------------
	if (( ${#funcSeries[@]} != ${#funcLabel[@]} )); then
		printf "\nERROR: Number of func labels does not equal number of func series numbers.\n\n"
		exit 1
	fi

	#----------------------------------------------------------------
	# Make sure the target atlas exists in the right place
	#----------------------------------------------------------------
	printf "\nLooking for atlas...\n"
	
	# See if it is one of the default atlases
	if [ $target == "711-2B" -o $target == "711-2C" -o $target == "711-2O" ] ; then
		printf "\nAtlas recognized.\n"
		
		# Set flag
		isCustomAtlas=false
		
		# Make sure it exists in REFDIR
		if [ -e $REFDIR/${target}.4dfp.img ] ; then
			printf "\nAtlas located.\n"
		else
			printf "\nERROR: Atlas was not found.\n\
	N.B., Try adding a symbolic link in ${targetPath} that\n\
	points to your 111 target atlas and name the link\n\
	something like 711-2?.4dfp.img (i.e., leave off anything\n\
	after the atlas name). And try again...\n"
			exit 1
		fi
	else
		printf "\nWARNING: Atlas not recognized. Searching in targetPath...\n"

		# Set flag
		isCustomAtlas=true
		
		# Make sure we can find it in the specified targetPath
		if  [ -e ${targetPath}/${target}.4dfp.img ] ; then
			printf "\nAtlas located.\n"
		else
			printf "\nERROR: Atlas was not found.\n\
	N.B., Try adding a symbolic link in ${targetPath} that\n\
	points to your 111 target atlas and name the link\n\
	something like 711-2?.4dfp.img (i.e., leave off anything\n\
	after the atlas name). And try again...\n"
			exit 1
		fi
	fi

	#------------------------------------------------------------------
	# check_preproc_dir.sh: Checks subject directory for old data from
	#+ previous preprocessing. 
	#------------------------------------------------------------------
	$preproc/util/check_preproc_dir.sh ${varsFile}
		# If script exited with error, exit this script too
		if (( $? > 0 )); then exit 1 ; fi

	#----------------------------------------------------------------
	# Traverse directories until we hit some ambiguity
	#----------------------------------------------------------------
	hasRawPath=1
	curDir=$(pwd)
	cd $rawPath >/dev/null
	while [ $hasRawPath -eq 1 ]; do
		nDirs=$( find * -maxdepth 0 -type d | wc -l )
		nFiles=$( find * -maxdepth 0 -type f | wc -l )
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
	
#----------------------------------------------------------------
# Check Data for Compatibility
#----------------------------------------------------------------
pushd $rawPath || { printf "\nERROR: Specified raw data directory $rawDir not found\n" ; exit 1; }

	#----------------------------------------------------------------
	# Check if there are subdirectories in rawPath
	#----------------------------------------------------------------
	# Make array of any dirs within raw data dir
	rawDirList=( $(find ./?* -maxdepth 0 -type d | cut -d "/" -f2) )
		
	# If length is null, assume all raw files are in $rawPath
	isSingleDir=false
	if (( ${#rawDirList[@]} == 0 )); then
		# Set rawDirList to the raw data directory
		rawDirList=$rawPath
		# Set flag
		isSingleDir=true
	fi

	#----------------------------------------------------------------
	# Check if files are compressed (i.e., .Z extension)
	#----------------------------------------------------------------
	printf "\nLooking for compressed files...\n"
	for (( i=0; i<${#rawDirList[@]}; i++ )); do
		# If there is at least 1 .Z file, decompress
		if (( $(ls -1 ${rawDirList[$i]}/*.Z 2>/dev/null | wc -l) > 0 )); then 
			printf "\nWARNING: Decompressing files in ${rawDirList[$i]}...\n"
			printf "\n\tDo not interupt or kill this process.\n"
			
			# Push into direcotry
			pushd "${rawDirList[$i]}" >/dev/null
			
			# Decompress files in this directory
			gunzip ./* 2>/dev/null
			
			popd >/dev/null
		fi
	done

	#----------------------------------------------------------------
	# Check that medcon can read these files
	#----------------------------------------------------------------
	pushd ${rawDirList[0]} > /dev/null
	# Grab first file in first directory
	file=$(ls | head -1)
	
	# Init flag as false and init blank institution id.
	isVision=false
	sInst=""
	
	# Run file through medcon, check exit status ($?)
	medcon -s -f $file >/dev/null
	cmdSuccess=$?
	
	# Did medcon exit without error
	if (( $cmdSuccess == 0 )); then
		printf "\nFile format recognized...\n"

		# Get institution tag.
		sInst=$( medcon -s -f $file -d | grep "Institution ID" | awk '{print $4}' )
	else 
	#---------------------------------------------------------------------
	# If medcon exited with error; Check if this is a Siemens Vision file
	#---------------------------------------------------------------------
		#FIXME: This seems like a terrible way to do this!!! Works though...
		test1=x$($afni/siemens_vision $file | grep -a Patient | awk '{print $3}')
		test2=x$($afni/siemens_vision $file | grep -a Manufact | awk '{print $3}')
		test3=x$($afni/siemens_vision $file | grep -a Institut | awk '{print $3}')
		
		#------------------------------------------------------------------
		# If AFNIs 'siemens_vision' program returns a value for these 
		#+ fields, then this is a Vision file. If the program cannot read 
		#+ the file correctly, these fields will turn up blank ('grep -a' 
		#+ forces text output to prevent returning irregular values).
		#------------------------------------------------------------------
		if [ "$test1" != "x" -a "$test2" != "x" -a "$test3" != "x" ]; then
			# Set flag
			isVision=true
			# Ask if user wants to continue with these files
			printf "\nWARNING: These are Siemens Vision files. They will be converted\n\
	with AFNI's to3d command to AFNI format, then to ANALYZE, and finally to 4dfp.\n\
	This conversion has not been quality checked, so we don't really know how the \n\
	data turn out... NOTE: This will copy your raw directory (to raw_bak) before \n\
	processing and run on the copy...\n\n"
			printf "Do you wish to continue? [Y/ n]: "
			read answer
			if [ -z $answer ]; then
				printf "\nContinuing...\n"
			else
				case $answer in
					[yY] | [yY][eE][sS] )
						printf "\nContinuing...\n"
						;;
					[nN] | [n|N][o|O] )
						printf "\nExiting...\n"
						exit 11
						;;
					#Check for anything else...
					* )
						printf "\nUnrecognized input. Exiting...\n"
						exit 12
						;;
				esac
			fi
			# Clear out temp variables
			unset test1; unset test2; unset test3; unset file
		else
			# If medcon and siemens_vision both fail, the files are
			#+ likely not compatible...
			printf "\nERROR: Unrecognized file format.\n"
			printf "Make sure these are legitimate dicom 3.0 files; check\n"
			printf "with Josh to make sure we can use these files...\n"
			exit 1
		fi
	fi	
	popd >/dev/null

	#------------------------------------------------------------------
	# Process and Convert Vision Files
	#------------------------------------------------------------------
	if [ $isVision == true ]; then
		#------------------------------------------------------------------
		# vision_convert.sh: This script will sort files into series 
		#+ directories (if necessary), rename the files (create symlinks 
		#+ that are sortable by sequence and image number), and convert 
		#+ them to 4dfp. Conversion uses AFNIs to3d to get the files to 
		#+ AFNI format; 3dAFNItoANALYZE converts to analyze format;
		#+ analyzeto4dfp converts them to 4dfp.
		#------------------------------------------------------------------
		printf "\nCalling script to convert Vision files...\n\n"
		$preproc/util/vision_convert.sh ${rawPath} "${isSingleDir}"
			if (( $? > 0 )); then 
				printf "\nERROR: Vision file conversion script did not finish successfully\n"
				exit 1
			fi

		# Prefix mprage filename with subjID 
		pushd ${subjPath}/atlas >/dev/null
		find mprage.* -maxdepth 0 -type f | xargs -i mv '{}' ${subjID}_'{}'
		popd >/dev/null

		# Find all the bold runs and reset funcLabel[*]
		pushd ${subjPath} >/dev/null

		# if series were specified...
		if [ ! -z $funcSeries ]; then
			unset funcLabel
			for (( i=0; i<${#funcSeries[@]}; i++ )); do
				boldrun=$(( $i+1 ))
				funcLabel=( ${funcLabel[*]} `printf "bold$(printf %02d $boldrun )"` )
				unset boldrun
			done
		else
			funcLabel=( $(find bold?? -maxdepth 0 -type d) )
			unset funcSeries
			for (( i=0; i<${#funcLabel[@]}; i++ )); do
				funcSeries=( ${funcSeries[*]} $(( $i + 1)) )
			done
		fi
		
		popd >/dev/null

		# Let's not worry about T2s...
		unset t2Series

		# Prefix bold filenames with subjID
		for (( i=0; i<${#funcLabel[@]}; i++ )); do
			pushd ${subjPath}/${funcLabel[$i]} >/dev/null
			find bold* -maxdepth 0 -type f |  xargs -i mv '{}' ${subjID}_'{}'
			popd >/dev/null
		done
	#------------------------------------------------------------------
	# Process and Convert DICOM 3.0 Compatible Files
	#------------------------------------------------------------------
	else
		#------------------------------------------------------------------
		# sort_series.sh: If all images are in the raw directory and not 
		#+ separated into series sub-directories, this will run to sort 
		#+ them into series directories. A backup copy of your starting raw
		#+ data directory will be made and placed in $rawPath/raw_bak 
		#+ before moving files.
		#------------------------------------------------------------------
		if [ ${isSingleDir} == true ]; then 
			printf "\nCalling script to sort series directories...\n\n"
			$preproc/util/sort_series.sh ${rawPath}
			# If script exited with error, exit this script too
			if (( $? > 0 )); then exit 1 ; fi
		fi

		#------------------------------------------------------------------
		# link_files.sh: Creates symbolic links to each file. This is done
		#+ for the sake of ease in finding files to convert. Links will be 
		#+ named according to series, acquisition, and instance. An
		#+ extension of '.sort.dcm' will suffix each link so we can grab 
		#+ all the files quickly and easily.
		#------------------------------------------------------------------
		printf "\nCalling script to create sortable links to raw files...\n\n"
		$preproc/util/link_files.sh ${rawPath}
			# If script exited with error, exit this script too
			if (( $? > 0 )); then exit 1 ; fi

		#------------------------------------------------------------------
		# link_series.sh: Creates symbolic links to each series directory.
		#+ Again, this is done just for the sake of ease in finding the 
		#+ directories. The links will correspond to the Siemens Series 
		#+ number of each series, padded to two digits.
		#------------------------------------------------------------------
		printf "\nCalling script to create links to series directories...\n\n"
		$preproc/util/link_series.sh ${rawPath}
			# If script exited with error, exit this script too
			if (( $? > 0 )); then exit 1 ; fi

		#------------------------------------------------------------------
		# convert_dcmto4dfp.sh: Converts files from dicom 3.0 to 4dfp. Uses
		#+ medcon to stack and convert to analyze format; then uses 
		#+ analyzeto4dfp to get files into 4dfp format. Will also get rid 
		#+ of all those symbolic links we set up in the raw dir(s). 
		#------------------------------------------------------------------
		printf "\nCalling script to convert files to 4dfp...\n\n"
		$preproc/util/convert_dcmto4dfp.sh ${varsFile}
			# If script exited with error, exit this script too
			if (( $? > 0 )); then exit 1 ; fi
	fi

popd > /dev/null

#----------------------------------------------------------------
# PREPROCESS FILES
#----------------------------------------------------------------
pushd ${subjPath} > /dev/null
	#----------------------------------------------------------------
	# PROCESS FUNC DATA
	#----------------------------------------------------------------
	printf "\n---------------------------------"
	printf "\nPreliminary Functional Processing"
	printf "\n---------------------------------"
	printf "\nUser $USER on $(date)\n"

	for (( i=0; i<${#funcSeries[@]}; i++ )); do
		pushd ${funcLabel[$i]} >/dev/null

		printf "\n--------------------"
		printf "\nProcessing series $( printf %02d ${funcSeries[$i]} )"
		printf "\n--------------------\n"
		
		#----------------------------------------------------------------
		# frame_align_4dfp: corrects asynchronous slice acquisition. 
		#+ Temporally aligns each slice to the temporal midpoint of each
		#+ frame.
		#----------------------------------------------------------------
		printf "\nFRAME ALIGNMENT...\n"
		# if slice total is even and institution is MRRC, use -N option to indicate even-odd interleaving.
		if [ $(( $zDim % 2 )) -eq 0 -a x$sInst == "xMRRC" ]; then
			printf "\n>>SLICE NUMBER IS EVEN AND DATA WERE COLLECTED AT MRRC<<"
			printf "\n>>ADDING -N OPTION TO FRAME_ALIGN_4DFP<<"
			printf "\n>>ASSUMING INTERLEAVING IS EVEN-ODD INSTEAD OF NORMAL ODD-EVEN<<\n\n"
			frame_align_4dfp ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]} $skip -N -TR_vol $TR -TR_slc $slcTR -d 0
		else
			frame_align_4dfp ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]} $skip -TR_vol $TR -TR_slc $slcTR -d 0
		fi
		
		printf "\nFrame alignment complete for series $( printf %02d ${funcSeries[$i]} )\n"
		
		#----------------------------------------------------------------
		# deband_4dfp: corrects systematic intensity differences across
		#+ slices in a frame caused by time of acquisition within a TR
		#+ (i.e., even or odd slices are acquired before or after the
		#+ other across a TR--briefly, interleaved acquisition).
		#----------------------------------------------------------------
		printf "\nDEBANDING...\n"
		deband_4dfp -n$skip ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}"_faln"
		
		printf "\nDebanding complete for ${funcSeries[$i]}\n"
		
		#----------------------------------------------------------------
		# Clean-up Directories
		#----------------------------------------------------------------
		if (( $econ > 2 )); then
			printf "\nCleaning up ./${funcLabel[$i]}/\n"
			
			# Get rid of raw 4dfp files
			printf "\nRemoving raw 4dfps...\n"
			/bin/rm ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}.4dfp.*
			
			# Get rid of frame-aligned stacks
			if (( $econ > 3 )); then
				printf "\nRemoving faln 4dfps...\n"
				/bin/rm ${subjPath}/${funcLabel[$i]}/${subjID}"_"${funcLabel[$i]}"_faln".4dfp.*
			fi
		fi
		popd >/dev/null
	done
	
	printf "\nPreliminary functional processing finished"
	printf "\nUser $USER on $(date)\n\n"
	
	#----------------------------------------------------------------
	# CHECK FOR EPI2ATL, PROCESS ACCORDINGLY
	#----------------------------------------------------------------
	printf "\nChecking atlas transform settings...\n\n"
	if (( $epi2atl != 2 )); then
		if [ -e ${subjID}"_xr3d".lst ]; then 
			/bin/rm ${subjID}"_xr3d".lst | touch ${subjID}"_xr3d".lst
		fi
		if [ -e ${subjID}"_anat".lst ]; then 
			/bin/rm ${subjID}"_anat".lst | touch ${subjID}"_anat".lst
		fi
		
		for (( i=0; i<${#funcSeries[@]}; i++ )); do
			printf "${funcLabel[$i]}/${subjID}_${funcLabel[$i]}_faln_dbnd\n" >> ${subjID}"_xr3d".lst
			printf "${funcLabel[$i]}/${subjID}_${funcLabel[$i]}_faln_dbnd_xr3d_norm\n" 1 >> ${subjID}"_anat".lst
		done
		cat ${subjID}"_xr3d".lst

		#----------------------------------------------------------------
		# cross_realign3d_4dfp: within and across run motion correction
		#+ algorithm. Uses rigid-body translations and rotations to 
		#+ adjust each frame to a reference frame.
		#----------------------------------------------------------------
		printf "\nCross-realignment of functionals\nUser $USER on $(date)\n\n"
		cross_realign3d_4dfp -n${skip} -qv${shouldNorm} -l${subjID}"_xr3d".lst
		
		#----------------------------------------------------------------
		# Compute and Apply Normalization
		#----------------------------------------------------------------
		printf "\nComputing normalization...\nUser $USER on $(date)\n\n"
		for (( i=0; i<${#funcSeries[@]}; i++ )); do
			pushd ${funcLabel[$i]} > /dev/null

			#----------------------------------------------------------------
			# normalize_4dfp: calculates values needed to achieve a mode
			#+ of 1000 across the run.
			#----------------------------------------------------------------
			normalize_4dfp ${subjID}"_"${funcLabel[$i]}"_faln_dbnd_r3d_avg"

			if (( $econ > 4 )) && (( $epi2atl == 0 )); then
				printf "\nCleaning up ${funcLabel[$i]}...\n"
				printf "\nRemoving faln_dbnd 4dfps\n\n"
				/bin/rm ${subjID}"_"${funcLabel[$i]}"_faln_dbnd".4dfp.*
			fi
			popd > /dev/null
		done

		printf "\nApplying normalization..."
		printf "\nUser $USER on $(date)\n\n"
		for (( i=0; i<${#funcSeries[@]}; i++ )); do
			pushd ${funcLabel[$i]} > /dev/null

			file="${subjID}_${funcLabel[$i]}_faln_dbnd_r3d_avg_norm.4dfp".img.rec
			f=1.0
			if [ -e $file ]; then f=$(head $file | awk '/original/{print 1000/$NF}'); fi
			
			#----------------------------------------------------------------
			# scale_4dfp: applies the normalization values calculated by 
			#+ normalize_4dfp
			#----------------------------------------------------------------
			scale_4dfp ${subjID}_${funcLabel[$i]}_faln_dbnd_xr3d $f -anorm
			
			printf "\nRemoving extraneous files...\n\n"
			/bin/rm "${subjID}_${funcLabel[$i]}_faln_dbnd_xr3d".4dfp.*
			
			popd > /dev/null
		done

		#----------------------------------------------------------------
		# Movement Analysis
		#----------------------------------------------------------------
		printf "\n-------------------------"
		printf "\nProcessing Movement Files"
		printf "\n-------------------------"
		printf "\nUser $USER on $(date)\n\n"
		
		if [ ! -d movement ]; then mkdir movement ; fi
		for (( i=0; i<${#funcSeries[@]}; i++ )); do
			printf "Analyzing ${funcLabel[$i]}...\n\n"
			frunpad=$(printf "%02d" ${frun})
			mat2dat ${funcLabel[$i]}/*"_xr3d".mat -RD -n${skip}
			/bin/mv ${funcLabel[$i]}/*_xr3d.dat movement/${subjID}_func$( printf %02d $(( $i + 1)) )_faln_dbnd_xr3d.dat
			/bin/mv ${funcLabel[$i]}/*_xr3d.ddat movement/${subjID}_func$( printf %02d $(( $i + 1)) )_faln_dbnd_xr3d.ddat
			/bin/mv ${funcLabel[$i]}/*_xr3d.rdat movement/${subjID}_func$( printf %02d $(( $i + 1)) )_faln_dbnd_xr3d.rdat
		done

		#----------------------------------------------------------------
		# ATLAS TRANSFORMATION
		#----------------------------------------------------------------
		printf "\n--------------------"
		printf "\nAtlas transformation"
		printf "\n--------------------"
		printf "\nUser $USER on $(date)\n\n"

		printf "\nMaking EPI average image for atlas transform...\n\n"
		
		cat ${subjID}_anat.lst

		paste_4dfp -p1 ${subjID}_anat.lst ${subjID}_anat_ave
		
		ifh2hdr -r2000 ${subjID}_anat_ave
		4dfptoanalyze ${subjID}_anat_ave
		
		if [ ! -d atlas ]; then mkdir atlas ; fi
		mv ${subjID}_anat* atlas
		
		pushd atlas >/dev/null
		
		#-------------------------------------------------------------------------------
		# Make MPRAGE average for atlas transform (if you acquired more than one mprage)
		#-------------------------------------------------------------------------------
			printf "\nCreate MPRAGE average for atlas transform\n"
			if [ ! -n $t1Series ]; then 
				printf "\nERROR: no mprage\n"
				exit 1
			fi
			mprave=${subjID}_mprage_n${#t1Series[@]}
			mprlst=()
			mprlst=($mprlst ${subjID}_mprage)

			#----------------------------------------------------------------
			# avgmpr_4dfp: averages multiple mprages together
			#----------------------------------------------------------------
			printf "\nComputing average MPRAGE to target transform"
			printf "\nUser $USER on $(date)\n\n"
			if [ $isCustomAtlas == false ]; then
				avgmpr_4dfp $mprlst $mprave $target useold
			else
				avgmpr_4dfp $mprlst $mprave -T${targetPath}/${target} useold
			fi
		 
		#----------------------------------------------------------------
		# Compute atlas transform
		#----------------------------------------------------------------
		printf "\nComputing atlas transform\n"
		# If t2Series is not empty... (i.e., exists)
		if [ ! -z $t2Series ]; then
			#----------------------------------------------------------------
			# epi2t2w2mpr2atl1_4dfp: wrapper script for the atlas transform.
			#+ Uses several programs to calculate how to transform your
			#+ average EPI/BOLD image to your T2 structural (same contrast,
			#+ same space, low->high res image); then calculates the 
			#+ transform for your T2 structural to your T1 structural
			#+ (different contrast, same space, high->high res image). Finally,
			#+ calculates the transform from T1 structural to atlas space T1 
			#+ structural (data space->atlas space)
			#----------------------------------------------------------------
			if [ $isCustomAtlas == false ] ; then
				epi2t2w2mpr2atl1_4dfp ${subjID}_anat_ave ${subjID}_t2w ${subjID}_mprage useold $target
			else
				epi2t2w2mpr2atl1_4dfp ${subjID}_anat_ave ${subjID}_t2w ${subjID}_mprage useold -T${targetPath}/${target}
			fi
		else
			#----------------------------------------------------------------
			# epi2mpr2atlv_4dfp: If no T2 structural is provided, this
			#+ script will skip the T2 transform, calculating the transform
			#+ directly from EPI/BOLD to T1 structural. Again, this is a
			#+ wrapper script for several other programs.
			#----------------------------------------------------------------
			if [ $isCustomAtlas == false ] ; then
				epi2mpr2atlv_4dfp ${subjID}_anat_ave ${subjID}_mprage useold $target
			else
				epi2mpr2atlv_4dfp ${subjID}_anat_ave ${subjID}_mprage useold -T${targetPath}/${target}
			fi
		fi
		
		if (( $econ > 5 )); then
			printf "\nCleaning up...\n"
			printf "Removing extra mpr files...\n\n"
			/bin/rm ${subjID}"_mprage"?.4dfp.* $mprave*.4dfp.* ${subjID}"_mpr"?.4dfp.*
		fi
		if [ -e *t4% ]; then /bin/rm *t4% ; fi
		
		#----------------------------------------------------------------------
		# Create atlas-transformed EPI/func average image in 222 and 333 space
		#----------------------------------------------------------------------
		printf "Creating atlas-transformed EPI averages in 222 and 333 space\n\n"
		
		#----------------------------------------------------------------
		# t4img_4dfp: applies the above calculated transform(s) to the
		#+ anat_ave (EPI/BOLD average) and to the T2, so that we have
		#+ nice atlas-space images of each.
		#----------------------------------------------------------------
		t4file=${subjID}_anat_ave_to_${target}_t4
			t4img_4dfp $t4file ${subjID}_anat_ave ${subjID}_anat_ave_t88_222 -O222 ; printf "\n"
			t4img_4dfp $t4file ${subjID}_anat_ave ${subjID}_anat_ave_t88_333 -O333 ; printf "\n"
			ifh2hdr -r2000 ${subjID}_anat_ave_t88_222 ; printf "\n"
			ifh2hdr -r2000 ${subjID}_anat_ave_t88_333 ; printf "\n"
		# If t2Series is not empty (i.e., exists)
		if [ ! -z $t2Series ]; then
			t4file=${subjID}_t2w_to_${target}_t4
			t4img_4dfp $t4file ${subjID}_t2w ${subjID}_t2w_t88_222 -O222 ; printf "\n"
			t4img_4dfp $t4file ${subjID}_t2w ${subjID}_t2w_t88_333 -O333 ; printf "\n"
			ifh2hdr -r2000 ${subjID}_t2w_t88_222 ; printf "\n"
			ifh2hdr -r2000 ${subjID}_t2w_t88_222 ; printf "\n"
		fi
		popd >/dev/null
		
		if (( $epi2atl == 0 )); then
			# if this is 0, that means we don't transform the stacks to
			#+ atlas space, so exit here...
			printf "\nPreprocessing finished by User $USER on $(date)\n"
			popd > /dev/null
			printf "\nCurrent directory: $(pwd)\n"
			exit 0
		fi
	else
		printf "\nSkipping to xreg, transform, resample step...\n\n"
	fi
	#----------------------------------------------------------------------
	# Make cross-realigned, atlas-transformed, resampled BOLD 4dfp stacks 
	#----------------------------------------------------------------------
	# Continue if epi2atl is 1 or 2 (finished if 0)
	for (( i=0; i<${#funcSeries[@]}; i++ )); do
		pushd ${funcLabel[$i]} >/dev/null
		file=${subjID}"_"${funcLabel[$i]}"_faln_dbnd_r3d_avg_norm".4dfp.img.rec
		f=1.0
		if [ -e $file ]; then f=$(head $file | awk '/original/{print 1000/$NF}'); fi

		#----------------------------------------------------------------
		# t4_xr3d_4dfp: takes the frame-aligned and debanded stacks
		#+ straight to atlas space in one resampling. So, this motion
		#+ corrects and applies the atlas transform in one step. Note,
		#+ this will produce some pretty huge file sizes...
		#----------------------------------------------------------------
		t4_xr3d_4dfp ../atlas/${subjID}_anat_ave_to_${target}_t4 ${subjID}_${funcLabel[$i]}_faln_dbnd -axr3d_atl -v${shouldNorm} -c$f -O${atlSpace}

		if (( $econ > 6 )); then
			printf "\nCleaning up:\n\tremoving xr3d_norm 4dfps\n"
			/bin/rm ${subjID}"_"${funcLabel[$i]}"_faln_dbnd_xr3d_norm".4dfp.*
		fi
		popd >/dev/null
	done

printf "\nPreprocessing finished by User $USER on $(date)\n"

popd >/dev/null
printf "\n\nCurrent directory: $(pwd)\n"


exit 0
