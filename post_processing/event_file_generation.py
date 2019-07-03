import argparse
import csv
import json
import os
import pprint

import numpy

pp = pprint.PrettyPrinter(indent=4)
parser = argparse.ArgumentParser(
    description="Script to process log files into event files."
)
parser.add_argument("path", metavar="p", default=".", type=str)
parser.add_argument(
    "num_runs", metavar="r", default=3, help="Number of runs in e/ experiment(ex: 3)"
)

args = parser.parse_args()


def read_files(path):
    """
    Finds the suitable log files in a given directory.
    Returns a list of log files in target directory.
    """
    file_list = []
    for fname in os.listdir(path):
        if fname.endswith(".log"):
            file_list.append(fname)
    return file_list


def read_data_per_file(input_file):
    """
    Given a tab seperated log file will read the data out that matches 
    an experiment and return this as a dictionary to be operated on.
    """
    with open(input_file) as file:
        fname_data = []
        loop = 0
        reader = csv.reader(file, delimiter="\t")
        for row in reader:
            if (
                not "Keypress: t" in row
                and "DATA " in row
                and not "Keypress: space" in row
            ):
                fname_data.append(row)
            if "Keypress: space" in row:
                # This is so we can realign for multiple runs.
                fname_data.append(row)
        return fname_data


def count_images_per_file(input_file):
    """ Count the total number of images in a log file."""
    with open(input_file) as file:
        num_images = 0
        reader = csv.reader(file, delimiter="\t")
        for row in reader:
            if "New trial " in row[2] and u"CountDown" not in row[2]:
                num_images += 1
        return num_images


def write_out(file, output):
    """ Write a file to disk, line by line."""
    with open("output_%s" % file, "w") as file:
        for line in output:
            file.write(line)


def run_alignment(file_location, number_of_runs):
    file_data = read_data_per_file(file_location)
    time_offset = 0
    stim_time = []
    count = 0
    number_of_images_per_run = int(
        count_images_per_file(file_location) / (number_of_runs)
    )
    for line in file_data:
        """ This signifies a space betwewen trials 1 & 2, 2 & 3.
        It is different however from the space between 3 & 4, and 6 & 7.
        """
        if line[2] == "Keypress: space":
            time_offset = (line[0] - stim_time[:][0]) + (2 * 1.5) + 1.5 + jitter
        else:
            count += 1
            # For trials 1, 4, 7, etc.
            if count == number_of_images_per_run * 3:
                time_offset = line[0] + (2 * 1.5) + 1.5 + jitter
                count = 0
            # Append the time and keypress to the event file.
            stim_time.append([float(line[0].strip()) + time_offset, line[2]])

    pp.pprint(stim_time)
    return stim_time


def main():
    number_of_runs = int(args.num_runs)
    file_list = read_files(args.path)
    for file in file_list:
        file_output = run_alignment(file, number_of_runs)
        write_out(file, file_output)


if __name__ == "__main__":
    main()
