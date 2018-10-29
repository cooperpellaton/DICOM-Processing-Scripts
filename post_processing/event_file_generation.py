import os
import csv
import numpy
import pprint
import argparse

pp = pprint.PrettyPrinter(indent=4)
parser = argparse.ArgumentParser(
    description='Script to process log files into event files.')
parser.add_argument('path', metavar='p', default='.', type=string)
parser.add_argument('# runs per experiment', metavar='r' dest='number_of_runs', default=3, 
    help='The number of runs in each experiment of your trial (ex: 3)')


def read_files():
    """
    Finds the suitable log files in a given directory.
    Returns a dictionary of log files in target directory.
    """
    file_list = {}
    for fname in os.listdir('.'):
        if fname.endswith('.log'):
            file_list.insert(fname)
    return file_list


def read_data_per_file(input_file):
    """
    Given a tab seperated log file will read the data out that matches 
    an experiment and return this as a dictionary to be operated on.
    """
    with open(input_file) as file:
        fname_data = []
        time_offset = 0
        loop = 0
        reader = csv.reader(file, delimiter='\t')
        for row in reader:
            if not "Keypress: t" in row and "DATA " in row and not "Keypress: space" in row:
                fname_data.append(row)
            if "text = u'1'" in row[2]:
                while (loop < 1):
                    time_offset = float(row[0])
                    loop += 1
        return fname_data, time_offset


def count_images_per_file(input_file):
    with open(input_file) as file:
        num_images = 0
        reader = csv.reader(file, delimiter='\t')
        for row in reader:
            if "New trial " in row[2] and u'CountDown' not in row[2]:
                num_images += 1
        return num_images


def main():
    args = parser.parse_args()
    # file_location = "C:/Users/coope/OneDrive/Documents/Projects/Research/DICOM-Processing-Scripts/post_processing/sample_files/raw_log.log"
    number_of_runs = args.r
    path_of_files = args.p
    file_list = read_files()
    for file in file_list:
        file_output = run_alignment(file, number_of_runs)
        write_out(fle, file_output)



def run_alignment(file_location, number_of_runs):
    file_data, time_offset = read_data_per_file(file_location)
    stim_time = []
    for line in file_data:
        stim_time.append([float(line[0].strip()) - time_offset, line[2]])

    number_of_images_per_run = int(
        count_images_per_file(file_location) / (number_of_runs))

    time_diffs = [item[0] for item in stim_time]
    diff = [time_diffs[n] - time_diffs[n-1] for n in range(1, len(time_diffs))]

    for y in range(1, number_of_runs):
        for x in range(number_of_images_per_run * y, 
            (number_of_images_per_run * y + 40) - 1):
            if x is number_of_images_per_run * y:
                # This is right for the first run, but not the second.
                new_offset_time = (247 * 1.5 * y) + (6 * 1.5)
                stim_time[x][0] = new_offset_time

            elif x is not number_of_images_per_run * y:
                print(x)
                print(stim_time[x - 1][0] + diff[x - 1])
                stim_time[x][0] = stim_time[x - 1][0] + diff[x - 1]

    pp.pprint(stim_time)
    return stim_time

if __name__ == "__main__":
    # execute only if run as a script
    main()
