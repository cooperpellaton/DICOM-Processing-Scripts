#!/usr/bin/env python
"""Script to process DCMs.

This script is intended to take a series of raw dicom files as 
obtained from a scanner and produce
4DFP images that can then be processed in software like FIDL and BOB.

This will be elaborated on in later time.
"""

import argparse
import json
import logging
import os
import pydicom
import pprint

__author__ = "Cooper Pellaton"
__copyright__ = "Copyright 2018, Georgia Institute of Technology"
__credits__ = "Wheeler Lab"
__license__ = "MIT"
__version__ = "0.0.1"
__maintainer__ = "Cooper Pellaton"
__email__ = "pellaton@gatech.edu"
__status__ = "Alpha"

# Parser setup.
parser = argparse.ArgumentParser(description='DCM processing utility.')
parser.add_argument('path', metavar='S', type=str,
                    help='the path of the directory to process')
args = parser.parse_args()

"""
Sample data:
/home/mw/wheeler_shared/elyse/FHWord/S201/raw/localizer_1/
/home/mw/wheeler_shared/elyse/FHWord/S201/raw/fMRI_physio_1_localizer_8/
/home/mw/wheeler_shared/elyse/FHWord/S201/raw/fMRI_physio_2_14/


Series 1 is the Siemens localizer, which is just 3 images, one in each 
plane. We don't use these images, they are for the MR tech.

Series 8 is the first acquisition of Elyse's functional localizer. 
Series 14 is the first run of Elyse's functional task.
"""


"""
A list of the values that need to be accounted for in the default, and 
updated values used to create a `.vars` file.

Vars:
- Study Directory
- Subject ID.
- Raw Data Directory
- Target Atlas
- MP RAGE or T1 Structural -- Can be discerned from the file name in the raw
    directory.
- T2 Weighted Structural
- Functional Series Numbers and Labels -- Can be discerned from the file
    numbers in the raw directory.
- DONE Image Dimensions
- Skip and Evict
- Normalization
"""


# Constants.
path = ""
subj_id = ""
atlas = ""
default_vals = {}
vals = {}
medcon = "/usr/local/xmedcon/bin/medcon"

def main():
    '''
    Our main method for the running of the pipeline.

    Set the path.
    Can be passed as '.' or /path/to/subj/directory
    

    Process Steps:
    Step 1. Create a vars file by taking information from the user.
    Step 2. Carry out preprocessing. (COMPLETED)
    Step 3. Quality check.
    Step 4. Concatenation.
    '''
    if args == '.':
        path = os.getcwd()
    else:
        path = os.path.abspath(args)
    # now delegate the work
    pre_process()
    to_write = update_defaults()
    write_out(to_write)


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
    ############################################
    # still need to figure out slice thickness #
    ############################################
    # walk through the dicoms to find the image dimensions listed inside
    # need to find TR and slices for functional
    #   default dimensions = 64x64
    #   default number slices for functionals = 38
    #   default TR = 2.0
    #   EX: Reptition time 2250?
    #   repitition time is in milliseconds:
    #       http://mriquestions.com/tr-and-te.html
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
    """
    Check if Atlas is in the path.
    if not then use default from vars file

    This is the information about fidl on our default image.
    --------------------------------------------------------------------------
    fidl = '/usr/local/washu/fidl_2.64/scripts/fidl vm'
    Atlas path = /usr/local/washu/fidl/bin_linux
    Necessary arguments:
    -bold_files: 4dfp stacks to be transformed to atlas space.
    -xform_file: 2A or 2B t4 file defining the transform to atlas space.
    -atlas:      Either 111, 222 or 333. Default is 222.
    -conc_name:  Conc file will be created with this name.
    -directory:  Specify directory for output files.
          Include backslash at end.
    --------------------------------------------------------------------------
    """
    target_atlas = ""
    for fname in os.listdir('.'):
        if fname.endswith('.t4'):
            target_atlas = os.path.abspath(fname)
            break
    else:
        default_atlas = ("711-2B", "711-2C")
        target_atlas = default_atlas("711-2B")
    vals.insert("target_atlas", target_atlas)


def create_defaults():
        # assemble all constants into a default struct
    default_vals = {
        "study_directory": "",
        "subject_id": "",
        "raw_data_directory": "",
        "target_atlas": "",
        "X Dimension": 64,
        "Y Dimension": 64,
        "TR": 2000,
        "TR_spacing": 0
    }


def update_defaults():
    # check the defaults with the user and update as appropriate
    return {**default_vals, **vals}


def write_out(values):
    with open('%sid.vars' % subject_id, 'w') as file:
        file.write(json.dumps(values))


if __name__ == "__main__":
    # execute only if run as a script
    main()
