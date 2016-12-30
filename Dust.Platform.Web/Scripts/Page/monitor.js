var zTreeObj;
var initMap = function (mapdiv, zoom, center) {
    var map = new AMap.Map(mapdiv);
    map.setZoom(zoom);
    map.setCenter(center);
    return map;
};

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