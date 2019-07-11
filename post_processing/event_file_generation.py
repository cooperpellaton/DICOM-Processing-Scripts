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
        trial_times = []
        found = False
        trial_count = 0
        loop = 0
        t_count = 0
        prev_line = None
        space_count = 0
        reader = csv.reader(file, delimiter="\t")
        for row in reader:
            if (
                not "Keypress: t" in row
                and "DATA " in row
                and not "Keypress: space" in row
            ):
                fname_data.append(row)

            if "Keypress: space" in row[2]:
                # This is so we can realign for multiple runs.
                fname_data.append(row)

            if prev_line != None:
                if "Created" in prev_line[2] and "Recall_Phase_Instructions" in row[2]:
                    found = True
                if found and "Recall_Phase" in prev_line[2] and "Keypress: t" in row:
                    print("added an autotrial")
                    found = False
                    trial_times.append(float(row[0]))
                """ 
                This finds long periods of t's (jitter) followed by a space which mark the beginning of a new trial run.
                """
                if "Fixation: autoDraw = True" in prev_line and "Keypress: t" in row:
                    found = True
                    t_count += 1
                if found and "Keypress: t" in prev_line and "Keypress: t" in row:
                    t_count += 1
                if "Keypress: t" in prev_line and "Fixation: autoDraw = False" in row:
                    print("adding a manual trial, t_count = %s" % t_count)
                    if trial_count <= 1:
                        trial_times.append(float(t_count * 1.5))
                    found = False
                    trial_count += 1
                    t_count = 0
            prev_line = row
        # The last trial has two spaces that follow it. Remove them.
        del fname_data[-1]
        del fname_data[-1]
        return fname_data, trial_times


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
    out_name = file.split(".")[0]
    with open("output_%s.out" % out_name, "w") as file:
        for line in output:
            file.write("%s, %s\n" % (line[0], line[1]))


def run_alignment(file_location, number_of_runs):
    file_data, trial_times = read_data_per_file(file_location)
    print("Trial times: %s" % trial_times)
    time_offset = 0
    stim_time = []
    count = 0
    number_of_images_per_run = count_images_per_file(file_location) / int(
        number_of_runs
    )
    for line in file_data:
        """ This signifies a space betwewen trials 1 & 2, 2 & 3.
        It is different however from the space between 3 & 4, and 6 & 7.
        """
        #print("Count: %s" % count)
        #if len(stim_time) > 1:
        #    print(stim_time[1])
        if "Keypress: space" in line:
            print("popping space")
            time_offset = (
                float(line[0]) - stim_time[-1][0]
                + (2 * 1.5)
                + 1.5
                + (trial_times.pop(0))
            )
            print("New offset after space alignment: %s" % time_offset)
        else:
            # For trials 1, 4, 7, etc.
            if count == 0:
                time_offset = (2 * 1.5) + 1.5 + trial_times.pop(0)
                print("New offset after 0 alingment: %s" % time_offset)
            if count == (number_of_images_per_run * 3):
                time_offset = (
                    float(line[0]) + (2 * 1.5) + 1.5 + trial_times.pop(0)
                )
                print("New Offset: %s" % time_offset)
                count = 0
            count += 1
            # Append the time and keypress to the event file.
            stim_time.append([float(line[0].strip()) - time_offset, line[2]])

    # pp.pprint(stim_time)
    return stim_time


def main():
    number_of_runs = int(args.num_runs)
    file_list = read_files(args.path)
    for file in file_list:
        file_output = run_alignment(file, number_of_runs)
        write_out(file, file_output)


if __name__ == "__main__":
    main()
