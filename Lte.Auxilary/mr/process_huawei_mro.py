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

os.chdir('huawei_mro')
date_dir=generate_date_twohours_ago()
afilter = ['Qci', 'Utra', 'Gsm', 'Tdd']
_startTime=''
db = MongoClient('mongodb://root:Abcdef9*@10.17.165.106')['ouyh']

for root, dirs_no, files in os.walk(date_dir):
    currrent_dir=os.path.join(root, '')
    for name in files:
        reader=MroReader(afilter)
        gFile=gzip.GzipFile(currrent_dir + name, 'r')
        root = etree.fromstring(gFile.read())
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
            mro_output=reader.map_rsrp_diff()
            if len(mro_output)>0:
                for item in mro_output:
                    item.update({'StartTime': startTime})
                db['mro_'+date_dir].insert_many(mro_output)
            print('insert from ', currrent_dir + name)
        os.remove(currrent_dir + name)