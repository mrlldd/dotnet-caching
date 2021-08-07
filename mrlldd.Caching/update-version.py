import sys
import re
import os
from xml.dom import minidom


def print_usage():
    print("  usage: update-version.py major.minor.build")
    print("example: update-version.py 1.2.3")


def main():
    if len(sys.argv) != 2:
        print_usage()
        return

    version = sys.argv[1]
    if not re.match('[\\d]+\\.[\\d]+\\.[\\d]+', version):
        print_usage()
        return

    this_script_location = os.path.dirname(os.path.realpath(__file__))
    modified_files = set()
    for current_folder, subfolders, files in os.walk(this_script_location):
        for file in files:
            if file.endswith('.csproj'):
                full_file_path = os.path.join(current_folder, file)

                xml_doc = minidom.parse(full_file_path)
                for element in xml_doc.getElementsByTagName('Version'):
                    element.childNodes[0].nodeValue = version
                    modified_files.add(full_file_path)
                    
                for element in xml_doc.getElementsByTagName('PackageVersion'):
                    element.childNodes[0].nodeValue = version
                    modified_files.add(full_file_path)
                    
                if full_file_path in modified_files:
                    with open(full_file_path, 'w') as f:
                        f.write(xml_doc.toxml().replace('<?xml version="1.0" ?>', ''))

    if len(modified_files) > 0:
        print('Version "{}" applied successfully. Modified files:'.format(version))
        for modified_file in modified_files:
            print('M: {}'.format(modified_file))
    else:
        print("Didn't find any .csproj files. Check if you're running this script from repository root.")


if __name__ == '__main__':
    main()