var zTreeObj;
var infoWindow = new AMap.InfoWindow();
var initMap = function (mapdiv, zoom, center, targetId, viewType) {
    var map = new AMap.Map(mapdiv);
    map.setZoom(zoom);
    map.setCenter(center);
    $.get('/Ajax/Devices', { targetId: targetId, viewType: viewType }, function (ret) {
        getDeviceStatus(ret, 0, map);
    });
    return map;
};

var markerClick = function(e) {
    infoWindow.setContent(e.target.content);
    infoWindow.open(e.target.rmap, e.target.getPosition());
};

var getDeviceStatus = function(source, startIndex, map) {
    var devs = source.slice(startIndex, 100);
    startIndex += 100;
    $.post('/Ajax/DeviceStatus', { 'deviceList': devs }, function (ret) {
        $.each(ret, function(index, dev) {
            var marker = new AMap.Marker({
                position: [dev.lon, dev.lat],
                title: dev.name
            });
            marker.rmap = map;
            marker.content = '<div class="panel panel-primary"><div class="panel-heading">' + dev.name
            + '</div><div class="panel-body">'
            + '扬尘值：' + dev.pm + '</br>'
            + '噪音值：' + dev.noise + '</br>'
            + '时间：' + dev.time
            + '</div></div>';
            marker.on('click', markerClick);
            marker.setMap(map);
        });
        if (startIndex < source.length) {
            getDeviceStatus(source, startIndex, map);
        }
        map.setFitView();
    });
}

var setupChart = function (chartdiv, target, tType, select) {
    $('#' + chartdiv).height($($('#' + chartdiv).parents('.panel')[1]).height() - 28);
    var chart = echarts.init(document.getElementById(chartdiv));
    $('#' + select).change(function () {
        updateMonitorChart(chart, target, tType, 1, $('#' + select).val(), 24);
    });

    return chart;
};

var updateMonitorChart = function (chart, target, tType, dCate, dType, count) {
    $.get('/Ajax/MonitorData',
    {
        target: target,
        tType: tType,
        dType: dType,
        dCate: dCate,
        count: count
    }, function (ret) {
        if (ret.success) {
            var params = {
                'title': dataType[dType],
                'seriesdata': ret.data.map(function (dt) { return dt.value; }),
                'category': ret.data.map(function (dt) { return dt.date })
            }
            var option = chartsOption.lineOptions(params);
            chart.setOption(option);
        }
    });

}

var dataType = {
    0: '粉尘(mg/m³)',
    1: 'Pm2.5(mg/m³)',
    2: 'Pm10(mg/m³)',
    3: '噪音(dB)',
    4: '温度(℃)',
    5: '湿度(%)',
    6: '风速(m/s)'
}