var chartsOption = {};

$(function () {
    chartsOption.barOption = function (params) {
        var option = {
            color:['#3398DB'],
            title: {
                text: params['title']
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            xAxis: {
                type: 'category',
                data: params['xAxis']
            },
            yAxis: {
                name: params['yAxisName']
            },
            series: params['series']
        }

        return option;
    };
})