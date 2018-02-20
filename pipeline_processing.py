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
import pprint

___author__ = "Cooper Pellaton"
__copyright__ = "Copyright 2018, Georgia Institute of Technology"
__credits__ = "Wheeler Lab"
__license__ = "MIT"
__version__ = "0.0.0"
__maintainer__ = "Cooper Pellaton"
__email__ = "pellaton@gatech.edu"
__status__ "Alpha"

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
# - MP RAGE or T1 Structural
# - T2 Weighted Structural
# - Functional Series Numbers and Labels
# - Image Dimensions
# - Skip and Evict
# - Normalization

# Constants.
path = ""
subj_id = ""
atlas = ""
default_vals = []

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
        path = args
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


def get_image_dimensions():
    # walk through the dicoms to find the image dimensions listed inside


def find_atlas():


def create_defaults():
        # assemble all constants into a default struct


def update_defaults():
        # check the defaults with the user and update as appropriate


if __name__ == "__main__":
    # execute only if run as a script
    main()
