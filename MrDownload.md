# MR��������
����һ����������˴ӱ���ӿ����ص��������̺ʹ��������
## ����ӿ����ظ�������
MR���ݶ�����ڱ���ӿ�FTP��������
������Ҫ�������ݲ����Ļ��ƶ������ظ���������
### ����ӿ����ݲ�������
* **��������**��15����
* **�ļ�����**��
ÿ���������ͣ�MRO��MRS��MRE���Լ�ÿ����վID����������һ��XML�ļ���
* **ѹ������**�����ö���ѹ����
����ÿ��������XML�ļ�ѹ��Ϊһ���ļ���
Ȼ���������15���ӵ�����ѹ���ļ���ѹ��Ϊһ�����ѹ���ļ���
* **��Ź���**�����ڻ�վ�����϶࣬
һ�����ҵ������ļ��ֱ�洢�ڶ��������
* **��������**����Ϊ�����ˣ������ѹ��������ļ������������в�ͬ
#### ѹ������
##### ����
�ڶ���ѹ��ʱ��ֱ�ӽ���һ��ѹ�����ɵ�ѹ���ļ�ѹ��Ϊһ���ļ���
ѹ���ļ�Ϊ.zip�ļ���
##### ��Ϊ
�ڶ���ѹ��ʱ��
�Ƚ���һ��ѹ�����ɵ�ѹ���ļ�����������վ��Ϊ����ļ��д洢��
��Щ�ļ����Ի�վ��������
Ȼ�󽫸����ļ���ѹ��Ϊһ��ѹ���ļ���
ѹ���ļ�Ϊ.xml.gz�ļ���
#### �ļ���������
##### ����
* **����**��FDD-LTE_MRS_ZTE_OMC1_502599_20161128044500.zip
* **˵��**��
��1����ʽFDD-LTE
��2��MR�ļ�����MRS
��3������ZTE
��4�����ܷ���������OMC1
��5����վ���502599
��6������ʱ�䣺20161128044500
##### ��Ϊ
* **����**��FDD-LTE_MRO_HUAWEI_501035_20161122113000.xml.gz
* **˵��**���󲿷���������ͬ�����������ܷ�����������һ�ֶ�
### ���������ж�ʵ��
#### ��Ϊʵ��
##### ��������
```python
def is_filename_huawei(name: str, file_type: str):
    my_type = name.split('_')[-4]
    return my_type == file_type and name.endswith('.xml.gz')
```
##### MRO
```python
def is_mro_filename(name: str):
    '''�ж��Ƿ�Ϊ��ΪMRO�ļ���������FDD-LTE_MRO_HUAWEI_501035_20161122113000.xml.gz'''
    return is_filename_huawei(name, 'MRO')
```
##### MRE
```python
def is_mre_filename(name):
    '''�ж��Ƿ�Ϊ��ΪMRE�ļ���������FDD-LTE_MRE_HUAWEI_500328_20161122113000.xml.gz'''
    return is_filename_huawei(name, 'MRE')
```
##### MRS
```python
def is_mrs_filename(name):
    '''�ж��Ƿ�Ϊ��ΪMRS�ļ���������FDD-LTE_MRS_HUAWEI_501195_20161122113000.xml.gz'''
    return is_filename_huawei(name, 'MRS')
```
#### ����ʵ��
##### ��������
```python
def is_filename_zte(name: str, file_type: str):
    my_type = name.split('_')[-5]
    return my_type == file_type and name.endswith('.zip')
```
##### MRO
```python
def is_mro_filename_zte(name: str):
    '''�ж��Ƿ�Ϊ����MRO�ļ���������FDD-LTE_MRO_ZTE_OMC1_501251_20170705194500.zip'''
    return is_filename_zte(name, 'MRO')
```
##### MRE
```python
def is_mre_filename_zte(name):
    '''�ж��Ƿ�Ϊ����MRE�ļ���������FDD-LTE_MRE_ZTE_OMC1_501819_20161218101500.zip'''
    return is_filename_zte(name, 'MRE')
```
##### MRS
```python
def is_mrs_filename_zte(name):
    '''�ж��Ƿ�Ϊ����MRS�ļ���������FDD-LTE_MRS_ZTE_OMC1_502599_20161128044500.zip'''
    return is_filename_zte(name, 'MRS')
```
## ����ӿ��������غ�Ԥ�������
## ����ӿ���������ʵ��

