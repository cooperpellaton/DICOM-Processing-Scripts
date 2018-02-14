/*=========================================================================

  Program:	rename_dicom_files.cxx
  Author:	Kate Fissell, University of Pittsburgh
  Date:		August, 2006

  The rename_dicom_files.cxx program was derived from the ITK-2.8.1
  example program Examples/IO/DicomSeriesReadPrintTags.cxx which 
  has the following header:

  Program:   Insight Segmentation & Registration Toolkit
  Module:    $RCSfile: DicomImageReadPrintTags.cxx,v $
  Language:  C++
  Date:      $Date: 2006/03/06 22:38:22 $
  Version:   $Revision: 1.15 $

  Copyright (c) Insight Software Consortium. All rights reserved.
  See ITKCopyright.txt or http://www.itk.org/HTML/Copyright.htm for details.

     This software is distributed WITHOUT ANY WARRANTY; without even 
     the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR 
     PURPOSE.  See the above copyright notices for more information.

=========================================================================*/
#if defined(_MSC_VER)
#pragma warning ( disable : 4786 )
#endif

#ifdef __BORLANDC__
#define ITK_LEAN_AND_MEAN
#endif

// 
// The headers of the main classes involved in this example are specified
// below. They include the image file reader, the GDCM image IO object, the
// Meta data dictionary and its entry element the Meta data object. 
//
#include "itkImageFileReader.h"
#include "itkGDCMImageIO.h"
#include "itkMetaDataDictionary.h"
#include "itkMetaDataObject.h"
#include "imgio_utils.h"
#include <errno.h>
#include <unistd.h>



// tags we are interested in extracting
std::string series_tagkey =	"0020|0011";
std::string acq_tagkey =	"0020|0012";
std::string inst_tagkey =	"0020|0013";



int main( int argc, char* argv[] )
{

extern int errno;
extern char *optarg;
extern int optind, opterr, optopt;

char *indir, *outdir;
char **filesarray;
char filename[2048];
char new_filename[2048];
char sys_command[4096];
char series_name[10];
char series_dir[2048];
int numfiles;
int c, ret, i, count;
int listflag, helpflag;

std::string series_labelId;
std::string acq_labelId;
std::string inst_labelId;
std::string series_value;
std::string acq_value;
std::string inst_value;


	/** parse command-line params */
    listflag = 0;
    helpflag = 0;
    while ((c = getopt(argc, argv, "hl")) != EOF)
	switch (c) {
	case 'l':
	    listflag = 1;
	    break;

	case 'h':
	case '?':
	default:
	    helpflag = 1;
	}


    if ((argc - optind != 2) || helpflag) {
    	std::cerr << std::endl << argv[0] << " renames a directory of DICOM slice files, setting the output name to <series id>-<volume-id>-<slice-id>.dcm" << std::endl;
	std::cerr << "Renamed output files are placed in series directories that are created in the designated output directory." << std::endl;
	std::cerr << "Note that this program uses the system copy command (cp) to simply rename files.  File contents should not be altered." << std::endl << std::endl;

	std::cerr << "Usage: " << argv[0] << " [-h -l] <input dir> <output dir> " << std::endl ;
	std::cerr << "\t\t-l\tlist flag, just list new file names, do not copy/rename" << std::endl;
	std::cerr << "\t\t-h\thelp flag, list this message and quit" << std::endl;
	std::cerr <<  std::endl;
	return EXIT_FAILURE;
    }


indir = argv[optind++];
outdir = argv[optind++];


// Check tag IDs
if( ! itk::GDCMImageIO::GetLabelFromTag( series_tagkey, series_labelId ) ) {
    std::cerr << "Error: DICOM series tag " << series_tagkey << " is not valid."  <<  std::endl;
    exit(1);
}
if( ! itk::GDCMImageIO::GetLabelFromTag( acq_tagkey, acq_labelId ) ) {
    std::cerr << "Error: DICOM acquisition tag " << acq_tagkey << " is not valid."  <<  std::endl;
    exit(1);
}
if( ! itk::GDCMImageIO::GetLabelFromTag( inst_tagkey, inst_labelId ) ) {
    std::cerr << "Error: DICOM instance tag " << inst_tagkey << " is not valid."  <<  std::endl;
    exit(1);
}


/// check input and output directories 
if (!isDir(indir)) {
    std::cerr << "Error: input directory " << indir << "does not exist" <<  std::endl;
    exit(1);
}

if (!isDir(outdir)) {

    if (mkdir(outdir, 0775)) {

    	std::cerr << "Error: output directory " << outdir << " does not exist and could not be created" <<  std::endl;
	perror("mkdir");
    	exit(1);
	}
}


// get all the files in the input directory
filesarray = getDirEntries(indir, 0, numfiles);
if (numfiles == 0) {
    	std::cerr << "Error: no files found in input directory " << indir <<  std::endl;
	exit(1);
}
else {
    	std::cerr << "Processing " << numfiles << " entries from directory " << indir <<  std::endl;
}



// 
//  We instantiate the type to be used for storing the image once it is read
//  into memory.
//
  typedef signed short       PixelType;
  const unsigned int         Dimension = 2;
  typedef itk::Image< PixelType, Dimension >      ImageType;


// 
// Using the image type as template parameter we instantiate the type of the
// image file reader and construct one instance of it.
//
  typedef itk::ImageFileReader< ImageType >     ReaderType;
  ReaderType::Pointer reader = ReaderType::New();


// 
// The GDCM image IO type is declared and used for constructing one image IO
// object.
//
  typedef itk::GDCMImageIO       ImageIOType;
  ImageIOType::Pointer dicomIO = ImageIOType::New();


// 
// Here we override the gdcm default value of 0xfff with a value of 0xffff
// to allow the loading of long binary stream in the DICOM file.
// This is particulray usefull when reading the private tag: 0029,1010
// from Siemens as it allows to completely specify the imaging parameters
//
  dicomIO->SetMaxSizeLoadEntry(0xffff);

// 
// We connect the ImageIO object to the reader
//
  reader->SetImageIO( dicomIO );



// loop over list of files to rename
for (i=0,count=0; i<numfiles; i++) {

  /// skip . and .. entries 
  if (!strncmp(filesarray[i], ".", 1))
	continue;
  if (!strncmp(filesarray[i], "..", 2))
	continue;

  sprintf(filename, "%s/%s", indir, filesarray[i]);

  if (isDir(filename)) {
	std::cerr << "Skipping directory " << filename << std::endl;
	continue;
  }

  if ( (i%100 == 0) && (!listflag) )
	std::cerr << "Processed " << i << " files..." << std::endl;

  ////std::cerr << "File " << i << " " << filename << std::endl;
  reader->SetFileName( filename );

  try
    {
    reader->Update();
    }
  catch (itk::ExceptionObject &ex)
    {
    std::cerr << ex << std::endl;
    return EXIT_FAILURE;
    }


    //// get the 3 tags we need
    if( !dicomIO->GetValueFromTag(series_tagkey, series_value) ) {
	std::cerr << "Error reading DICOM series tag from file " << filename << std::endl;
	exit(1);
    }
    if( !dicomIO->GetValueFromTag(acq_tagkey, acq_value) ) {
	std::cerr << "Error reading DICOM acquisition tag from file " << filename << std::endl;
	exit(1);
    }
    if( !dicomIO->GetValueFromTag(inst_tagkey, inst_value) ) {
	std::cerr << "Error reading DICOM instance tag from file " << filename << std::endl;
	exit(1);
    }


    /// make the new names 
    sprintf(series_dir, "%s/%03d", outdir, atoi(series_value.c_str()));
    sprintf(new_filename, "%s/%03d-%05d-%08d.dcm", series_dir, atoi(series_value.c_str()), atoi(acq_value.c_str()), atoi(inst_value.c_str()));
    /////std::cerr << "Rename " << filename << " " << new_filename << std::endl;

    
    ///// make series dir if necessary
    if ( (!isDir(series_dir)) && (!listflag) ) {

    	if (mkdir(series_dir, 0775)) {

    		std::cerr << "Error: output series directory " << series_dir << " does not exist and could not be created" <<  std::endl;
		perror("mkdir");
    		exit(1);
	}
	std::cerr << "Made series directory " << series_dir << std::endl;
    }


    ///// check that output file does not already exist
    if ( (isFile(new_filename)) && (!listflag) ) {
		std::cerr << "ERROR: wanted to rename " << filename << " as " << new_filename << " but that file already exists." << std::endl;
		exit(1);
    }


    /// print old and new name
    if (listflag) {
	std::cout << filename << "\t" << new_filename << std::endl;
    }

    /// generate and run copy command
    else {
    	sprintf(sys_command, "cp %s %s", filename, new_filename);
    	ret = system(sys_command);
    	if (ret == -1) {
		std::cerr << "ERROR: could not launch copy command: " << sys_command << std::endl;
		exit(1);
    	} 
    }


    count++;

}   // loop over list of files

  std::cerr << "Processed " << count << " files  from directory " << indir <<  std::endl;

  return EXIT_SUCCESS;

}

