import ftputil
import shutil
import os

HOST_HW = ['132.122.152.115', '132.122.152.112', '132.122.152.124'] 
FOLDER_HW = ['/MR_HW_SOURCE_D', '/MR_HW_SOURCE_E']

for host_ip in HOST_HW:
    for folder in FOLDER_HW:
        status = False      
        while not status:
            try:
                host = ftputil.FTPHost(host_ip, 'ouyh18', 'O123#')
                for root, dirs, files in host.walk(folder):
                    if root.count('/') < 3 and root.endswith(date.today().strftime('%Y%m%d')):
                        print(len(files))
                        print(files)

                status = True
            except:
                continue