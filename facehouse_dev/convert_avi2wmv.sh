#!/bin/bash
#
# convert_avi2wmv.sh
#
# 03.28.2011
# 
# Josh Tremel
# University of Pittsburgh

program=$0
program=$program:t

MINARGS=2

if [ $# -lt MINARGS ]; then
	echo -e "Script to batch convert uncompressed .avi videos to .wmv. This "
	echo -e "uses the ffmpeg utility to convert the files. The script will "
	echo -e "process an entire directory <indir> of .avi files and output a "
	echo -e "directory <outdir> of .wmv files."
	echo -e ""
	echo -e "USAGE:"
	echo -e "./convert_avi2wmv.sh <indir> <outdir>"
	exit 1
fi

# Parse cmd line
indir=$1
outdir=$2

# Make <outdir> if it doesn't exist
if [ ! -d $outdir ]; then mkdir $outdir ; fi

# Make sure <outdir> is empty, exit if not
pushd $outdir >/dev/null
	if [ `find . -type f | wc -l` -gt 0 ]; then
		echo "outdir is not empty... exiting."
		popd
		exit 1
	else
popd >/dev/null
	fi

pushd $indir >/dev/null
	# Count avi files in <indir>
	nfiles=`find *.avi -type f | wc -l`
	
	# List files
	files=(`ls -1 *.avi`)

	echo "Found $nfiles files in $indir"
	
# Convert
filecount=1
errfiles=0
iter=0
while [ $iter -lt $nfiles ]; do
	infile=${files[$iter]}
	filenm=`basename $infile .avi`
	outfile=${filenm}.wmv
	
	ffmpeg -sameq -i $infile -s 300x300 -vcodec wmv2 ../$outdir/$outfile
	
	if [ "${filecount:(-1)}" == '0' ]; then echo "$filecount files processed" ; fi
	
	# Advance counters
	(( iter++ ))
	(( filecount++ ))
done

(( filecount-- ))
echo ""
echo "${filecount} files processed. Please double check for errors."
echo ""

popd >/dev/null

echo "Script finished by `whoami` on `date`"

exit 0
