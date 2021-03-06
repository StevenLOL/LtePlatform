import os
import io
import gzip
from lxml import etree
import dateutil.parser
from mr_service import MroReader
import json
from customize_utilities import *
import pymongo
from pymongo import MongoClient
import sys

os.chdir('/home/wireless/huawei_mro')
date_dir=generate_date_hours_shift(shift=-5)
afilter = ['Qci', 'Utra', 'Gsm', 'Tdd']
_startTime=''
db = MongoClient('mongodb://root:Abcdef9*@10.17.165.106')['ouyh']

_DFlist = list(db['Mro_DFlist_'+date_dir].find({}, {'dfName': 1, '_id': 0}))      
DFList = [item.get('dfName') for item in _DFlist]

for root, dirs_no, files in os.walk('/home/wireless/huawei_mro/'+date_dir):
    currrent_dir=os.path.join(root, '')
    for name in files:
        if not name.endswith(sys.argv[1] + '00.xml.gz'):
            continue
        reader=MroReader(afilter)
        print(name)
        if name in DFList:
            pass
        try:
            gFile=gzip.GzipFile(currrent_dir + name, 'r')
            root = etree.fromstring(gFile.read())
        except:
            print('Unzip failed. Continue to unzip other files')
            continue
        item_id=''
        for item in root.iterchildren():
            item_key = []
            if item.tag == 'fileHeader':
                startTime= item.attrib['startTime']
            elif item.tag == 'eNB':
                item_id = item.attrib.get('id')
                for item_measurement in item.iterchildren():
                    reader.read(item_measurement, item_id)
        if (item_id!=''):
            mro_output=reader.map_rsrp_diff(item_id)
            if len(mro_output)>0:
                print(name+':')
                print(len(mro_output))
                for item in mro_output:
                    item.update({'StartTime': startTime})
                print('mro_'+date_dir)
                db['mro_'+date_dir].insert_many(mro_output)
            if len(reader.item_positions)>0:
                for item in reader.item_positions:
                    item.update({'StartTime': startTime})
                db['position_'+date_dir].insert_many(reader.item_positions)
            if len(reader.neighbor_stats)>0:
                neighbor_output=reader.map_neighbor_stats()
                if len(neighbor_output)>0:
                    for item in neighbor_output:
                        item.update({'StartTime': startTime})
                    db['precise_'+date_dir].insert_many(neighbor_output)
        print('insert from ', currrent_dir + name)
        DFList.append(name)
        db['Mro_DFlist_'+date_dir].insert({'dfName': name})
        try:
            os.remove(currrent_dir + name)
        except:
            print('Unable to delete this file: ' + name)

try:
    if db['mro_'+date_dir].index_information().get('Pci_1_NeighborPci_1')==None:
        db['mro_'+date_dir].create_index([("Pci", pymongo.ASCENDING), ("NeighborPci", pymongo.ASCENDING)],background=True)
except:
    print('The colletion is initialized')

try:
    if db['position_'+date_dir].index_information().get('CellId_1')==None:
        db['position_'+date_dir].create_index([("CellId", pymongo.ASCENDING)],background=True)
except:
    print('The colletion is initialized')

try:
    if db['precise_'+date_dir].index_information().get('CellId_1_Pci_1')==None:
        db['precise_'+date_dir].create_index([("CellId", pymongo.ASCENDING), ("Pci", pymongo.ASCENDING)],background=True)
except:
    print('The colletion is initialized')
