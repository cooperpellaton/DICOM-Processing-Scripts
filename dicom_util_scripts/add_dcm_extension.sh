#! /bin/bash
# this script adds the .dcm extension to the NIC's extension-less dicoms
#
# 09.30.2010 jt
#

echo "What is the input type?"
echo "1) Directory of sub-dirs containing files"
echo "2) Files in current directory"
echo "3) Changed my mind...I'm not ready for this"
echo ""
echo "Please make sure you only renaming dicom images..."
echo ""
read intype
	if [ $intype = 3 ]; then
		exit 0
	elif [ $intype = 2 ]; then
		echo "adding .dcm extension to files in "`pwd`
		echo ""
		find . -type f | xargs -i mv '{}' '{}'.dcm
		echo "finished"
		echo ""
	elif [ $intype = 1 ]; then
		echo "Enter the full path to directory you want to process:
		(i.e., this dir contains sub-dirs with all the images to fix)"
		read dirproc
		echo "finding directories..."
		ndir=`find $dirproc/* -type d -prune | wc -l`
		dirlist=(`ls -d1 $dirproc/*`)
		echo "renaming files in $ndir directories"
		pushd $dirproc
		x=0
		while [ $x -lt $ndir ]; do
			pushd ${dirlist[$x]}
			echo "adding .dcm extension to files in "${dirlist[$x]}
			echo ""
			find . -type f | xargs -i mv '{}' '{}'.dcm
			echo "finished"
			echo ""
			popd
			let x=x+1
		done
		popd
		echo "done"
	fi
exit 0
