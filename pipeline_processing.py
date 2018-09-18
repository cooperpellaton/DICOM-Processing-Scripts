#!/usr/bin/env python
"""Script to process DCMs.

This script is intended to take a series of raw dicom files as obtained from a scanner and produce
4DFP images that can then be processed in software like FIDL and BOB.

This will be elaborated on in later time.
"""

import argparse
import json
import logging
import os
import pydicom
import pprint

___author__ = "Cooper Pellaton"
__copyright__ = "Copyright 2018, Georgia Institute of Technology"
__credits__ = "Wheeler Lab"
__license__ = "MIT"
__version__ = "0.0.0"
__maintainer__ = "Cooper Pellaton"
__email__ = "pellaton@gatech.edu"
__status__ = "Alpha"

# Parser setup.
parser = argparse.ArgumentParser(description='DCM processing utility.')
parser.add_argument('path', metavar='S', type=str,
                    help='the path of the directory to process')
args = parser.parse_args()

# Vars:
# - Study Directory
# - Subject ID.
# - Raw Data Directory
# - Target Atlas
# - MP RAGE or T1 Structural -- Can be discerned from the file name in the raw directory.
# - T2 Weighted Structural
# - Functional Series Numbers and Labels -- Can be discerned from the file numbers in the raw directory.
# - Image Dimensions
# - Skip and Evict
# - Normalization


"""
/home/mw/wheeler_shared/elyse/FHWord/S201/raw/localizer_1/

/home/mw/wheeler_shared/elyse/FHWord/S201/raw/fMRI_physio_1_localizer_8/

/home/mw/wheeler_shared/elyse/FHWord/S201/raw/fMRI_physio_2_14/


Series 1 is the Siemens localizer, which is just 3 images, one in each 
plane. We don't use these images, they are for the MR tech.

Series 8 is the first acquisition of Elyse's functional localizer. 
Series 14 is the first run of Elyse's functional task.
"""

# Constants.
path = ""
subj_id = ""
atlas = ""
default_vals = []
vals = []

# Step 1. Create a vars file by taking information from the user.
# Step 2. Carry out preprocessing.
# Step 3. Quality check.
# Step 4. Concatenation.

medcon = "/usr/local/xmedcon/bin/medcon"


def main():
    # Set the path.
    # Can be passed as '.' or /path/to/subj/directory
    if args == '.':
        path = os.getcwd()
    else:
        path = os.path.abspath(args)
    # now delegate the work
    pre_process()
    update_defaults()


def pre_process():
    try:
        get_dir_name()
        get_image_dimensions()
        find_atlas()
        '''do the rest'''
        create_defaults()
    except:
        logging.info("Error: ", sys.exc_info()[0])
        raise


def get_dir_name():
    path = os.path.dirname(path)

def get_image_dimensions(dataset):
    # still need to figure out slice thickness
    ###########################################
    # walk through the dicoms to find the image dimensions listed inside
    # need to find TR and slices for functional
    #   default dimensions = 64x64
    #   default number slices for functionals = 38
    #   default TR = 2.0
    #   EX: Reptition time 2250?
    #   repitition time is in milliseconds: http://mriquestions.com/tr-and-te.html
    for element in dataset:
        if element.VR == "SQ":
            for sequence_item in element.value:
                if sequence_item == "Rows":
                    vals.insert("X Dimension", sequence_item)
                elif sequence_item == "Columns":
                    vals.insert("Y Dimension", sequence_item)
                elif sequence_item == "Repetition Time":
                    # now we have the TR time
                    vals.insert("TR", sequence_item)
                elif sequence_item == "Slice Location":
                    # this is TR per slice, should be 0.
                    vals.insert("TR_spacing", sequence_item)
                    # assert error here
                    raise
                
def find_atlas():
    # check if in path
    # if not then use default from vars file


def create_defaults():
        # assemble all constants into a default struct

def update_defaults():
        # check the defaults with the user and update as appropriate

if __name__ == "__main__":
    # execute only if run as a script
    main()
