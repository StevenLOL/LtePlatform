# Angular�����������·�ɲ�
����һ���У������һЩ������������ݴ���ģ�����
��Ϊ���ġ��˵�����ʽ�����κ��������ģ�顣
## app.coreģ��
���Ǻ���ģ�飬��ǰ��ģ����������ķ���
��Ҫ����appUrlService��generalHttpService�����������Ǵ���ṹ���£�
```javascript
angular.module('app.core', [])
...
    .factory('appUrlService', function(publicNetworkIp) {
    ...
    })
    ...
    .factory('generalHttpService', function ($q, $http, $sce, appUrlService) {
    ...
    });
```
### appUrlService����
### generalHttpService����
## app.menuģ��
## app.formatģ��
## app.geometryģ��
## app.calculation