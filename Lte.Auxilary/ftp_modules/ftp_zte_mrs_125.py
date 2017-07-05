import ftputil
import os
import shutil
import pymongo
from pymongo import MongoClient
from customize_utilities import *
import datetime
import sys

db = MongoClient('mongodb://root:Abcdef9*@132.110.71.123')['ouyh']
  
host_ip = '132.122.152.125'
FOLDER_ZTE = ['/'+sys.argv[2]+'/']
sub_ips=[sys.argv[1]]

if not os.path.isdir('zte_mrs'):
    os.mkdir('zte_mrs')
os.chdir('zte_mrs')
delay=-int(sys.argv[3])-2
hour=datetime.datetime.now().hour
minute=datetime.datetime.now().minute
if hour>12 and int(sys.argv[3])>2:
    delay=-(hour-minute%hour)
date_dir=generate_date_hours_shift(shift=delay)
_DFlist = list(db['DFlist_'+date_dir].find({}, {'dfName': 1, '_id': 0}))      
DFList = [item.get('dfName') for item in _DFlist]
if not os.path.isdir(date_dir):
    os.mkdir(date_dir)
os.chdir(date_dir)

print(host_ip)
try:
    print("######")
    host = ftputil.FTPHost(host_ip, 'ouyh18', 'O123#')
    downloader=MrDownloader(host,sub_ips,DFList,db,host_ip)
    for folder in FOLDER_ZTE:
        ftpdir=generate_time_dir_shift(prefix = folder, shift=delay)
        print(ftpdir)
        downloader.download_mrs_zte(ftpdir)
    host.close()
except:
    print('Cannot connect to', host_ip)