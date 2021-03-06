from lxml import etree
import dateutil.parser
from pandas import DataFrame, Series
import pandas as pd
from functools import reduce
import json
import pymongo
from pymongo import MongoClient

def to_dec(value: str):
    '''
    扩展的字符串转数值函数
    value: 待转换的字符串
    '''
    if '.' in value:
        return float(value)
    elif '_' not in value:
        return int(value)
    else:
        return value

def get_mro_item_key(item_element):
    '''读取MRO字段名称'''
    return item_element.text.replace('MR.', '').split(' ')

class NeighborStat:
    def __init__(self, cellId, pci, **kwargs):
        super().__init__(**kwargs)
        self.stat={'CellId': cellId, 'Neighbors': 0, 'IntraNeighbors': 0, 'Pci': pci}

    def update(self, item_sub_dict):
        if item_sub_dict['LteNcRSRP']>item_sub_dict['LteScRSRP']-6:
            self.stat['Neighbors']+=1
            if item_sub_dict['LteScEarfcn']==item_sub_dict['LteNcEarfcn']:
                self.stat['IntraNeighbors']+=1

class ObjectElement:
    '''
    读取测量记录基本信息的对象
    '''
    def __init__(self, item_element):
        super().__init__()
        self.item_element=item_element
    def get_user_num(self):
        '''读取用户MMEUeS1ApId'''
        if 'MmeUeS1apId' in self.item_element.attrib.keys():
            return self.item_element.attrib['MmeUeS1apId']
        else:
            return self.item_element.attrib['MR.MmeUeS1apId']
    def get_sector_id(self, item_id: str):
        '''
        读取小区的扇区编号（即502120-48中的48）。
        在新版中兴的MR格式中并不记录扇区编号，只有CGI，因此需要从CGI中反推出扇区编号，因此有基站编号的输入参数。
        item_id:输入的基站编号
        '''
        if 'MR.objectId' in self.item_element.attrib.keys():
            return self.item_element.attrib['MR.objectId']
        else:
            return str(to_dec(self.item_element.attrib['id'])-to_dec(item_id)*256)
    def get_huawei_sector_id(self):
        '''读取华为文件中的小区编号'''
        return self.item_element.attrib['id']

class MroReader:
    def __init__(self, afilter, **kwargs):
        super().__init__(**kwargs)
        self.item_dicts=[]
        self.item_positions=[]
        self.neighbor_stats=[]
        self.afilter=afilter

    def display(self):
        for item_dict in self.item_dicts:
            print(item_dict)

    def read(self, item_measurement, item_id):
        '''华为MRO数据读取过程'''
        lon_dict={}
        lat_dict={}
        for item_element in item_measurement:
            if item_element.tag == 'smr':
                item_key = get_mro_item_key(item_element)
                if 'LteScEarfcn' not in item_key:
                    return
            else:
                center_filled=False
                has_position=False
                item_dict = {}
                item_position={}
                neighbor_list=[]
                object_element = ObjectElement(item_element)
                user_num = object_element.get_user_num()
                sector_id = object_element.get_huawei_sector_id()
                neighbor_stat=NeighborStat(item_id+'-'+sector_id, 0)
                for item_v in item_element:
                    item_value = item_v.text.replace('NIL', '-1').replace('N','').replace('E','').replace('HRPD', '-2').replace('BC0', '-2').replace('|', '1').split(' ')
                    _item_sub_dict = dict(zip(item_key, map(to_dec, item_value)))
                    _item_sub_dict = {k: v for k, v in _item_sub_dict.items() if not any(ext in k for ext in self.afilter)}
                    max_telecom_rsrp=0
                    telecom_earfcn=0
                    max_mobile_rsrp=0
                    mobile_earfcn=0
                    max_unicom_rsrp=0
                    unicom_earfcn=0
                    if not center_filled:
                        item_dict.update(item_element.attrib)
                        item_dict.update({'Rsrp': _item_sub_dict['LteScRSRP']})
                        item_dict.update({'SinrUl': _item_sub_dict['LteScSinrUL']})
                        item_dict.update({'Ta': _item_sub_dict['LteScTadv']})
                        item_dict.update({'Pci': _item_sub_dict['LteScPci']})
                        neighbor_stat.stat['Pci']=_item_sub_dict['LteScPci']
                        item_dict.update({'Earfcn': _item_sub_dict['LteScEarfcn']})
                        center_filled=True
                        longtitute=_item_sub_dict['Longitude']
                        lattitute=_item_sub_dict['Latitude']
                        if longtitute==-1 or lattitute==-1:
                            if user_num in lon_dict.keys():
                                longtitute=lon_dict[user_num]
                            if user_num in lat_dict.keys():
                                lattitute=lat_dict[user_num]
                        if longtitute!=-1 and lattitute!=-1:
                            item_position.update({'CellId': item_id+'-'+item_element.attrib['id']})
                            item_position.update({'Rsrp': _item_sub_dict['LteScRSRP']})
                            item_position.update({'Ta': _item_sub_dict['LteScTadv']})
                            has_position=True
                            item_position.update({'Lontitute': longtitute})
                            item_position.update({'Lattitute': lattitute})
                            lon_dict.update({user_num: longtitute})
                            lat_dict.update({user_num: lattitute})
                            if _item_sub_dict['LteScRSRP']>max_telecom_rsrp and _item_sub_dict['LteScEarfcn'] in (100,75,1825,1850,2452,2446,41400):
                                max_telecom_rsrp=_item_sub_dict['LteScRSRP']
                                telecom_earfcn=_item_sub_dict['LteScEarfcn']
                            if _item_sub_dict['LteScRSRP']>max_mobile_rsrp and _item_sub_dict['LteScEarfcn'] in (37900,38098,38400,38950):
                                max_mobile_rsrp=_item_sub_dict['LteScRSRP']
                                mobile_earfcn=_item_sub_dict['LteScEarfcn']
                            if _item_sub_dict['LteScRSRP']>max_unicom_rsrp and _item_sub_dict['LteScEarfcn'] in (1650,1506):
                                max_unicom_rsrp=_item_sub_dict['LteScRSRP']
                                unicom_earfcn=_item_sub_dict['LteScEarfcn']
                    if _item_sub_dict['LteNcPci']>=0:
                        _neighbor={}
                        _neighbor.update({'Pci': _item_sub_dict['LteNcPci']})
                        _neighbor.update({'Rsrp': _item_sub_dict['LteNcRSRP']})
                        _neighbor.update({'Earfcn': _item_sub_dict['LteNcEarfcn']})
                        neighbor_list.append(_neighbor)
                        neighbor_stat.update(_item_sub_dict)
                        if has_position:
                            if _item_sub_dict['LteNcRSRP']>max_telecom_rsrp and _item_sub_dict['LteNcEarfcn'] in (100,75,1825,1850,2452,2446,41400):
                                max_telecom_rsrp=_item_sub_dict['LteNcRSRP']
                                telecom_earfcn=_item_sub_dict['LteNcEarfcn']
                            if _item_sub_dict['LteNcRSRP']>max_mobile_rsrp and _item_sub_dict['LteNcEarfcn'] in (37900,38098,38400,38950):
                                max_mobile_rsrp=_item_sub_dict['LteNcRSRP']
                                mobile_earfcn=_item_sub_dict['LteNcEarfcn']
                            if _item_sub_dict['LteNcRSRP']>max_unicom_rsrp and _item_sub_dict['LteNcEarfcn'] in (1650,1506):
                                max_unicom_rsrp=_item_sub_dict['LteNcRSRP']
                                unicom_earfcn=_item_sub_dict['LteNcEarfcn']
                    else:
                        break
                if has_position:
                    item_position.update({'MaxTelecomRsrp': max_telecom_rsrp})
                    item_position.update({'MaxUnicomRsrp': max_unicom_rsrp})
                    item_position.update({'MaxMobileRsrp': max_mobile_rsrp})
                    item_position.update({'TelecomEarfcn': telecom_earfcn})
                    item_position.update({'UnicomEarfcn': unicom_earfcn})
                    item_position.update({'MoblieEarfcn': mobile_earfcn})
                    self.item_positions.append(item_position)
                if len(neighbor_list)>0:
                    item_dict.update({'NeighborList': neighbor_list})
                    self.item_dicts.append(item_dict)
                self.neighbor_stats.append(neighbor_stat.stat)

    def read_zte(self, item_measurement, item_id):
        '''中兴MRO数据读取过程'''
        lon_dict={}
        lat_dict={}
        for item_element in item_measurement:
            if item_element.tag == 'smr':
                item_key = get_mro_item_key(item_element)
                if 'LteScEarfcn' not in item_key:
                    return
            else:
                center_filled=False
                has_position=False
                item_dict = {}
                item_position={}
                neighbor_list=[]
                object_element=ObjectElement(item_element)
                user_num=object_element.get_user_num()
                sector_id=object_element.get_sector_id(item_id)
                neighbor_stat=NeighborStat(item_id+'-'+sector_id, 0)
                for item_v in item_element:
                    item_value = item_v.text.replace('NIL', '-1').split(' ')
                    _item_sub_dict = dict(zip(item_key, map(to_dec, item_value)))
                    _item_sub_dict = {k: v for k, v in _item_sub_dict.items() if not any(ext in k for ext in self.afilter)}
                    if 'LteFddNcPci' in _item_sub_dict.keys() and _item_sub_dict['LteFddNcPci']>=0 and _item_sub_dict['LteNcPci']<0:
                        _item_sub_dict['LteNcPci']=_item_sub_dict['LteFddNcPci']
                        _item_sub_dict['LteNcRSRP']=_item_sub_dict['LteFddNcRSRP']
                        _item_sub_dict['LteNcEarfcn']=_item_sub_dict['LteFddNcEarfcn']
                    max_telecom_rsrp=0
                    telecom_earfcn=0
                    max_mobile_rsrp=0
                    mobile_earfcn=0
                    max_unicom_rsrp=0
                    unicom_earfcn=0
                    if not center_filled:
                        item_dict.update({'id': item_id+'-'+sector_id})                        
                        item_dict.update({'Rsrp': _item_sub_dict['LteScRSRP']})                        
                        item_dict.update({'SinrUl': _item_sub_dict['LteScSinrUL']})
                        item_dict.update({'Ta': _item_sub_dict['LteScTadv']})                        
                        item_dict.update({'Pci': _item_sub_dict['LteScPci']})
                        neighbor_stat.stat['Pci']=_item_sub_dict['LteScPci']
                        item_dict.update({'Earfcn': _item_sub_dict['LteScEarfcn']})                        
                        center_filled=True
                        if 'Longitude' in _item_sub_dict.keys():
                            longtitute=_item_sub_dict['Longitude']
                            lattitute=_item_sub_dict['Latitude']
                            if longtitute==-1 or lattitute==-1:
                                if user_num in lon_dict.keys():
                                    longtitute=lon_dict[user_num]
                                if user_num in lat_dict.keys():
                                    lattitute=lat_dict[user_num]
                            if longtitute!=-1 and lattitute!=-1:
                                item_position.update({'CellId': item_id+'-'+item_element.attrib['MR.objectId']})
                                item_position.update({'Rsrp': _item_sub_dict['LteScRSRP']})
                                item_position.update({'Ta': _item_sub_dict['LteScTadv']})
                                has_position=True
                                if  longtitute>200:
                                    longtitute=_item_sub_dict['Longitude'] * 360 *1.0/ 16777216
                                    lattitute=_item_sub_dict['Latitude'] * 90 / 8388608
                                item_position.update({'Lontitute': longtitute})
                                item_position.update({'Lattitute': lattitute})
                                lon_dict.update({user_num: longtitute})
                                lat_dict.update({user_num: lattitute})
                                if _item_sub_dict['LteScRSRP']>max_telecom_rsrp and _item_sub_dict['LteScEarfcn'] in (100,75,1825,1850,2452,2446,2505,41400):
                                    max_telecom_rsrp=_item_sub_dict['LteScRSRP']
                                    telecom_earfcn=_item_sub_dict['LteScEarfcn']
                                if _item_sub_dict['LteScRSRP']>max_mobile_rsrp and _item_sub_dict['LteScEarfcn'] in (37900,38098,38400,38950):
                                    max_mobile_rsrp=_item_sub_dict['LteScRSRP']
                                    mobile_earfcn=_item_sub_dict['LteScEarfcn']
                                if _item_sub_dict['LteScRSRP']>max_unicom_rsrp and _item_sub_dict['LteScEarfcn'] in (1650,1506):
                                    max_unicom_rsrp=_item_sub_dict['LteScRSRP']
                                    unicom_earfcn=_item_sub_dict['LteScEarfcn']
                    if _item_sub_dict['LteNcPci']>=0:
                        _neighbor={}
                        _neighbor.update({'Pci': _item_sub_dict['LteNcPci']})
                        _neighbor.update({'Rsrp': _item_sub_dict['LteNcRSRP']})
                        _neighbor.update({'Earfcn': _item_sub_dict['LteNcEarfcn']})
                        neighbor_list.append(_neighbor)
                        neighbor_stat.update(_item_sub_dict)
                        if has_position:
                            if _item_sub_dict['LteNcRSRP']>max_telecom_rsrp and _item_sub_dict['LteNcEarfcn'] in (100,75,1825,1850,2452,2446,41400):
                                max_telecom_rsrp=_item_sub_dict['LteNcRSRP']
                                telecom_earfcn=_item_sub_dict['LteNcEarfcn']
                            if _item_sub_dict['LteNcRSRP']>max_mobile_rsrp and _item_sub_dict['LteNcEarfcn'] in (37900,38098,38400,38950):
                                max_mobile_rsrp=_item_sub_dict['LteNcRSRP']
                                mobile_earfcn=_item_sub_dict['LteNcEarfcn']
                            if _item_sub_dict['LteNcRSRP']>max_unicom_rsrp and _item_sub_dict['LteNcEarfcn'] in (1650,1506):
                                max_unicom_rsrp=_item_sub_dict['LteNcRSRP']
                                unicom_earfcn=_item_sub_dict['LteNcEarfcn']
                    else:
                        break
                if has_position:
                    item_position.update({'MaxTelecomRsrp': max_telecom_rsrp})
                    item_position.update({'MaxUnicomRsrp': max_unicom_rsrp})
                    item_position.update({'MaxMobileRsrp': max_mobile_rsrp})
                    item_position.update({'TelecomEarfcn': telecom_earfcn})
                    item_position.update({'UnicomEarfcn': unicom_earfcn})
                    item_position.update({'MoblieEarfcn': mobile_earfcn})
                    self.item_positions.append(item_position)
                if len(neighbor_list)>0:
                    item_dict.update({'NeighborList': neighbor_list})
                    self.item_dicts.append(item_dict)
                self.neighbor_stats.append(neighbor_stat.stat)

    def _filter_by_neighbor_len(self, length):
        return list(filter(lambda x: True if len(x['NeighborList'])==length else False, self.item_dicts))

    def _map_neighbor_rsrp_diff(self, index):
        measureList=self._filter_by_neighbor_len(index)
        if len(measureList)==0:
            return []
        return list(map(lambda item: {
            'CellId': item['id'],
            'Earfcn': item['Earfcn'],
            'NeighborPci': item['NeighborList'][index-1]['Pci'],
            'NeighborEarfcn': item['NeighborList'][index-1]['Earfcn'],
            'NeighborRsrp': item['NeighborList'][index-1]['Rsrp'],
            'RsrpDiff': item['Rsrp']-item['NeighborList'][index-1]['Rsrp'],
            'Rsrp': item['Rsrp'],
            'Pci': item['Pci'],
            'Ta': item['Ta'],
            'SinrUl': item['SinrUl']
        }, measureList))

    def map_rsrp_diff(self, eNodebId):
        diff_list=list(map(lambda index: self._map_neighbor_rsrp_diff(index+1), list(range(6))))
        combined_list=reduce(lambda first,second: first+second,diff_list,[])
        if len(combined_list)==0:
            return []
        stat_list=list(map(lambda item: {
            'CellId': eNodebId + '-' + item['CellId'],
            'Earfcn': item['Earfcn'],
            'NeighborPci': item['NeighborPci'],
            'Pci': item['Pci'],
            'NeighborEarfcn': item['NeighborEarfcn'],
            'Diff0': 1 if item['RsrpDiff']<=0 else 0,
            'Diff3': 1 if item['RsrpDiff']<=3 and item['RsrpDiff']>0 else 0,
            'Diff6': 1 if item['RsrpDiff']<=6 and item['RsrpDiff']>3 else 0,
            'Diff9': 1 if item['RsrpDiff']<=9 and item['RsrpDiff']>6 else 0,
            'Diff12': 1 if item['RsrpDiff']<=12 and item['RsrpDiff']>9 else 0,
            'DiffLarge': 1 if item['RsrpDiff']>12 else 0,
            'RsrpBelow120': 1 if item['Rsrp']<20 else 0,
            'RsrpBetween120110': 1 if item['Rsrp']<30 and item['Rsrp']>=20 else 0,
            'RsrpBetween110105': 1 if item['Rsrp']<35 and item['Rsrp']>=30 else 0,
            'RsrpBetween105100': 1 if item['Rsrp']<40 and item['Rsrp']>=35 else 0,
            'RsrpBetween10090': 1 if item['Rsrp']<50 and item['Rsrp']>=40 else 0,
            'RsrpAbove90': 1 if item['Rsrp']>=50 else 0,
            'NeighborRsrpBelow120': 1 if item['NeighborRsrp']<20 else 0,
            'NeighborRsrpBetween120110': 1 if item['NeighborRsrp']<30 and item['NeighborRsrp']>=20 else 0,
            'NeighborRsrpBetween110105': 1 if item['NeighborRsrp']<35 and item['NeighborRsrp']>=30 else 0,
            'NeighborRsrpBetween105100': 1 if item['NeighborRsrp']<40 and item['NeighborRsrp']>=35 else 0,
            'NeighborRsrpBetween10090': 1 if item['NeighborRsrp']<50 and item['NeighborRsrp']>=40 else 0,
            'NeighborRsrpAbove90': 1 if item['NeighborRsrp']>=50 else 0,
            'Ta0or1': 1 if item['Ta']==0 or item['Ta']==1 else 0,
            'Ta2or3': 1 if item['Ta']==2 or item['Ta']==3 else 0,
            'Ta4or5': 1 if item['Ta']==4 or item['Ta']==5 else 0,
            'Ta6or7': 1 if item['Ta']==6 or item['Ta']==7 else 0,
            'Ta8or9': 1 if item['Ta']==8 or item['Ta']==9 else 0,
            'Ta10to12': 1 if item['Ta']>=10 and item['Ta']<=12 else 0,
            'Ta13to15': 1 if item['Ta']>=13 and item['Ta']<=15 else 0,
            'Ta16to19': 1 if item['Ta']>=16 and item['Ta']<=19 else 0,
            'Ta20to24': 1 if item['Ta']>=20 and item['Ta']<=24 else 0,
            'Ta25to29': 1 if item['Ta']>=25 and item['Ta']<=29 else 0,
            'Ta30to39': 1 if item['Ta']>=30 and item['Ta']<=39 else 0,
            'TaAbove40': 1 if item['Ta']>=40 else 0,
            'SinrUl0to9': 1 if item['SinrUl']>=0 and item['SinrUl']<=9 else 0,
            'SinrUl10to19': 1 if item['SinrUl']>=10 and item['SinrUl']<=19 else 0,
            'SinrUl20to24': 1 if item['SinrUl']>=20 and item['SinrUl']<=24 else 0,
            'SinrUl25to29': 1 if item['SinrUl']>=25 and item['SinrUl']<=29 else 0,
            'SinrUl30to34': 1 if item['SinrUl']>=30 and item['SinrUl']<=34 else 0,
            'SinrUlAbove35': 1 if item['SinrUl']>=35 else 0
        }, combined_list))
        df = DataFrame(stat_list)
        stat=df.groupby(['CellId','Pci','NeighborPci', 'Earfcn', 'NeighborEarfcn']).sum().reset_index()
        return json.loads(stat.T.to_json()).values()

    def map_neighbor_stats(self):
        stat_list=list(map(lambda item: {
            'CellId': item['CellId'],
            'Pci': item['Pci'],
            'Neighbors0': 1 if item['Neighbors']==0 else 0,
            'Neighbors1': 1 if item['Neighbors']==1 else 0,
            'Neighbors2': 1 if item['Neighbors']==2 else 0,
            'Neighbors3': 1 if item['Neighbors']==3 else 0,
            'NeighborsMore': 1 if item['Neighbors']>3 else 0,
            'IntraNeighbors0': 1 if item['IntraNeighbors']==0 else 0,
            'IntraNeighbors1': 1 if item['IntraNeighbors']==1 else 0,
            'IntraNeighbors2': 1 if item['IntraNeighbors']==2 else 0,
            'IntraNeighbors3': 1 if item['IntraNeighbors']==3 else 0,
            'IntraNeighborsMore': 1 if item['IntraNeighbors']>3 else 0
        }, self.neighbor_stats))
        df = DataFrame(stat_list)
        stat=df.groupby(['CellId','Pci']).sum().reset_index()
        return json.loads(stat.T.to_json()).values()

    def map_rsrp_diff_zte(self):
        diff_list=list(map(lambda index: self._map_neighbor_rsrp_diff(index+1), list(range(6))))
        combined_list=reduce(lambda first,second: first+second,diff_list,[])
        if len(combined_list)==0:
            return []
        stat_list=list(map(lambda item: {
            'CellId': item['CellId'],
            'Earfcn': item['Earfcn'],
            'NeighborPci': item['NeighborPci'],
            'NeighborEarfcn': item['NeighborEarfcn'],
            'Pci': item['Pci'],
            'Diff0': 1 if item['RsrpDiff']<=0 else 0,
            'Diff3': 1 if item['RsrpDiff']<=3 and item['RsrpDiff']>0 else 0,
            'Diff6': 1 if item['RsrpDiff']<=6 and item['RsrpDiff']>3 else 0,
            'Diff9': 1 if item['RsrpDiff']<=9 and item['RsrpDiff']>6 else 0,
            'Diff12': 1 if item['RsrpDiff']<=12 and item['RsrpDiff']>9 else 0,
            'DiffLarge': 1 if item['RsrpDiff']>12 else 0,
            'RsrpBelow120': 1 if item['Rsrp']<20 else 0,
            'RsrpBetween120110': 1 if item['Rsrp']<30 and item['Rsrp']>=20 else 0,
            'RsrpBetween110105': 1 if item['Rsrp']<35 and item['Rsrp']>=30 else 0,
            'RsrpBetween105100': 1 if item['Rsrp']<40 and item['Rsrp']>=35 else 0,
            'RsrpBetween10090': 1 if item['Rsrp']<50 and item['Rsrp']>=40 else 0,
            'RsrpAbove90': 1 if item['Rsrp']>=50 else 0,
            'NeighborRsrpBelow120': 1 if item['NeighborRsrp']<20 else 0,
            'NeighborRsrpBetween120110': 1 if item['NeighborRsrp']<30 and item['NeighborRsrp']>=20 else 0,
            'NeighborRsrpBetween110105': 1 if item['NeighborRsrp']<35 and item['NeighborRsrp']>=30 else 0,
            'NeighborRsrpBetween105100': 1 if item['NeighborRsrp']<40 and item['NeighborRsrp']>=35 else 0,
            'NeighborRsrpBetween10090': 1 if item['NeighborRsrp']<50 and item['NeighborRsrp']>=40 else 0,
            'NeighborRsrpAbove90': 1 if item['NeighborRsrp']>=50 else 0,
            'Ta0or1': 1 if item['Ta']==0 or item['Ta']==1 else 0,
            'Ta2or3': 1 if item['Ta']==2 or item['Ta']==3 else 0,
            'Ta4or5': 1 if item['Ta']==4 or item['Ta']==5 else 0,
            'Ta6or7': 1 if item['Ta']==6 or item['Ta']==7 else 0,
            'Ta8or9': 1 if item['Ta']==8 or item['Ta']==9 else 0,
            'Ta10to12': 1 if item['Ta']>=10 and item['Ta']<=12 else 0,
            'Ta13to15': 1 if item['Ta']>=13 and item['Ta']<=15 else 0,
            'Ta16to19': 1 if item['Ta']>=16 and item['Ta']<=19 else 0,
            'Ta20to24': 1 if item['Ta']>=20 and item['Ta']<=24 else 0,
            'Ta25to29': 1 if item['Ta']>=25 and item['Ta']<=29 else 0,
            'Ta30to39': 1 if item['Ta']>=30 and item['Ta']<=39 else 0,
            'TaAbove40': 1 if item['Ta']>=40 else 0,
            'SinrUl0to9': 1 if item['SinrUl']>=0 and item['SinrUl']<=9 else 0,
            'SinrUl10to19': 1 if item['SinrUl']>=10 and item['SinrUl']<=19 else 0,
            'SinrUl20to24': 1 if item['SinrUl']>=20 and item['SinrUl']<=24 else 0,
            'SinrUl25to29': 1 if item['SinrUl']>=25 and item['SinrUl']<=29 else 0,
            'SinrUl30to34': 1 if item['SinrUl']>=30 and item['SinrUl']<=34 else 0,
            'SinrUlAbove35': 1 if item['SinrUl']>=35 else 0
        }, combined_list))
        df = DataFrame(stat_list)
        stat=df.groupby(['CellId','Pci','NeighborPci', 'Earfcn', 'NeighborEarfcn']).sum().reset_index()
        return json.loads(stat.T.to_json()).values()

class MrsReader:
    def __init__(self, mrNames, startTime, date_dir, db, eNodebId, **kwargs):
        self.mrNames=mrNames
        self.startTime=startTime
        self.date_dir=date_dir
        self.db=db
        self.eNodebId=eNodebId
        return super().__init__(**kwargs)

    def read(self, item_measurement):
        mrName=item_measurement.attrib['mrName'].replace('MR.','')
        if mrName in self.mrNames:
            item_dicts=[]
            for item_element in item_measurement.iterchildren():
                if item_element.tag == 'smr':
                    item_key = item_element.text.replace('MR.', '').replace('.','_').split(' ')
                else:
                    item_dict={}
                    item_dict.update({'CellId': self.eNodebId + '-' + item_element.attrib['id']})
                    item_value = item_element[0].text.split(' ')
                    item_dict.update(dict(zip(item_key, map(int, item_value))))
                    item_dict.update({'StartTime': self.startTime})
                    item_dicts.append(item_dict)
            if len(item_dicts)>0:
                self.db['mrs_'+mrName+'_'+self.date_dir].insert_many(item_dicts)

    def read_zte(self, item_measurement, eNodebId):
        mrName=item_measurement.attrib['mrName'].replace('MR.','')
        if mrName in self.mrNames:
            item_dicts=[]
            for item_element in item_measurement.iterchildren():
                if item_element.tag == 'smr':
                    item_key = item_element.text.replace('MR.', '').replace('.','_').split(' ')
                else:
                    item_dict={}
                    if 'MR.objectId' in item_element.attrib.keys():
                        sector_id=item_element.attrib['MR.objectId']
                    else:
                        sector_id=item_element.attrib['id']
                    item_dict.update({'CellId': eNodebId+'-'+sector_id})
                    item_value = item_element[0].text.split(' ')
                    item_dict.update(dict(zip(item_key, map(int, item_value))))
                    item_dict.update({'StartTime': self.startTime})
                    item_dicts.append(item_dict)
            if len(item_dicts)>0:
                self.db['mrs_'+mrName+'_'+self.date_dir].insert_many(item_dicts)