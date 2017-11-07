<?php
header("content-Type: text/html; charset=utf-8");
header("Access-Control-Allow-Origin: *");
//require'fun.php';
include'fun.php';
class AssessmentAction extends Action
{
	
	public function index() {
	}
		
	//列表查询
	public function search(){
		$results = array();
		$page_size = getParam('page_size',0);//每页个数
		$curr_page = getParam('curr_page',0);//第几页
		$areaName = getParam('areaName',1);//第几页
		$cycle = getParam('cycle',1);//第几页

		if($curr_page == NULL || $curr_page == '' || $curr_page <= 0)
			$curr_page = 1;

		$wherequery = ' 1 = 1   ';

		if($areaName != NULL || $areaName != '')
		{
			$wherequery .= " and areaName = '".$areaName."'";	
		}

		if($cycle != NULL || $cycle != '')
		{
			$wherequery .= " and cycle like '%".$cycle."%'";	
		}

		if($page_size == NULL || $page_size == '')
		{
			$page_size = 30;
		}


		$Special = M("assessment");

		$total = $Special->where($wherequery)->count(); // 查询数据
		if( $total >0 ){
			$total_pages = ceil($total/$page_size);
		}else{
			$total_pages = 0;
		}

		if ($curr_page > $total_pages) 
			$curr_page=$total_pages;
		$start = $page_size * $curr_page - $page_size; //开始位置

		$list = $Special -> where($wherequery)->limit($start.','.$page_size)->select();

		$fields = $Special -> getDbFields();
		$size = count($fields);
		$i = 0;
		foreach($list as $l)
		{
			$j = 0;
			foreach($fields as $f)	
			{
				if($j == $size-1)
					break;
				$result[$f] = urlencode($l[$f]); //编码
				$j++;
			}
			$results[$i] = $result;
			$i++;
		}

		$responce['curr_page'] = $curr_page;
		$responce['total_pages'] = $total_pages;
		$responce['records'] = $total;
		$responce['rows'] = $results;
		echo getResponse($responce,'','','');
	}
	public function check(){
		$results = array();
		$page_size = getParam('page_size',0);//每页个数
		$curr_page = getParam('curr_page',0);//第几页
		$areaName = getParam('areaName',1);//
		$status = getParam('status',1);//
		$type = getParam('type',1);//

		if($curr_page == NULL || $curr_page == '' || $curr_page <= 0)
			$curr_page = 1;
		if($page_size == NULL || $page_size == '')
		{
			$page_size = 30;
		}

		$wherequery = ' tbs.id = tbc.stationid ';


		if($areaName != NULL && $areaName != '')
		{
			$wherequery .= " and id like '".$areaName."%'";	
		}
		if($type != NULL && $type != '')
		{
			$wherequery .= " and id like '%".$type."%'";	
		}
		if($status != NULL && $status != '')
		{
			$wherequery .= " and XJDZT027='".$status."'";	
		}

		$Station = M("stationcommon");
		$total = $Station->table('tb_stationcommon tbs,tb_checkplan tbc')->where($wherequery)->count(); // 查询数据

		if( $total >0 ){
			$total_pages = ceil($total/$page_size);
		}else{
			$total_pages = 0;
		}

		if ($curr_page > $total_pages) 
			$curr_page=$total_pages;
		$start = $page_size * $curr_page - $page_size; //开始位置

		$list = $Station->table('tb_stationcommon tbs,tb_checkplan tbc')->where($wherequery)->limit($start.','.$page_size)->select();

		$fields = $Station -> getDbFields();
		$size = count($fields);
		$i = 0;
		foreach($list as $l)
		{
			$j = 0;
			foreach($fields as $f)	
			{
				if($j == $size-2)
					break;
				$result[$f] = urlencode($l[$f]); //编码
				$j++;
			}
			$results[$i] = $result;
			$i++;
		}

		$responce['curr_page'] = $curr_page;
		$responce['total_pages'] = $total_pages;
		$responce['records'] = $total;
		$responce['rows'] = $results;
		echo getResponse($responce,'','','');

	}
	public function details(){
		$results = array();
		$StationId = getParam('id',1); 	
		$wherequery = " StationId = '".$StationId."'";

		$Station = M("checkdetails");
		$list = $Station -> where($wherequery)->select();
		$fields = $Station -> getDbFields();
		$size = count($fields);
		$i = 0;
		foreach($list as $l)
		{
			$j = 0;
			foreach($fields as $f)	
			{
				if($j == $size-2)
					break;
				$result[$f] = urlencode($l[$f]);
				$j++;
			}
			$results[$i] = $result;
			$i++;
				
		}
		echo getResponse($results,'','','');
			
	}

	public function show()
	{
		Log::Write(json_encode($_GET),Log::DEBUG,'');
		$this->display(); // 输出模板
	}
}
?>