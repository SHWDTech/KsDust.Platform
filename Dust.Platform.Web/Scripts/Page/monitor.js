var zTreeObj;
var devStatus = {
    OffLine: 0x00,

    Good: 0x01,

    Alarm: 0x02,

    Bad: 0x03
};
var infoWindow = new AMap.InfoWindow({
    offset: new AMap.Pixel(10, -30)
});
var initMap = function (mapdiv, zoom, center, targetId, viewType) {
    var map = new AMap.Map(mapdiv);
    map.setZoom(zoom);
    map.setCenter(center);
    $.get('/Ajax/Devices', { targetId: targetId, viewType: viewType }, function (ret) {
        getDeviceStatus(ret, 0, map);
    });
    return map;
};

var markerClick = function (e) {
    infoWindow.setContent(e.target.windowInfo);
    infoWindow.open(e.target.rmap, e.target.getPosition());
};

var getDeviceStatus = function (source, startIndex, map) {
    var devs = source.slice(startIndex, 100);
    startIndex += 100;
    $.post('/Ajax/DeviceStatus', { 'deviceList': devs }, function (ret) {
        $.each(ret, function (index, dev) {
            var marker = new AMap.Marker({
                position: [dev.lon, dev.lat],
                title: dev.name,
                content: getMarkerContent(dev)
        });
            marker.rmap = map;
            marker.windowInfo = '<div class="panel panel-primary"><div class="panel-heading">' + dev.name
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

var getMarkerContent = function (dev) {
    switch (dev.status) {
        case devStatus.OffLine:
            return '<div class="marker-route offline">' + dev.pm + '</div>';
        case devStatus.Good:
            return '<div class="marker-route good">' + dev.pm + '</div>';
        case devStatus.Alarm:
            return '<div class="marker-route alarm">' + dev.pm + '</div>';
        case devStatus.Bad:
            return '<div class="marker-route bad">' + dev.pm + '</div>';
        default:
            return '<div class="marker-route offline">' + dev.pm + '</div>';
    }
};

var setupChart = function (chartdiv, target, tType, select) {
    $('#' + chartdiv).height($($('#' + chartdiv).parents('.panel')[1]).height() - 28);
    var chart = echarts.init(document.getElementById(chartdiv));
    $('#' + select).change(function () {
        updateMonitorChart(chart, target, tType, 1, $('#' + select).val(), 24);
    });
    $('#' + select).select2();

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
    1: '粉尘(mg/m³)',
    2: 'Pm2.5(mg/m³)',
    3: 'Pm10(mg/m³)',
    4: '噪音(dB)',
    5: '温度(℃)',
    6: '湿度(%)',
    7: '风速(m/s)'
}