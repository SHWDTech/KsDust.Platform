var chartsOption = {};

chartsOption.barOption = function (params) {
    var option = {
        color: ['#3398DB'],
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

chartsOption.lineOptions = function (params) {
    var option = {
        color: ['#3398DB'],
        title: {
            text: params['title']
        },
        tooltip: {
            trigger: 'axis',
            //formatter: function (params) {
            //    params = params[0];
            //    return params.date + ':' + params.value[1];
            //},
            axisPointer: {
                animation: false
            }
        },
        xAxis: {
            type: 'category',
            boundaryGap: false,
            data: params['category']
        },
        yAxis: {
            type: 'value'
        },
        series: [{
            name: params['title'],
            type: 'line',
            data: params['seriesdata']
        }]
    };

    return option;
};