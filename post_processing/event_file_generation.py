import os
import csv

def read_files():
	"""
	Finds the suitable log files in a given directory.
	Returns a dictionary of log files in target directory.
	"""
	file_list={}
	for fname in os.listdir('.'):
		if fname.endswith('.log'):
			file_list.insert(fname)
	return file_list

def read_data_per_file(input_file):
	"""
	Given a tab seperated log file will read the data out that matches 
	an experiment and return this as a dictionary to be operated on.
	"""
	with open(r"%s"%input_file) as file:
		reader = csv.reader(file, delimiter='\t')
			for row in reader:
				if not "Keypress: t" in row and "DATA " in row:
					fname_data.append(row)

def main():
	to_do = read_file()
	for file in to_do:
		read_data_per_file(file)
	# TODO(Cooper): Parse the event data.

if __name__ == "__main__":
    # execute only if run as a script
    main()