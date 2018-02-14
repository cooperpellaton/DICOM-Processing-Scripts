#! /bin/bash
#
version=1.2
#
# 01.28.2011 jt - 1.2	changed lots of stuff. Added initial data checks to make
#			sure everything will work. Made script more general so
#			that it will find the data directories, instead of having
#			to change it every time the directory structures change,
#			etc.
#
# 11.18.2010 jt - NIC file names changed again. Fixed.
#
# 09.29.2010 jt - rescripting in bash; should be faster, easier, and less confusing.
#
#
#
# Change 'go' to 0 for test mode...
go=1

# Subject ID
patid=fh019

# main study directory
studypath=/data/data2/FH

# raw data directory is called (within study dir/subj ID):
rawdir=raw

# Target atlas:
target=711-2B

# If this is a custom atlas, where is it? (otherwise, just leave blank)
targetpath=

# MP-RAGE series number:
mprage=(4)

#### Orientation of mprage flag? Grab from dcms? see line 217
####

# T2w series number:
tse=(3)

# Assign labels for each functional run (place corresponding series number below)
func_label=(  run1 run2 run3 run4 run5 run6 run7 loc )
func_series=( 5    6    7    8    9    10   11   12  )

# TR in seconds
TR_vol=1.5

# TR time per slice in seconds (0 assumes even spacing)
# (Typically, this will be 0)
TR_slc=0

# X dimension for functional volumes (standard = 64)
XDIM=64

# Y dimension for functional volumes (standard = 64)
YDIM=64

# Number of slices for functionals (Z dimension)
ZDIM=29

# Number of pre-functional frames to ignore while processing (NOTE: does not evict scans, just ignores them for preproc!)
skip=0

# When to transform volumes to atlas space
#	0: leave processed time series in EPI/data space
#	1: transform to 222 space (i.e., transform entire bold stack to atlas space in a single resampling)
#	2: proceed directly to t4_xr3d_4dfp.
epi2atl=0

	# which atlas space do you want to use (111, 222, 333)
	atlspace=222


# Enable per-frame volume intensity equalization (normalize...) (1=yes, 0 for no operation)
normode=1

econ=5		# econ = 0			no compression or intermediate file removal
		# econ > 1			rm anz copies of images
		# econ > 2			+ rm raw bold 4dfp stacks
		# econ > 3			+ rm frame aligned stacks
		# econ > 4 & epi2atl = 0	+ rm debanded stacks
		# econ > 5			+ rm raw mprage
		# econ > 6			+ rm x-reg 3d and normalized stacks

#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#

program=$0; program=$program:t

#~~~~~~~~~~~~~~~~~~#
# Setup and Checks #
#~~~~~~~~~~~~~~~~~~#

# Count sequence series
if [ -n $mprage ] ; then nmprage=${#mprage[@]} ; fi
nruns=${#func_series[@]}
if [ -n $tse ] ; then ntse=${#tse[@]} ; fi

# set path to medcon
medcon=/usr/local/pkg/xmedcon/bin/medcon

#Check to see if # of func series equals # of func labels
if [ ${#func_series[@]} != ${#func_label[@]} ]; then
	echo "Number of func labels does not equal number of func series numbers."
	exit 1
fi

#Check to see if atlas exists and find it
echo "looking for atlas..."
if [ $target == "711-2B" -o $target == "711-2C" -o $target == "711-2O" ] ; then
	echo "Atlas recognized."

	customatl=no
	if [ -e $REFDIR/${target}.4dfp.img ] ; then
		echo "Atlas located."
	elif [ -e $REFDIR/${target}_111.4dfp.img ] ; then
		echo "Atlas located."
	else
		echo "Atlas was not found. Exiting..."
		exit 1
	fi
else
	echo "Atlas not recognized. Searching in targetpath..."
	customatl=yes
	if [ -e `find ${targetpath}/${target}*4dfp.img | grep 111` ] ; then
		echo "Atlas located."
		atlfilenm=`find ${targetpath}/${target}*4dfp.img | grep 111 | cut -d "." -f1`
	elif  [ -e ${targetpath}/${target}.4dfp.img ] ; then
		echo "Atlas located."
		atlfilenm=${targetpath}/${target}
	else
		echo "Atlas was not found. Exiting..."
		echo "N.B., Try adding a symbolic link in ${targetpath} that"
		echo "points to your 111-space target atlas and name the link"
		echo "something like 711-2R.4dfp.img (i.e., leave off anything"
		echo "after the echo atlas name). And try again..."
		exit 1
	fi
fi

# alias full subject dir path and full raw dir path
inpath=${studypath}/${patid}/${rawdir}
subjpath=${studypath}/${patid}

#Find raw data and check to see that everything looks good.
	# Use medcon libraries to read dicom tags...
pushd $inpath > /dev/null

	# make temp file to list all dirs in raw data dir
	nrawdir=`find ./* -maxdepth 0 -type d | wc -l`
	rawdirlist=( `ls -1d ./* | cut -d "/" -f2` )
	i=0

	# make symbolic links to series dirs by reading dicom tag info. #
	#	this makes it easier later when processing series dirs  #
	while [ $i -lt $nrawdir ] ; do
		pushd "${rawdirlist[$i]}" > /dev/null
		if [ `find *.dcm -type f | wc -l` -gt 1 ] ; then
			echo "`find *.dcm -type f | wc -l` .dcm files found."
		else
			echo "No dicom files found. This might not be true,"
			echo "though. They might just need .dcm extensions."
			echo "In which case, run this cmd in the appropriate"
			echo "directory and re-run:"
			echo "find . -type f | xargs -i '{}' '{}'.dcm "
		fi
		$medcon -f `find *.dcm -type f | head -1` -fb-dicom -s \
		-d | grep "Series Number" | awk '{print $4}' > series.tmp

		seriestmp=`cat series.tmp`
		seriestmp=`printf "%02d" $seriestmp`
		\rm series.tmp
		popd > /dev/null

		# make link in inpath	#
		ln -s ${rawdirlist[$i]} ${inpath}/${seriestmp}
		
		let i=i+1
	done

#	# Check to make sure filenames for T2 series look ok.	#
#	# (if acquired interleaved, filenames may be numbered	#
#	# incorrectly).						#
#	if [ -n $tse ] ; then echo "no T2 series...skipping check"
#	else	
#		pushd `printf "%02d" $tse`
#		
#	fi

popd > /dev/null

echo "User "`whoami`" on "`date`

pushd ${subjpath} > /dev/null
#~~~~~~~~~~~~~~~~~~~~~#
# CONVERT DCM TO 4DFP #
#~~~~~~~~~~~~~~~~~~~~~#
	echo ""
	#~~~~~~~~~~~~~~~~~#
	# PROCESS MP-RAGE #
	#~~~~~~~~~~~~~~~~~#
	echo "---------------"
	echo "Convert MP-RAGE"
	echo "---------------"

	if [ ! -d ${subjpath}/atlas ]; then mkdir ${subjpath}/atlas ; fi
	
	# zero-pad mprage series number
	mprage=(`printf %02d ${mprage}`)
	echo ""
	mpr_name=`printf ${inpath}/${mprage}`
	echo "Raw MP-RAGE series dir: "$mpr_name
	echo ""

	echo "converting dicom to analyze format:"
	echo "	stacking images to one file..."
	echo ""
	echo "${medcon} -f \`ls -1 ${mpr_name}/*.dcm | sort -t- -n -k3\` -c anlz -sqr -pad -fb-dicom -noprefix -stack3d -s -o atlas/${patid}_mprage"
	echo ""

	${medcon} -f `ls -1 ${inpath}/${mprage}/*.dcm | sort -t- -n -k3` -c anlz -sqr -pad -fb-dicom -noprefix -stack3d -s -o atlas/${patid}_mprage

	echo ""
	echo "converting analyze format to 4dfp..."
	echo "analyzeto4dfp -xy atlas/${patid}_mprage"
	echo ""
		
	# -xy flag flips orientation for atlas transform
	# note: this is for sagittal-acquired mprage
	# (use -yz for axial mprage)
	analyzeto4dfp -xy atlas/${patid}_mprage

	#~~~~~~~~~~~~~~~~#
	# Clean-up files #
	#~~~~~~~~~~~~~~~~#
		echo "Cleaning up ./atlas/"
		echo ""
		
		# remove anz copies of images	
			if [ $econ -gt 1 ]; then
			echo "	removing anz copies..."
			echo ""
			/bin/rm atlas/${patid}_mprage.???
			fi

	#~~~~~~~~~~~~~#
	# CONVERT T2s #
	#~~~~~~~~~~~~~#
	echo "-----------"
	echo "Convert T2s"
	echo "-----------"
	echo "User "`whoami`" on "`date`
	if [ -n $tse ]; then
		if [ ! -d ${subjpath}/atlas ]; then mkdir ${subjpath}/atlas ; fi

		tse=`printf "%02d" ${tse}`
		echo ""
		t2w_name=`printf ${inpath}/${tse}`
		echo "Raw T2w series dir: "$t2w_name
		echo ""

		echo "converting dicom to analyze format:"
		echo "	stacking images to one file..."
		echo ""
		echo "${medcon} -f \`ls -1 ${t2w_name}/*.dcm | sort -t- -n -k3\` -c anlz -sqr -pad -fb-dicom -noprefix -stack3d -s -o atlas/${patid}_t2w"
		echo ""
		${medcon} -f `ls -1 ${inpath}/${tse}/*.dcm | sort -t- -n -k3` -c anlz -sqr -pad -fb-dicom -noprefix -stack3d -s -o atlas/${patid}_t2w

		echo ""
		echo "converting analyze format to 4dfp..."
		echo "analyzeto4dfp atlas/${patid}_t2w"
		echo ""
		
		#Flip for axial-acquired T2s (so R=R and stack Inf to Sup)
		analyzeto4dfp -yz atlas/${patid}_t2w

		#~~~~~~~~~~~~~~~~#
		# Clean-up files #
		#~~~~~~~~~~~~~~~~#
		echo "Cleaning up ./atlas/"
		echo ""
		
		# remove anz copies of images	
			if [ $econ -gt 1 ]; then
			echo "	removing anz copies..."
			echo ""
			/bin/rm atlas/${patid}_t2w.???
			fi
	fi

	#~~~~~~~~~~~~~~~~~~~#
	# CONVERT FUNC DATA #
	#~~~~~~~~~~~~~~~~~~~#
	nrun=0
	while [ $nrun -lt ${nruns} ]; do
	
		echo "-------------------------"
		echo "Converting BOLD series "${func_series[$nrun]}
		echo "-------------------------"
	
		if [ ! -d ${func_label[$nrun]} ]; then mkdir ${func_label[$nrun]} ; fi

		func_series=`printf "%02d" ${func_series[$nrun]}`
		funcname=`printf ${inpath}/${func_series[$nrun]}`	
		echo ""
		echo "Current func series dir: "$funcname

		echo ""
		echo "converting dicom to analyze format:"
		echo "	stacking images to one file..."
		echo ""
		echo "${medcon} -f \`ls -1 ${funcname}/*.dcm | sort -t- -n -k2,2\` -c anlz -stack4d -fb-dicom -s -fmosaic=${XDIM}x${YDIM}x${ZDIM} -noprefix -o ${subjpath}/${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}"
		echo ""

		${medcon} -f `ls -1 ${inpath}/${func_series}/*.dcm | sort -t- -n -k2,2` -c anlz -stack4d -fb-dicom -s -fmosaic=${XDIM}x${YDIM}x${ZDIM} -noprefix -o ${subjpath}/${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}
	
		# NOTE func data are acquired in radiological orientation (L is R), anzto4dfp is set to flip images to neuropsych orientation (R is R) via the -yz flag.
		echo ""
		echo "converting analyze to 4dfp..."
		echo "analyzeto4dfp -yz ${subjpath}/${func_label[$nrun]}/${patid}_${func_label[$nrun]}"
		echo ""

		# -yz is a flip for axial-acquired functionals
		# Required for atlas transform
		analyzeto4dfp -yz ${subjpath}/${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}
	
	#~~~~~~~~~~~~~~~~#
	# Clean-up files #
	#~~~~~~~~~~~~~~~~#
		echo ""
		echo "cleaning up ./${func_label[$nrun]}/"
		echo ""
		
		# remove anz copies of images
			if [ $econ -gt 1 ]; then
			echo "	removing anz copies..."
			echo ""
			/bin/rm ${subjpath}/${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}.???
			fi

		let nrun=nrun+1
	done
	
	# Remove symbolic links in $inpath
	pushd $inpath > /dev/null
		find ./* -maxdepth 1 -type l | xargs rm 
	popd > /dev/null
	

	echo "Conversions finished."
	echo "User "`whoami`" on "`date`
	echo ""
	echo "Process func data..."

#~~~~~~~~~~~~~~~~~~~#
# PROCESS FUNC DATA #
#~~~~~~~~~~~~~~~~~~~#
	err=0
	nrun=0
	while [ ${nrun} -lt ${nruns} ]; do
		pushd ${func_label[$nrun]}

		echo "-------------------------"
		echo "Processing BOLD series "${func_series[$nrun]}
		echo "-------------------------"
		
		echo ""
		echo "Frame alignment..."
		echo ""
		echo "frame_align_4dfp ${subjpath}/${func_label[$nrun]}/${patid}_${func_label[$nrun]} $skip -TR_vol $TR_vol
		-TR_slc $TR_slc -d 0"
		echo ""
		
		if [ $go -gt 0 ]; then
		
		frame_align_4dfp ${subjpath}/${func_label[$nrun]}/${patid}"_"${func_label[$nrun]} $skip -TR_vol $TR_vol -TR_slc $TR_slc -d 0
		
		echo ""
		echo "Frame alignment complete for "${func_series[$nrun]}
		
		echo ""
		echo "Debanding..."
		echo ""
		echo "deband_4dfp -n$skip ${subjpath}/${func_label[$nrun]}/${patid}_${func_label[$nrun]}_faln"
		echo ""
		
		deband_4dfp -n$skip ${subjpath}/${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}"_faln"
		
		echo ""
		echo "Debanding complete for "${func_series[$nrun]}
		fi
		
		#~~~~~~~~~~~~~~~~#
		# Clean-up files #
		#~~~~~~~~~~~~~~~~#
			echo ""
			echo "cleaning up ./${func_label[$nrun]}/"
			echo ""
			
			if [ $econ -gt 2 ]; then
				if [ $go -gt 0 ]; then
				echo "removing raw 4dfps"
				echo ""
				/bin/rm ${subjpath}/${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}.4dfp.*
				fi
			fi
			if [ $econ -gt 3 ]; then
				if [ $go -gt 0 ]; then
				echo "	removing faln 4dfps"
				echo ""
				/bin/rm ${subjpath}/${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}"_faln".4dfp.*
				fi
			fi
			popd
		let nrun=nrun+1
	done
	
	if [ $err -gt 0 ]; then
		echo $program": one or more BOLD runs failed preliminary processing"
		exit 1
	fi
	echo ""
	echo "Preliminary BOLD processing finished"
	echo "User "`whoami`" on "`date`
	echo ""
	
#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
# CHECK FOR EPI2ATL, PROCESS ACCORDINGLY #
#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
	echo "Checking atlas transform settings..."
	echo ""
	if [ $epi2atl -ne 2 ]; then
		if [ -e ${patid}"_xr3d".lst ]; then /bin/rm ${patid}"_xr3d".lst | touch ${patid}"_xr3d".lst ; fi
		if [ -e ${patid}"_anat".lst ]; then /bin/rm ${patid}"_anat".lst | touch ${patid}"_anat".lst ; fi
		
		nrun=0
		while [ $nrun -lt $nruns ]; do
			echo ${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}"_faln_dbnd" >> ${patid}"_xr3d".lst
			echo ${func_label[$nrun]}/${patid}"_"${func_label[$nrun]}"_faln_dbnd_xr3d_norm" 1 >> ${patid}"_anat".lst
			let nrun=nrun+1
		done
		cat ${patid}"_xr3d".lst

		#~~~~~~~~~~~~~~~~~~~#
		# Cross-realignment #
		#~~~~~~~~~~~~~~~~~~~#
		echo ""
		echo "Cross realignment of functionals"
		echo "User "`whoami`" on "`date`
		echo ""
		if [ $go -gt 0 ]; then
			cross_realign3d_4dfp -n${skip} -qv${normode} -l${patid}"_xr3d".lst
		fi
		echo ""
		
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		# compute and apply normalization to mode=1000 #
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		echo "Compute normalization..."
		echo "User "`whoami`" on "`date`
		echo ""
		nrun=0
		while [ $nrun -lt $nruns ]; do
			pushd ${func_label[$nrun]} > /dev/null
			echo "normalize_4dfp ${patid}_${func_label[$nrun]}_faln_dbnd_r3d_avg"
			if [ $go -gt 0 ]; then 
				normalize_4dfp ${patid}"_"${func_label[$nrun]}"_faln_dbnd_r3d_avg"
				echo ""
				echo "Cleaning up ${func_label[$nrun]}..."
				echo ""
				if [ $econ -gt 4 && $epi2atl -eq 0 ]; then
					echo "	removing faln_dbnd 4dfps"
					echo ""
					/bin/rm ${patid}"_"${func_label[$nrun]}"_faln_dbnd".4dfp.*
				fi
			fi
			popd > /dev/null
			let nrun=nrun+1
		done
		echo ""
		echo "Applying normalization..."
		echo "User "`whoami`" on "`date`
		echo ""
		nrun=0
		while [ $nrun -lt $nruns ]; do
			pushd ${func_label[$nrun]} > /dev/null
			file=${patid}"_"${func_label[$nrun]}"_faln_dbnd_r3d_avg_norm".4dfp.img.rec
			f=1.0
			if [ -e $file ]; then f=`head $file | awk '/original/{print 1000/$NF}'` ; fi
			echo "scale_4dfp ${patid}_${func_label[$nrun]}_faln_dbnd_xr3d $f -anorm"
			echo ""
			if [ $go -gt 0 ]; then scale_4dfp ${patid}_${func_label[$nrun]}_faln_dbnd_xr3d $f -anorm ; fi
			echo "removing extraneous files..."
			echo ""
			if [ $go -gt 0 ]; then /bin/rm ${patid}"_"${func_label[$nrun]}"_faln_dbnd_xr3d".4dfp.* ; fi
			popd > /dev/null
			let nrun=nrun+1
		done

		#~~~~~~~~~~~~~~~~~~~#
		# Movement Analysis #
		#~~~~~~~~~~~~~~~~~~~#
		echo "~~~~~~~~~~~~~~~~~~~"
		echo " Movement Analysis "
		echo "~~~~~~~~~~~~~~~~~~~"
		echo ""
		echo "User "`whoami`" on "`date`
		echo ""
		
		if [ ! -d movement ]; then mkdir movement ; fi
		nrun=0
		frun=1
		while [ $nrun -lt $nruns ]; do
			echo "Analyzing ${func_label[$nrun]}..."
			echo ""
			if [ $go -gt 0 ]; then 
				mat2dat ${func_label[$nrun]}/*"_xr3d".mat -RD -n${skip}
				/bin/mv ${func_label[$nrun]}/*_xr3d.dat movement/${patid}_func${frun}_faln_dbnd_xr3d.dat
				/bin/mv ${func_label[$nrun]}/*_xr3d.ddat movement/${patid}_func${frun}_faln_dbnd_xr3d.ddat
				/bin/mv ${func_label[$nrun]}/*_xr3d.rdat movement/${patid}_func${frun}_faln_dbnd_xr3d.rdat
			fi
			let nrun=nrun+1
			let frun=frun+1
		done
		
		
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		# Make EPI/func average image for atlas transform #
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		echo ""
		echo "Make EPI/func average image for atlas transform..."
		echo ""
		
		cat ${patid}_anat.lst
		echo "grab first frame from each func run and average"
		echo ""
		if [ $go -gt 0 ]; then paste_4dfp -p1 ${patid}_anat.lst ${patid}_anat_ave ; fi
		echo "convert to analyze format..."
		echo ""
		if [ $go -gt 0 ]; then ifh2hdr -r2000 ${patid}_anat_ave ; fi
		if [ $go -gt 0 ]; then 4dfptoanalyze ${patid}_anat_ave ; fi
		
		
		#~~~~~~~~~~~~~~~~~~~~~~~~~#
		# Compute atlas transform #
		#~~~~~~~~~~~~~~~~~~~~~~~~~#
		echo ""
		echo "compute atlas transform..."
		echo ""
		if [ ! -d atlas ]; then mkdir atlas ; fi
		if [ $go -gt 0 ]; then mv ${patid}_anat* atlas ; fi
		
		pushd atlas > /dev/null
		
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		# Make MPRAGE average for atlas transform (if you acquired more than one mprage) #
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		echo "create MPRAGE average for atlas transform"
		if [ -z $nmprage ]; then 
			echo "no mprage"
			exit 1
		fi
		mprave=${patid}_mprage_n${nmpr}
		mprlst=()
		nrun=0
		mprlst=($mprlst ${patid}_mprage)
		echo ""
		echo "Computing average MPRAGE to target transform"
		echo "User "`whoami`" on "`date`
		echo ""
		if [ $go -gt 0 ]; then 
			if [ $customatl == no ]; then
				avgmpr_4dfp $mprlst $mprave $target useold
			else
				avgmpr_4dfp $mprlst $mprave -T${targetpath}/${target} useold
			fi
		fi
		 
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		# Compute T2 atlas transform #
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		echo ""
		echo "Computing atlas transform"
		echo "User "`whoami`" on "`date`
		echo ""
		
		if [ ${ntse} -gt 0 ]; then
			if [ $go -gt 0 ]; then 
				if [ $customatl == no ] ; then
					epi2t2w2mpr2atl1_4dfp ${patid}_anat_ave ${patid}_t2w ${patid}_mprage useold $target
				else
					epi2t2w2mpr2atl1_4dfp ${patid}_anat_ave ${patid}_t2w ${patid}_mprage useold -T${targetpath}/${target}
				fi
			fi
		else
			if [ $go -gt 0 ]; then 
				if [ $customatl == no ] ; then
					epi2mpr2atlv_4dfp ${patid}_anat_ave ${patid}_mprage useold $target
				else
					epi2mpr2atlv_4dfp ${patid}_anat_ave ${patid}_mprage useold -T${targetpath}/${target}
				fi
			fi
		fi
		
		echo ""
		echo "Cleaning up..."
		echo ""
		if [ $econ -lt 5 ]; then
			echo "removing extra mpr files..."
			if [ $go -gt 0 ]; then /bin/rm ${patid}"_mprage"?.4dfp.* $mprave*.4dfp.* ${patid}"_mpr"?.4dfp.* ; fi
		fi
		if [ -e *t4% ]; then /bin/rm *t4% ; fi
		
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		# Create atlas-transformed EPI/func average image in 222 and 333 space #
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		echo ""
		echo "create atlas-transformed EPI averages in 222 and 333 space"
		echo ""
		t4file=${patid}_anat_ave_to_${target}_t4
			if [ $go -gt 0 ]; then
				t4img_4dfp $t4file ${patid}_anat_ave ${patid}_anat_ave_t88_222 -O222
				t4img_4dfp $t4file ${patid}_anat_ave ${patid}_anat_ave_t88_333 -O333
				ifh2hdr -r2000 ${patid}_anat_ave_t88_222
				ifh2hdr -r2000 ${patid}_anat_ave_t88_333
			fi
		t4file=${patid}_t2w_to_${target}_t4
			if [ $go -gt 0 ]; then
				t4img_4dfp $t4file ${patid}_t2w ${patid}_t2w_t88_222 -O222
				t4img_4dfp $t4file ${patid}_t2w ${patid}_t2w_t88_333 -O333
				ifh2hdr -r2000 ${patid}_t2w_t88_222
				ifh2hdr -r2000 ${patid}_t2w_t88_222
			fi
		popd
		
			if [ $epi2atl -eq 0 ]; then exit 0 ; fi
	else
		echo ""
		echo "Skipping to xreg, transform, resample step..."
		echo ""
	fi			
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		# Make cross-realigned, atlas-transformed, resampled BOLD 4dfp stacks #
		#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~#
		# continue if epi2atl is 1 or 2 (finished if 0)
		nrun=0
		while [ $nrun -lt $nruns ]; do
			pushd ${func_label[$nrun]}
			file=${patid}"_"${func_label[$nrun]}"_faln_dbnd_r3d_avg_norm".4dfp.img.rec
			f=1.0
			if [ -e $file ]; then f=`head $file | awk '/original/{print 1000/$NF}'`
			fi
			echo ""
			echo "t4_xr3d_4dfp ../atlas/${patid}_anat_ave_to_${target}_t4 ${patid}_${func_label[$nrun]}_faln_dbnd
			-axr3d_atl -v${normode} -c$f -O${atlspace}"
			if [ $go -gt 0 ]; then
			t4_xr3d_4dfp ../atlas/${patid}_anat_ave_to_${target}_t4 ${patid}_${func_label[$nrun]}_faln_dbnd -axr3d_atl -v${normode} -c$f -O${atlspace}
			fi
			
			if [ $econ -gt 6 ]; then
				echo "removing xr3d_norm 4dfps"
				if [ $go -gt 0 ] ; then
				/bin/rm ${patid}"_"${func_label[$nrun]}"_faln_dbnd_xr3d_norm".4dfp.*
				fi
			fi
			popd > /dev/null
		let nrun=nrun+1
		done
echo "Preprocessing finished by User "`whoami`" on "`date`
popd > /dev/null
echo ""
echo "Current directory: "`pwd`
echo ""

exit 0
