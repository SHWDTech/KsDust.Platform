var districtChart = null;
var projectChart = null;
$(function () {
    $('#districtTopten').height($('#districtTopten').parents('.panel').height() - 28);
    districtChart = echarts.init(document.getElementById('districtTopten'));
    districtChart.setOption(chartsOption.barOption({
        title: "昆山市区县颗粒物小时均值",
        xAxis: window.districtTopTen.map(function (d) { return d.DistrictName }),
        yAxisName:'浓度mg/m³',
        series: [{ name: '颗粒物', type: 'bar', data: window.districtTopTen.map(function (d) { return d.CurrentTsp }) }]
    }));

    $('#projectTopten').height($('#projectTopten').parents('.panel').height() - 28);
    projectChart = echarts.init(document.getElementById('projectTopten'));
    projectChart.setOption(chartsOption.barOption({
        title: "昆山市工程颗粒物小时均值",
        xAxis: window.prjTopTen.map(function (d) { return d.ProjectName }),
        yAxisName: '浓度mg/m³',
        xAxisNameRotate: -45,
        series: [{ name: '颗粒物', type: 'bar', data: window.prjTopTen.map(function (d) { return d.CurrentTsp }) }]
    }));
});