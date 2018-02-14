/*=========================================================================

  Program.....: nic_dicom_rename
  Author......: Max Novelli
  Institution.: University of Pittsburgh
  Date........: 2010/12/21


  Based on:
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
// 
// Include dcmtk package headers
//
//#include "dcmtk/config/osconfig.h"
#include "dcmtk/config/cfunix.h"
//#include "osconfig.h"
//#include "dcmtk/ofstd/oftypes.h"
#include "dcmtk/ofstd/oftypes.h"
#include "dcfilefo.h"
#include "dcdict.h"
#include "dcdicent.h"

#include <errno.h>
#include <unistd.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <dirent.h>
//#include <stddef.h>
//#include <stdio.h>

// number of properties used
#define dcDcmTags 9

// print time stamp
void timestamp(char* timestamp)
{
  // timestamp time
  time_t ltime;
  // timestamp formatted
  char cTimestamp[256];
  // get current calendar time
  ltime=time(NULL); 
  // format timestamp
  sprintf(timestamp, "%s",asctime( localtime(&ltime) ) );
  return;
}

// main program
int main( int argc, char* argv[] )
{

  // array of structure for group and element of dcm tag
  struct sDcmTag {
    Uint16 g;
    Uint16 e;
  } asDcmTags [dcDcmTags];
  // tags we are interested in extracting
  // Requesting Physician
  #define dcRequestingPhysician 0 
  asDcmTags[0].g = 0x0032;
  asDcmTags[0].e = 0x1032;
  // Patient Id
  #define cdPatientId 1
  asDcmTags[1].g = 0x0010;
  asDcmTags[1].e = 0x0020;
  // Series Number
  #define dcSeriesNumber 2
  asDcmTags[2].g = 0x0020;
  asDcmTags[2].e = 0x0011;
  // Acquisition Number
  #define dcAcquisitionNumber 3
  asDcmTags[3].g = 0x0020;
  asDcmTags[3].e = 0x0012;
  // Instance Number
  #define dcInstanceNumber 4
  asDcmTags[4].g = 0x0020;
  asDcmTags[4].e = 0x0013;
  // Study Date
  #define dcStudyDate 5
  asDcmTags[5].g = 0x0008;
  asDcmTags[5].e = 0x0020;
  // Study Time
  #define dcStudyTime 6
  asDcmTags[6].g = 0x0008;
  asDcmTags[6].e = 0x0030;
  // Study Description
  #define dcStudyDescription 7
  asDcmTags[7].g = 0x0008;
  asDcmTags[7].e = 0x1030;
  // Protocol Name
  #define dcProtocolName 8
  asDcmTags[8].g = 0x0018;
  asDcmTags[8].e = 0x1030;


//// main program
//int main( int argc, char* argv[] )
//{

  // variable used for input options and arguments
  extern int errno;
  extern char *optarg;
  extern int optind, opterr, optopt;
  char option;

  // Stat structure
  struct stat sStat;
  // integer function result
  int iRes;

  // directory stream
  DIR *dsDir;
  // directory entry
  struct dirent *sEntry; 

  // generic counter/index
  int i;
  // dry run flag
  int bDry;
  // help flag
  int bHelp;
  // copy flag
  int bCopy;
  // temporary string;
  char cTemp[256];
  // source directory
  char *cSourceDir;
  // destination directory
  char *cDestDir;
  // destination path
  char cPath[256];
  // destination file name
  char cFile[256];
  // move command
  char cCommand[256];
  // timestamp
  char cTimestamp[256];

  // dcmtk dicom file object
  DcmFileFormat *oDcmFile;
  // array of dcmtk objects DcmTags with 
  DcmTag *aoDcmTags[dcDcmTags];
  // array of dcmtk tag keys objects
  DcmTagKey *aoDcmTagKeys[dcDcmTags];
  DcmTagKey oTagName;
  // array of dcmtk tag keys values objects
  OFString *aoDcmTagValues[dcDcmTags];
  // results from dcmtk operation
  OFCondition oCond;
  // consdotion string, used to define custom OFCondition
  OFConditionString *oCondSt;

  // counters
  // files checked counter
  int iFilesChecked;
  // files transferred counter
  int iFilesTransferred;

  // begin timestamp
  timestamp(cTimestamp);
  std::cout << "Begin Run. Time: " << cTimestamp;
  /** parse command-line params */
  bDry = 0;
  bHelp = 0;
  bCopy = 0;
  while ((option = getopt(argc, argv, "hdc")) != EOF) {
    switch (option) {
      case 'd':
        bDry = 1;
        break;
      case 'c':
        bCopy = 1;
        break;
      case 'h':
      case '?':
      default:
        bHelp = 1;
    }
  }
  std::cout << std::endl << "Program Options" << std::endl;
  sprintf(cTemp,"%d",bDry);
  std::cout << " - Dry Run .: " << cTemp << std::endl;
  sprintf(cTemp,"%d",bHelp);
  std::cout << " - Help ....: " << cTemp << std::endl;
  sprintf(cTemp,"%d",bCopy);
  std::cout << " - Copy ....: " << cTemp << std::endl << std::endl;
  

  /** print help if required or wrong number of argument **/
  if ((argc - optind != 2) || bHelp) {
    std::cout << std::endl << argv[0] << std::endl;
    std::cout << "Renames raw dicom files and move them from source to destination folder." << std::endl;
    std::cout << "The renamed files are placed in subfolder with the following path:" << std::endl;
    std::cout << " > <RequestingPhysician>/<Study_Id>/<Series>/" << std::endl;
    std::cout << "where StudyId is: " << std::endl;
    std::cout << " > <Study_date><Study_Time>" << std::endl << std::endl;
    std::cout << "The file will be renamed as follows:" << std::endl;
    std::cout << " > <Study_Id>_<Series><Acquisition><Instance>.dcm" << std::endl; 
    std::cout << std::endl;
    std::cout << "<Series> will be padded with zeros to 2 digits," << std::endl;
    std::cout << "<Acquisition> will padded to 3 digits, " << std::endl;
    std::cout << "<Instance> will padded to 4 digits." << std::endl;
    std::cout << std::endl;
    std::cout << "This program has been developed specifically for NIC at University of Pittsburgh" << std::endl;
    std::cout << std::endl;
    std::cout << "IMPORTANT: File contents will not be altered." << std::endl;
    std::cout << std::endl;
    std::cout << "Usage: " << std::endl;
    std::cout << "  " << argv[0] << " [-h -d] <source dir> <destination dir> " << std::endl ;
    std::cout << "    -d : Dry run, do not perform any action. Just provide loggin output" << std::endl;
    std::cout << "    -c : Copy files instead of moving them. Preserve originals." << std::endl;
    std::cout << "    -h : Help, print this message and quit" << std::endl;
    std::cout <<  std::endl;
    exit(10);
  }

  /* save source and destination folders */ 
  cSourceDir = argv[optind++];
  cDestDir = argv[optind++];

  std::cout << std::endl << "Program Arguments" << std::endl;
  std::cout << " - Source Dir ......: " << cSourceDir << std::endl;
  std::cout << " - Destination Dir .: " << cDestDir << std::endl << std::endl;

  // load dictionary
  // global data dictionary is created with the include
  // check if global data dictionary is loaded
  std::cout << "Loading Dicom Dictionary: " << std::endl;
  if ( !dcmDataDict.isDictionaryLoaded() ) {   
    std::cout << "Warning: no data dictionary loaded, "
              << "check environment variable: "
              << DCM_DICT_ENVIRONMENT_VARIABLE;
    exit(11);
  }
  // lock global data dictionary, so we can check tags requested
  const DcmDataDictionary& coGlobalDataDict = dcmDataDict.rdlock();
  const DcmDictEntry *coDicent;
  std::cout << " ...Done" << std::endl;

  std::cout << "Verifying Dicom Tags: " << std::endl;
  // Load tag ids in dcmtk object
  for ( int i=0; i<dcDcmTags; i++) {
    // instatiate dcmtk object
    aoDcmTags[i] = new DcmTag(asDcmTags[i].g,asDcmTags[i].e);
    // check if it is a real tag accordingly to dictionary  
    oTagName = aoDcmTags[i]->getXTag();
    coDicent = coGlobalDataDict.findEntry(oTagName,NULL);
    if( coDicent == NULL ) {
      std::cout << "Error: unrecognised tag name: '" << oTagName << "'" << endl;
      dcmDataDict.unlock();
      exit(100+i);
    }
    /* saved for later */
    aoDcmTagKeys[i] = new DcmTagKey(coDicent->getKey());
  }
  // release database
  dcmDataDict.unlock();
  std::cout << " ...Done" << std::endl;

  // check source folder
  std::cout << "Checking source folder... " << std::endl;
  iRes = stat(cSourceDir, &sStat);
  if ( iRes != 0 || ! S_ISDIR(sStat.st_mode) ) {
    std::cout << "Error: source directory " << cSourceDir << "does not exist" <<  std::endl;
    exit(12);
  }
  // open source folder
  dsDir = opendir(cSourceDir);
  if ( dsDir == NULL ) {
    std::cout << "Error: source directory " << cSourceDir << " not accessible" << std::endl;
    exit(13);  
  }
  std::cout << " ...Done" << std::endl;


  // check destination folder
  std::cout << "Checking destination folder... " << std::endl;
  iRes = stat(cDestDir, &sStat);
  if ( iRes != 0 ) {
    // the folder does not exists
    std::cout << " - Destination folder does not exists." << std::endl;
    // create the folder
    std::cout << " - Creating destination folder" << std::endl;
    if ( mkdir(cDestDir, 0775) ) {
      // folder creation not successful
      std::cout << "Error: output directory " << cDestDir << " does not exist and was not created" <<  std::endl;
      exit(14);
    }
    std::cout << "   ...Done" << std::endl;
  }
  // folder/file exists
  // checking if it is folder
  if ( !S_ISDIR(sStat.st_mode) ) {
    std::cout << "Error: a file name with same name as destination directory " << cDestDir << " already exists" << std::endl;
    exit(15);
  }
  std::cout << " ...Done" << std::endl;

  // define max bytes read when loading dicom object
  const Uint32 NDR_MaxReadLength = 256;
  // instiantiate dcmtk dicom object
  oDcmFile = new DcmFileFormat();

  // initilize file counters
  iFilesChecked = 0;
  iFilesTransferred = 0;

  // loop over al the entries in source folder
  while ( sEntry = readdir(dsDir) ) {
    std::cout << " - -----------------------------------" << std::endl;
    // file begin timestamp
    timestamp(cTimestamp);
    std::cout << " - Begin file. Time: " << cTimestamp;
    std::cout << " - Processing entry ..:" << sEntry->d_name << std::endl;

    // full path name
    string sPath (cSourceDir);
    sPath += "/";
    sPath += sEntry->d_name;
    std::cout << " - Full path name ....:" << sPath.c_str() << std::endl;
    
    // checking if is a dir
    std::cout << " - Checking if it is a folder..." << std::endl;
    iRes = stat(sPath.c_str(), &sStat);
    if ( iRes != 0 ) {
      std::cout << " - Error checking. Skipping" << std::endl;
      continue;
    }
    if ( S_ISDIR(sStat.st_mode) ) {
      std::cout << " - It's a directory. Skipping"  << std::endl;
      continue;
    }
    std::cout << " - ...Done: OK." << std::endl;
    
    // update number of file checked
    iFilesChecked++;
    // load dicom file
    std::cout << " - Loading DCM header..." << std::endl;
    oCond = oDcmFile->loadFile(sPath.c_str(), EXS_Unknown, EGL_noChange, NDR_MaxReadLength, ERM_fileOnly);
    // check outcome from loadin file
    if ( oCond.bad() ) {
      // error
      std::cout << " - Error loading file: " << sPath.c_str() << ". Skipping!!!" << std::endl;
      
    } else {
      std::cout << " - ...Done. OK." << std::endl;
    
      // load dicom tag keys values
      std::cout << " - Loading DCM tags values..." << std::endl;
      for ( int i=0; i<dcDcmTags; i++) {
        
        // initialize value
        aoDcmTagValues[i] = new OFString("Unknown");
        
        // convert everything to string
        //std::cout << "DCM Tag EVR ..: " << aoDcmTags[i]->getEVR() << std::endl;
        //std::cout << "DCM Tag Name .: " << aoDcmTags[i]->getTagName() << std::endl;
        switch (aoDcmTags[i]->getEVR()) {
          case EVR_PN:
          case EVR_SH:
          case EVR_IS:
          case EVR_DA:
          case EVR_TM:
          case EVR_LO:
            oCond = oDcmFile->getDataset()->findAndGetOFString(*aoDcmTagKeys[i],*aoDcmTagValues[i]);
            //std::cout << "DCM tag found" << std::endl;
            break;
          default:
            //oCond = new OFCondition( new OfConditionConst(100,100,OF_error,"Unknown VR") );
            oCond = * new OFCondition( new OFConditionString(1000,1000,OF_error,"Unknown VR") );
            //std::cout << "DCM tag not found" << std::endl;
            break;
        }
        //std::cout << "DCM Tag Value : " << aoDcmTagValues[i]->c_str() << std::endl;
        if ( oCond.bad() ) {
          std::cout << "- Bad Value for property " << aoDcmTags[i]->getTagName() << std::endl;
          std::cout << "- Skipping this file" << std::endl;
          continue;
        }
        // logs tags value
        sprintf(cTemp,"%04x:%04x",aoDcmTags[i]->getGTag(),aoDcmTags[i]->getETag());
        std::cout << " - - Dicom Tag : " << 
          aoDcmTags[i]->getTagName() << 
          "(" << cTemp << ") => " << 
          aoDcmTagValues[i]->c_str() << std::endl;
      }
      std::cout << " - ...Done: OK." << std::endl;
      
      // build path and file name
      std::cout << " - Building path names... " << std::endl;
      // get study time
      string sStudyTime (aoDcmTagValues[dcStudyTime]->c_str());
      // start building study id
      string sStudyId (aoDcmTagValues[dcStudyDate]->c_str());
      // prepare study time: remove fraction
      sStudyTime.erase(sStudyTime.find("."));
      // append study time to study date to complete study id
      // StudyId = Year(4)Month(2)Day(2)Hour(2)Minutes(2)Seconds(2)
      sStudyId += sStudyTime;

      // path to folder    
      sprintf(cPath,
        "%s/%s/%s/%02d",
        cDestDir,
        aoDcmTagValues[dcRequestingPhysician]->c_str(), 
        sStudyId.c_str(), 
        atoi(aoDcmTagValues[dcSeriesNumber]->c_str()) );
      // file name with relative path 
      // PI/StudyId/Series/StudyId_Series(3)Acquisition(3)Instance(4).dcm"
      sprintf(cFile,
        "%s/%s_%02d%03d%05d.dcm", 
        cPath, 
        sStudyId.c_str(), 
        atoi(aoDcmTagValues[dcSeriesNumber]->c_str()), 
        atoi(aoDcmTagValues[dcAcquisitionNumber]->c_str()),
        atoi(aoDcmTagValues[dcInstanceNumber]->c_str()) );
      std::cout << " - ...Done: OK" << std::endl;
      
      // print log
      std::cout << " - Source file name .......:" << sPath.c_str() << std::endl;
      std::cout << " - Destination file name ..:" << cFile << std::endl;
        
      // make container folder if necessary
      std::cout << " - Checking destination folder..." << std::endl;
      iRes = stat(cPath, &sStat);
      if ( iRes != 0 ) {
        // file does not exists, we are going to create it
        std::cout << " - Creating new folder: " << cPath << std::endl;
        // generate and run mkdir command
        sprintf(cCommand, "mkdir -p %s", cPath);
        std::cout << " - Executing command: " << cCommand << std::endl; 
        // skip if dry run
        iRes = 0;
        if ( !bDry ) {
          iRes = system(cCommand);
        }
        if ( iRes != 0 ) {
          // we were not able to create the new folder
          // send message and skip to next
          std::cout << " - Error: Unable to create new folder. Skipping file" <<  std::endl;
          continue;
        }
        std::cout << " - Folder " << cPath << " created." << std::endl;
      }
      else if ( ! S_ISDIR(sStat.st_mode) ) {
        // the path exists and it's not a folder
        std::cout << " - Error: dicom destination folder " << cPath << " exist, but it's not a folder. Skipping file " << cFile <<  std::endl;
        continue;
      }
      std::cout << " ...Done" << std::endl;

      // check if destination output file does not already exist
      std::cout << " - Checking destination file..." << std::endl;
      iRes = stat(cFile, &sStat);
      if ( iRes == 0 ) {
        // file does exists, we skip it
        std::cout << " - Error: Destination file already exists. Skipping file" << cFile << std::endl;
        continue;
      }
      std::cout << " - ...Done: OK." << std::endl;

      // creating command
      if ( bCopy ) {
        std::cout << " - Renaming/Coping file." << std::endl;
        // generate and run copy command
        sprintf(cCommand, "cp %s %s", sPath.c_str(), cFile);      
      }
      else {
        std::cout << " - Renaming/Moving file." << std::endl;
        // generate and run copy command
        sprintf(cCommand, "mv %s %s", sPath.c_str(), cFile);
      }
      std::cout << " - Executing command: " << cCommand << std::endl;
      iRes = 0;
      if ( !bDry ) {
        iRes = system(cCommand);
      } 
      // check results  
      if (iRes != 0) {
		std::cout << " - Command not successfull. File not moved or renamed"  << std::endl;
      } else {
        std::cout << " - File renamed and moved/copied. Command successfull" << std::endl;
    	iFilesTransferred++;
      }
    }
    
  }
  // close dir
  closedir(dsDir);
  // print summary
  std::cout << "----------------------" << std::endl;
  // end timestamp
  timestamp(cTimestamp);
  std::cout << "End Run. Time: " << cTimestamp;
  // summary
  std::cout << "Files Processed ....: " << iFilesChecked << std::endl;
  std::cout << "Files Transferred ..: " << iFilesTransferred <<  std::endl;

  return EXIT_SUCCESS;

}
