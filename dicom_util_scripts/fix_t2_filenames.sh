#! /bin/bash
#
# This script will rename your t2 dicoms by slice number instead of by time of acquisition, since they are interleaved...

# 10.06.2010 jt
#
#

echo "T2 series number?"
read t2series

echo ""
echo "Raw data dir containing t2 series directory? (1 or 2)"
echo "1) Current working directory"
echo "2) enter full path manually"
echo "3) cancel; exit script"
read intype
	if [ $intype = 1 ]; then
		indir=`pwd`
	elif [ $intype = 2 ]; then
		"Enter full path of raw data directory:"
		read indir
	elif [ $intype = 3 ]; then
		exit 0
	else
		echo "error; exiting..."
		exit -1
	fi
	
	pushd ${indir}/\[$t2series\]*
	
	nfiles=`find . -type f | wc -l`
	echo "Renaming $nfiles files"
	
	printdcm=/usr/local/pkg/nis/bin/print_dicom_tags
	
	filelist=(`ls -1 *.dcm`)
	
	ifile=0
	while [ $ifile -lt $nfiles ]; do
		dcmtag=(`$printdcm ${filelist[$ifile]}`)
		patid=${dcmtag[4]}
		nseries=${dcmtag[25]}
		nac=${dcmtag[31]}
		
		nac=`printf %03d $nac`
		nseries=`printf %02d $nseries`
		
		mv ${filelist[$ifile]} ${patid}_${nseries}_${nac}.dcm
		
		let ifile=ifile+1
	done

	popd
	echo "done"
exit 0
